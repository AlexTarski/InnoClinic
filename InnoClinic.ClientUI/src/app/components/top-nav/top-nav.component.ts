import {Component, ViewContainerRef, inject, signal, computed, ViewEncapsulation} from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import {AccountPanelComponent} from "../account-panel/account-panel.component";
import {ComponentPortal} from '@angular/cdk/portal';
import {Overlay, OverlayRef} from "@angular/cdk/overlay";
import {OidcSecurityService} from "angular-auth-oidc-client";
import {HttpClient} from "@angular/common/http";
import {ToastService} from "../../data/services/toast.service";
import {ConfigService} from "../../data/services/config.service";

@Component({
  selector: 'app-top-nav',
  standalone: true,
	imports: [CommonModule, RouterLink, RouterLinkActive, NgOptimizedImage],
  template: `
		<nav class="top-nav">
			<div class="nav-brand">
				<img ngSrc="/assets/imgs/innoclinic-logo.png" alt="InnoClinic Logo" width="133" height="57">
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
				<button class="main-positive-btn">Make an appointment</button>
				<div class="user-info">
					@if (authenticated().isAuthenticated) {
						<span class="user-avatar">👤</span>
						<span class="user-name">{{ userName() }}</span>
					}
				</div>
				<div class="user-menu">
					@if (!authenticated().isAuthenticated) {
						<button class="sign-in-btn" (click)="login()">
							<svg class="sign-in-icon" viewBox="0 0 40 40">
								<circle cx="20" cy="12" r="8.5" fill="none" stroke="currentColor" stroke-width="2"/>
								<path d="M5,39 C5,33 12,27 20,27 C28,27 35,33 35,39"
											fill="none" stroke="currentColor" stroke-width="2"/>
							</svg>
							<div class="sign-in-text">
								Sign In
							</div>
						</button>
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
	styleUrl: `./top-nav.component.css`,
	encapsulation: ViewEncapsulation.Emulated
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
							private http: HttpClient,
							private configService: ConfigService,) {
	}

	userName = computed(() => this.userData().userData?.email);

	login(): void {
		if (this.isPopupOpen) {
			console.warn('Popup already open');
			return;
		}

      this.isPopupOpen = true;
			const popupOptions = { width: 330, height: 500, left: 50, top: 50 };

      this.oidc.authorizeWithPopUp(undefined, popupOptions).subscribe({
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