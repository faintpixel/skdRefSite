import { Injectable } from '@angular/core';
import { Message } from './models/message';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  messages: Message[] = [];

  public add(message: Message) {
    this.messages.push(message);
    setTimeout(() => {
      this.messages.shift();
    }, 5000);
  }

  public clear() {
    this.messages = [];
  }
}
