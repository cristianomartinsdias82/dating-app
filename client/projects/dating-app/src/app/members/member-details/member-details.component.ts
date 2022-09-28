import { AccountService } from './../../services/account.service';
import { MembersService } from './../../services/members.service';
import { Member } from './../../models/member';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { ParamValidationService } from '../../services/param-validation.service';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';

@Component({
  selector: 'dta-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.scss']
})
export class MemberDetailsComponent implements OnInit {

  member: Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private paramValidationService: ParamValidationService,
    public membersService: MembersService,
    private accountService: AccountService) { }

  ngOnInit(): void {
    const userName = this.activatedRoute.snapshot.paramMap.get('id');

    this.loadMemberData(userName);
  }

  loadMemberData(userName: string)
  {
    this
      .membersService
      .getMemberByUserName(userName)
      .subscribe(member => {

        if (!member)
          this.router.navigate(['/not-found']);

        this.member = member;

        this.initImageGallery();

      });
  }

  initImageGallery() {
    this.galleryOptions = [
      {
        width: '400px',
        height: '400px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ];

    this.galleryImages = this.getGalleryImages();
  }

  getGalleryImages(): NgxGalleryImage[] {
    return this.member?.photos
               .map<NgxGalleryImage>(p => { return { small : p?.url, medium : p?.url, big: p?.url } });
  }
}
