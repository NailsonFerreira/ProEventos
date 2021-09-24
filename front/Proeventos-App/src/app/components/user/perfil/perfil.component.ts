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

  get f(): any {
    return this.form.controls;
  }
  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.validation();
  }

  private validation():void{

    const formOptions: AbstractControlOptions = {
      validators: ValidatorFields.MustMatch('senha','confirmeSenha')
    };

    this.form = this.fb.group({
      primeiroNome: ['', [Validators.required]],
      ultimoNome: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      descricao: ['', [Validators.required]],
      telefone: ['', [Validators.required]],
      senha: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(10)]],
      confirmeSenha: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(10)]],
    },formOptions);
  }

  onSubmit():void{
    if(this.form.invalid){
      return;
    }
  }

  public resetForm(event: any): any{
      event.preventDefault();
      this.form.reset();
  }
}
