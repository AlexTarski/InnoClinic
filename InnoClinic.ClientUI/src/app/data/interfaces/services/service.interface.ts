import {Entity} from "./entity.interface";

export interface Service extends Entity {
	dateOfBirth: Date;
	categoryId: string;
	specializationId: string;
	price: number;
	isActive: boolean;
}