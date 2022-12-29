import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Item } from '../model/item.model';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.css']
})
export class ItemsComponent implements OnInit {

  public items: Item[] | undefined;
  public currentItem: Item | undefined | null;

  public itemCreateView: boolean = false;
  public itemName: string = "";
  public itemDescription: string = "";
  public itemPrice: number | undefined | null;
  public itemAmount: number | undefined | null;

  constructor(private client: HttpClient, private _snackBar: MatSnackBar) {

  }

  ngOnInit(): void {
    this.client.get<Item[]>('http://localhost/ShopAPI/Items').toPromise().then(r => {
      if (r != null) {
        this.items = r;
      }
    }).catch(() =>
    {
      this.openSnackBar('Failed to load Items', 'Ok');
    });
  }

  public newItem(): void {
    this.currentItem = null;
    this.itemCreateView = true;
    this.itemName = "";
    this.itemDescription = "";
    this.itemPrice = null;
    this.itemAmount = null;
  }

  public createItem(): void {
    let item = new Item();
    item.name = this.itemName;
    item.description = this.itemDescription;
    item.price = this.itemPrice!;
    item.amount = this.itemAmount!;

    this.client.post<Item>('http://localhost/SHopAPI/Items', item).toPromise().then(r => {
      if (r != null) {
        this.items?.push(r);
        this.itemCreateView = false;
      }
    });
  }

  public openItem(item: Item): void {
    this.itemCreateView = false;
    this.currentItem = item;
  }

  public openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action);
  }
}
