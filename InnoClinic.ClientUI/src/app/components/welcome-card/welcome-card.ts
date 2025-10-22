import {Component, ViewEncapsulation} from '@angular/core';

@Component({
	selector: 'app-welcome-card',
	imports: [],
	template: `
		<div class="content-header">
			<h2>Welcome to InnoClinic</h2>
			<p>Your comprehensive healthcare management system</p>
		</div>
	`,
	styles: `
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
		}`,
	encapsulation: ViewEncapsulation.Emulated
})
export class WelcomeCard {}