import {Entity} from "./entity.interface";
import {Service} from "./service.interface";

export interface Specialization extends Entity {
	isActive: boolean;
	services?: Service[];
}