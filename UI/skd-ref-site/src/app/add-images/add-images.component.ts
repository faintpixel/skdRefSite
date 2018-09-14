import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-images',
  templateUrl: './add-images.component.html',
  styleUrls: ['./add-images.component.css']
})
export class AddImagesComponent implements OnInit {

  questionsAnswered = false;
  imagesUploaded = false;
  answers: object = null;
  images: Array<object> = [];
  isDirty = false;

  constructor(private router: Router) { }

  ngOnInit() {
  }

  onQuestionsAnswered(answers): void {
    this.answers = answers;
    this.questionsAnswered = true;
  }

  onImagesUploaded(images): void {
    this.imagesUploaded = true;

    for (const image of images) {
      image.classifications = {};
    }

    this.images = images;
    console.log(JSON.stringify(images));
    // TO DO - verify success. try running with service turned off
  }
}
