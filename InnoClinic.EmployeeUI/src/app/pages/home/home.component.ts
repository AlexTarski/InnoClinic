import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-doctors',
    standalone: true,
    imports: [CommonModule],
    template: `
            <div class="content">
                <p>You can schedule your appointments, look through the list of all services we provide and choose a doctor</p>
            </div>
    `,
    styles: [`
        .content {
            flex: 1;
            padding: 20px;
            background: #f8f9fa;
            overflow-y: auto;
            min-height: 400px;
        }

        .content-header p {
            margin: 0;
            color: #2c3e50;
            font-size: 1.2rem;
        }
    `]
})
export class HomeComponent {}