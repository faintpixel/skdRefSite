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
  limit = 3;
  offset = 0;
  loading = true;

  constructor(private newsService: NewsService) { }

  ngOnInit() {
    this.updateNews();
    this.newsService.getAnnouncement().subscribe(announcement => this.announcement = announcement.value);
  }

  updateNews() {
    this.loading = true;
    this.newsService.get(this.limit, this.offset).subscribe(news => {
      this.news = news;
      this.loading = false;
    });
  }

  showOlderNews() {
    if (this.news.length === this.limit && !this.loading) {
      this.offset += this.limit;
      this.updateNews();
    }
  }

  showNewerNews() {
    if (this.offset > 0 && !this.loading) {
      this.offset -= this.limit;
      if (this.offset < 0) {
        this.offset = 0;
      }
      this.updateNews();
    }
  }

}
