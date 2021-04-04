import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { filter } from 'rxjs/operators'
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title:string="Gvn ECommerce"
  constructor() {}

  ngOnInit(): void {

    var observe=new Observable<number>(item=>{
      item.next(1),
      item.next(2),
      item.next(5),
      item.next(7)
    });

    observe.pipe(
       filter(x=>x%2==1)
    ).subscribe(item=>console.log(item))



  }
}
