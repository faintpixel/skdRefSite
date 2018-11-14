import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient } from '@angular/common/http';

import { AppComponent } from './app.component';

import { FileUploadModule } from 'ng2-file-upload';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { faBackward, faForward, faPause, faPlay, faStop, faInfoCircle, faQuestionCircle } from '@fortawesome/free-solid-svg-icons';

import { AppRoutingModule } from './/app-routing.module';
import { AddImagesComponent } from './add-images/add-images.component';
import { HomeComponent } from './home/home.component';
import { NewsComponent } from './news/news.component';
import { ReferenceFiltersComponent } from './reference-filters/reference-filters.component';
import { LoginComponent } from './login/login.component';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { MessagesComponent } from './messages/messages.component';
import { HeaderComponent } from './header/header.component';
import { WelcomeComponent } from './welcome/welcome.component';
import { BatchEditorComponent } from './batch-editor/batch-editor.component';
import { LogsComponent } from './logs/logs.component';
import {TranslateModule, TranslateLoader} from '@ngx-translate/core';
import {TranslateHttpLoader} from '@ngx-translate/http-loader';
import { BatchListComponent } from './batch-list/batch-list.component';
import { ImageViewerComponent } from './image-viewer/image-viewer.component';
import { ImageUploaderComponent } from './image-uploader/image-uploader.component';
import { ImagePreUploadQuestionsComponent } from './image-pre-upload-questions/image-pre-upload-questions.component';
import { TranslationManagerComponent } from './translation-manager/translation-manager.component';
import { NewsManagerComponent } from './news-manager/news-manager.component';
import { ImageCategorizerComponent } from './image-categorizer/image-categorizer.component';
import { ImageThumbnailComponent } from './image-thumbnail/image-thumbnail.component';
import { ImageSearchComponent } from './image-search/image-search.component';
import { ImageEditorComponent } from './image-editor/image-editor.component';
import { ContributorsComponent } from './contributors/contributors.component';

library.add(faBackward, faForward, faPause, faPlay, faStop, faInfoCircle, faQuestionCircle);

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [
    AppComponent,
    AddImagesComponent,
    HomeComponent,
    NewsComponent,
    ReferenceFiltersComponent,
    LoginComponent,
    AuthCallbackComponent,
    NotFoundComponent,
    MessagesComponent,
    HeaderComponent,
    WelcomeComponent,
    BatchEditorComponent,
    LogsComponent,
    BatchListComponent,
    ImageViewerComponent,
    ImageUploaderComponent,
    ImagePreUploadQuestionsComponent,
    TranslationManagerComponent,
    NewsManagerComponent,
    ImageCategorizerComponent,
    ImageThumbnailComponent,
    ImageSearchComponent,
    ImageEditorComponent,
    ContributorsComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    FileUploadModule,
    AppRoutingModule,
    HttpClientModule,
    FontAwesomeModule,
    NgbModule.forRoot(),
    TranslateModule.forRoot({
      loader: {
          provide: TranslateLoader,
          useFactory: HttpLoaderFactory,
          deps: [HttpClient]
      }
  })
  ],
  providers: [
    { provide: 'canDeactivateAddImages', useValue: checkDirtyState }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

export function checkDirtyState(component: AddImagesComponent) {
  if (component.isDirty) {
    return window.confirm('Are you sure you want to leave this page?');
  } else {
    return true;
  }
}
