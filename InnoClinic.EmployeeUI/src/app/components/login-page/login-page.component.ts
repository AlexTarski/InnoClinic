import {Component, inject} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";

@Component({
	selector: 'app-login-page',
	standalone: true,
	imports: [CommonModule, ReactiveFormsModule],
	template: `
		<form 
				class="login-form"
				[formGroup]="form"
				(ngSubmit)="onSubmit()">
			<h1 class="login-header">Login</h1>
			<div class="block">
				<label>
					Email:
					<input
							formControlName="email"
							class="input" type="email">
				</label>
			</div>
			<div class="block">
				<label>
					Password:
					<input
							formControlName="password"
							class="input" type="password">
				</label>
			</div>
			<div class="block">
				<button class="sign-in-button" type="submit">Sign In</button>
			</div>
		</form>
	`,
	styles: [`
		.login-header {
			font-family: Segoe UI, serif;
			color: #332600;
			font-size: 30px;
			text-align: center;
		}

		.input {
			font-size: 16px;
			font-family: Segoe UI, serif;
			background-color: #fff;
			width: 100%;
		}

		.text-danger {
			color: #e90b0b;
		}

		.sign-in-button {
			font-size: 20px;
			background-color: #3498db;
			border: none;
			border-radius: 10px;
			padding: 10px;
			color: #fff;
			cursor: pointer;
			font-family: Segoe UI, serif;
			width: 100%;
		}

		.sign-in-button:hover {
			background-color: #2980b9;
		}

		.sign-in-button:active {
			background-color: #2471a3;
			transform: scale(0.98);
		}

		.sign-in-button:disabled {
			background-color: #75bceb;
			color: #c5d0db;
			cursor: not-allowed !important;
		}

		.block {
			margin-top: 10px;
		}

		.input.input-error {
			border: 2px solid red;
		}

		span.text-danger:not(.show-error) {
			display: none;
		}
	`]
})
export class LoginPageComponent {

	form: FormGroup = new FormGroup({
		email: new FormControl('', [Validators.required, Validators.email]),
		password: new FormControl('', [Validators.required]),
	});
	protected readonly onsubmit = onsubmit;

	onSubmit() {

	}
}