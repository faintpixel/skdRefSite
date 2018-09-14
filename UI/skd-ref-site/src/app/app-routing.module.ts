import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AddImagesComponent } from './add-images/add-images.component';
import { HomeComponent } from './home/home.component';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { ScopeGuardService as ScopeGuard } from './scope-guard.service';
import { NotFoundComponent } from './not-found/not-found.component';
import { BatchEditorComponent } from './batch-editor/batch-editor.component';
import { LogsComponent } from './logs/logs.component';
import { BatchListComponent } from './batch-list/batch-list.component';
import { ImageViewerComponent } from './image-viewer/image-viewer.component';
import { NewsManagerComponent } from './news-manager/news-manager.component';
import { TranslationManagerComponent } from './translation-manager/translation-manager.component';

const routes: Routes = [
  { path: '', redirectTo: 'en', pathMatch: 'full' },
  { path: 'home', redirectTo: '' },
  { path: 'addImages',
      component: AddImagesComponent, canDeactivate: ['canDeactivateAddImages'], canActivate: [ScopeGuard],
      data: { expectedRoles: ['admin']} },
  { path: 'news', component: NewsManagerComponent, canActivate: [ScopeGuard], data: { expectedRoles: ['admin']} },
  { path: 'editBatch/:id', component: BatchEditorComponent, data: { expectedRoles: ['admin']} },
  { path: 'batches', component: BatchListComponent, data: { expectedRoles: ['admin']} },
  { path: 'logs', component: LogsComponent, canActivate: [ScopeGuard], data: { expectedRoles: ['admin']} },
  { path: 'translate', component: TranslationManagerComponent },
  { path: 'callback', component: AuthCallbackComponent },
  { path: 'en', component: HomeComponent, pathMatch: 'full' },
  { path: ':lang/view/:type', component: ImageViewerComponent },
  { path: ':lang', component: HomeComponent },
  { path: '**',  component: NotFoundComponent },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [ RouterModule ]
})
export class AppRoutingModule { }
