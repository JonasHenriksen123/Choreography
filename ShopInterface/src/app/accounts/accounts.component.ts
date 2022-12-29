import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Account } from '../model/account.model';
import { Invoice } from '../model/invoice.model';

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.css']
})
export class AccountsComponent implements OnInit {

  public accounts: Account[];
  public currentAccount: Account | undefined | null;

  public accountCreateView: boolean = false;
  public accountName: string | null = null;
  public accountCredit: number | null = null;

  displayedColumns: string[] = ['invoiceId', 'orderId', 'amount', 'state']

  constructor(private client: HttpClient, private _snackBar: MatSnackBar) {
    this.accounts = [];
  }

  ngOnInit(): void {
    this.client.get<Account[]>('http://localhost/ShopAPI/Accounts').toPromise().then(r => {
      if (r != null) {
        this.accounts = r;
      }
    }).catch(() => {
      this.openSnackBar('Failed to load Accounts', 'Ok');
    });
  }

  public newAccount(): void {
    this.currentAccount = null;
    this.accountCreateView = true;
    this.accountName = "";
    this.accountCredit = null;
  }

  public createAccount(): void {
    let account = new Account();
    account.name = this.accountName!;
    account.creditAmount = this.accountCredit!;
    
    this.client.post<Account>('http://localhost/SHopAPI/Accounts', account).toPromise().then(r => {
      if (r != null) {
        this.accounts?.push(r);
        this.accountCreateView = false;
      }
    });
  }

  public openAccount(account: Account): void {
    this.accountCreateView = false;
    if (account.invoices == null) {
      account.invoices = [];
      this.client.get<Invoice[]>("http://localhost/shopapi/accounts/" + account.accountId + '/invoices').toPromise().then(invoices => {
        if (invoices == null) {
          return;
        }
        account.invoices = invoices;
      })
    }
    this.currentAccount = account;
  }

  public openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action);
  }
}
