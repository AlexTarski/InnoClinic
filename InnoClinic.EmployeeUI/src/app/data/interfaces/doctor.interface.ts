export interface Doctor {
    id: string;
    lastName: string;
    firstName: string;
    middleName: string;
    careerStartYear: Date;
		dateOfBirth: Date
    phoneNumber: string;
    avatar: string | null;
		status: string;
}