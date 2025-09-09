import {Address} from "./address.interface";

export interface Office {
	id: string;
	address: Address;
	registryPhoneNumber: string,
	isActive: boolean;
	avatar: string | null
}