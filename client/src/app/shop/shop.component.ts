import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss'],
})
export class ShopComponent implements OnInit {
  @ViewChild('search',{static:false}) searchTerm:ElementRef
  products: Array<IProduct> = [];
  types: Array<IType> = [];
  brands: Array<IBrand> = [];
  shopParams = new ShopParams();
  totalCount: number;
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' },
  ];

  constructor(private shopService: ShopService) {}

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe(
      (response) => {
        this.products = response.data;
        this.shopParams.pageSize = response.pageSize;
        this.shopParams.pageNumber = response.pageIndex;
        this.totalCount = response.count;
      },
      (error) => {
        console.log(error);
      }
    );
  }
  getBrands() {
    this.shopService.getBrands().subscribe(
      (response) => {
        this.brands = [{ id: 0, name: 'All' }, ...response];
      },
      (error) => {
        console.log(error);
      }
    );
  }
  getTypes() {
    this.shopService.getTypes().subscribe(
      (response) => {
        this.types = [{ id: 0, name: 'All' }, ...response];
      },
      (error) => {
        console.log(error);
      }
    );
  }
  onBrandSelected(brandId: number) {
    this.shopParams.brandId = brandId;
    this.shopParams.pageNumber=1;
    this.getProducts();
  }
  onTypeSelected(typeId: number) {
    this.shopParams.typeId = typeId;
    this.shopParams.pageNumber=1;
    this.getProducts();
  }
  onSortSelected(sort: string) {
    this.shopParams.sort = sort;
    this.getProducts();
  }
  onPageChanged(event: any) {
    this.shopParams.pageNumber = event.page;
    this.getProducts();
  }
  onSearch(){
    this.shopParams.search=this.searchTerm.nativeElement.value;
    this.getProducts()
  }
  onReset(){
    this.searchTerm.nativeElement.value=undefined;
    this.shopParams=new ShopParams();
    this.getProducts();
  }
}
