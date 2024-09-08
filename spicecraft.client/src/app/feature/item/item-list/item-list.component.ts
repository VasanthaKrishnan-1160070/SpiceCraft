import {Component, inject, OnInit} from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";
import {ItemService} from "../../../core/service/item.service";

@Component({
  selector: 'sc-item-list',
  standalone: true,
  imports: [
    TitleComponent
  ],
  templateUrl: './item-list.component.html',
  styleUrl: './item-list.component.css'
})
export class ItemListComponent implements OnInit {
    private _itemService = inject(ItemService);

  ngOnInit() {
    this._itemService.getItems().subscribe(
      s => console.log(s);
    )
  }
}
