import {Component, ViewContainerRef, inject, signal, computed, ViewEncapsulation, Signal, effect} from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import {AccountPanelComponent} from "../account-panel/account-panel.component";
import {ComponentPortal} from '@angular/cdk/portal';
import {Overlay, OverlayRef} from "@angular/cdk/overlay";
import {OidcSecurityService} from "angular-auth-oidc-client";
import {ToastService} from "../../data/services/toast.service";
import {SafeUrl} from "@angular/platform-browser";
import {FileService} from "../../data/services/file.service";
import {UserService} from "../../data/services/user.service";
import {User} from "../../data/interfaces/user.interface";

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
					<span>Doctors</span>
				</a>
				<a routerLink="/specializations" routerLinkActive="active" class="nav-item">
					<span>Specializations</span>
				</a>
			</div>

			<div class="nav-user">
				<button class="main-positive-btn">Make an appointment</button>
				<div class="user-info">
					@if (authenticated().isAuthenticated) {
						<div class="user-photo">
							@defer (when isReady()) {
								<img [src]="photoUrl()" alt="user-photo" class="user-photo">
							}
						</div>
						<span class="user-name">{{ userName() }}</span>
						<span class="user-name">{{ userFullName() }}</span>
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
	private overlayRef: OverlayRef | null = null;
	private isPopupOpen = false;
	authenticated = this.oidc.authenticated;
	accountPanelVisible = signal(false);
	userData = this.oidc.userData;
	userName = computed(() => this.userData().userData?.email);
	userFullName = signal<string>('');
	photoId = computed(() => this.userData().userData?.photo_id);
	photoUrl = signal<SafeUrl>('');
	isReady = signal(false);

	constructor(private overlay: Overlay,
							private vcr: ViewContainerRef,
							private fileService: FileService,
							private userService: UserService,) {
		effect(() => {
			const url = this.photoUrl();
			if (url !== '') {
				this.isReady.set(true);
			}
		});

		this.loadUserFullName();
		this.getPhotoUrl();
	}

	async login(): Promise<void> {
		if (this.isPopupOpen) {
			console.warn('Popup already open');
			return;
		}

		this.isPopupOpen = true;
		const popupOptions = {width: 330, height: 500, left: 50, top: 50};

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

	private async getPhotoUrl() {
		this.photoUrl.set(await this.fileService.getUserPhoto(this.photoId()));
	}

	private loadUserFullName() {
		try {
			this.userService
					.getUserProfile(this.userData().userData?.role, this.userData().userData?.sub)
					.subscribe({
						next: (profile: User) => {
							this.userFullName.set(`${profile.firstName} ${profile.lastName}`);
						},
						error: () => {
							this.userFullName.set('Unknown User');
						}
					});
		}
		catch (error) {
			this.userFullName.set('Unknown User');
		}
	}
}