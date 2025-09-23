import { AbstractControl, ValidationErrors } from '@angular/forms';

export function photoTypeValidator(control: AbstractControl): ValidationErrors | null {
	const file = control.value as File | null;
	if (!file) return null; // empty is valid; use required if needed

	// Primary check: MIME type
	const allowedTypes = ['image/jpeg', 'image/png', 'image/gif'];
	if (file.type && allowedTypes.includes(file.type)) return null;

	// Fallback: extension check (handles rare cases where file.type is empty)
	const name = file.name?.toLowerCase() ?? '';
	const allowedExt = /\.(jpe?g|png|gif)$/i;
	if (allowedExt.test(name)) return null;

	return { invalidFormat: true };
}
