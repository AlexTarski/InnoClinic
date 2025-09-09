import {Component, Input} from '@angular/core';
import {Office} from "../../data/interfaces/office.interface";

@Component({
  selector: 'app-office-card',
  imports: [],
	templateUrl: `./office-card.component.html`,
	styleUrl: `./office-card.component.css`
})
export class OfficeCard {
	@Input() office?: Office;
}
