import { Component, OnInit, Inject } from '@angular/core';
import { OrderService } from '../shared/order.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styles: []
})
export class OrdersComponent implements OnInit {
  orderList;

  constructor(
    private service: OrderService,
    private router: Router) { }

  ngOnInit() {
    this.service.getOrdersList().then(res => this.orderList = res);
  }

  openForEdit(orderId: number) {
    var route = '/order/edit/' + orderId;
    console.log('Trying to navigate to: ' + route);
    this.router.navigate([route]);
    console.log("navigated to " + route);
  }

  redirectToMain() {
    this.router.navigate(['/order']);
  }
}
