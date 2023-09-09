import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Gender } from '../models/gender';
import { PoseType } from '../models/poseType';
import { ViewAngle } from '../models/viewAngle';
import { ReferenceType } from '../models/referenceType';
import { Species } from '../models/species';
import { AnimalCategory } from '../models/animalCategory';
import { BodyPart } from '../models/bodyPart';
import { Clothing } from '../models/clothing';
import { ReferenceService } from '../reference.service';
import { SessionService } from '../session.service';
import { Router } from '@angular/router';
import { ClassService } from '../class.service';
import { LanguageService } from '../language.service';
import { StructureType } from '../models/structureType';
import { VegetationPhotoType } from '../models/vegetationPhotoType';
import { VegetationType } from '../models/vegetationType';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-reference-filters',
  templateUrl: './reference-filters.component.html',
  styleUrls: ['./reference-filters.component.css']
})
export class ReferenceFiltersComponent implements OnInit {
  @ViewChild('fileInput') fileInput: ElementRef;

  filters: any = {
    FullBodies: {
      Gender: 'All',
      Clothing: 'All',
      PoseType: 'All',
      ViewAngle: 'All'
    },
    BodyParts: {
      BodyPart: 'All',
      ViewAngle: 'All',
      Gender: 'All'
    },
    Animals: {
      Species: 'All',
      Category: 'All',
      ViewAngle: 'All'
    },
    Structures: {
      StructureType: 'All'
    },
    Vegetation: {
      VegetationType: 'All',
      PhotoType: 'All'
    },
    Time: 30,
    Class: 0.5
  };
  genders: Array<string>;
  poseTypes: Array<string>;
  viewAngles: Array<string>;
  clothing: Array<string>;
  bodyParts: Array<string>;
  species: Array<string>;
  animalCategories: Array<string>;
  structureTypes: Array<string>;
  vegetationTypes: Array<string>;
  vegetationPhotoTypes: Array<string>;
  classOptions: Array<any>;
  currentTab = 'FullBodiesTab';

  imageCount?: any = null;
  selectedFiles: File[] = [];
  testImage: any = '';

  constructor(private referenceService: ReferenceService,
    private sessionService: SessionService,
    private classService: ClassService,
    private router: Router,
    private sanitizer: DomSanitizer,
    private languageService: LanguageService) {

    this.genders = Object.keys(Gender);
    this.genders.unshift('All');

    this.poseTypes = Object.keys(PoseType);
    this.poseTypes.unshift('All');

    this.viewAngles = Object.keys(ViewAngle);
    this.viewAngles.unshift('All');

    this.clothing = Object.keys(Clothing);
    this.clothing.unshift('All');

    this.species = Object.keys(Species);
    this.species.unshift('All');

    this.animalCategories = Object.keys(AnimalCategory);
    this.animalCategories.unshift('All');

    this.bodyParts = Object.keys(BodyPart);
    this.bodyParts.unshift('All');

    this.structureTypes = Object.keys(StructureType);
    this.structureTypes.unshift('All');

    this.vegetationPhotoTypes = Object.keys(VegetationPhotoType);
    this.vegetationPhotoTypes.unshift('All');

    this.vegetationTypes = Object.keys(VegetationType);
    this.vegetationTypes.unshift('All');

    this.classOptions = this.classService.getClassOptions();
  }

  ngOnInit() {
    this.updateCount();
  }

  getClassBreakdown() {
    return this.classService.getBreakdown(this.filters.Class);
  }

  updateCount(): void {
    this.imageCount = null;
    if (this.currentTab == 'YourPicsTab') {
      const files: FileList = this.fileInput.nativeElement.files;
      this.imageCount = files.length;
    } else {
      const referenceType = this.currentTab.replace('Tab', '');
      const filters = this.removeUnusedFilters(this.filters[referenceType]);
      filters.recentImagesOnly = this.filters.OnlyMostRecent || false;
      this.referenceService.getReferenceCount(referenceType, filters).subscribe(count => this.imageCount = count);
    }
  }

  start(): void {
    const referenceType = this.currentTab.replace('Tab', '');
    let filter: any = this.removeUnusedFilters(this.filters[referenceType]);
    if (!this.filters.ClassMode) {
      filter.Time = this.filters.Time;
    } else {
      filter.Class = this.filters.Class;
    }

    if (referenceType === 'FullBodies') {
      filter = this.referenceService.fixFullBodyFilters(filter);
    }
    filter.recentImagesOnly = this.filters.OnlyMostRecent || false;
    console.log(this.languageService.language);
    console.log(referenceType);

    if(referenceType === 'YourPics') {

      const userPics = [];

      for (const file of this.selectedFiles) {
        userPics.push(URL.createObjectURL(file));
      }

      localStorage.setItem('yourPics', JSON.stringify(userPics));
    }

    this.router.navigate([this.languageService.language, 'view', referenceType], { queryParams: filter });
  }

  handleFileInput(event: any) {
    this.selectedFiles = event.target.files;

    // for (const file of this.selectedFiles) {
      
    //   this.testImage = URL.createObjectURL(file);
    //   console.log(this.testImage);
    // }

  }

  removeUnusedFilters(filters): any {
    const updatedFilter = {};
    for (const property in filters) {
      if (filters[property] !== 'All') {
        updatedFilter[property] = filters[property];
      }
    }

    return updatedFilter;
  }

  tabChanged(e: any) {
    console.log('tab changed! ' + e.nextId);
    this.currentTab = e.nextId;
    this.updateCount();
  }
}
