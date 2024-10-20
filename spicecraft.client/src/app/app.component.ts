import { HttpClient } from '@angular/common/http';
import {AfterViewInit, Component, OnInit} from '@angular/core';
import {RouteChangeService} from "./core/service/route-change.service";
import {Element} from "@angular/compiler";

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
export class AppComponent implements OnInit, AfterViewInit {
  public forecasts: WeatherForecast[] = [];

  constructor(private routeChangeService: RouteChangeService) {}


  ngOnInit() {

  }

  ngAfterViewInit(): void {
    // this.wrapAndHideDxLicense();
  }

  // wrapAndHideDxLicense(): void {
  //   const dxLicenseElements = document.getElementsByTagName('dx-license');
  //   console.log(dxLicenseElements);
  //   console.log("length of dxLicenseElements.length", dxLicenseElements.length);
  //
  //   for (let i = 0; i < dxLicenseElements.length; i++) {
  //     const dxLicenseElement = dxLicenseElements[i];
  //
  //     // Create a new parent element
  //     const newParent = document.createElement('div');
  //
  //     // Wrap the dx-license element with the new parent
  //     dxLicenseElement?.parentNode.insertBefore(newParent, dxLicenseElement);
  //     newParent.appendChild(dxLicenseElement);
  //
  //     // Set the new parent to display: none
  //     newParent.style.display = 'none';
  //   }
  // }



  title = 'spicecraft.client';
}
