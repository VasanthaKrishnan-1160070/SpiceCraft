import {inject, Injectable} from "@angular/core";
import {WebAPIService} from "./webAPI.service";
import {Observable} from "rxjs";
import {ResultDetailModel} from "../model/result-detail.model";
import {ProductInventoryModel} from "../model/inventory/product-inventory.model";
import {MenuItemModel} from "../model/item/menu-item.model";
import {SentimentPredictionModel} from "../model/sentimentAnalysis/sentiment-prediction.model";

@Injectable({
  providedIn: 'root',
})
export class SentimentAnalysisService {
  private _api = inject(WebAPIService);

  trainSentimentAnalysisModel(): Observable<any> {
    return this._api.post(`/sentimentAnalysis/train`, {});
  }

  analyseDescription(ratingDescription: string): Observable<SentimentPredictionModel> {
    return this._api.get<SentimentPredictionModel>(`/sentimentAnalysis/analyze/${ratingDescription}`);
  }

}
