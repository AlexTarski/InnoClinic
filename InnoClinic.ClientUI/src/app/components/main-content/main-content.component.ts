import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-main-content',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  template: `
    <main class="main-content">
      <div class="content-header">
        <h2>Welcome to InnoClinic</h2>
        <p>Your comprehensive healthcare management system</p>
      </div>
      
      <div class="content-body">
        <router-outlet />
      </div>
    </main>
  `,
  styles: [`
    .main-content {
      flex: 1;
      padding: 20px;
      background: #f8f9fa;
      height: 95vh;
      width: 110vh;
      overflow-y: auto;
      scrollbar-width: none;
    }
    
    .content-header {
      margin-bottom: 30px;
      padding: 20px;
      background: white;
      border-radius: 8px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }
    
    .content-header h2 {
      margin: 0 0 10px 0;
      color: #2c3e50;
      font-size: 1.8rem;
      font-weight: 600;
    }
    
    .content-header p {
      margin: 0;
      color: #7f8c8d;
      font-size: 1rem;
    }
    
    .content-body {
      min-height: 400px;
    }
  `]
})
export class MainContentComponent {} 