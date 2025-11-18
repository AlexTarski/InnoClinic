import {Entity} from "./entity.interface";
import {Service} from "./service.interface";

export interface ServiceCategory extends Entity {
	timeSlotSize: string;
	services?: Service[];
}