import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ReferenceType } from '../models/referenceType';

@Component({
  selector: 'app-image-search',
  templateUrl: './image-search.component.html',
  styleUrls: ['./image-search.component.css']
})
export class ImageSearchComponent implements OnInit {
  @Output() searchEvent: EventEmitter<any> = new EventEmitter();
  
  filters: any = {
    limit: 10
  };
  statuses: Array<string> = ['', 'Active', 'Deleted', 'Pending', 'Rejected'];
  referenceTypes = [];
  
  constructor() { 
    this.referenceTypes = Object.keys(ReferenceType);
    this.referenceTypes.unshift('');
  }

  ngOnInit() {
  }

  search() {
    this.searchEvent.emit(this.filters);
  }
}
