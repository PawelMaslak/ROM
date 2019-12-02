import { Component, OnInit } from '@angular/core';
import { Item } from '../shared/item.model';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styles: []
})
export class ItemsComponent implements OnInit {
  products: Item[];
  constructor() { }

  ngOnInit() {
  }

}
