import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LanguageService } from '../language.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  selectedLanguage = 'en';

  constructor(public languageService: LanguageService) { }

  ngOnInit() {
    this.selectedLanguage = this.languageService.language;
  }

  changeLanguage() {
    this.languageService.redirectToLanguageHome(this.selectedLanguage);
  }
}
