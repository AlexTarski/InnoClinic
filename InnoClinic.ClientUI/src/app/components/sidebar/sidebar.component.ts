import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  template: `
    <aside class="sidebar">
      <div class="sidebar-header">
        <h3>Menu</h3>
      </div>
      
      <nav class="sidebar-nav">
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
        </div>
      </nav>
    </aside>
  `,
  styles: [`
    .sidebar {
      width: 250px;
      background: #34495e;
      color: white;
      height: 100vh;
      overflow-y: auto;
      box-shadow: 2px 0 4px rgba(0,0,0,0.1);
    }
    
    .sidebar-header {
      padding: 20px;
      border-bottom: 1px solid #2c3e50;
    }
    
    .sidebar-header h3 {
      margin: 0;
      font-size: 1.2rem;
      font-weight: 600;
    }
    
    .sidebar-nav {
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
      background: rgba(255,255,255,0.1);
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
export class SidebarComponent {} 