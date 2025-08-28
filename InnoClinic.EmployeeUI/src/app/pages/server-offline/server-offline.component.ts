import {Component} from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';

@Component({
	selector: 'app-server-offline',
	standalone: true,
	imports: [CommonModule, NgOptimizedImage],
	template: `
		<div class="content">
			<img ngSrc="/assets/imgs/innoclinic-logo.png" alt="InnoClinic Logo" width="231" height="99">
			<p class="sign">🚫</p>
			<p class="text">Oops! Looks like our sign in service is having a moment.</p>
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
		
		.sign {
			margin-top: 40px;
			font-size: 4rem;
		}
		
		.text{
			font-size: 2rem;
		}
	`]
})
export class ServerOfflineComponent {
}