import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import {
  Basket,
  IBasket,
  IBasketItem,
  IBasketTotals,
} from '../shared/models/basket';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<IBasket>(null);
  basket$ = this.basketSource.asObservable();
  private basketTotalSource = new BehaviorSubject<IBasketTotals>(null);
  baksetTotal$ = this.basketTotalSource.asObservable();

  constructor(private httpClient: HttpClient) {}
  private calculateTotals() {
    const basket = this.getCurrentBasketValue();
    const shipping = 0;
    const subTotal = basket.items.reduce((a, b) => b.price * b.quantity + a, 0);
    const total = subTotal + shipping;
    this.basketTotalSource.next({ shipping, total, subTotal });
  }

  getBasket(id: string) {
    return this.httpClient.get(this.baseUrl + 'basket?id=' + id).pipe(
      map((basket: IBasket) => {
        this.basketSource.next(basket);
        this.calculateTotals();
      })
    );
  }
  setBasket(basket: IBasket) {
    return this.httpClient.post(this.baseUrl + 'basket', basket).subscribe(
      (response: IBasket) => {
        this.basketSource.next(response);
        this.calculateTotals();
      },
      (error) => {
        console.log(error);
      }
    );
  }
  getCurrentBasketValue() {
    return this.basketSource.value;
  }
  addItemToBasket(item: IProduct, quantity = 1) {
    const itemtoAdd: IBasketItem = this.mapProductItemToBasketItem(
      item,
      quantity
    );
    const basket = this.getCurrentBasketValue() ?? this.createBasket();

    basket.items = this.addOrUpdateItem(basket.items, itemtoAdd, quantity);
    this.setBasket(basket);
  }
  private addOrUpdateItem(
    items: IBasketItem[],
    itemtoAdd: IBasketItem,
    quantity: number
  ): IBasketItem[] {
    const index = items.findIndex((i) => i.id === itemtoAdd.id);
    if (index === -1) {
      itemtoAdd.quantity = quantity;
      items.push(itemtoAdd);
    } else {
      items[index].quantity += quantity;
    }
    return items;
  }

  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }
  mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketItem {
    return {
      quantity,
      id: item.id,
      productName: item.name,
      brand: item.productBrand,
      pictureUrl: item.pictureUrl,
      price: item.price,
      type: item.productType,
    };
  }
}
