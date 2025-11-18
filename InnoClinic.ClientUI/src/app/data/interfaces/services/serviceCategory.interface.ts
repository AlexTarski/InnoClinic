import {Entity} from "./entity.interface";
import {Service} from "./service.interface";

export interface ServiceCategory extends Entity {
	officeId: string;
	timeSlotSize: string;
	services?: Service[];
}