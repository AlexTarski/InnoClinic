export class TimeSpan {
	constructor(
			public hours: number = 0,
			public minutes: number = 0,
			public seconds: number = 0,
			public milliseconds: number = 0
	) {}

	get totalMilliseconds(): number {
		return (this.hours * 3600 + this.minutes * 60 + this.seconds) * 1000 + this.milliseconds;
	}

	get totalSeconds(): number {
		return this.totalMilliseconds / 1000;
	}

	get totalMinutes(): number {
		return this.totalSeconds / 60;
	}

	get totalHours(): number {
		return this.totalMinutes / 60;
	}

	// Convert to .NET TimeSpan string format
	toNetString(): string {
		const totalSeconds = Math.floor(this.totalSeconds);
		const hours = Math.floor(totalSeconds / 3600);
		const minutes = Math.floor((totalSeconds % 3600) / 60);
		const seconds = totalSeconds % 60;

		if (this.milliseconds > 0) {
			return `${this.pad(hours)}:${this.pad(minutes)}:${this.pad(seconds)}.${this.pad(this.milliseconds, 3)}`;
		}

		return `${this.pad(hours)}:${this.pad(minutes)}:${this.pad(seconds)}`;
	}

	// Convert to readable string
	toString(): string {
		const parts: string[] = [];
		if (this.hours > 0) parts.push(`${this.hours}h`);
		if (this.minutes > 0) parts.push(`${this.minutes}m`);
		if (this.seconds > 0) parts.push(`${this.seconds}s`);
		if (this.milliseconds > 0) parts.push(`${this.milliseconds}ms`);

		return parts.join(' ') || '0';
	}

	// Format as HH:MM:SS for display
	toTimeString(): string {
		return `${this.pad(this.hours)}:${this.pad(this.minutes)}:${this.pad(this.seconds)}`;
	}

	private pad(num: number, length: number = 2): string {
		return num.toString().padStart(length, '0');
	}

	static fromNetString(timeString: string): TimeSpan {
		if (!timeString) return new TimeSpan();

		// Handle .NET TimeSpan formats:
		// "00:30:00", "01:45:30", "01:45:30.123", "1.02:30:00" (days.hours:minutes:seconds)

		// Check for day format: "1.02:30:00"
		const dayMatch = timeString.match(/^(\d+)\.(\d{2}):(\d{2}):(\d{2})(?:\.(\d+))?$/);
		if (dayMatch) {
			const days = parseInt(dayMatch[1]);
			const hours = parseInt(dayMatch[2]) + (days * 24);
			const minutes = parseInt(dayMatch[3]);
			const seconds = parseInt(dayMatch[4]);
			const milliseconds = dayMatch[5] ? parseInt(dayMatch[5].substring(0, 3)) : 0;

			return new TimeSpan(hours, minutes, seconds, milliseconds);
		}

		// Standard format: "HH:mm:ss" or "HH:mm:ss.fffffff"
		const parts = timeString.split(':');
		if (parts.length >= 2) {
			let hours = parseInt(parts[0]) || 0;
			let minutes = parseInt(parts[1]) || 0;
			let seconds = 0;
			let milliseconds = 0;

			if (parts.length >= 3) {
				const secondParts = parts[2].split('.');
				seconds = parseInt(secondParts[0]) || 0;
				if (secondParts.length > 1) {
					// Handle fractional seconds (milliseconds)
					const frac = secondParts[1].substring(0, 3); // Take up to 3 digits
					milliseconds = parseInt(frac.padEnd(3, '0')) || 0;
				}
			}

			return new TimeSpan(hours, minutes, seconds, milliseconds);
		}

		throw new Error(`Invalid .NET TimeSpan format: ${timeString}`);
	}

	static fromMilliseconds(ms: number): TimeSpan {
		const hours = Math.floor(ms / (1000 * 60 * 60));
		ms -= hours * (1000 * 60 * 60);

		const minutes = Math.floor(ms / (1000 * 60));
		ms -= minutes * (1000 * 60);

		const seconds = Math.floor(ms / 1000);
		ms -= seconds * 1000;

		return new TimeSpan(hours, minutes, seconds, ms);
	}

	static fromMinutes(minutes: number): TimeSpan {
		return TimeSpan.fromMilliseconds(minutes * 60 * 1000);
	}

	static fromHours(hours: number): TimeSpan {
		return TimeSpan.fromMilliseconds(hours * 60 * 60 * 1000);
	}
}