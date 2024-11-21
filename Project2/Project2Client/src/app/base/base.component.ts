import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';


export class BaseComponent  {

  constructor(private spinner: NgxSpinnerService){

  }

  showSpinner(spinnerNameType : SpinnerType){
    //this.spinner.show(spinnerNameType);  sayfaların açılması sırasındaki spinner

    //setTimeout(() => this.hideSpinner(spinnerNameType), 1000);
  }

  hideSpinner(spinnerNameType: SpinnerType){
    this.spinner.hide(spinnerNameType);
  }

}


export enum SpinnerType{
  BallSpinClockwise = "spinner1",
  BallClimbingdot = "spinner2",
  BallPulse = "spinner3"
}
