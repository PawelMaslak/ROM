import { Component, OnInit } from '@angular/core';
import { OrderService } from 'src/app/shared/order.service';
import { NgForm } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { OrderItemsComponent } from '../order-items/order-items.component';
import { Paymentmethod } from 'src/app/shared/paymentmethod.model';
import { PaymentmethodService } from 'src/app/shared/paymentmethod.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styles: []
})
export class OrderComponent implements OnInit {

  paymentMethodsList: Paymentmethod[];

  constructor(
    private service: OrderService,
    private dialog: MatDialog,
    private paymentService: PaymentmethodService) { }


  ngOnInit() {
    this.resetForm()
    this.paymentService.getPaymentMethods()
      .then(res => this.paymentMethodsList = res as Paymentmethod[])
  }

  resetForm(form?: NgForm) {
    if (form != null)
      form.resetForm();
    this.service.formData = {
      OrderId: null,
      OrderNo: Math.floor(100000 + Math.random() * 900000).toString(),
      CustomerId: 0,
      PaymentMethod: '',
      Total: 0
    };

    this.service.orderItems = [];
  }

  addOrEditOrderItem(OrderItemIndex, OrderId) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.disableClose = false; //prevents dialog from closing if one clicks outside the dialog window
    dialogConfig.width = "50%";
    //Pasing the OrderItemIndex and OrderId
    dialogConfig.data = { OrderItemIndex, OrderId };
    this.dialog.open(OrderItemsComponent, dialogConfig)
  }

  openDialog() {
    console.log("success");
  }
}
