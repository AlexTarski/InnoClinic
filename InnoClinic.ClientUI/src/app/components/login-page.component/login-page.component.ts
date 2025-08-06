import {Component, EventEmitter, inject, OnInit, Output} from '@angular/core';
import {OidcSecurityService} from "angular-auth-oidc-client";

@Component({
  selector: 'app-login-page',
  imports: [],
    templateUrl: `./login-page.component.html`,
    styleUrl: `./login-page.component.css`})
export class LoginPageComponent implements OnInit {
    @Output() close = new EventEmitter<void>();
    oidc = inject(OidcSecurityService);

    closeModal(): void {
        this.close.emit();
    }

    ngOnInit(): void {
        this.oidc
            .authorizeWithPopUp()
            .subscribe(({ isAuthenticated, errorMessage }) => {
                if (isAuthenticated) {
                    console.log('✅ Popup login successful');
                } else {
                    console.error('❌ Popup login failed:', errorMessage);
                }

                this.closeModal();
            });
    }
}
