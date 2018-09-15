import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LanguageService } from '../language.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  constructor(private activeRoute: ActivatedRoute, private languageService: LanguageService) {
  }

  ngOnInit() {
    this.languageService.updateLanguageFromRoute(this.activeRoute);
  }

}
