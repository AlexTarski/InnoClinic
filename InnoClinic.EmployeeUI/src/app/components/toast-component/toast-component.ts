import { Component, inject } from '@angular/core';
import {ToastService} from "../../data/services/toast.service";

@Component({
    selector: 'app-toast',
    standalone: true,
    template: `
    @if (svc.toast(); as t) {
      <div class="toast-root" [class]=" 'toast ' + t.type " role="status" aria-live="polite">
        <span class="msg">{{ t.message }}</span>
        <button class="close" (click)="svc.hide()" aria-label="Close notification">×</button>
      </div>
    }
  `,
    styles: [`
    :host { position: fixed; top: 20px; right: 20px; z-index: 10000; }
    .toast-root { animation: fadeIn .18s ease-out; }
    .toast {
      display: inline-flex; align-items: center; gap: .75rem;
      padding: .75rem 1rem; border-radius: 10px;
      box-shadow: 0 6px 24px rgba(0,0,0,.18);
      font-weight: 600;
    }
    .toast.success { background: #e6f4ea; color: #0b3b2e; }
    .toast.error   { background: #fde8e8; color: #7a1e1e; }
    .toast.info    { background: #e8f1fd; color: #173a7a; }
    .toast.warning { background: #fff4e5; color: #7a4f17; }
    .msg { line-height: 1.2; }
    .close { border: none; background: transparent; font-size: 20px; cursor: pointer; color: inherit; }
    @keyframes fadeIn { from { opacity: 0; transform: translateY(-6px); } to { opacity: 1; transform: translateY(0); } }
  `]
})
export class ToastComponent {
    protected svc = inject(ToastService);
}