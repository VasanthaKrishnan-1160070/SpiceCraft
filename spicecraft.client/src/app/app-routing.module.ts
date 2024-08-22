import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LandingPageComponent} from "./feature/home/landing-page/landing-page.component";
import {LoginComponent} from "./feature/login/login.component";
import {ClientHomeComponent} from "./feature/home/client-home/client-home.component";

const routes: Routes = [
  { path: '', component: ClientHomeComponent},
  { path: 'home', component: ClientHomeComponent},
  { path: 'login', component: LoginComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
