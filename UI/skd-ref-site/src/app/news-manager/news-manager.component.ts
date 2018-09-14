import { Component, OnInit } from '@angular/core';
import { NewsService } from '../news.service';

@Component({
  selector: 'app-news-manager',
  templateUrl: './news-manager.component.html',
  styleUrls: ['./news-manager.component.css']
})
export class NewsManagerComponent implements OnInit {

  currentDate = new Date();
  news = {
    id: '',
    title: '',
    date: '',
    content: '',
    datePicker: {
      year: this.currentDate.getFullYear(),
      month: this.currentDate.getMonth() + 1,
      day: this.currentDate.getDate()
    }
  };

  saving = false;
  loadingNews = false;
  loadingAnnouncement = false;
  oldNews: Array<object> = [];
  announcement = '';
  currentAnnouncement = '';

  constructor(private newsService: NewsService) { }

  ngOnInit() {
    this.loadingNews = true;
    this.loadingAnnouncement = true;
    this.getNews();
    this.getAnnouncement();
  }

  new() {
    this.news.id = '';
    this.news.title = '';
    this.news.date = '';
    this.news.content = '';
    const now = new Date();
    this.news.datePicker = {
      year: now.getFullYear(),
      month: now.getMonth() + 1,
      day: now.getDate()
    };
  }

  getNews() {
    this.newsService.get().subscribe(news => {
      this.oldNews = news;
      this.loadingNews = false;
    });
  }

  save() {
    this.saving = true;
    this.news.date = new Date(this.news.datePicker.year, this.news.datePicker.month - 1, this.news.datePicker.day).toISOString();
    this.newsService.save(this.news)
      .subscribe(result => {
        if (result) {
          this.getNews();
          this.new();
          alert('saved!');
        } else {
          alert('error saving.');
        }
        this.saving = false;
      });
  }

  setNews(news) {
    const date = new Date(news.date);
    news.datePicker = {
      year: date.getFullYear(),
      month: date.getMonth() + 1,
      day: date.getDate()
    };
    this.news = { ...news };
  }

  saveAnnouncement() {
    this.newsService.saveAnnouncement(this.announcement)
      .subscribe(result => {
        if (result) {
          this.getAnnouncement();
          alert('saved!');
        } else {
          alert('error saving.');
        }
        this.saving = false;
      });
  }

  getAnnouncement() {
    this.newsService.getAnnouncement().subscribe(announcement => {
      this.setAnnouncement(announcement);
      this.loadingAnnouncement = false;
    });
  }

  setAnnouncement(announcement: any) {
    console.log('got it:');
    console.log(announcement);
    this.announcement = announcement.value;
    this.currentAnnouncement = announcement.value;
  }

}
