import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorFields } from '@app/helpers/ValidatorFields';
import { User } from '@app/models/identity/user';
import { AccountService } from '@app/service/account.service';
import { ToastrService } from 'ngx-toastr';
import { LogUtil } from '@app/util/log-util';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  form!: FormGroup;
  user = {} as User;

  min = 4;
  max = 10;

  get f(): any {
    return this.form.controls;
  }
  constructor(private fb: FormBuilder,
              private accountService: AccountService,
              private route: Router,
              private toast:ToastrService) { }

  ngOnInit(): void {
    this.validation();
  }

  private validation(): void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorFields.MustMatch('password','confirmePassword')
    };

    this.form = this.fb.group({
      primeiroNome: ['', [Validators.required]],
      ultimoNome: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      userName: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(this.min), Validators.maxLength(this.max)]],
      confirmePassword: ['', [Validators.required, Validators.minLength(this.min), Validators.maxLength(this.max)]],
    },formOptions);
  }

  public register():void{
    this.user = {...this.form.value};
    this.accountService.register(this.user).subscribe(
      {
        next:(response:any)=>{
          this.route.navigateByUrl('/dashboard');
        },
        error:(error:any)=>{
          if(error.status==401){
            this.toast.error("Usuário ou senha inválido");
          }else{
            LogUtil.log("registration() Error:", error);
          }

        }
      }
    );
  }

}
