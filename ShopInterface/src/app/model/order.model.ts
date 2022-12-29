import { Item } from "./item.model";
import { OrderLine } from "./order-line.model";

export class Order {
  public orderId: String | undefined;
  public amount: number | undefined;
  public itemCount: number | undefined;
  public accountId: string | null | undefined;
  public state: number | undefined;
  public lines: OrderLine[] | undefined;

  constructor() {
    this.amount = 0;
    this.itemCount = 0;
    this.lines = [];
  }

  public addToOrder(item: Item, amount: number): void {
    var line = this.lines?.find(line => line.itemId == item.itemId)

    if (line != null) {
      line.count! += amount;
      return;
    }

    line = new OrderLine();
    line.itemId = item.itemId;
    line.amount = item.price;
    line.count = amount;
    line.total = item.price! * amount;

    this.amount! += line.total;
    this.itemCount! += line.count;

    this.lines?.push(line);
  }
}
