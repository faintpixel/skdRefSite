import { Component, OnInit, Input, SimpleChanges, OnChanges } from '@angular/core';
import { Gender } from '../models/gender';
import { PoseType } from '../models/poseType';
import { ViewAngle } from '../models/viewAngle';
import { ReferenceType } from '../models/referenceType';
import { Species } from '../models/species';
import { AnimalCategory } from '../models/animalCategory';
import { BodyPart } from '../models/bodyPart';
import { ReferenceService } from '../reference.service';
import { environment } from '../../environments/environment';
import { VegetationType } from '../models/vegetationType';
import { VegetationPhotoType } from '../models/vegetationPhotoType';
import { StructureType } from '../models/structureType';

@Component({
  selector: 'app-image-categorizer',
  templateUrl: './image-categorizer.component.html',
  styleUrls: ['./image-categorizer.component.css']
})
export class ImageCategorizerComponent implements OnInit, OnChanges {
  @Input() images;
  @Input() referenceType;

  genders: Array<string>;
  selectedGender: string;

  poseTypes: Array<string>;
  selectedPose: string;

  viewAngles: Array<string>;
  selectedViewAngle: string;

  species: Array<string>;
  selectedSpecies: string;

  animalCategories: Array<string>;
  selectedAnimalCategory: string;

  bodyParts: Array<string>;
  selectedBodyPart: string;

  structureTypes: Array<string>;
  selectedStructureType: string;

  vegetationTypes: Array<string>;
  selectedVegetationType: string;

  vegetationPhotoTypes: Array<string>;
  selectedVegetationPhotoType: string;

  yesNo: Array<string> = ['', 'true', 'false'];
  selectedNSFW: string;
  selectedClothed: string;

  selectedPhotographerName: string;
  selectedPhotographerWebsite: string;
  selectedModelName: string;
  selectedModelWebsite: string;
  selectedTerms: string;

  statuses: Array<string> = ['', 'Active', 'Deleted', 'Pending', 'Rejected'];
  selectedStatus: string;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['referenceType']) {
      this.validateAllImages();
    }
    if (changes['images']) {
      if(this.images == null) {
        this.images = [];
      }
      this.validateAllImages();
    }
  }

  constructor(private referenceService: ReferenceService) {
    this.genders = Object.keys(Gender);
    this.genders.unshift('');

    this.poseTypes = Object.keys(PoseType);
    this.poseTypes.unshift('');

    this.viewAngles = Object.keys(ViewAngle);
    this.viewAngles.unshift('');

    this.species = Object.keys(Species);
    this.species.unshift('');

    this.animalCategories = Object.keys(AnimalCategory);
    this.animalCategories.unshift('');

    this.bodyParts = Object.keys(BodyPart);
    this.bodyParts.unshift('');

    this.vegetationTypes = Object.keys(VegetationType);
    this.vegetationTypes.unshift('');

    this.vegetationPhotoTypes = Object.keys(VegetationPhotoType);
    this.vegetationPhotoTypes.unshift('');

    this.structureTypes = Object.keys(StructureType);
    this.structureTypes.unshift('');
  }

  ngOnInit() {
    for (let i = 0; i < this.images.length; i++) {
      this.images[i].Classifications = {};
      this.images[i].Photographer = {};
      this.images[i].Model = {};
      this.images[i].validationStatus = false;
    }
  }

  selectAll(): void {
    for (const image of this.images) {
      image.selected = true;
    }

    this.selectionChanged();
  }

  selectNone(): void {
    for (const image of this.images) {
      image.selected = false;
    }

    this.selectionChanged();
  }

  selectImage(image): void {
    image.selected = !image.selected;
    this.selectionChanged();
  }

  imageSelected(): boolean {
    for (const image of this.images) {
      if (image.selected) {
        return true;
      }
    }
    return false;
  }

  selectionChanged(): void {
    let gender = null;
    let pose = null;
    let view = null;
    let nsfw = null;
    let clothed = null;
    let bodyPart = null;
    let species = null;
    let structureType = null;
    let vegetationType = null;
    let vegetationPhotoType = null;
    let animalCategory = null;
    let photographerName = null;
    let photographerWebsite = null;
    let modelName = null;
    let modelWebsite = null;
    let terms = null;
    let status = null;

    for (const image of this.images) {
      if (image.selected) {
        gender = this.getCommonValue(gender, image.classifications.gender);
        pose = this.getCommonValue(pose, image.classifications.poseType);
        view = this.getCommonValue(view, image.classifications.viewAngle);
        nsfw = this.getCommonValue(nsfw, image.classifications.nsfw);
        clothed = this.getCommonValue(clothed, image.classifications.clothing);
        bodyPart = this.getCommonValue(bodyPart, image.classifications.bodyPart);
        species = this.getCommonValue(species, image.classifications.species);
        structureType = this.getCommonValue(structureType, image.classifications.structureType);
        vegetationType = this.getCommonValue(vegetationType, image.classifications.vegetationType);
        vegetationPhotoType = this.getCommonValue(vegetationPhotoType, image.classifications.photoType);
        animalCategory = this.getCommonValue(animalCategory, image.classifications.category);
        if (image.photographer !== undefined && image.photographer !== null) {
          photographerName = this.getCommonValue(photographerName, image.photographer.name);
          photographerWebsite = this.getCommonValue(photographerWebsite, image.photographer.webpage);
        }
        if (image.model !== undefined && image.model !== null) {
          modelName = this.getCommonValue(modelName, image.model.name);
          modelWebsite = this.getCommonValue(modelWebsite, image.model.webpage);
        }

        terms = this.getCommonValue(terms, image.termsOfUse);
        status = this.getCommonValue(status, image.status);
      }
    }

    this.selectedGender = gender;
    this.selectedPose = pose;
    this.selectedViewAngle = view;
    this.selectedNSFW = nsfw;
    this.selectedClothed = clothed;
    this.selectedBodyPart = bodyPart;
    this.selectedSpecies = species;
    this.selectedVegetationPhotoType = vegetationPhotoType;
    this.selectedVegetationType = vegetationType;
    this.selectedStructureType = structureType;
    this.selectedAnimalCategory = animalCategory;
    this.selectedPhotographerName = photographerName;
    this.selectedPhotographerWebsite = photographerWebsite;
    this.selectedModelName = modelName;
    this.selectedModelWebsite = modelWebsite;
    this.selectedTerms = terms;
    this.selectedStatus = status;
  }

  getCommonValue(currentValue, newValue): string {
    if (currentValue == null) {
      currentValue = newValue;
    }
    if (currentValue !== newValue || newValue == null || newValue === undefined) {
      currentValue = '';
    }
    return currentValue;
  }

  propertyChanged(value, property): void {
    for (const image of this.images) {
      if (image.selected) {
        image.classifications[property] = value;
        this.updateImageValidationStatus(image);
      }
    }
  }

  validateAllImages() {
    if (this.images !== null) {
      for (const image of this.images) {
        this.updateImageValidationStatus(image);
      }
    }
  }

  termsChanged(value, property): void {
    for (const image of this.images) {
      if (image.selected) {
        image.termsOfUse = value;
        this.updateImageValidationStatus(image);
      }
    }
  }

  statusChanged(): void {
    for (const image of this.images) {
      if (image.selected) {
        image.status = this.selectedStatus;
        this.updateImageValidationStatus(image);
      }
    }
  }

  contactChanged(value, contact, property): void {
    for (const image of this.images) {
      if (image.selected) {
        image[contact][property] = value;
        this.updateImageValidationStatus(image);
      }
    }
  }

  selectedImageCount(): Number {
    let count = 0;
    for (const image of this.images) {
      if (image.selected) {
        count++;
      }
    }
    return count;
  }

  updateImageValidationStatus(image): void {
    let status = true;

    if (image.photographer === undefined || image.photographer.name === undefined) {
      status = false;
    }
    if (image.photographer.name == null || image.photographer.name.length < 1) {
      status = false;
    }

    if (image.status === '') {
      status = false;
    }

    if (status !== false) {
      if (ReferenceType[this.referenceType] === ReferenceType.Animal) {
        status = this.validateAnimalClassifications(image);
      } else if (ReferenceType[this.referenceType] === ReferenceType.FullBody) {
        status = this.validateFullBodyClassifications(image);
      } else if (ReferenceType[this.referenceType] === ReferenceType.BodyPart) {
        status = this.validateBodyPartClassifications(image);
      } else if (ReferenceType[this.referenceType] === ReferenceType.Vegetation) {
        status = this.validateVegetationClassifications(image);
      } else if (ReferenceType[this.referenceType] === ReferenceType.Structure) {
        status = this.validateStructureClassifications(image);
      }
    }

    image.validationStatus = status;
  }

  validateStructureClassifications(image): boolean {
    // TO DO - is there some typescript/angular magic i could do to validate this instead?
    const requiredFields = ['structureType'];
    for (const field of requiredFields) {
      if (image.classifications[field] === '' || image.classifications[field] === undefined) {
        return false;
      }
    }

    return true;
  }

  validateVegetationClassifications(image): boolean {
    // TO DO - is there some typescript/angular magic i could do to validate this instead?
    const requiredFields = ['vegetationType', 'photoType'];
    for (const field of requiredFields) {
      if (image.classifications[field] === '' || image.classifications[field] === undefined) {
        return false;
      }
    }

    return true;
  }

  validateAnimalClassifications(image): boolean {
    // TO DO - is there some typescript/angular magic i could do to validate this instead?
    const requiredFields = ['species', 'category', 'viewAngle'];
    for (const field of requiredFields) {
      if (image.classifications[field] === '' || image.classifications[field] === undefined) {
        return false;
      }
    }

    return true;
  }

  validateFullBodyClassifications(image): boolean {
    // TO DO - is there some typescript/angular magic i could do to validate this instead?
    const requiredFields = ['nsfw', 'clothing', 'gender', 'poseType', 'viewAngle'];
    for (const field of requiredFields) {
      if (image.classifications[field] === '' || image.classifications[field] === undefined) {
        return false;
      }
    }

    return true;
  }

  validateBodyPartClassifications(image): boolean {
    // TO DO - is there some typescript/angular magic i could do to validate this instead?
    const requiredFields = ['bodyPart', 'gender', 'viewAngle'];
    for (const field of requiredFields) {
      if (image.classifications[field] === '' || image.classifications[field] === undefined) {
        return false;
      }
    }

    return true;
  }

  allReady(): boolean {
    for (const image of this.images) {
      if (image.validationStatus !== true) {
        return false;
      }
    }

    return true;
  }

  performSearch(filters) {
    const refType = this.getRefType();
    this.referenceService.searchReference(refType, filters)
      .subscribe(x => this.images = x);
  }

  getRefType() {
    let refType = '';

    if (ReferenceType[this.referenceType] === ReferenceType.Animal) {
      refType = 'Animals';
    } else if (ReferenceType[this.referenceType] === ReferenceType.FullBody) {
      refType = 'FullBodies';
    } else if (ReferenceType[this.referenceType] === ReferenceType.BodyPart) {
      refType = 'BodyParts';
    } else if (ReferenceType[this.referenceType] === ReferenceType.Vegetation) {
      refType = 'Vegetation';
    } else if (ReferenceType[this.referenceType] === ReferenceType.Structure) {
      refType = 'Structure';
    }

    return refType;
  }

  save(): void {
    const refType = this.getRefType();

    this.referenceService.updateReference(refType, this.images).subscribe(result => {
      if (result) {
        alert('success!');
      } else {
        alert('error saving.');
      }
    });

  }

}
