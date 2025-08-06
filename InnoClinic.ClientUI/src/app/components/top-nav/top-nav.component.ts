import {Component, ViewContainerRef, OnInit, inject} from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import {AccountPanelComponent} from "../account-panel/account-panel.component";
import { ComponentPortal } from '@angular/cdk/portal';
import {Overlay, OverlayRef} from "@angular/cdk/overlay";
import {AuthenticatedResult, OidcSecurityService} from "angular-auth-oidc-client";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {map, Observable} from "rxjs";
import {LoginPageComponent} from "../login-page.component/login-page.component";

@Component({
  selector: 'app-top-nav',
  standalone: true,
    imports: [CommonModule, RouterLink, RouterLinkActive, AccountPanelComponent, LoginPageComponent],
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
          <span class="user-avatar">👤</span>
          <span class="user-name">Mr. Smith</span>
        </div>
        <div class="user-menu">
            <button (click)="showLoginPage()">Show Login Page</button>
            @if (isLoginPageVisible) {
                <app-login-page (close)="hideLoginPage()"></app-login-page>
            }


            @if(authenticated().isAuthenticated)
            {
                <button (click)="logout()">Logout</button>
            } 
            @else
            {
                <button (click)="login()">Login</button>
            }
            <button (click)="callApi()">callApi</button>
            <button #panelButton (click)="toggleAccPanel(panelButton)" class="menu-btn">⚙️</button>
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
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
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
      background: rgba(255,255,255,0.1);
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
      background: rgba(255,255,255,0.1);
    }
  `]
})
export class TopNavComponent {

  oidc = inject(OidcSecurityService);
  private loginPopup: Window | null = null;
  isLoginPageVisible: boolean = false;
  authenticated = this.oidc.authenticated;
  private overlayRef: OverlayRef | null = null;

  constructor(private overlay: Overlay,
              private vcr: ViewContainerRef,
              private http: HttpClient) {
  }

  showLoginPage(): void {
      this.isLoginPageVisible = true;
  }

  hideLoginPage(): void {
      this.isLoginPageVisible = false;
  }

  login(){
    // this.oidc.authorize();

      this.oidc
          .authorizeWithPopUp()
          .subscribe(({ isAuthenticated, errorMessage }) => {
              if (isAuthenticated) {
                  console.log('✅ Popup login successful');
              } else {
                  console.error('❌ Popup login failed:', errorMessage);
              }

              // Close popup if it’s still open
              if (this.loginPopup && !this.loginPopup.closed) {
                  this.loginPopup.close();
                  this.loginPopup = null;
              }
          });
  }


  logout() {
      this.oidc.logoff().subscribe((result) => console.log(result));
  }

  callApi(){
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
    });

    const portal = new ComponentPortal(AccountPanelComponent, this.vcr);
    this.overlayRef.attach(portal);
  }

}