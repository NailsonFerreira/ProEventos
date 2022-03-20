import { LogUtil } from './../../../util/log-util';
import { UserUpdate } from './../../../models/identity/user-update';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { AccountService } from '@app/service/account.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControlOptions } from '@angular/forms';
import { ValidatorFields } from '@app/helpers/ValidatorFields';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {

  form!: FormGroup;
  userUpdate = {} as UserUpdate;

  get f(): any {
    return this.form.controls;
  }
  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router,
    private toast: ToastrService,
    private spinner: NgxSpinnerService,
    ) { }

  ngOnInit() {
    this.validation();
    this.loadUser();
  }

  private loadUser():void{
    this.spinner.show();
    this.accountService.getUser().subscribe({
      next:(userU: UserUpdate)=>{
        LogUtil.log("loadUser()", userU);
        this.userUpdate = userU;
        this.form.patchValue(this.userUpdate);
        this.toast.success("Usuário carregado","Sucesso");
      },
      error:(error:any)=>{
        LogUtil.log("loadUser() ERROR:", error);
        this.toast.error("Ocorreu um erro ao carregar usuário", "Erro");
        this.router.navigate(["/dashboard"]);
      }
    }).add(()=>this.spinner.hide());
  }
  private validation():void{

    const formOptions: AbstractControlOptions = {
      validators: ValidatorFields.MustMatch('password','confirmePassword')
    };

    this.form = this.fb.group({
      userName: [''],
      primeiroNome: ['', [Validators.required]],
      titulo: ['NaoInformado', [Validators.required]],
      ultimoNome: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      descricao: ['', [Validators.required]],
      funcao: ['NaoInformado', [Validators.required]],
      phoneNumber: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(10)]],
      confirmePassword: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(10)]],
    },formOptions);
  }

  onSubmit():void{
    // if(this.form.invalid){
    //   return;
    // }
    this.atualizarUsuario();
  }

  public atualizarUsuario():void{
    this.userUpdate = {...this.form.value};
    this.spinner.show();

    this.accountService.updateUser(this.userUpdate).subscribe({
      next:()=>{
        this.toast.success("Usuário atualizado", "Sucesso");
      },
      error:(error:any)=>{
        this.toast.error("Ocorreu um erro ao atualizar usuário","Falha");
        LogUtil.log("atualizarUsuario() ERROR: ", error);
      }
    }).add(()=> this.spinner.hide());
  }

  public resetForm(event: any): any{
      event.preventDefault();
      this.form.reset();
  }
}
