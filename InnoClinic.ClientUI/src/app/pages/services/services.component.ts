import {Component, ViewEncapsulation} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SvgIconComponent} from "../../components/svg-icon/svg-icon.component";
import {MatTab, MatTabGroup} from "@angular/material/tabs";
import {ServicesService} from "../../data/services/services.service";
import {Service} from "../../data/interfaces/services/service.interface";
import {ServiceCategory} from "../../data/interfaces/services/serviceCategory.interface";
import {Specialization} from "../../data/interfaces/services/specialization.interface";

@Component({
  selector: 'app-services',
  standalone: true,
	imports: [CommonModule, SvgIconComponent, MatTabGroup, MatTab],
  templateUrl: './services.component.html',
  styleUrl: './services.component.css',
	encapsulation: ViewEncapsulation.Emulated
})
export class ServicesComponent {
	protected selectedTabIndex: unknown;
	services: Service[] = [];
	serviceCategories: ServiceCategory[] = [];
	specializations: Specialization[] = [];

  constructor(private servicesService: ServicesService) {
		this.servicesService.getServices().subscribe(services => {
			this.services = services;
		});

		this.servicesService.getServiceCategories().subscribe(serviceCategories => {
			this.serviceCategories = serviceCategories;
		});

		this.servicesService.getSpecializations().subscribe(services => {
			this.specializations = services;
		});
  }
}