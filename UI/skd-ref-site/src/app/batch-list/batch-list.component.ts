import { Component, OnInit } from '@angular/core';
import { ReferenceService } from '../reference.service';

@Component({
  selector: 'app-batch-list',
  templateUrl: './batch-list.component.html',
  styleUrls: ['./batch-list.component.css']
})
export class BatchListComponent implements OnInit {

  batches: Array<any> = [];

  constructor(private referenceService: ReferenceService) { }

  ngOnInit() {
    this.referenceService.getBatches().subscribe(batches => this.batches = batches);
  }
}
