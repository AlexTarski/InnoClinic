import {Component, ViewEncapsulation} from '@angular/core';
import { CommonModule } from '@angular/common';
import {TopNavComponent} from "../../components/top-nav/top-nav.component";
import {SidebarComponent} from "../../components/sidebar/sidebar.component";
import {MainContentComponent} from "../../components/main-content/main-content.component";

@Component({
    selector: 'app-home',
    standalone: true,
	imports: [CommonModule, TopNavComponent, SidebarComponent, MainContentComponent],
    template: `
			<div class="app-container">
				<app-top-nav/>
				<div class="app-body">
					<app-sidebar/>
					<div class="main-content">
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