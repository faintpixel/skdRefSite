import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ErrorService } from './error.service';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReferenceService {

  constructor(private http: HttpClient, private errorService: ErrorService) { }

  updateReference(type: string, images: Array<object>) {
    return this.http.post<Array<object>>(environment.baseUrl + type, images, {
      headers: new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('id_token')}`)
    }).pipe(catchError(this.errorService.handleError('Error updating reference.', 'updateReference', false)));
  }

  getReference(type: string, filters: any, excludeIds: Array<string>) {
    return this.http.post<object>(environment.baseUrl + type + '/Next', excludeIds, { params: filters })
      .pipe(catchError(this.errorService.handleError('Error getting reference.', 'getReference', null)));
  }

  searchReference(type: string, filters: any) {
    return this.http.get<Array<any>>(environment.baseUrl + type, { params: filters })
      .pipe(catchError(this.errorService.handleError('Error searching references.', 'searchReference', [])));
  }

  getReferenceCount(type: string, filters: any) {
    if (type === 'FullBodies') {
      filters = this.fixFullBodyFilters(filters);
    }

    return this.http.get<number>(environment.baseUrl + type + '/Count', {
      params: filters
    }).pipe(catchError(this.errorService.handleError('Error getting reference count.', 'getReferenceCount', '?')));
  }

  fixFullBodyFilters(filters) {
    if (filters.Clothing !== undefined) {
      filters.Clothing = filters.Clothing === 'Clothed';
    }
    return filters;
  }

  getBatches() {
    return this.http.get<Array<any>>(environment.baseUrl + 'Batches', {
      headers: new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('id_token')}`)
    }).pipe(catchError(this.errorService.handleError('Error getting batches.', 'getBatches', [])));
  }

  getBatchImages(id: string) {
    return this.http.get<any>(environment.baseUrl + 'Batches/' + id + '/images', {
      headers: new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('id_token')}`)
    }).pipe(catchError(this.errorService.handleError('Error getting batch images.', 'getBatchImages', {})));
  }

  getTranslation(language: string) {
    return this.http.get<any>('/assets/i18n/' + language + '.json')
      .pipe(catchError(this.errorService.handleError('Error getting translation.', 'getTranslation', {})));
  }

  submitTranslation(language: string, author: string, comments: string, translation: string) {
    const body = {
      language: language,
      author: author,
      comments: comments,
      translationFile: translation
    };
    return this.http.post<Array<object>>(environment.baseUrl + 'translations', body)
      .pipe(catchError(this.errorService.handleError('Error updating reference.', 'updateReference', false)));
  }
}
