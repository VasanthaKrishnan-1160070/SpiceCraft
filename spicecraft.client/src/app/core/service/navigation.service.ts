import {inject, Injectable} from "@angular/core";
import {WebAPIService} from "./webAPI.service";
import {ResultDetailModel} from "../model/result-detail.model";
import {NavigationPredictionModel} from "../model/ml/navigation/navigation-prediction.model";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root',
})
export class NavigationService {

  private _api = inject(WebAPIService)

// Fetch Navigation Predictions for a specific user
  public getNavigationPredictions(userId: number): Observable<ResultDetailModel<NavigationPredictionModel[]>> {
    return this._api.get<ResultDetailModel<NavigationPredictionModel[]>>(`/navigation/predict-navigation/${userId}`);
  }

  public logNavigationActivity(userId: number, navigationItem: string, routing: string, timeSpent: number, clickCount: number): Observable<any> {
    const log = {
      UserId: userId,
      NavigationItem: navigationItem,
      TimeSpent: timeSpent,
      routing: routing,
      ClickCount: clickCount,
      SessionId: sessionStorage.getItem('sessionId') || this.generateSessionId()  // Use existing session or create a new one
    };

    return this._api.post('/navigation/log-activity', log);
  }

  // Helper to generate a session ID if none exists
  private generateSessionId(): string {
    const sessionId = Math.random().toString(36).substring(2);
    sessionStorage.setItem('sessionId', sessionId);
    return sessionId;
  }
}
