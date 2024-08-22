import { Injectable } from '@angular/core';
import {environment} from "../../../environments/environment";


@Injectable({
  providedIn: 'root'
})
export class ConstantsService {

  constructor() { }

  get apiUrl() {
    return environment.apiUrl;
  }
}
