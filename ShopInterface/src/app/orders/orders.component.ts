import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Account } from '../model/account.model';
import { Item } from '../model/item.model';
import { OrderLine } from '../model/order-line.model';
import { Order } from '../model/order.model';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {

  public orders: Order[] | undefined;
  public currentOrder: Order | undefined | null;
  public currentAccountName: string | null = null;
  displayedColumns: string[] = ['itemName', 'count', 'amount', 'total']
  public loadedItems: Item[] = [];

  constructor(private client: HttpClient, private _snackBar: MatSnackBar) {

  }

  ngOnInit(): void {
    this.client.get<Order[]>('http://localhost/ShopAPI/Orders').toPromise().then(r => {
      if (r != null) {
        this.orders = r;
      }
    }).catch(() =>
    {
      this.openSnackBar("Failed to get orders", "Ok");
      });
  }

  public openOrder(order: Order): void {
    this.currentAccountName = null;
    this.currentOrder = order;
    if (order.accountId != null && order.accountId != '') {
      this.client.get<Account>("http://localhost/shopapi/accounts/" + order.accountId).toPromise().then(r => {
        if (r != null) {
          this.currentAccountName = r.name!;
        }
      })
    }
    if (order.lines == null) {
      this.client.get<OrderLine[]>("http://localhost/shopapi/orders/" + order.orderId + '/orderlines').toPromise().then(lines => {
        if (lines == null) {
          return;
        }
        order.lines = lines;
        lines.forEach(line => {
          let item = this.loadedItems.find(i => i.itemId == line.itemId);
          if (item == null) {
            this.client.get<Item>("http://localhost/shopapi/items/" + line.itemId).toPromise().then(r => {
              if (r != null) {
                this.loadedItems.push(r!)
                line.itemName = r.name;
              }
            })
          }
          else {
            line.itemName = item.name;
          }
        })
      })
    }
  }

  public openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action);
  }
}
