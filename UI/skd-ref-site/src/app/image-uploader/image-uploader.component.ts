import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FileUploader, FileItem, ParsedResponseHeaders } from 'ng2-file-upload';
import { environment } from '../../environments/environment';

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

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  public fileOverAnother(e: any): void {
    this.hasAnotherDropZoneOver = e;
  }

  constructor() {
    this.uploader = new FileUploader({ url: environment.baseUrl + 'Image' });
    this.uploader.onCompleteAll = () => {
      console.log('files have been uploaded.');
      this.imagesUploaded.emit(this.uploadedFiles);
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => this.onSuccessItem(item, response, status, headers);

    const authHeader: Array<any> = [];
    authHeader.push({ name: 'Authorization', value: 'Bearer ' + localStorage.getItem('id_token') });
    const uploadOptions = {
      headers: authHeader
    };
    this.uploader.setOptions(uploadOptions);
  }

  onSuccessItem(item: FileItem, response: string, status: number, headers: ParsedResponseHeaders): any {
    const data = JSON.parse(response); // success server response
    this.uploadedFiles = this.uploadedFiles.concat(data);
  }

  ngOnInit() {
    this.uploader.onBuildItemForm = (fileItem: any, form: any) => {
      form.append('batch', JSON.stringify(this.batchInfo));
    };
  }

}
