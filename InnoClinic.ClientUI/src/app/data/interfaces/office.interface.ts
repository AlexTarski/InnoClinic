import {Address} from "./address.interface";

export interface Office {
	id: string | undefined;
	address: Address;
	registryPhoneNumber: string,
	isActive: boolean;
	photoId: string | undefined;
}