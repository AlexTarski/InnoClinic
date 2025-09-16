import { Directive, HostListener } from '@angular/core';

@Directive({
	selector: '[appPhonePlusValidator]'
})
export class PhonePlusValidatorDirective {

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
		(event.target as HTMLInputElement).value = '+' + text;
	}
}