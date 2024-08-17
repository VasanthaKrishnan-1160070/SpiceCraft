import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {LandingPageComponent} from "./feature/home/landing-page/landing-page.component";


@NgModule({
  declarations: [
    AppComponent
  ],
    imports: [
        BrowserModule, HttpClientModule,
        AppRoutingModule,
      LandingPageComponent
    ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
