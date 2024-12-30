import { Component, OnInit, ViewChild, OnDestroy, ElementRef } from '@angular/core';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { SessionService } from '../session.service';
import { ReferenceService } from '../reference.service';
import { Time } from '../models/time';
import { Router, ActivatedRoute } from '@angular/router';
import {
  trigger,
  state,
  style,
  animate,
  transition
} from '@angular/animations';
import { ClassService } from '../class.service';
import { environment } from '../../environments/environment';
import { TranslateService } from '@ngx-translate/core';
import { LanguageService } from '../language.service';
@Component({
  selector: 'app-image-viewer',
  templateUrl: './image-viewer.component.html',
  styleUrls: ['./image-viewer.component.css'],
  animations: [
    trigger('fadeOut', [
      state('0', style({
        opacity: 1
      })),
      state('1', style({
        opacity: 0
      })),
      transition('0 => 1', animate('5000ms ease-in')),
      transition('1 => 0', animate('500ms ease-in'))
    ])
  ]
})
export class ImageViewerComponent implements OnInit, OnDestroy {

  image: any = {};
  filters: any = {};
  referenceType: string;
  time: Time = { minutes: 0, seconds: 10 };
  timer: any;
  paused = true;
  previousImages: Array<string> = [];
  fadeOut = false;
  params: any;
  classInfo: any = null;
  classIndex: number;
  break: boolean;
  loadingImage = false;
  imageUrl: string = null;
  nextImageUrl: string = null;
  reporting = false;
  reportType = '4';
  comment = '';
  imageUrls = [];
  disableNavigation = false;
  selectingPics = false;
  selectedFiles: File[] = [];
  userPics: Array<any> = [];
  userPicIndex = 0;
  shufflePictures = true;
  gridState = 0; // 0: No grid, 1: Black grid, 2: White grid

  @ViewChild('classComplete') private classCompleteModal;
  @ViewChild('imageContainer') imageContainer: ElementRef;

  constructor(
    private modalService: NgbModal,
    private referenceService: ReferenceService,
    private sessionService: SessionService,
    private classService: ClassService,
    private router: Router,
    private route: ActivatedRoute,
    private translate: TranslateService,
    public languageService: LanguageService
  ) { }

  ngOnInit() {
    this.languageService.updateLanguageFromRoute(this.route);

    this.params = this.route.params.subscribe(params => {
      this.referenceType = params['type'];
      this.sessionService.referenceType = this.referenceType;
    });

    this.route.queryParamMap.subscribe(params => {
      const p: any = params;
      this.filters = p.params;
    });

    if (this.filters.Class) {
      this.classInfo = this.classService.getClass(this.filters.Class);
      for (const record of this.classInfo.breakdown) {
        record.current = 0;
      }
    }

    this.classIndex = 0;
    this.break = false;
    this.setTimer();
    this.sessionService.ClearHistory();

    if (this.referenceType == 'YourPics') {
      this.selectingPics = true;
    } else {
      this.previousImages = this.sessionService.GetPreviousIds();
      this.nextImage(false);
    }    
  }

  ngOnDestroy() {
    if (this.timer != null) {
      clearInterval(this.timer);
    }
  }

  openModal(content) {
    this.paused = true;

    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
      this.paused = false; // clicked the button
    }, (reason) => {
      this.paused = false; // clicked the page
    });
  }

  incrementClass(): void {
    if (!this.filters.Class) {
      return;
    }

    const currentBreakdown = this.classInfo.breakdown[this.classIndex];

    if (currentBreakdown.current < currentBreakdown.count - 1) {
      currentBreakdown.current++;
    } else {
      if (this.classIndex < this.classInfo.breakdown.length - 1) {
        this.classIndex++;
      } else {
        this.openModal(this.classCompleteModal);
      }
    }
  }

  decrementClass(): void {
    if (!this.filters.Class) {
      return;
    }

    if (this.classIndex > 0) {
      this.classIndex--;
    }
  }

  nextImage(incrementClass: boolean = true): void {
    clearInterval(this.timer);
    if (incrementClass !== false) {
      this.incrementClass();
    }

    const nextImage = this.sessionService.NextImage();
    if (nextImage != null) {
      this.showNewImage(nextImage, false);
    } else {
      const previousIds = this.sessionService.GetPreviousIds();

      if (this.referenceType == 'YourPics') {
        this.showNewImage(this.userPics[this.userPicIndex], true);
        this.userPicIndex++;
      } else {
        this.referenceService.getReference(this.referenceType, this.filters, previousIds).subscribe(image => {
          this.showNewImage(image, true);
        });
      }
    }
  }

  previousImage(): void {
    this.decrementClass();

    clearInterval(this.timer);
    const previousImage = this.sessionService.PreviousImage();
    this.showNewImage(previousImage, false);
  }

  imageLoaded() {
    this.setTimer();
    this.fadeOut = false;
    this.paused = false;
    if (this.timer != null) {
      clearInterval(this.timer);
    }
    this.timer = setInterval(() => { this.decrementTime(); }, 1000);
    this.loadingImage = false;
  }

  showNewImage(image: any, addToHistory: boolean) {
    this.loadingImage = true;
    this.imageUrl = this.getUrl(image);
    const previousIds = this.sessionService.GetPreviousIds();
    if (this.referenceType != 'YourPics') {
      this.referenceService.getReference(this.referenceType, this.filters, previousIds).subscribe(i => this.preloadNextImage(i));
    }
    this.image = image;

    this.imageUrls = [ this.imageUrl ]; // workaround to get images to display while they load

    if (addToHistory) {
      this.sessionService.AddToImageHistory(image, this.referenceType);
      this.previousImages = this.sessionService.GetPreviousIds();
    }
  }

  preloadNextImage(image: any) {
    this.sessionService.addPreloadedImage(image, this.referenceType);
    this.nextImageUrl = this.getUrl(image);
  }

  getUrl(image: any) {
    if (image === null || image === undefined) {
      return '';
    } else if (this.referenceType == 'YourPics') {
      return image.file;
    } else {
      return environment.imageUrl + image.file;
    }
  }

  togglePause(): void {
    this.paused = !this.paused;
  }

  toggleBlackAndWhite(): void {
    const element = this.imageContainer.nativeElement;
    element.classList.toggle('blackAndWhite');
  }

  // toggleFlip(): void {
  //   const element = this.imageContainer.nativeElement;
  //   element.classList.toggle('flipHorizontally');
  // }

  toggleGrid(): void {
    const element = this.imageContainer.nativeElement;
    element.classList.remove('gridOverlayDark', 'gridOverlayLight');
    this.gridState = (this.gridState + 1) % 3;
    if (this.gridState === 1) {
      element.classList.add('gridOverlayDark');
    } else if (this.gridState === 2) {
      element.classList.add('gridOverlayLight');
    }
  }

  toggleFlip(): void {
    const element = this.imageContainer.nativeElement;
    if (element.classList.contains('flipHorizontally')) {
      element.classList.remove('flipHorizontally');
      this.updateTransform(element);
    } else {
      element.classList.add('flipHorizontally');
      this.updateTransform(element);
    }
  }

  toggleMirror(): void {
    const element = this.imageContainer.nativeElement;
    if (element.classList.contains('mirror')) {
      element.classList.remove('mirror');
      this.updateTransform(element);
    } else {
      element.classList.add('mirror');
      this.updateTransform(element);
    }
  }

  private updateTransform(element: HTMLElement): void {
    const isFlipped = element.classList.contains('flipHorizontally');
    const isMirrored = element.classList.contains('mirror');
  
    if (isFlipped && isMirrored) {
      element.style.transform = 'scaleX(-1) scaleY(-1)';
    } else if (isFlipped) {
      element.style.transform = 'scaleY(-1)';
    } else if (isMirrored) {
      element.style.transform = 'scaleX(-1)';
    } else {
      element.style.transform = '';
    }
  }

  stop(): void {
    clearInterval(this.timer);
    console.log('stopping');
    this.languageService.redirectToLanguageHome(this.languageService.language);
  }

  keyPressed(e: KeyboardEvent): void {
    if (e.key === 'ArrowLeft') {
      this.previousImage();
    } else if (e.key === 'ArrowRight') {
      this.nextImage();
    } else if (e.key === 'Escape') {
      this.stop();
    } else if (e.key === ' ') {
      this.togglePause();
    } else if (e.key === 'm') {
      this.toggleMirror();
    } else if (e.key === 'f') {
      this.toggleFlip();
    } else if (e.key === 'b') {
      this.toggleBlackAndWhite();
    } else if (e.key === 'g') {
      this.toggleGrid();
    }
  }

  decrementTime(): void {
    if (this.paused) {
      return;
    }

    if (this.time.seconds !== 0) {
      this.time.seconds -= 1;
    } else {
      this.time.minutes -= 1;
      this.time.seconds = 59;
    }

    if (this.time.minutes === 0 && this.time.seconds === 5) {
      this.fadeOut = true;
    } else if (this.time.minutes === 0 && this.time.seconds === 0) {
      this.break = false;
      this.nextImage();
    }
  }

  setTimer(): void {
    let totalSeconds = this.filters.Time;
    if (this.filters.Class) {
      totalSeconds = this.classInfo.breakdown[this.classIndex].key;
      this.break = this.classInfo.breakdown[this.classIndex].type === 'break';
    }
    const minutes = Math.floor(totalSeconds / 60);
    const seconds = totalSeconds - minutes * 60;

    this.time.minutes = minutes;
    this.time.seconds = seconds;
  }

  imageFound() {
    return this.image !== {} && this.image != null;
  }

  shuffle (arr) {
    var j, x, index;
    for (index = arr.length - 1; index > 0; index--) {
        j = Math.floor(Math.random() * (index + 1));
        x = arr[index];
        arr[index] = arr[j];
        arr[j] = x;
    }
    return arr;
  }

  startYourPics() {
    this.userPics = [];

    let i = 0;
    let files = [];
    for (const file of this.selectedFiles) {
      files.push({
        id: i,
        file: URL.createObjectURL(file) 
      });
      i++;
    }

    if (this.shufflePictures) {
      this.userPics = this.shuffle(files);
    } else {
      this.userPics = files;
    }

    this.selectingPics = false;
    this.previousImages = this.sessionService.GetPreviousIds();
    this.nextImage(false);
  }

  handleFileInput(event: any) {
    this.selectedFiles = event.target.files;
  }

  report() {
    console.log('here');
    if (this.comment.length === 0) {
      alert('Please enter a comment to describe the issue.');
    } else {
      this.reporting = true;
      this.referenceService.reportImage(this.image.id, this.comment, this.reportType, this.referenceType)
        .subscribe(x => {
          if (x != false) {
            alert('Image has been reported.');
            this.reporting = false;
          } else {
            alert('Error reporting.');
          }                   
        });
    }
  }

}
