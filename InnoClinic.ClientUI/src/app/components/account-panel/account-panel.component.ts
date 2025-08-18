import {Component, inject} from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import {MatMenuItem} from "@angular/material/menu";
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

                  <button (click)="logout()" class="signout-btn">Sign Out</button>

              </div>
          </nav>
      </aside>
  `,
  styles: [`
      .account-panel {
          width: 250px;
          background: white;
          border-radius: 5px;
          color: black;
          box-shadow: 2px 0 4px rgba(0, 0, 0, 0.1);
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
          color: black;
          text-decoration: none;
          transition: background-color 0.2s;
          border-left: 3px solid transparent;
      }

      .nav-link:hover {
          background: rgba(0, 0, 0, 0.1) !important;
          border-left-color: #3498db;
      }

      .nav-link.active {
          background: #3498db;
          border-left-color: #2980b9;
      }

      .nav-icon {
          font-size: 1.1rem;
          width: 20px;
          text-align: center;
      }

      .nav-link span:last-child {
          font-weight: 500;
      }

      .signout-btn {
          background-color: #dc3545; /* Soft red */
          color: #fff;
          border: none;
          border-radius: 6px;
          padding: 10px 20px;
          margin: 20px 0px 20px 26px;
          font-size: 16px;
          font-weight: 500;
          cursor: pointer;
          box-shadow: 0 2px 6px rgba(220, 53, 69, 0.3);
          transition: background-color 0.3s ease, box-shadow 0.2s ease;
      }

      .signout-btn:hover {
          background-color: #c82333;
          box-shadow: 0 4px 10px rgba(200, 35, 51, 0.4);
      }

      .signout-btn:active {
          background-color: #bd2130;
          box-shadow: inset 0 2px 4px rgba(0, 0, 0, 0.2);
      }
  `]
})

export class AccountPanelComponent {
    oidc = inject(OidcSecurityService);

    logout() {
        this.oidc.logoff().subscribe((result) => console.log(result));
    }
}