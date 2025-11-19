import {Component, Input} from '@angular/core';
import { Service } from "../../data/interfaces/services/service.interface";

@Component({
  selector: 'app-service-card',
  imports: [],
  template: `
    <div class="component-container">
			<div class="component-header">
				{{service.name}}
			</div>
			<div class="component-text">
				Cost: {{service.price}}$
			</div>
		</div>
  `,
  styles: `
		.component-container {
			flex-direction: column !important;
			align-items: flex-start;
			gap: 0 !important;
			margin: 10px 50px !important;
			box-shadow: 0 2px 4px var(--container-shadow-color) !important;
		}

		.component-container:hover {
			transform: translateY(-2px);
			box-shadow: 0 4px 8px var(--container-shadow-color-hover);
		}
	`,
})
export class ServiceCard {
	@Input() service!: Service;
}
