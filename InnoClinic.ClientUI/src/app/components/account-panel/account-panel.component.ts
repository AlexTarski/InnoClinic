import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import {MatMenuItem} from "@angular/material/menu";

@Component({
  selector: 'app-account-panel',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive, MatMenuItem],
  template: `
    <aside class="account-panel">
      <div class="account-menu-header">
        <h3>Menu</h3>
      </div>

      <nav class="account-panel">
        <div class="nav-section">
          <h4 class="section-title">Appointments & Results</h4>
          <a routerLink="/dashboard" routerLinkActive="active" class="nav-link">
            <span class="nav-icon">📅</span>
            <span>My appointments</span>
          </a>

          <a routerLink="/medical-results" routerLinkActive="active" class="nav-link">
            <span class="nav-icon">📊</span>
            <span>My Medical Results</span>
          </a>
          <button mat-menu-item (click)="logout()">Logout</button>
        </div>
      </nav>
    </aside>
  `,
  styles: [`
    .account-panel {
      width: 250px;
      background: #2c3e50;
      color: white;
      box-shadow: 2px 0 4px rgba(0, 0, 0, 0.1);
      
    }

    .account-menu-header {
      padding: 20px;
      border-bottom: 1px solid #2c3e50;
    }

    .account-menu-header h3 {
      margin: 0;
      font-size: 1.2rem;
      font-weight: 600;
    }

    .account-menu-nav {
      padding: 20px 0;
    }

    .nav-section {
      margin-bottom: 30px;
    }

    .section-title {
      margin: 0 0 15px 20px;
      font-size: 0.9rem;
      font-weight: 600;
      color: #bdc3c7;
      text-transform: uppercase;
      letter-spacing: 0.5px;
    }

    .nav-link {
      display: flex;
      align-items: center;
      gap: 12px;
      padding: 12px 20px;
      color: #ecf0f1;
      text-decoration: none;
      transition: background-color 0.2s;
      border-left: 3px solid transparent;
    }

    .nav-link:hover {
      background: rgba(255, 255, 255, 0.1);
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
  `]
})
export class AccountPanelComponent {
  logout() {
    // Perform logout
  }
}