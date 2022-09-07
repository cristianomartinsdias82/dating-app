import { AuthenticationGuard } from './guards/authentication.guard';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailsComponent } from './members/member-details/member-details.component';
import { MembersListComponent } from './members/members-list/members-list.component';
import { HomeComponent } from './home/home.component';
import { Routes } from "@angular/router";

export const AppRoutes: Routes = [
 { path: '', component: HomeComponent },
 {
   path: '',
   runGuardsAndResolvers: 'always',
   canActivate: [AuthenticationGuard],
   children: [
    { path: 'members', component: MembersListComponent },
    { path: 'members/:id', component: MemberDetailsComponent },
    { path: 'lists', component: ListsComponent },
    { path: 'messages', component: MessagesComponent }
   ]
 },
 { path: '**', redirectTo: '', pathMatch: 'full' }
];
