import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {TopNavComponent} from "../../components/top-nav/top-nav.component";
import {SidebarComponent} from "../../components/sidebar/sidebar.component";
import {ToastComponent} from "../../components/toast-component/toast-component";
import {MainContentComponent} from "../../components/main-content/main-content.component";

@Component({
    selector: 'app-doctors',
    standalone: true,
	imports: [CommonModule, TopNavComponent, SidebarComponent, ToastComponent, MainContentComponent],
    template: `
			<div class="app-container">
				<app-top-nav/>
				<div class="app-body">
					<app-sidebar/>
					<app-toast/>
					<app-main-content/>
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
export class HomeComponent {}