import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ReferenceType } from '../models/referenceType';

@Component({
  selector: 'app-image-pre-upload-questions',
  templateUrl: './image-pre-upload-questions.component.html',
  styleUrls: ['./image-pre-upload-questions.component.css']
})
export class ImagePreUploadQuestionsComponent implements OnInit {
  @Output() questionsAnsweredSuccessfully = new EventEmitter<object>();

  answers = {
    source: '',
    permission: '',
    licensed: '',
    from: '',
    name: '',
    type: ''
  };
  referenceTypes: Array<string>;

  constructor() {
    this.referenceTypes = Object.keys(ReferenceType);
    this.referenceTypes.unshift('');
  }

  ngOnInit() {
  }

  PermissionGranted(): boolean {
    if (this.answers.source === 'photographer') {
      return this.answers.permission === 'yes';
    } else {
      return this.answers.licensed === 'yes';
    }
  }

  AllQuestionsAnswered(): boolean {
    if (this.answers.source === 'photographer' && this.answers.permission !== '') {
      return true;
    } else if (this.answers.source !== 'photographer' && this.answers.licensed !== '' && this.answers.from !== '') {
      return true;
    } else {
      return false;
    }
  }

  Completed(): void {
    this.questionsAnsweredSuccessfully.emit(this.answers);
  }

}
