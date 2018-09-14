import { MessageTypes } from './messageTypes';

export class Message {
 public type: MessageTypes;
 public details: string;

 constructor(type: MessageTypes, details: string) {
     this.type = type;
     this.details = details;
 }
}
