import {inject, Injectable} from "@angular/core";
import {WebAPIService} from "./webAPI.service";

@Injectable({
  providedIn: 'root',
})
export class ReportService {
  private _api = inject(WebAPIService);

  public getReportByName(reportName: string) {
    return this._api.get('/report/' + reportName)
  }
}
