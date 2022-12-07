import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {

  APIVersion: string = "";
  BrokerVersion: string = "";
  StockVersion: string = "";
  AccountsVersion: string = "";
  OrdersVersion: string = "";

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.http.get("http://localhost/ShopAPI/info/version", { responseType: 'text' }).toPromise().then(r => {
      if (r != null) {
        this.APIVersion = r.toString();
      }
    })

    this.http.get("http://localhost/Broker/info/version", { responseType: 'text' }).toPromise().then(r => {
      if (r != null) {
        this.BrokerVersion = r.toString();
      }
    })

    this.http.get("http://localhost/Accounts/info/version", { responseType: 'text' }).toPromise().then(r => {
      if (r != null) {
        this.AccountsVersion = r.toString();
      }
    })

    this.http.get("http://localhost/Orders/info/version", { responseType: 'text' }).toPromise().then(r => {
      if (r != null) {
        this.OrdersVersion = r.toString();
      }
    })

    this.http.get("http://localhost/Stock/info/version", { responseType: 'text' }).toPromise().then(r => {
      if (r != null) {
        this.StockVersion = r.toString();
      }
    })
    }
}
