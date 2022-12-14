import { MemberDetailsResolver } from './resolvers/member-details.resolver';
import { PreventUnsavedDataLossGuard } from './guards/prevent-unsaved-data-loss.guard';
import { NotFoundComponent } from './not-found/not-found.component';
import { AuthenticationGuard } from './guards/authentication.guard';
import { MessagesComponent } from './messages/messages.component';
//import { ListsComponent } from './lists/lists.component';
import { MemberDetailsComponent } from './members/member-details/member-details.component';
import { MembersListComponent } from './members/members-list/members-list.component';
import { HomeComponent } from './home/home.component';
import { Routes } from "@angular/router";
import { ErrorComponent } from './error/error.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { LikesComponent } from './members/likes/likes.component';

export const AppRoutes: Routes = [
 { path: '', component: HomeComponent },
 {
   path: '',
   runGuardsAndResolvers: 'always',
   canActivate: [AuthenticationGuard],
   children: [
    { path: 'members', component: MembersListComponent },
    { path: 'members/:id', component: MemberDetailsComponent, resolve: {member: MemberDetailsResolver} },
    { path: 'member/profile', component: MemberEditComponent, canDeactivate: [PreventUnsavedDataLossGuard] },
    { path: 'lists', component: LikesComponent },
    { path: 'messages', component: MessagesComponent }
   ]
 },
 { path: 'error', component: ErrorComponent },
 { path: 'not-found', component: NotFoundComponent },
 { path: '**', redirectTo: 'not-found', pathMatch: 'full' }
];
