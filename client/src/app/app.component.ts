import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { filter } from 'rxjs/operators'
import { BasketService } from './basket/basket.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title:string="Gvn ECommerce"
  constructor(private basketService:BasketService) {}

  ngOnInit(): void {
    const basketId=localStorage.getItem('basket_id');
    if(basketId){
      this.basketService.getBasket(basketId).subscribe(()=>{
        console.log("initailized basket")
      },error=>{
        console.log(console.error);
      
      })
    }
  }
}
