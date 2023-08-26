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
    limit: 30,
    searchType: 'Animal'
  };
  statuses: Array<string> = ['', 'Active', 'Deleted', 'Pending', 'Rejected'];
  referenceTypes = [];
  uploadDateStartPicker: any;
  uploadDateEndPicker: any;

  constructor() {
    this.referenceTypes = Object.keys(ReferenceType);
  }

  ngOnInit() {
  }

  search() {
    const uploadStartDate = this.setDatePickerFilter(this.uploadDateStartPicker);
    const uploadEndDate = this.setDatePickerFilter(this.uploadDateEndPicker);

    if (uploadStartDate) {
      this.filters.uploadDateStart = this.setDatePickerFilter(this.uploadDateStartPicker);
    }
    if (uploadEndDate) {
      this.filters.uploadDateEnd = this.setDatePickerFilter(this.uploadDateEndPicker);
    }
    
    this.searchEvent.emit(this.filters);
  }

  setDatePickerFilter(datePickerValue) {
    if (datePickerValue == null) {
      return null;
    } else {
      return new Date(datePickerValue.year, datePickerValue.month - 1, datePickerValue.day).toISOString();
    }
  }
}
