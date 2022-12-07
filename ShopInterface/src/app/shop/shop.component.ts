import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css']
})
export class ShopComponent implements OnInit {

  accounts: string = "";

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.http.get("http://localhost/ShopAPI/accounts", { responseType: 'text' }).toPromise().then(r => {
      if (r != null) {
        this.accounts = r;
      }
    })
    }

}
