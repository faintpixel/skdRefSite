import { Component, OnInit, ViewChild } from '@angular/core';
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
export class ImageViewerComponent implements OnInit {

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
  reportType = 4;
  comment = '';

  @ViewChild('classComplete') private classCompleteModal;
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
    this.previousImages = this.sessionService.GetPreviousIds();
    this.nextImage(false);
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
      this.referenceService.getReference(this.referenceType, this.filters, previousIds).subscribe(image => {
        this.showNewImage(image, true);
      });
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
    this.timer = setInterval(() => { this.decrementTime(); }, 1000);
    this.loadingImage = false;
  }

  showNewImage(image: any, addToHistory: boolean) {
    this.loadingImage = true;
    this.imageUrl = this.getUrl(image);
    const previousIds = this.sessionService.GetPreviousIds();
    this.referenceService.getReference(this.referenceType, this.filters, previousIds).subscribe(i => this.preloadNextImage(i));
    this.image = image;
    if (addToHistory) {
      this.sessionService.AddToImageHistory(image);
      this.previousImages = this.sessionService.GetPreviousIds();
    }
  }

  preloadNextImage(image: any) {
    this.sessionService.addPreloadedImage(image);
    this.nextImageUrl = this.getUrl(image);
  }

  getUrl(image: any) {
    if (image === null || image === undefined) {
      return '';
    } else {
      return environment.imageUrl + image.file;
    }
  }

  togglePause(): void {
    this.paused = !this.paused;
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

  report() {
    this.reporting = true;
    this.referenceService.reportImage(this.image.id, this.comment, this.reportType, this.referenceType)
      .subscribe(x => {
        alert('Image has been reported.');
        this.reporting = false;
      });
  }

}
