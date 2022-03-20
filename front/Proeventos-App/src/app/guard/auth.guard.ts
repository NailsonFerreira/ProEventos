import { AccountService } from '@app/service/account.service';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router,
    private toast: ToastrService,
    private accountService: AccountService) {

  }

  canActivate(): boolean {
    if (this.accountService.hasUserLogged())
      return true;

    this.toast.info("Usuário não autenticado", "Deslogando...");
    this.router.navigate(["/user/login"]);
    return false;
  }

}
