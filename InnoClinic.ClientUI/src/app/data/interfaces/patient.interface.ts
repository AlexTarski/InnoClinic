import {User} from "./user.interface";

export interface Patient extends User {
	dateOfBirth: Date;
	isLinkedToAccount: boolean;
}