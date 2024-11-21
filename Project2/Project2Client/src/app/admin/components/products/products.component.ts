import { Component, OnInit, ViewChild } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { Create_Product } from 'src/app/contracts/create_product';
import { HttpClientService } from 'src/app/services/common/http-client.service';
import { ListComponent } from './list/list.component';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent extends BaseComponent implements OnInit {

  constructor(spinner: NgxSpinnerService, private httpClientService: HttpClientService){
    super(spinner)
  }

  ngOnInit(): void {

    // this.showSpinner(SpinnerType.BallSpinClockwise);
    // this.httpClientService.get<Product[]>({
    //   controller:"products"
    // }).subscribe(data => console.log(data));

    // this.httpClientService.post({             burası metotların test verileri post, put, delete
    //   controller:"products"
    // },{
    //     name:"Kalem",
    //     stock: 100,
    //     price: 15
    // }).subscribe()

    // // this.httpClientService.post({
    // //   controller:"products"
    // // },{
    // //     name:"Kağıt",
    // //     stock: 500,
    // //     price: 20
    // // }).subscribe()

    // // this.httpClientService.post({
    // //   controller:"products"
    // // },{
    // //     name:"Çanta",
    // //     stock: 100,
    // //     price: 50.50
    // // }).subscribe()

    // this.httpClientService.put({                       put metodunun test verileri
    //   controller:"products"
    // },{
    //   id:"1b901026-4816-4b1d-4e5e-08db3a55fd72",
    //   name:"Kağıt",
    //   stock:2500,
    //   price:10.5
    // }).subscribe()


    // this.httpClientService.delete({         delete metodunun test verileri
    //   controller:"products"
    // }, "1B901026-4816-4B1D-4E5E-08DB3A55FD72").subscribe();


    // this.httpClientService.get({
    //   baseUrl: "https://jsonplaceholder.typicode.com",
    //   controller: "posts"
    //   //üstteki baseurl ve controller yerine fullendpoint: https://jsonplaceholder.typicode.com/posts olarakta kullanılabilir.
    // }).subscribe(data => console.log(data));


  }

  @ViewChild(ListComponent) listComponent: ListComponent

  createdProduct(createdProduct: Create_Product){
    this.listComponent.getProducts();
  }

}
