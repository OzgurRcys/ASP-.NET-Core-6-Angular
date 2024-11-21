import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, of } from 'rxjs';
import { CustomToastrService,ToastrMessageType,ToastrPosition} from '../ui/custom-toastr.service';
import { UserAuthService } from './models/user-auth.service';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorHandlerInterceptorService implements HttpInterceptor{

  constructor(private toastrService: CustomToastrService, private userAuthService: UserAuthService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(catchError(error => {
      switch(error.status){

        case HttpStatusCode.Unauthorized:
        this.toastrService.message("Yetkisiz İşlem Hatası !", "",{
          messageType: ToastrMessageType.Warning,
          position: ToastrPosition.TopRight
        });

       this.userAuthService.refreshTokenLogin(localStorage.getItem("refreshToken")).then(data => {

       });
        break;

        case HttpStatusCode.InternalServerError:
          this.toastrService.message("Sunucu Erişim Hatası !", "Sunucu Hatası",{
            messageType: ToastrMessageType.Warning,
            position: ToastrPosition.TopRight
          })
        break;

        case HttpStatusCode.BadRequest:
          this.toastrService.message("Geçersiz İstek !", "Dikkat",{
            messageType: ToastrMessageType.Warning,
            position: ToastrPosition.TopRight
          })
        break;

        case HttpStatusCode.NotFound:
          this.toastrService.message("İstenilen Sayfa Bulunamadı !", "",{
            messageType: ToastrMessageType.Warning,
            position: ToastrPosition.TopRight
          })
        break;

        default:
          this.toastrService.message("Beklenemeyen Bir Hata !", "",{
            messageType: ToastrMessageType.Warning,
            position: ToastrPosition.TopRight,
          })
        break;

      }
      return of(error);
    }));
  }
}
