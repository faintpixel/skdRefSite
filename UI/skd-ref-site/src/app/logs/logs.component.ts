import { Component, OnInit } from '@angular/core';
import { LogsService } from '../logs.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-logs',
  templateUrl: './logs.component.html',
  styleUrls: ['./logs.component.css']
})
export class LogsComponent implements OnInit {

  reportedImages: any = {};
  logs: Array<any> = [];
  viewModalContent = '';

  constructor(private modalService: NgbModal, private logsService: LogsService) { }

  ngOnInit() {
    this.getLogs();
  }

  view(content, text: string) {
    this.viewModalContent = text;
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
      // clicked the button
    }, (reason) => {
      // clicked the page
    });
  }

  getLogs() {
    this.logsService.getLogs().subscribe(logs => { this.logs = logs; this.parseLogs(); });
  }

  parseLogs() {
    console.log('parsing logs');
    const reportedImages = {};
    this.logs.forEach(log => {
      if (log.type === 'Report') {
        const parameters = JSON.parse(log.parameters);
        const imageId = parameters.ImageId;
        if (!reportedImages[imageId]) {
          reportedImages[imageId] = [];
        }
        reportedImages[imageId].push(parameters);
      }
    });
    this.reportedImages = reportedImages;
  }

  objectKeys(obj) {
    return Object.keys(obj);
  }

  delete(id: string) {
    if (window.confirm('Are you sure you want to delete this log?')) {
      this.logsService.deleteLog(id).subscribe(r => {
        this.getLogs();
      });
    }
  }
}
