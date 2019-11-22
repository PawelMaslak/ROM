import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { OrderItem } from 'src/app/shared/order-item.model';

@Component({
  selector: 'app-order-items',
  templateUrl: './order-items.component.html',
  styles: []
})
export class OrderItemsComponent implements OnInit {
  formData: OrderItem;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data,
    public dialogRef: MatDialogRef<OrderItemsComponent>) { }

  ngOnInit() {
  }

}
