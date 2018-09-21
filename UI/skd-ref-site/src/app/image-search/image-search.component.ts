import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-image-search',
  templateUrl: './image-search.component.html',
  styleUrls: ['./image-search.component.css']
})
export class ImageSearchComponent implements OnInit {
  @Output() searchEvent: EventEmitter<any> = new EventEmitter();
  
  filters: any = {};
  
  constructor() { }

  ngOnInit() {
  }

  search() {
    this.searchEvent.emit(this.filters);
  }
}
