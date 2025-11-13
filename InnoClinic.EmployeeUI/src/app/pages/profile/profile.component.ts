import {Component, computed, signal, ViewEncapsulation} from '@angular/core';
import { CommonModule } from '@angular/common';
import {OidcSecurityService} from "angular-auth-oidc-client";
import {SafeUrl} from "@angular/platform-browser";
import {FileService} from "../../data/services/file.service";

@Component({
    selector: 'app-profile',
    standalone: true,
	imports: [CommonModule],
    template: `
			<div class="app-container">
				<div class="user-photo">
					<img [src]="photoUrl()" alt="user-photo" class="user-photo">
				</div>
			</div>
		`,
	styles: [`
		.app-container {
			height: 100vh;
			display: flex;
			flex-direction: column;
		}
		
		.user-photo {
			max-width: 500px;
		}
	`],
	encapsulation: ViewEncapsulation.Emulated
})
export class ProfileComponent {
	userData;
	photoId = computed(() => this.userData().userData?.photo_id);
	photoUrl = signal<SafeUrl>('');
	constructor(private oidc: OidcSecurityService,
							private fileService: FileService)
	{
		this.userData = oidc.userData;

		this.getPhotoUrl();
	}

	private async getPhotoUrl() {
		this.photoUrl.set(await this.fileService.getUserPhoto(this.photoId()));
	}
}