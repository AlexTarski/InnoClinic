import {User} from "./user.interface";

export interface Doctor extends User {
	dateOfBirth: Date;
	specializationId: string;
	officeId: string;
	careerStartYear: number;
}