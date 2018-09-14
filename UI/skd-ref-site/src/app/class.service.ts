import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ClassService {

  private classOptions: Array<any> = [
    {
      key: 0.5, value: '30 minutes', breakdown: [
        { type: 'image', key: 30, value: '30 seconds', count: 10 },
        { type: 'image', key: 60, value: '1 minute', count: 5 },
        { type: 'image', key: 300, value: '5 minutes', count: 2 },
        { type: 'image', key: 600, value: '10 minutes', count: 1 }
      ]
    },
    {
      key: 1, value: '1 hour', breakdown: [
        { type: 'image', key: 30, value: '30 seconds', count: 10 },
        { type: 'image', key: 60, value: '1 minute', count: 5 },
        { type: 'image', key: 300, value: '5 minutes', count: 2 },
        { type: 'image', key: 600, value: '10 minutes', count: 1 },
        { type: 'break', key: 300, value: '5 minutes', count: 1 },
        { type: 'image', key: 1500, value: '25 minutes', count: 1 }
      ]
    },
    {
      key: 2, value: '2 hours', breakdown: [
        { type: 'image', key: 30, value: '30 seconds', count: 6 },
        { type: 'image', key: 60, value: '1 minute', count: 3 },
        { type: 'image', key: 300, value: '5 minutes', count: 2 },
        { type: 'image', key: 600, value: '10 minutes', count: 2 },
        { type: 'image', key: 1200, value: '20 minutes', count: 1 },
        { type: 'break', key: 840, value: '14 minutes', count: 1 },
        { type: 'image', key: 3000, value: '50 minutes', count: 1 }
      ]
    },
    {
      key: 3, value: '3 hours', breakdown: [
        { type: 'image', key: 30, value: '30 seconds', count: 10 },
        { type: 'image', key: 60, value: '1 minute', count: 5 },
        { type: 'image', key: 300, value: '5 minutes', count: 2 },
        { type: 'image', key: 600, value: '10 minutes', count: 1 },
        { type: 'image', key: 1200, value: '20 minutes', count: 1 },
        { type: 'break', key: 600, value: '10 minutes', count: 1 },
        { type: 'image', key: 1800, value: '30 minutes', count: 2 },
        { type: 'break', key: 600, value: '10 minutes', count: 1 },
        { type: 'image', key: 3000, value: '50 minutes', count: 1 }
      ]
    },
    {
      key: 6, value: '6 hours', breakdown: [
        { type: 'image', key: 30, value: '30 seconds', count: 10 },
        { type: 'image', key: 60, value: '1 minute', count: 5 },
        { type: 'image', key: 300, value: '5 minutes', count: 2 },
        { type: 'image', key: 600, value: '10 minutes', count: 1 },
        { type: 'image', key: 1200, value: '20 minutes', count: 1 },
        { type: 'break', key: 600, value: '10 minutes', count: 1 },
        { type: 'image', key: 1800, value: '30 minutes', count: 2 },
        { type: 'break', key: 600, value: '10 minutes', count: 1 },
        { type: 'image', key: 3000, value: '50 minutes', count: 1 },
        { type: 'break', key: 2700, value: '45 minutes', count: 1 },
        { type: 'image', key: 30, value: '30 seconds', count: 6 },
        { type: 'image', key: 60, value: '1 minute', count: 4 },
        { type: 'image', key: 300, value: '5 minutes', count: 3 },
        { type: 'break', key: 600, value: '10 minutes', count: 1 },
        { type: 'image', key: 2700, value: '45 minutes', count: 1 },
        { type: 'break', key: 600, value: '10 minutes', count: 1 },
        { type: 'image', key: 6600, value: '1 hour and 50 minutes', count: 1 }
      ]
    }
  ];

  constructor() { }

  getClassOptions(): Array<any> {
    return this.classOptions;
  }

  getBreakdown(key: number): Array<any> {
    for (const c of this.classOptions) {
      if (c.key === key) {
        return c.breakdown;
      }
    }
    return [];
  }

  getClass(key: number): any {
    for (const c of this.classOptions) {
      if (c.key === key) {
        return c;
      }
    }
    return {};
  }
}
