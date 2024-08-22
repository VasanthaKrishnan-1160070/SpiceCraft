import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import {RouteChangeService} from "./core/service/route-change.service";

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

@Component({
  selector: 'sc-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public forecasts: WeatherForecast[] = [];

  constructor(private routeChangeService: RouteChangeService) {}


  ngOnInit() {

  }



  title = 'spicecraft.client';
}
