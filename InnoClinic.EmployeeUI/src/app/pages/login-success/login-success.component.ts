import {Component, OnInit, ViewEncapsulation} from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import {AppConfigService} from "../../data/services/app-config.service";
import {Router} from "@angular/router";

@Component({
	selector: 'app-login-success',
	standalone: true,
	imports: [CommonModule, NgOptimizedImage],
	template: `
		<div class="content">
			<div class="message">
				<img ngSrc="/assets/imgs/success.png" alt="SUCCESS!" width="170" height="170">
				<p class="text">You've signed in successfully!</p>
				<p class="text">You will be redirected to main page in 5 seconds.</p>
				<p class="text">If not, <a [href]="baseUrl">click here</a></p>
			</div>
		</div>
	`,
	styles: [`
		:host {
			display: block;
			width: 100vw;
			height: 100vh;
		}

		.content {
			display: flex;
			height: 100vh;
			flex-direction: column;
			justify-content: center;
			align-items: center;
		}

		.message {
			background-color: #ffffff;
			color: #325346;
			border: 1px solid #ececec;
			border-radius: 10px;
			box-shadow: 0 0 10px -5px #2c3e50;
			margin-bottom: 20px;
			margin-top: 20px;
			max-width: 400px;
			text-align: center;
			align-self: center;
		}

		.content p {
			margin: 0 10px 18px 10px;
			color: #325346;
			font-size: 1.4rem;
		}

		.text {
			font-size: 2rem;
		}
	`],
	encapsulation: ViewEncapsulation.Emulated
})
export class LoginSuccessComponent implements OnInit {
	baseUrl?: string;
	private delay(ms: number) {
		return new Promise(resolve => setTimeout(resolve, ms));
	}
	constructor(private configService: AppConfigService,
							private router: Router) {
		this.baseUrl = this.configService.employeeUiUrl;
	}

	async ngOnInit() {
		await this.delay(5000);
		await this.router.navigateByUrl('');
	}
}