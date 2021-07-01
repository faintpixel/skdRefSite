import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Router, ActivatedRoute } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {

  public languages: Array<any> = [
    { code: 'en', name: 'English' },
    { code: 'ar', name: 'العربية' },
    { code: 'bg', name: 'Български' },
    { code: 'cs', name: 'Česky' },
    { code: 'da', name: 'Dansk' },
    { code: 'de', name: 'Deutsch' },
    { code: 'es', name: 'Español' },
    { code: 'et', name: 'Eesti' },
    { code: 'fa', name: 'فارسی' },
    { code: 'fi', name: 'Suomi' },
    { code: 'fr', name: 'Français' },
    { code: 'id', name: 'Indonesian' },
    { code: 'it', name: 'Italiano' },
    { code: 'iw', name: 'עברית' },
    { code: 'ja', name: '日本語' },
    { code: 'ko', name: '한국말' },
    { code: 'lv', name: 'Latvian' },
    { code: 'hi', name: 'हिन्दी' },
    { code: 'hu', name: 'Magyar' },
    { code: 'mk', name: 'Македонски' },
    { code: 'nl', name: 'Nederlands' },
    { code: 'nn', name: 'Norsk' },
    { code: 'po', name: 'Polski' },
    { code: 'pt', name: 'Português' },
    { code: 'ro', name: 'Romanian' },
    { code: 'ru', name: 'Русский' },
    { code: 'sr', name: 'Српски' },
    { code: 'sv', name: 'Svenska' },
    { code: 'tl', name: 'Tagalog' },
    { code: 'th', name: 'Thai' },
    { code: 'tr', name: 'Türkçe' },
    { code: 'tt', name: 'Татар теле' },
    { code: 'uk', name: 'Українська' },
    { code: 'vi', name: 'Tiếng Việt' },
    { code: 'zh', name: '简体中文' },
    { code: 'zh-hant', name: '繁體中文' }
  ];

  public language: 'en';

  constructor(private translateService: TranslateService, private router: Router) { }

  changeLanguage(language) {
    for (const l of this.languages) {
      if (l.code === language) {
        this.language = language;
        this.translateService.use(language);
      }
    }
  }

  redirectToLanguageHome(language) {
    this.router.navigate(['', language]);
  }

  updateLanguageFromRoute(activeRoute) {
    activeRoute.params.subscribe(routeParams => {
      let language = routeParams.lang;
      console.log('router got language: ' + language);
      if (language === undefined) {
        language = 'en';
      }
      this.changeLanguage(language);
    });
  }

}
