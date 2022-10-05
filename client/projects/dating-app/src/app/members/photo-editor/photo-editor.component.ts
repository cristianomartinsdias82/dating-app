import { Photo } from './../../models/photo';
import { MembersService } from './../../services/members.service';
import { ToastrService } from 'ngx-toastr';
import { User } from './../../models/user';
import { AccountService } from './../../services/account.service';
import { environment } from './../../../environments/environment.prod';
import { Member } from './../../models/member';
import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs';

@Component({
  selector: 'dta-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.scss']
})
export class PhotoEditorComponent implements OnInit {

  @Input() member: Member;
  user: User;

  uploader:FileUploader;
  hasBaseDropZoneOver:boolean;

  constructor (
    private accountService: AccountService,
    private toastrService: ToastrService,
    private membersService: MembersService) {
    this.accountService
        .currentUser$
        .pipe(take(1))
        .subscribe(u => this.user = u);
  }

  ngOnInit() {
    this.initializePhotoUploader();
  }

  public fileOverBase(e:any):void {
    this.hasBaseDropZoneOver = e;
  }

  initializePhotoUploader() {
    this.uploader = new FileUploader({

      url: `${environment.baseUrl}users/add-photo`,
      authToken: `Bearer ${this.user.token}`,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
      //Not setting this to false requires extra configurations
      //in the back-end concerned to CORS and allow credentials to go with the request
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {

      if (!response)
        return;

      if (!this.member.photos || this.member.photos.length === 0) {

        const currentUser = this.accountService.getLoggedUser();

        currentUser.photoUrl = JSON.parse(response)['url'];

        this.accountService.setCurrentUser(currentUser);
      }

      this.member.photos.push(JSON.parse(response));
    };
  }

  setAsMainPhoto(photo: Photo) {
    this.accountService
        .setMainPhoto(photo)
        .subscribe({
          next: () => {
          this.member.photos.forEach(item => item.isMain = item.id === photo.id);

          this.member.photoUrl = photo.url;

          this.toastrService.success('Main photo set successfully.');
        }
      });
  }

  deletePhoto(photoId: string) {
    this.membersService
        .deletePhoto(this.member.id, photoId)
        .subscribe({
          next: () => {

            this.member.photos.splice(this.member.photos.findIndex(x => x.id === photoId), 1);

            this.toastrService.success('Photo deleted successfully.');
          }
        });
  }
}
