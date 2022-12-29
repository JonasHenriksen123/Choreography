import { Invoice } from "./invoice.model";

export class Account {
  public accountId: string | undefined;
  public name: string | undefined;
  public creditAmount: number | undefined;
  public credit: number | undefined;
  public invoices: Invoice[] | undefined;
}
