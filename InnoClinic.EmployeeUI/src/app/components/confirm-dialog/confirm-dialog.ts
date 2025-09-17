import {Component, ViewEncapsulation} from '@angular/core';
import {DialogRef} from "@angular/cdk/dialog";

@Component({
  selector: 'app-confirm-dialog',
	imports: [
	],
  template: `
		<div class="component-container">
			<div class="confirm-dialog-container">
				<h3 class="component-header">Do you really want to cancel?</h3>
				<p class="component-text">Entered data will not be saved.</p>
				<div class="confirm-dialog-buttons">
					<button class="main-positive-btn" (click)="close(true)">Yes</button>
					<button class="main-positive-btn" (click)="close(false)">No</button>
				</div>
			</div>
		</div>
  `,
	styles: `
		.confirm-dialog-container {
			display: flex;
			flex-direction: column;
			padding: 10px;
		}
		
		.confirm-dialog-buttons {
			display: flex;
			flex-direction: row;
			align-items: flex-end;
			justify-content: flex-end;
			margin-top: 15px;
		}
		
		.main-positive-btn {
			margin: 0 5px;
			width: 60px;
			height: 30px;
			padding: 5px 10px;
			color: #1e3449;
			background-color: white;
			font-size: 0.9rem;
		}
	`,
	encapsulation: ViewEncapsulation.Emulated
})
export class ConfirmDialog {
	constructor(private ref: DialogRef<boolean>) {}

	close(result: boolean) {
		this.ref.close(result);
	}
}
