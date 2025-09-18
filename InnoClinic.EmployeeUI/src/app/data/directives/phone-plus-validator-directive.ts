import { Directive, HostListener } from '@angular/core';
import {NgControl} from "@angular/forms";

@Directive({
	selector: '[appPhonePlusValidator]'
})
export class PhonePlusValidatorDirective {
	constructor(private ngControl: NgControl) {}

	@HostListener('input', ['$event'])
	onInput(event: Event) {
		const input = event.target as HTMLInputElement;
		if (input.value && !input.value.startsWith('+')) {
			input.value = '+' + input.value.replace(/^\+*/, '');
		}
	}

	@HostListener('paste', ['$event'])
	onPaste(event: ClipboardEvent) {
		event.preventDefault();
		let text = event.clipboardData?.getData('text') ?? '';
		text = text.replace(/^\+*/, '');
		const newValue = '+' + text;

		this.ngControl.control?.setValue(newValue);
		this.ngControl.control?.markAsTouched();
		this.ngControl.control?.updateValueAndValidity();
	}
}