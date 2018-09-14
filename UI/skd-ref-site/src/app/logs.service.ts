import { Injectable } from '@angular/core';
import { ErrorService } from './error.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../environments/environment';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LogsService {

  constructor(private http: HttpClient, private errorService: ErrorService) { }

  getLogs() {
    return this.http.get<Array<any>>(environment.baseUrl + 'logs', {
      headers: new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('id_token')}`)
    }).pipe(catchError(this.errorService.handleError('Error getting logs.', 'getLogs', [] )));
  }

  getLogCount() {
    return this.http.get<number>(environment.baseUrl + 'logs/count', {
      headers: new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('id_token')}`)
    }).pipe(catchError(this.errorService.handleError('Error getting log count.', 'getLogCount', null )));
  }

  deleteLog(id: string) {
    return this.http.delete<boolean>(environment.baseUrl + 'logs/' + id, {
      headers: new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('id_token')}`)
    }).pipe(catchError(this.errorService.handleError('Error deleting log.', 'deleteLog', [] )));
  }
}
