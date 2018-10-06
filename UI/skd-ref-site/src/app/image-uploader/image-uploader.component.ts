import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FileUploader, FileItem, ParsedResponseHeaders } from 'ng2-file-upload';
import { environment } from '../../environments/environment';
import { ReferenceService } from '../reference.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-image-uploader',
  templateUrl: './image-uploader.component.html',
  styleUrls: ['./image-uploader.component.css']
})
export class ImageUploaderComponent implements OnInit {
  @Input() batchInfo;
  @Output() imagesUploaded = new EventEmitter<object>();

  public uploader: FileUploader;
  public hasBaseDropZoneOver = false;
  public hasAnotherDropZoneOver = false;
  public uploadedFiles: Array<object> = [];
  public failedFiles = [];
  public loading = false;

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  public fileOverAnother(e: any): void {
    this.hasAnotherDropZoneOver = e;
  }

  constructor(private referenceService: ReferenceService, private router: Router) {
    this.uploader = new FileUploader({ 
      url: environment.baseUrl + 'Image',
      maxFileSize: 5*1024*1024 // 5 MB
    });
    this.uploader.onCompleteAll = () => this.allFilesCompleted();

    this.uploader.onSuccessItem = (item, response, status, headers) => this.onSuccessItem(item, response, status, headers);
    this.uploader.onErrorItem = (item, response, status, headers) => this.onErrorItem(item, response, status, headers);

    const authHeader: Array<any> = [];
    authHeader.push({ name: 'Authorization', value: 'Bearer ' + localStorage.getItem('id_token') });
    const uploadOptions = {
      headers: authHeader
    };
    this.uploader.setOptions(uploadOptions);
  }

  allFilesCompleted() {
    console.log('files have been uploaded.');
    if(this.failedFiles.length > 0 && this.uploadedFiles.length === 0) {
      alert('File upload failed. Please try again later.');
    } else if(this.uploadedFiles.length > 0 && this.failedFiles.length > 0) {
      this.handlePartialSuccess();      
    } else  {
      this.imagesUploaded.emit(this.uploadedFiles);
    }   
    
    this.uploader.clearQueue();
  }

  handlePartialSuccess() {
    if(window.confirm(this.failedFiles.length + ' of ' + this.uploadedFiles.length + ' files failed to upload. Continue?')) {
      this.imagesUploaded.emit(this.uploadedFiles);
    } else {
      this.loading = true;
      this.referenceService.deleteBatch(this.batchInfo.id)
        .subscribe(x => {
          this.loading = false;
          this.router.navigate(['']);
        });
    }
  }

  onSuccessItem(item: FileItem, response: string, status: number, headers: ParsedResponseHeaders): any {
    const data = JSON.parse(response); // success server response
    if (data.success) {
      this.uploadedFiles = this.uploadedFiles.concat(data.images);
    } else {
      this.failedFiles.push(item.file.name);
    }    
    if (data.batchId) {
      this.batchInfo.id = data.batchId;
    }
  }

  onErrorItem(item: FileItem, response: string, status: number, headers: ParsedResponseHeaders): any {
    this.failedFiles.push(item.file.name);
  }

  ngOnInit() {
    this.uploader.onBuildItemForm = (fileItem: any, form: any) => {
      form.append('batch', JSON.stringify(this.batchInfo));
    };
  }

}
