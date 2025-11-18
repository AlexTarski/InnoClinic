import {Component, computed, inject, signal, ViewEncapsulation} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SvgIconComponent} from "../../components/svg-icon/svg-icon.component";
import {MatTab, MatTabGroup} from "@angular/material/tabs";
import {ServicesService} from "../../data/services/services.service";
import {Service} from "../../data/interfaces/services/service.interface";
import {Specialization} from "../../data/interfaces/services/specialization.interface";
import {ServiceCategoryTypeService} from "../../data/services/serviceCategoryType.service";
import {ServiceCategoryType} from "../../data/config/serviceCategoryType";
import {toSignal} from "@angular/core/rxjs-interop";
import {ServiceCard} from "../../components/service-card/service-card";

@Component({
  selector: 'app-services',
  standalone: true,
	imports: [CommonModule, SvgIconComponent, MatTabGroup, MatTab, ServiceCard],
  templateUrl: './services.component.html',
  styleUrl: './services.component.css',
	encapsulation: ViewEncapsulation.Emulated
})
export class ServicesComponent {
	private servicesService = inject(ServicesService);
	private serviceCategoryTypeService = inject(ServiceCategoryTypeService);
	protected selectedTabIndex: unknown;
	protected services = toSignal(this.servicesService.getServices(), { initialValue: [] });
	protected specializations = toSignal(this.servicesService.getSpecializations(), { initialValue: [] });
	protected serviceCategories = signal<Record<ServiceCategoryType, string>>({
		Analyses: "",
		Consultations: "",
		Diagnostics: ""
	});
	protected filteredSpecializations = computed(() => {
		const services = this.services();
		const specializations = this.specializations();
		const categories = this.serviceCategories();

		return specializations.filter(specialization => {
			return services.some(service =>
					service.specializationId === specialization.id &&
					service.categoryId === categories.Consultations
			);
		});
	});

  constructor() {
		this.servicesService.getServiceCategories().subscribe(serviceCategories => {
			this.serviceCategories.set(this.serviceCategoryTypeService.createServiceCategoriesRecord(serviceCategories));
		});
  }

	protected readonly ServiceCategoryType = ServiceCategoryType;
}