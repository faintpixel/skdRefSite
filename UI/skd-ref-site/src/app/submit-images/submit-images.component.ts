import { Component, OnInit } from '@angular/core';
import { ReferenceType } from '../models/referenceType';

@Component({
  selector: 'app-submit-images',
  templateUrl: './submit-images.component.html',
  styleUrls: ['./submit-images.component.css']
})
export class SubmitImagesComponent implements OnInit {

  public firstConfirmed = false;

  constructor() {
  }

  ngOnInit() {
  }

}
