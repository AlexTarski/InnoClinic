import {
	Component,
	DestroyRef,
	effect,
	inject,
	Inject,
	OnDestroy, OnInit,
	signal,
	ViewEncapsulation
} from '@angular/core';
import {DIALOG_DATA, DialogRef} from "@angular/cdk/dialog";
import {Office} from "../../data/interfaces/office.interface";
import {EditOfficeForm} from "../edit-office-form/edit-office-form";
import {Router} from "@angular/router";
import {FileService} from "../../data/services/file.service";
import {takeUntilDestroyed} from "@angular/core/rxjs-interop";
import {SafeUrl} from "@angular/platform-browser";

@Component({
  selector: 'app-office-card',
	imports: [
		EditOfficeForm
	],
	template: `
			<div class="component-container">
				<div class="office-photo">
					@defer (when isReady())
					{
						<img [src]="photoUrl()" alt="office-photo">
					}
				</div>
				@if(isEditing)
				{
					<app-edit-office-form [office]="office.office" (onChanged)="onChanged($event)"></app-edit-office-form>
				}
				@else {
					<div class="office-main-content">
						<div class="office-info-container">
							<div class="office-info">
								<h3 class="component-header">Office address:</h3>
								<h4 class="component-text">
									{{ office.office.address.city }},
									{{ office.office.address.street }},
									{{ office.office.address.houseNumber }},
									office {{ office.office.address.officeNumber }}
								</h4>
							</div>
							<div class="office-info">
								<h3 class="component-header">Registry phone number:</h3>
								<h4 class="component-text">{{ office.office.registryPhoneNumber }}</h4>
							</div>
							<div class="office-status">
								@if (office.office.isActive) {
									<div class="status">✅ Active</div>
								} @else {
									<div class="status">❌ Inactive</div>
								}
							</div>
						</div>
						<button class="main-positive-btn" (click)="editOffice()">Edit</button>
					</div>
					<button class="close-btn" (click)="close()">×</button>
				}
			</div>
	`,
	styleUrl: `./office-card.component.css`,
	encapsulation: ViewEncapsulation.Emulated
})
export class OfficeCard implements OnDestroy, OnInit {
	public isEditing: boolean = false
	public wasEdited: boolean = false
	photoUrl = signal<SafeUrl>('');
	isReady = signal(false);
	destroyRef = inject(DestroyRef);

	constructor(
			@Inject(DIALOG_DATA) public office:
			{
				office: Office
			},
			private dialogRef: DialogRef<OfficeCard>,
			private router: Router,
			private fileService: FileService,
	)
	{
		effect(() => {
			const url = this.photoUrl();
			if (url !== '') {
				this.isReady.set(true);
			}
		});

		this.dialogRef.closed.subscribe(() =>
		{
			if(this.wasEdited)
			{
				const currentUrl = this.router.url;
				this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
					this.router.navigate([currentUrl]);
				});
			}
		});
	}

	onChanged(data:{ edited: boolean; office: Office | undefined }) {
		if (data.edited) {
			this.office.office = data.office!
			this.wasEdited = true;
			this.ngOnInit();
		}

		this.isEditing = false;
		this.dialogRef.disableClose = false;
	}

	editOffice() {
		this.isEditing=true
		this.dialogRef.disableClose = true;
	}

	close() {
		this.isReady.set(false);
		this.dialogRef.close();
	}

	ngOnInit() {
		this.fileService.getPhoto(this.office.office.photoId)
				.pipe(takeUntilDestroyed(this.destroyRef))
				.subscribe(url => {
					this.photoUrl.set(url);
				});
	}

	ngOnDestroy() {
		this.photoUrl.set('');
		this.isReady.set(false);
	}
}