import {Component, ViewContainerRef} from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import {AccountPanelComponent} from "../account-panel/account-panel.component";
import { ComponentPortal } from '@angular/cdk/portal';
import {Overlay, OverlayRef} from "@angular/cdk/overlay";

@Component({
  selector: 'app-top-nav',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive, AccountPanelComponent],
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
  private overlayRef: OverlayRef | null = null;

  constructor(private overlay: Overlay, private vcr: ViewContainerRef) {}

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