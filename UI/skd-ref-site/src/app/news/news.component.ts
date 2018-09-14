import { Component, OnInit } from '@angular/core';
import { NewsService } from '../news.service';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.css']
})
export class NewsComponent implements OnInit {

  news: Array<object> = [];
  announcement = '';

  constructor(private newsService: NewsService) { }

  ngOnInit() {
    this.newsService.get().subscribe(news => this.news = news);
    this.newsService.getAnnouncement().subscribe(announcement => this.announcement = announcement.value);
  }

}
