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
  cssModeIcon = 'sun';

  constructor(public languageService: LanguageService) { }

  ngOnInit() {
    this.selectedLanguage = this.languageService.language;
    this.loadCssMode();
  }

  changeLanguage() {
    this.languageService.redirectToLanguageHome(this.selectedLanguage);
  }

  loadCssMode() {
    const lightCSS = document.getElementById('light-css') as HTMLLinkElement;
    if (localStorage.getItem('cssMode') == 'light' && lightCSS.disabled) {
      this.toggleCssMode();
    }
  }

  toggleCssMode() {
    const lightCSS = document.getElementById('light-css') as HTMLLinkElement;
    const darkCSS = document.getElementById('dark-css') as HTMLLinkElement;

    if (lightCSS.disabled) {
      darkCSS.setAttribute('disabled', 'disabled');
      lightCSS.removeAttribute('disabled');
      localStorage.setItem('cssMode', 'light');
      this.cssModeIcon = 'moon';
    } else {
      lightCSS.setAttribute('disabled', 'disabled');
      darkCSS.removeAttribute('disabled');
      localStorage.setItem('cssMode', 'dark');
      this.cssModeIcon = 'sun';
    }    
  }
}
