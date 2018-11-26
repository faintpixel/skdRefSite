import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SessionService {

  constructor() { }

  private imageHistory: Array<any> = [];
  private imageIds: Array<string> = [];
  private historyIndex = -1;
  public referenceType = '';

  AddToImageHistory(image: any, type: string) {
    if (image == null || image === {}) {
      return;
    }

    if (type !== this.referenceType) {
      console.log('invalid reference');
    }

    if (this.imageHistory.length > 100) {
      this.imageHistory.shift();
      this.imageIds.shift();
    }
    this.imageHistory.push(image);
    this.imageIds.push(image.id);
  }

  addPreloadedImage(image: any, type: string): any {
    if (image == null || image === {}) {
      return;
    }

    if (type !== this.referenceType) {
      console.log('invalid reference2');
    }

    if (!this.imageIds.includes(image.id)) {
      this.AddToImageHistory(image, type);
    }
  }

  PreviousImage(): any {
    if (this.historyIndex > 0) {
      this.historyIndex--;
    }
    return this.imageHistory[this.historyIndex];
  }

  NextImage(): any {
    if (this.historyIndex < this.imageHistory.length) {
      this.historyIndex++;
      return this.imageHistory[this.historyIndex];
    } else {
      return null;
    }
  }

  ClearHistory(): void {
    this.historyIndex = -1;
    this.imageHistory = [];
    this.imageIds = [];
  }

  GetPreviousIds(): Array<string> {
    return this.imageIds;
  }
}
