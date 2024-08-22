import { Injectable } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class RouteChangeService {
  constructor(private router: Router) {
    this.router.events.subscribe((event) => {
      this.wrapAndHideDxLicense();
    });
  }

  wrapAndHideDxLicense() {
    const dxLicenseElements = document.getElementsByTagName('dx-license');

    for (let i = 0; i < dxLicenseElements.length; i++) {
      const dxLicenseElement = dxLicenseElements[i];

      // Create a new parent element
      const newParent = document.createElement('div');

      // Wrap the dx-license element with the new parent
      // @ts-ignore
      dxLicenseElement.parentNode.insertBefore(newParent, dxLicenseElement);
      newParent.appendChild(dxLicenseElement);

      // Set the new parent to display: none
      newParent.style.display = 'none';
    }
  }
}
