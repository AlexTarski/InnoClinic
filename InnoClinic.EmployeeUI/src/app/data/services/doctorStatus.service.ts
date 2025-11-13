import { Injectable } from '@angular/core';
import {DoctorStatus} from "../enums/doctorStatus";

@Injectable({ providedIn: 'root' })
export class DoctorStatusService {
	DoctorStatusLabel: Record<DoctorStatus, string> = {
		[DoctorStatus.AtWork]: "At work",
		[DoctorStatus.OnVacation]: "On vacation",
		[DoctorStatus.SickDay]: "Sick Day",
		[DoctorStatus.SickLeave]: "Sick Leave",
		[DoctorStatus.SelfIsolation]: "Self-isolation",
		[DoctorStatus.LeaveWithoutPay]: "Leave without pay",
		[DoctorStatus.Inactive]: "Inactive"
	};

	DoctorStatuses = Object.values(DoctorStatus)
			.filter(v => typeof v === 'number') as DoctorStatus[];

	constructor() {
	}

	getDoctorStatus(doctorStatus: DoctorStatus) {
		return this.DoctorStatusLabel[doctorStatus];
	}
}