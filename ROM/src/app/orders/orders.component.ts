import { Component, OnInit, Inject } from '@angular/core';
import { OrderService } from '../shared/order.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Order } from '../shared/order.model';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styles: []
})
export class OrdersComponent implements OnInit {
  orderList;

  constructor(
    private service: OrderService,
    private router: Router,
    private toastr: ToastrService) {

     }

  ngOnInit() {
    this.refreshList();
  }

  refreshList() {
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

  deleteOrder(orderId: number, orderNo: string) {
    console.log("Deleting the Order with Id: " + orderId);
    this.service.deleteOrder(orderId).then(res => {
      this.refreshList();
      this.toastr.warning(message, "Order deleted!");
    });

    var message: string = "Order no " + orderNo + " has been deleted successfully!";

  }
}
