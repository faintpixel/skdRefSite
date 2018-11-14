import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Router, ActivatedRoute } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {

  public languages: Array<any> = [
    { code: 'en', name: 'English' },
    { code: 'de', name: 'Deutsch' },
    { code: 'es', name: 'Español' },
    { code: 'ko', name: '한국말' },
    { code: 'nl', name: 'Nederlands' },
    { code: 'fr', name: 'Français' },
    { code: 'pt', name: 'Português' },
    { code: 'ru', name: 'Русский' },
    { code: 'sv', name: 'Svenska' }
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
