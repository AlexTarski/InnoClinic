import {Component, computed, effect, inject, signal, ViewEncapsulation} from '@angular/core';
import {CommonModule} from '@angular/common';
import {OidcSecurityService} from "angular-auth-oidc-client";
import {FileService} from "../../data/services/file.service";
import {SafeUrl} from "@angular/platform-browser";
import {User} from "../../data/interfaces/user.interface";
import {UserService} from "../../data/services/user.service";
import {RouterLink, RouterLinkActive} from "@angular/router";

@Component({
	selector: 'app-account-panel',
	standalone: true,
	imports: [CommonModule, RouterLink, RouterLinkActive],
	template: `
		<div class="user-panel">
			<a routerLink="/profile" routerLinkActive="active" class="user-info">
				<div class="user-photo">
					<img [src]="photoUrl()" alt="user-photo" class="user-photo">
				</div>
				<span class="user-name">{{ userFullName() }}</span>
			</a>
			<div class="logout-btn">
				<button (click)="logout()" class="main-negative-btn">Sign Out</button>
			</div>
		</div>
	`,
	styles: [`
		.user-panel {
			display: flex;
			flex-direction: column;
			align-items: center;
			justify-content: center;
			width: 250px;
			gap: 8px;
		}

		.user-info {
			display: flex;
			flex-direction: row;
			align-items: center;
			justify-content: flex-start;
			padding: 2px 6px;
			gap: 12px;
			min-width: 250px;
			background: none;
			color: var(--text-color-light);
			text-decoration: none;
			border: none;
			cursor: pointer;
			border-radius: 4px;
			transition: background-color 0.2s;
		}

		.user-info:hover {
			background: var(--default-hover-color-dark);
		}

		.user-info.active {
			background: var(--element-accent-color-darker);
		}

		.user-photo {
			width: 60px;
			height: 60px;
			min-width: 60px;
			border-radius: 50%;
			overflow: hidden;
			display: inline-block;
			aspect-ratio: 1 / 1;
		}

		.user-photo img {
			width: 100%;
			height: auto;
			object-fit: cover;
			object-position: left center;
			display: block;
		}

		.user-name {
			flex: 1;
			hyphens: auto;
			overflow-wrap: break-word;
			max-width: fit-content;
			font-weight: 500;
		}
		
		.logout-btn {
			display: flex;
			flex-direction: row;
			justify-content: space-around;
			width: 100%;
		}
		
		.main-negative-btn {
			padding: 4px 0 !important;
			width: 80%;
		}
	`],
	encapsulation: ViewEncapsulation.Emulated
})
export class AccountPanelComponent {
	oidc = inject(OidcSecurityService);
	userData = this.oidc.userData;
	userFullName = signal<string>('');
	photoId = computed(() => this.userData().userData?.photo_id);
	photoUrl = signal<SafeUrl>('');
	isReady = signal(false);

	constructor(private fileService: FileService,
							private userService: UserService) {
		effect(() => {
			const url = this.photoUrl();
			if (url !== '') {
				this.isReady.set(true);
			}
		});

		this.loadUserFullName();
		this.getPhotoUrl();
	}

	logout() {
		this.oidc.logoffAndRevokeTokens().subscribe((result) => console.log(result));
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