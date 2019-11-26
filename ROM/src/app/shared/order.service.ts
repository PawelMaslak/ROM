import { Injectable } from '@angular/core';
import { Order } from './order.model';
import { OrderItem } from './order-item.model';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  formData: Order;
  orderItems: OrderItem[];

  constructor(private http: HttpClient) { }

  saveOrUpdateOrder() {
    console.log("OrderId has been set as: " + this.formData.OrderId)
    var body = {
      ...this.formData,
      OrderDetail: this.orderItems
    }

    console.log('Items iteration...');
    this.orderItems.forEach(item => {
      console.log("Order items OrderId is set to: " + item.OrderId);
      console.log("Item name: " + item.ItemName);
    });

    return this.http.post(environment.apiURL + '/orders', body);
  }

}

