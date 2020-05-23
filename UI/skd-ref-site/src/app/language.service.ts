import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Router, ActivatedRoute } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {

  public languages: Array<any> = [
    { code: 'en', name: 'English' },
    { code: 'cs', name: 'Česky' },
    { code: 'de', name: 'Deutsch' },
    { code: 'es', name: 'Español' },
    { code: 'fa', name: 'فارسی' },
    { code: 'id', name: 'Indonesian' },
    { code: 'it', name: 'Italiano' },
    { code: 'iw', name: 'עברית' },
    { code: 'ja', name: '日本語' },
    { code: 'ko', name: '한국말' },
    { code: 'hu', name: 'Magyar' },
    { code: 'nl', name: 'Nederlands' },
    { code: 'nn', name: 'Norsk' },
    { code: 'po', name: 'Polski' },
    { code: 'fr', name: 'Français' },
    { code: 'pt', name: 'Português' },
    { code: 'ru', name: 'Русский' },
    { code: 'sv', name: 'Svenska' },
    { code: 'th', name: 'Thai' },
    { code: 'tr', name: 'Türkçe' },
    { code: 'uk', name: 'Українська' },
    { code: 'vi', name: 'Tiếng Việt' },
    { code: 'zh', name: '简体中文' }
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
