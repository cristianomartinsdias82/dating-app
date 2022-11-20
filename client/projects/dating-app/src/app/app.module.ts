import { FileUploadModule } from 'ng2-file-upload';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { SharedModule } from './modules/shared.module';

import { EmbedBearerTokenHeaderInterceptor } from './interceptors/embed-bearer-token-header.interceptor';
import { ErrorHandlingInterceptor } from './interceptors/error-handling-interceptor.interceptor';

import { AppComponent } from './app.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { HomeComponent } from './home/home.component';
import { RegistrationComponent } from './registration/registration.component';
import { MembersListComponent } from './members/members-list/members-list.component';
import { MemberDetailsComponent } from './members/member-details/member-details.component';
import { MessagesComponent } from './messages/messages.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { ErrorComponent } from './error/error.component';
import { MembersListItemComponent } from './members/members-list/members-list-item.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { ProgressIndicatorInterceptor } from './interceptors/progress-indicator.interceptor';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { LikesComponent } from './members/likes/likes.component';
import { LikeMembersListComponent } from './members/likes/like-members-list.component';
import { LikeMembersListItemComponent } from './members/likes/like-members-list-item.component';
import { MessagesThreadComponent } from './members/messages-thread/messages-thread.component';
//import { DateInputComponent } from './forms/date-input/date-input.component'; //NOT WORKING!
//import { TextInputComponent } from './forms/text-input/text-input.component'; //NOT WORKING!

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    HomeComponent,
    RegistrationComponent,
    MembersListComponent,
    MemberDetailsComponent,
    MessagesComponent,
    NotFoundComponent,
    ErrorComponent,
    MembersListItemComponent,
    MemberEditComponent,
    PhotoEditorComponent,
    LikesComponent,
    LikeMembersListComponent,
    LikeMembersListItemComponent,
    MessagesThreadComponent
    //DateInputComponent, //NOT WORKING
    //TextInputComponent //NOT WORKING
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    SharedModule,
    FileUploadModule,
    ReactiveFormsModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ProgressIndicatorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: EmbedBearerTokenHeaderInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorHandlingInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
