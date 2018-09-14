import { Component, OnInit } from '@angular/core';
import { ReferenceService } from '../reference.service';
import { LanguageService } from '../language.service';

@Component({
  selector: 'app-translation-manager',
  templateUrl: './translation-manager.component.html',
  styleUrls: ['./translation-manager.component.css']
})
export class TranslationManagerComponent implements OnInit {

  english: any;
  loading = true;
  translation = [];
  selectedLanguage = '';
  languages = [];
  newLanguageName = '';
  author = '';
  comments = '';

  constructor(private referenceService: ReferenceService, public languageService: LanguageService) { }

  ngOnInit() {
    this.languages = this.languageService.languages;
    this.referenceService.getTranslation('en').subscribe(x => {
      const translation = this.translationToArray(x);
      this.english = translation;
      this.loading = false;
    });
  }

  submit() {
    const data = {};

    for (const i of Object.keys(this.translation)) {
      const property = i.split('.')[0];
      const subProperty = i.split('.')[1];
      if (data[property] === undefined) {
        data[property] = {};
      }
      data[property][subProperty] = this.translation[i];
    }

    if (confirm('Are you sure you want to submit this translation?')) {
      this.loading = true;
      console.log(data);

      let language = '';
      if (this.selectedLanguage === '') {
        language = this.newLanguageName;
      } else {
        language = this.selectedLanguage;
      }

      this.referenceService.submitTranslation(language, this.author, this.comments, JSON.stringify(data)).subscribe(x => {
        this.loading = false;
        alert('Thanks! Your translation has been submitted and will be reviewed soon!');
      });

    }


  }

  changeLanguage() {
    if (this.selectedLanguage === '') {
      this.translation = [];
      return;
    }
    this.loading = true;

    this.referenceService.getTranslation(this.selectedLanguage).subscribe(x => {
      const translation = this.translationToArray(x);

      this.translation = [];
      for (const i of translation) {
        this.translation[i.property + '.' + i.subProperty] = i.value;
      }
      this.loading = false;
    });
  }

  private translationToArray(translation) {
    const result = [];
    for (const property of Object.keys(translation)) {
      for (const subProperty of Object.keys(translation[property])) {
        result.push({
          property: property,
          subProperty: subProperty,
          value: translation[property][subProperty]
        });
      }
    }

    return result;
  }

}
