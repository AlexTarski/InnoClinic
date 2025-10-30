import {Component, Input, ViewEncapsulation} from "@angular/core";
import {CommonModule} from "@angular/common";


@Component({
	selector: 'svg[icon]',
	standalone: true,
	imports: [CommonModule],
	template: `
		<svg:use [attr.href]="href"></svg:use>
	`,
	styles: [`
	`],
	encapsulation: ViewEncapsulation.Emulated
})

export class SvgIconComponent {
	@Input() icon: string = '';

	get href() {
		return `/assets/imgs/icons/${this.icon}.svg#${this.icon}`;
	}
}