import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CustomToastrService, ToastrPosition, ToastrMessageType } from './services/ui/custom-toastr.service';
import { AuthService } from './services/common/auth.service';
import { Router } from '@angular/router';
declare var $: any //jquery

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Project2Client';

  constructor(public authService: AuthService, private toastrService: CustomToastrService,private router: Router){
  authService.identityCheck();
  }

  signOut(){
    localStorage.removeItem("accessToken");
    this.authService.identityCheck();
    this.router.navigate([""]);
    this.toastrService.message("Oturumdan Başarıyla Çıkış Yapılmıştır...", " Rahat Olun  !",{
      messageType: ToastrMessageType.Info,
      position: ToastrPosition.TopRight
    })
  }

  // constructor(private toastrService: CustomToastrService) {

  //   toastrService.message("merhaba", "Zgr",
  //   {messageType: ToastrMessageType.Info, position:ToastrPosition.TopCenter});

  //   toastrService.message("merhaba", "Zgr",
  //   {messageType: ToastrMessageType.Success, position:ToastrPosition.TopCenter});

  //   toastrService.message("merhaba", "Zgr",
  //   {messageType: ToastrMessageType.Error, position:ToastrPosition.TopCenter});

  //   toastrService.message("merhaba", "Zgr",
  //   {messageType: ToastrMessageType.Warning, position:ToastrPosition.TopCenter});
  // }
}



