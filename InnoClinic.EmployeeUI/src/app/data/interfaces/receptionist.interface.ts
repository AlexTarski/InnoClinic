import {User} from "./user.interface";

export interface Receptionist extends User {
	officeId: string;
}