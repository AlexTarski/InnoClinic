import { Injectable } from '@angular/core';
import {ServiceCategory} from "../interfaces/services/serviceCategory.interface";
import {ServiceCategoryType} from "../config/serviceCategoryType";

@Injectable({ providedIn: 'root' })
export class ServiceCategoryTypeService {
	constructor() {
	}

	//ServiceCategory.Name must be equal to ServiceCategoryType
	createServiceCategoriesRecord(serviceCategories: ServiceCategory[]) {
		const record = {} as Record<ServiceCategoryType, string>;

		serviceCategories.forEach(category => {
			if (Object.values(ServiceCategoryType).includes(category.name as ServiceCategoryType)) {
				record[category.name as ServiceCategoryType] = category.id;
			}
		});

		return record;
	}
}