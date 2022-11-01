import { LikeTypes } from './like-types.enum';
import { Component } from '@angular/core';

@Component({
  selector: 'dta-likes',
  templateUrl: './likes.component.html',
  styleUrls: ['./likes.component.scss']
})
export class LikesComponent {
  followers = LikeTypes.followers;
  following = LikeTypes.following;
}
