import {Entity} from "./entity.interface";

export interface Service extends Entity {
	categoryId: string;
	specializationId: string;
	price: number;
	isActive: boolean;
}