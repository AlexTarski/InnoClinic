import {Component} from '@angular/core';
import {CommonModule} from '@angular/common';

@Component({
	selector: 'app-server-offline',
	standalone: true,
	imports: [CommonModule],
	template: `
		<div class="content">
			<p class="text">🚫 Oops! Looks like our login service is having a moment.</p>
			<p class="text">Please try again shortly or reach out to support if it persists.</p>
		</div>
	`,
	styles: [`
		.content {
			flex: 1;
			padding: 20px;
			background: #f8f9fa;
			overflow-y: auto;
			min-height: 400px;
			align-items: center;
			text-align: center;
		}

		.content-header p {
			margin: 0;
			color: #2c3e50;
			font-size: 1.4rem;
		}
		
		.text{
			font-size: 2rem;
		}
	`]
})
export class ServerOfflineComponent {
}