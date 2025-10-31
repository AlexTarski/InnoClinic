import {Component, ViewEncapsulation} from '@angular/core';
import { CommonModule } from '@angular/common';
import {TopNavComponent} from "../../components/top-nav/top-nav.component";
import {MainContentComponent} from "../../components/main-content/main-content.component";
import {ToastComponent} from "../../components/toast-component/toast-component";

@Component({
    selector: 'app-doctors',
    standalone: true,
	imports: [CommonModule, MainContentComponent, TopNavComponent, ToastComponent],
    template: `
			<div class="app-container">
				<app-top-nav/>
				<div class="app-body">
					<div class="main-content">
						<app-toast/>
						<app-main-content/>
					</div>
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
				justify-content: space-between;
				align-items: stretch;
				overflow: hidden;
				width: 100%;
				max-width: 100%;
			}

			.main-content {
				flex: 1;
			}
    `],
	encapsulation: ViewEncapsulation.Emulated
})
export class HomeComponent {}