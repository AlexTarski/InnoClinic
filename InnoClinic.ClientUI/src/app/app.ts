import {Component, inject, OnInit, signal} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TopNavComponent } from './components/top-nav/top-nav.component';
import { MainContentComponent } from './components/main-content/main-content.component';
import {OidcSecurityService} from "angular-auth-oidc-client";
import {ToastComponent} from "./components/toast-component/toast-component";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, TopNavComponent, MainContentComponent, ToastComponent],
  template: `
    <div class="app-container">
      <app-top-nav />
      <div class="app-body">
          <app-toast />
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

export class App implements OnInit {
  protected readonly title = signal('InnoClinic');

  constructor(private oidc: OidcSecurityService) {}

  ngOnInit() {
    this.oidc.checkAuth().subscribe();
  }
}
