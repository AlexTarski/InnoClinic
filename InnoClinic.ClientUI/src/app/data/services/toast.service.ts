import { Injectable, signal } from '@angular/core';

export type ToastType = 'success' | 'error' | 'info' | 'warning';

export interface ToastOptions {
    type?: ToastType;
    duration?: number;
}

@Injectable({ providedIn: 'root' })
export class ToastService {
    readonly toast = signal<{ message: string; type: ToastType } | null>(null);
    private hideTimer: ReturnType<typeof setTimeout> | null = null;

    show(message: string, options: ToastOptions = {}) {
        const type = options.type ?? 'info';
        const duration = options.duration ?? 3000;

        if (this.hideTimer) clearTimeout(this.hideTimer);
        this.toast.set({ message, type });
        this.hideTimer = setTimeout(() => this.hide(), duration);
    }

    hide() {
        if (this.hideTimer) clearTimeout(this.hideTimer);
        this.hideTimer = null;
        this.toast.set(null);
    }
}