import {Component, ViewEncapsulation} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-main-content',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  template: `
    <main class="main-content">
      <div class="content-body">
        <router-outlet />
      </div>
    </main>
  `,
  styles: [`
		.main-content {
			flex: 1;
			background: #f8f9fa;
			height: 100%;
			width: 100%;
			max-width: 100%;
			overflow-y: auto;
			scrollbar-width: none;
			box-sizing: border-box;
		}

		.content-body {
			padding: 20px;
			width: 100%;
			min-height: 400px;
		}
  `],
	encapsulation: ViewEncapsulation.Emulated
})
export class MainContentComponent {} 