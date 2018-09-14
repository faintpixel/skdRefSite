import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { ErrorService } from './error.service';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NewsService {

  constructor(private http: HttpClient, private errorService: ErrorService) { }

  save(news: object): Observable<boolean> {
    return this.http.post<boolean>(environment.baseUrl + 'news', news, {
      headers: new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('id_token')}`)
    }).pipe(catchError(this.errorService.handleError('Error saving news.', 'save', false)));
  }

  get(): Observable<object[]> {
    return this.http.get<Array<object>>(environment.baseUrl + 'news')
      .pipe(catchError(this.errorService.handleError('Error getting news.', 'get', [])));
  }

  saveAnnouncement(announcement: string): Observable<boolean> {
    const obj = { value: announcement };
    return this.http.post<boolean>(environment.baseUrl + 'announcement', obj, {
      headers: new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('id_token')}`)
    }).pipe(catchError(this.errorService.handleError('Error saving announcement.', 'saveAnnouncement', false)));
  }

  getAnnouncement(): Observable<any> {
    return this.http.get<object>(environment.baseUrl + 'announcement')
      .pipe(catchError(this.errorService.handleError('Error getting announcements.', 'getAnnouncement', null)));
  }
}
