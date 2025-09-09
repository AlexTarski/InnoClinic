import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  template: `
    <aside class="sidebar">
      <nav class="sidebar-nav">
        <div class="nav-section">
          <a routerLink="/dashboard" routerLinkActive="active" class="nav-link">
            <span class="nav-icon">📅</span>
            <span>My appointments</span>
          </a>
          
          <a routerLink="/medical-results" routerLinkActive="active" class="nav-link">
            <span class="nav-icon">📊</span>
            <span>My Medical Results</span>
          </a>

					<a routerLink="/offices" routerLinkActive="active" class="nav-link">
						<span class="nav-icon">📅</span>
						<span>Offices</span>
					</a>

          <a routerLink="/patients" routerLinkActive="active" class="nav-link">
            <span class="nav-icon">👥</span>
            <span>Patients</span>
          </a>

          <a routerLink="/doctors" routerLinkActive="active" class="nav-link">
            <span class="nav-icon">👥</span>
            <span>Doctors</span>
          </a>
          
          <a routerLink="/specializations" routerLinkActive="active" class="nav-link">
            <span class="nav-icon">📋</span>
            <span>Specializations</span>
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