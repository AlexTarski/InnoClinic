import {Component, signal} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TopNavComponent } from './components/top-nav/top-nav.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { MainContentComponent } from './components/main-content/main-content.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, TopNavComponent, SidebarComponent, MainContentComponent],
  template: `
    <div class="app-container">
      <app-top-nav />
      <div class="app-body">
        <app-sidebar />
        <app-main-content />
      </div>
    </div>
  `,
  styles: [`
    .app-container {
      height: 100vh;
      display: flex;
      flex-direction: column;
    }
    
    .app-body {
      display: flex;
      flex: 1;
      overflow: hidden;
    }
  `],
})
export class App {
  protected readonly title = signal('InnoClinic');
}
