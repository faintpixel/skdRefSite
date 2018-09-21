import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-image-editor',
  templateUrl: './image-editor.component.html',
  styleUrls: ['./image-editor.component.css']
})
export class ImageEditorComponent implements OnInit {

  images = [];
  referenceType = '';

  constructor() { }

  ngOnInit() {
  }

}
