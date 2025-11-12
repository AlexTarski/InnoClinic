import {User} from "./user.interface";
import {DoctorStatus} from "../enums/doctorStatus";

export interface Doctor extends User {
	dateOfBirth: Date;
	specializationId: string;
	officeId: string;
	careerStartYear: number;
	status: DoctorStatus;
}