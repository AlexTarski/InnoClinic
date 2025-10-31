import {Component, computed, inject, signal, ViewContainerRef, ViewEncapsulation} from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import {AccountPanelComponent} from "../account-panel/account-panel.component";
import {ComponentPortal} from '@angular/cdk/portal';
import {Overlay, OverlayRef} from "@angular/cdk/overlay";
import {OidcSecurityService} from "angular-auth-oidc-client";

@Component({
	selector: 'app-top-nav',
	standalone: true,
	imports: [CommonModule, NgOptimizedImage],
	template: `
		<nav class="top-nav">
			<div class="nav-brand">
				<img ngSrc="/assets/imgs/innoclinic-logo.png" alt="InnoClinic Logo" width="133" height="57">
			</div>

			<div class="nav-user">
				<div class="user-info">
					@if (authenticated().isAuthenticated) {
						<span class="user-avatar">👤</span>
						<span class="user-name">{{ userName() }}</span>
					}
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
			background: var(--element-main-color);
			color: var(--text-color-light);
			box-shadow: 0 2px 4px var(--container-shadow-color);
		}

		.nav-brand {
			display: flex;
			justify-content: center;
			align-items: center;
			margin: 0;
			max-height: 60px;
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
			color: var(--text-color-light);
			font-size: 1.2rem;
			cursor: pointer;
			padding: 8px;
			border-radius: 4px;
			transition: background-color 0.2s;
		}

		.menu-btn:hover {
			background: var(--default-hover-color-dark);
		}

		.menu-btn.active {
			cursor: pointer !important;
			background: var(--element-accent-color);
		}
	`],
	encapsulation: ViewEncapsulation.Emulated
})
export class TopNavComponent {
	oidc = inject(OidcSecurityService);
	private overlayRef: OverlayRef | null = null;
	accountPanelVisible = signal(false);
	userData = this.oidc.userData;
	authenticated = this.oidc.authenticated;


	constructor(private overlay: Overlay,
							private vcr: ViewContainerRef) {
	}

	userName = computed(() => this.userData().userData?.email);

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