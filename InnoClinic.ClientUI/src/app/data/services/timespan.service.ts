
import { Injectable } from '@angular/core';
import {TimeSpan} from "../models/timespan.model";

@Injectable({
	providedIn: 'root'
})
export class TimeSpanService {

	// Parse .NET TimeSpan string from API
	parseFromApi(timeString: string): TimeSpan {
		return TimeSpan.fromNetString(timeString);
	}

	// Convert to .NET format for sending to API
	toApiFormat(timeSpan: TimeSpan): string {
		return timeSpan.toNetString();
	}

	// Utility methods for common operations
	add(timeSpan1: TimeSpan, timeSpan2: TimeSpan): TimeSpan {
		const totalMs = timeSpan1.totalMilliseconds + timeSpan2.totalMilliseconds;
		return TimeSpan.fromMilliseconds(totalMs);
	}

	subtract(timeSpan1: TimeSpan, timeSpan2: TimeSpan): TimeSpan {
		const totalMs = timeSpan1.totalMilliseconds - timeSpan2.totalMilliseconds;
		return TimeSpan.fromMilliseconds(Math.max(0, totalMs)); // Prevent negative
	}

	compare(timeSpan1: TimeSpan, timeSpan2: TimeSpan): number {
		return timeSpan1.totalMilliseconds - timeSpan2.totalMilliseconds;
	}

	// Create from components
	create(hours: number = 0, minutes: number = 0, seconds: number = 0): TimeSpan {
		return new TimeSpan(hours, minutes, seconds);
	}
}