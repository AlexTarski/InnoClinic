import {Component, computed, inject, signal, ViewContainerRef} from '@angular/core';
import {RouterLink, RouterLinkActive} from '@angular/router';
import {CommonModule} from '@angular/common';
import {AccountPanelComponent} from "../account-panel/account-panel.component";
import {ComponentPortal} from '@angular/cdk/portal';
import {Overlay, OverlayRef} from "@angular/cdk/overlay";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {ToastService} from "../../data/services/toast.service";

@Component({
	selector: 'app-top-nav',
	standalone: true,
	imports: [CommonModule, RouterLink, RouterLinkActive],
	template: `
		<nav class="top-nav">
			<div class="nav-brand">
				<h1>InnoClinic</h1>
			</div>

			<div class="nav-user">
				<div class="user-info">
<!--					<button (click)="callApi()">callApi</button>-->
<!--					@if (authenticated().isAuthenticated) {-->
<!--						<span class="user-avatar">👤</span>-->
<!--						<span class="user-name">{{ userName() }}</span>-->
<!--					}-->
				</div>
				<div class="user-menu">
					<button #panelButton
									(click)="toggleAccPanel(panelButton)"
									class="menu-btn"
									[class.active]="accountPanelVisible()">⚙️
					</button>
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
	`]
})
export class TopNavComponent {
	private overlayRef: OverlayRef | null = null;
	accountPanelVisible = signal(false);


	constructor(private overlay: Overlay,
							private vcr: ViewContainerRef,
							private http: HttpClient) {
	}

	// callApi() {
	// 	this.oidc.getAccessToken().subscribe((token) => {
	// 		const httpOptions = {
	// 			headers: new HttpHeaders({
	// 				Authorization: 'Bearer ' + token,
	// 			}),
	// 			responseType: 'text' as const,
	// 		};
	//
	// 		this.http.get('https://localhost:7036/secret', httpOptions).subscribe({
	// 			next: (response) => {
	// 				console.log('API response:', response);
	// 			},
	// 			error: (error) => {
	// 				console.error('API error:', error);
	// 			},
	// 		});
	// 	});
	// }

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