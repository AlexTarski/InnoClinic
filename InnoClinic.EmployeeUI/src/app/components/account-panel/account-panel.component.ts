import {Component, ViewEncapsulation} from '@angular/core';
import {RouterLink, RouterLinkActive} from '@angular/router';
import {CommonModule} from '@angular/common';
import {OidcSecurityService} from "angular-auth-oidc-client";

@Component({
	selector: 'app-account-panel',
	standalone: true,
	imports: [CommonModule, RouterLink, RouterLinkActive],
	template: `
		<aside class="account-panel">
			<nav class="account-panel">
				<div class="nav-section">
					<a routerLink="/dashboard" routerLinkActive="active" class="nav-link">
						<span class="nav-icon">📅</span>
						<span>My appointments</span>
					</a>

					<a routerLink="/medical-results" routerLinkActive="active" class="nav-link">
						<span class="nav-icon">📊</span>
						<span>My Medical Results</span>
					</a>
					<button (click)="logout()" class="main-negative-btn">Sign Out</button>
				</div>
			</nav>
		</aside>
	`,
	styles: [`
		.account-panel {
			width: 250px;
			background: var(--container-background-color);
			border-radius: 5px;
			color: var(--text-color-dark);
			box-shadow: 2px 0 4px var(--container-shadow-color);
		}

		.account-menu-header h3 {
			margin: 0;
			font-size: 1.2rem;
			font-weight: 600;
		}

		.nav-section {
			margin-bottom: 30px;
		}

		.nav-link {
			display: flex;
			align-items: center;
			gap: 12px;
			padding: 12px 20px;
			color: var(--text-color-dark);
			text-decoration: none;
			transition: background-color 0.2s;
			border-left: 3px solid transparent;
		}

		.nav-link:hover {
			background: var(--default-hover-color-light) !important;
			border-left-color: var(--element-accent-color);
		}

		.nav-link.active {
			background: var(--default-hover-color-light);
			border-left-color: var(--element-accent-color);
		}

		.nav-icon {
			font-size: 1.1rem;
			width: 20px;
			text-align: center;
		}

		.nav-link span:last-child {
			font-weight: 500;
		}

		.main-negative-btn {
			margin: 20px 0px 20px 26px;
		}
	`],
	encapsulation: ViewEncapsulation.Emulated
})
export class AccountPanelComponent {

	constructor(private oidc: OidcSecurityService) {}

	logout() {
		this.oidc.logoffAndRevokeTokens().subscribe((result) => console.log(result));
	}
}