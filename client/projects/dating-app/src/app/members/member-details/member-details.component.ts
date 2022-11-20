import { CreateMessage } from './../../models/create-message';
import { MessageService } from './../../services/message.service';
import { MembersService } from './../../services/members.service';
import { Member } from './../../models/member';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Message } from '../../models/message';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Subscription } from 'rxjs';

@Component({
  selector: 'dta-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.scss']
})
export class MemberDetailsComponent implements OnInit, OnDestroy {

  member: Member;
  messages: Message[];
  hasLike = false;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  @ViewChild('tabset', { static: true }) tabset: TabsetComponent;
  activeTab: TabDirective;
  messagesTabHasBeenPreviouslyActivated = false;
  messagesTabHeadingTitle = 'Messages';
  memberDataResolverSubsc: Subscription;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    public membersService: MembersService,
    private messageService: MessageService) { }

  ngOnInit(): void {

    const tabToActivate = this.activatedRoute.snapshot.queryParams['tab'];

    //The resolver MemberDetailsResolver - that is attached to the route that activates this component
    //handles the member data loading
    this.memberDataResolverSubsc = this.activatedRoute.data.subscribe(data => {

      this.member = data['member'];

    });

    this.initImageGallery();

    if (tabToActivate) {
      setTimeout(() => {
      this.onTabActivated(this.getTabByHeadingTitle(tabToActivate));
      }, 100);
    }
  }

  ngOnDestroy(): void {
    this.memberDataResolverSubsc.unsubscribe();
  }

  getTabByHeadingTitle(title: string) {
    return this.tabset.tabs.find(x => x.heading.toUpperCase() === title.toUpperCase());
  }

  loadMessagesThread(memberId: string) {
    this.messageService
        .getMessagesThread(memberId)
        .subscribe({
          next: messages => this.messages = messages
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

  onLikeClick(event: any) {
    this
      .membersService
      .toggleLike(this.member.id)
      .subscribe({
        next: (result:any) => { this.hasLike = result.didLike; }
      });
  }

  onMessageClick(event: any) {
    const messagesTab = this.tabset.tabs.find(x => x.heading === this.messagesTabHeadingTitle);
    if (messagesTab) {
      this.onTabActivated(messagesTab);
    }
  }

  onMessageCreated(message: CreateMessage) {
    this.messageService
        .createMessage(message)
        .subscribe(_ => this.loadMessagesThread(this.member.id));
  }

  onTabActivated (tab: TabDirective) {

    if (!tab)
      return;

    this.activeTab = tab;
    this.activeTab.active = true;

    if (this.activeTab.heading === this.messagesTabHeadingTitle) {

      if (!this.messagesTabHasBeenPreviouslyActivated) {
        this.messagesTabHasBeenPreviouslyActivated = true;

        this.loadMessagesThread(this.member.id);
      }

    }

  }
}
