import { Component, OnInit, Input } from '@angular/core';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-image-thumbnail',
  templateUrl: './image-thumbnail.component.html',
  styleUrls: ['./image-thumbnail.component.css']
})
export class ImageThumbnailComponent implements OnInit {
  @Input() image;
  @Input() referenceType;
  
  constructor() { }

  ngOnInit() {
  }

  getUrl(image: string) {
    return environment.imageUrl + image;
  }
}
