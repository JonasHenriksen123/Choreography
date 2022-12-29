import { HttpClient } from '@angular/common/http';
import { Component, ContentChild, ElementRef, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Item } from '../model/item.model';
import { OrderLine } from '../model/order-line.model';
import { Order } from '../model/order.model';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Account } from '../model/account.model';
import { BreakpointObserver } from '@angular/cdk/layout';
import { MatStepper, StepperOrientation } from '@angular/material/stepper';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css']
})
export class ShopComponent implements OnInit {

  public items: Item[] | undefined;

  public orderItems: CartItem[];
  public orderCount: number = 0;
  public orderTotal: number = 0;

  @ViewChild('stepper') private stepper!: MatStepper;

  public showingCart: boolean = false;
  public makingOrder: boolean = false;
  public processing: boolean = false;
  public failed: boolean = false;

  public firstFormGroup = this.formBuilder.group({
    accountControl: ['', Validators.required],
  });
  public accounts: Account[] = [];
  account: string | null = null;
  public order: Order | undefined | null;

  public secondFormGroup = this.formBuilder.group({
    secondCtrl: [''],
    thirdCtrl: [''],
    fourthCtrl: [''],
    fifthCtrl: ['']
  });

  constructor(private http: HttpClient, private _snackBar: MatSnackBar, private formBuilder: FormBuilder, breakpointObserver: BreakpointObserver) {
    this.orderItems = [];

  }

  ngOnInit(): void {
    this.http.get<Item[]>('http://localhost/ShopAPI/items').toPromise().then(r => {
      if (r != null) {
        this.items = r;
      }
    }).catch(() => {
      this.openSnackBar('Failed To load Items', 'Ok');
    })
  }

  public addToCart(item: Item, amountString: string): void {
    var amount = Number.parseInt(amountString);
    let line = this.orderItems.find(oi => oi.Item?.itemId == item.itemId);

    this.orderCount += amount;
    if (line != null) {
      line.amount += amount;
      return;
    }

    var next = new CartItem();
    next.Item = item;
    next.amount = amount;
    this.orderItems.push(next);
  }

  public updateCart(): void {
    let count = 0;
    let total = 0;
    let remove = null;
    this.orderItems.forEach((line, index) => {
      if (line.amount < 1) {
        remove = index;
        return;
      }
      count += line.amount;
      total += line.Item?.price! * count;
    })
    if (remove != null) {
      this.orderItems.splice(remove, 1);
    }

    this.orderCount = count;
    this.orderTotal = total;
  }

  public makeOrder() {
    this.makingOrder = true;
    this.showingCart = false;
    this.http.get<Account[]>('http://localhost/shopapi/accounts').toPromise().then(r => {
      if (r != null) {
        this.accounts = r;
      }
    });
  }

  public placeOrder() {
    this.processing = true;

    this.order = new Order();
    this.orderItems.forEach(item => {
      this.order?.addToOrder(item.Item!, item.amount);
    })
    if (this.account == 'null') {
      this.order.accountId = null;
    }
    else {
      this.order.accountId = this.account;
    }

    this.http.post<Order>('http://localhost/ShopAPI/Orders', this.order).toPromise().then(r => {
      this.order = r;
      this.waitForResult();
    })
      .catch(() => {
        this.processing = false;
        this.failed = true;
      })
  }

  private waitForResult() {
    setTimeout(() => {
      this.http.get<Order>('http://localhost/ShopAPI/Orders/' + this.order?.orderId).toPromise().then(r => {
        if (r == null) {
          //the order was deleted, unable to create order
          this.failed = true;
          this.processing = false;
          return;
        }

        if (r?.state == 1) {
          //payment information is needed to process
          this.processing = false;
          return;
        }
        if (r?.state == 2) {
          //paid with credit go to done
          this.processing = false;
          setTimeout(() => {
            this.stepper.selectedIndex = 2;
          }, 1)
          return;
        }

        this.waitForResult()

      }).catch(() => {
        //the order was deleted, unable to create order
        this.failed = true;
        this.processing = false;
      });
    }, 1000)
  }

  public showCart() {
    this.showingCart = true;
    this.updateCart();
  }

  public hideCart() {
    this.showingCart = false;
  }

  public openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action);
  }
}

class CartItem {
  Item: Item | undefined;
  amount: number = 0;
}
