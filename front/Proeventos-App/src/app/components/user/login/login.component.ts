import { LogUtil } from './../../../util/log-util';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { UserLogin } from './../../../models/identity/user-login';
import { Component, OnInit } from '@angular/core';
import { AccountService } from '@app/service/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  model= {} as UserLogin;

  constructor(private accountService: AccountService,
              private route: Router,
              private toast:ToastrService) { }

  ngOnInit(): void {
  }

  public login():void{
      this.accountService.login(this.model).subscribe(
        {
          next:(response:any)=>{
            this.route.navigateByUrl('/dashboard');
          },
          error:(error:any)=>{
            if(error.status==401){
              this.toast.error("Usuário ou senha inválido");
            }else{
              LogUtil.log("login() Error:", error);
            }

          }
        }
      );
  }

}
