import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ReferenceService } from '../reference.service';

@Component({
  selector: 'app-batch-editor',
  templateUrl: './batch-editor.component.html',
  styleUrls: ['./batch-editor.component.css']
})
export class BatchEditorComponent implements OnInit {

  params: any;
  batchId: string;
  images: Array<any> = [];
  referenceType: string;

  constructor(private router: Router,
    private route: ActivatedRoute,
    private referenceService: ReferenceService
  ) { }

  ngOnInit() {
    this.params = this.route.params.subscribe(params => {
      this.batchId = params['id'];
      this.getImages();
    });
  }

  getImages() {
    this.referenceService.getBatchImages(this.batchId).subscribe(bi => {
      if (bi !== {} && bi != null) {
        this.images = bi.images;
      }
      this.referenceType = bi.batch.type;
    });
  }

}
