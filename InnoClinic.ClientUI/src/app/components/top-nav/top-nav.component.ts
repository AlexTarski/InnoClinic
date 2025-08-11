import {Component, ViewContainerRef, inject, signal, computed, OnInit} from '@angular/core';
import {RouterLink, RouterLinkActive} from '@angular/router';
import {CommonModule} from '@angular/common';
import {AccountPanelComponent} from "../account-panel/account-panel.component";
import {ComponentPortal} from '@angular/cdk/portal';
import {Overlay, OverlayRef} from "@angular/cdk/overlay";
import {OidcSecurityService, UserDataResult} from "angular-auth-oidc-client";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {ToastService} from "../../data/services/toast.service";

@Component({
	selector: 'app-top-nav',
	standalone: true,
	imports: [CommonModule, RouterLink, RouterLinkActive, AccountPanelComponent],
	template: `
		<nav class="top-nav">
			<div class="nav-brand">
				<h1>InnoClinic</h1>
			</div>

			<div class="nav-menu">
				<a routerLink="/doctors" routerLinkActive="active" class="nav-item">
					<span class="nav-icon">👥</span>
					<span>Doctors</span>
				</a>
				<a routerLink="/specializations" routerLinkActive="active" class="nav-item">
					<span class="nav-icon">📋</span>
					<span>Specializations</span>
				</a>
			</div>

			<div class="nav-user">
				<div class="user-info">
					<button (click)="callApi()">callApi</button>
					@if (authenticated().isAuthenticated) {
						<span class="user-avatar">👤</span>
						<span class="user-name">{{ userName() }}</span>
					}

				</div>
				<div class="user-menu">
					@if (!authenticated().isAuthenticated) {
						<button class="signup-btn" (click)="login()">Sign In</button>
					} @else {
						<button #panelButton
										(click)="toggleAccPanel(panelButton)"
										class="menu-btn"
										[class.active]="accountPanelVisible()">⚙️
						</button>
					}
				</div>
			</div>
		</nav>
	`,
	styles: [`
		.top-nav {
			display: flex;
			align-items: center;
			justify-content: space-between;
			padding: 0 20px;
			height: 60px;
			background: #2c3e50;
			color: white;
			box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
		}

		.nav-brand h1 {
			margin: 0;
			font-size: 1.5rem;
			font-weight: 600;
		}

		.nav-menu {
			display: flex;
			gap: 20px;
		}

		.nav-item {
			display: flex;
			align-items: center;
			gap: 8px;
			padding: 8px 16px;
			color: white;
			text-decoration: none;
			border-radius: 6px;
			transition: background-color 0.2s;
		}

		.nav-item:hover {
			background: rgba(255, 255, 255, 0.1);
		}

		.nav-item.active {
			background: #3498db;
		}

		.nav-icon {
			font-size: 1.2rem;
		}

		.nav-user {
			display: flex;
			align-items: center;
			gap: 15px;
		}

		.user-info {
			display: flex;
			align-items: center;
			gap: 8px;
		}

		.user-avatar {
			font-size: 1.5rem;
		}

		.user-name {
			font-weight: 500;
		}

		.menu-btn {
			background: none;
			border: none;
			color: white;
			font-size: 1.2rem;
			cursor: pointer;
			padding: 8px;
			border-radius: 4px;
			transition: background-color 0.2s;
		}

		.menu-btn:hover {
			background: rgba(255, 255, 255, 0.1);
		}

		.menu-btn.active {
			cursor: pointer !important;
			background: #3498db;
		}

		.signup-btn {
			background-color: #007BFF; /* Primary blue */
			color: #fff;
			border: none;
			border-radius: 6px;
			padding: 10px 20px;
			font-size: 16px;
			font-weight: 500;
			cursor: pointer;
			box-shadow: 0 2px 6px rgba(0, 123, 255, 0.3);
			transition: background-color 0.3s ease, box-shadow 0.2s ease;
		}

		.signup-btn:hover {
			background-color: #0056b3; /* Darker blue */
			box-shadow: 0 4px 10px rgba(0, 86, 179, 0.4);
		}

		.signup-btn:active {
			background-color: #004085; /* Even darker blue */
			box-shadow: inset 0 2px 4px rgba(0, 0, 0, 0.2);
		}

	`]
})

export class TopNavComponent {
	oidc = inject(OidcSecurityService);
	private toast = inject(ToastService);
	authenticated = this.oidc.authenticated;
	private overlayRef: OverlayRef | null = null;
	accountPanelVisible = signal(false);
	private isPopupOpen = false;
	userData = this.oidc.userData;


	constructor(private overlay: Overlay,
							private vcr: ViewContainerRef,
							private http: HttpClient) {
	}

	userName = computed(() => this.userData().userData?.email);

	login(): void {
		if (this.isPopupOpen) {
			console.warn('Popup already open');
			return;
		}

		this.isPopupOpen = true;

		this.oidc.authorizeWithPopUp().subscribe({
			next: (result) => {
				console.log('Login successful', result);

				this.isPopupOpen = false;

				if (result.errorMessage != "User closed popup") {
					this.toast.show('You\'ve signed in successfully!', {type: 'success', duration: 2500});
					setTimeout(() => window.location.reload(), 1200);
				} else {
					window.location.reload();
				}
			},
			error: (err) => {
				console.error('Login failed', err);
				this.isPopupOpen = false;
				window.location.reload();
			}
		});
	}

	callApi() {
		this.oidc.getAccessToken().subscribe((token) => {
			const httpOptions = {
				headers: new HttpHeaders({
					Authorization: 'Bearer ' + token,
				}),
				responseType: 'text' as const,
			};

			this.http.get('https://localhost:7036/secret', httpOptions).subscribe({
				next: (response) => {
					console.log('API response:', response);
				},
				error: (error) => {
					console.error('API error:', error);
				},
			});
		});
	}

	toggleAccPanel(trigger: HTMLElement) {
		if (this.overlayRef) {
			this.overlayRef.dispose();
			this.overlayRef = null;
			this.accountPanelVisible.set(false);
			return;
		}

		const positionStrategy = this.overlay.position()
				.flexibleConnectedTo(trigger)
				.withPositions([
					{
						originX: 'end',
						originY: 'bottom',
						overlayX: 'end',
						overlayY: 'top',
						offsetY: 8
					}
				]);

		this.overlayRef = this.overlay.create({
			positionStrategy,
			hasBackdrop: true,
			backdropClass: 'transparent-backdrop'
		});

		this.overlayRef.backdropClick().subscribe(() => {
			this.overlayRef?.dispose();
			this.overlayRef = null;
			this.accountPanelVisible.set(false);
		});

		const portal = new ComponentPortal(AccountPanelComponent, this.vcr);
		this.overlayRef.attach(portal);
		this.accountPanelVisible.set(true);
	}
}