import { Component } from '@angular/core';
import { AuthService } from './auth.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor(translate: TranslateService, public auth: AuthService) {
    auth.handleAuthentication();

    const userLang = navigator.language;
    console.log('actual language: ' + userLang);
    console.log(translate.getBrowserLang());

    translate.setDefaultLang('en');

    const userLanguage = translate.getBrowserLang();
    if (userLanguage !== undefined) {
      translate.use(userLanguage);
    } else {
      translate.use('en');
    }
  }
}
