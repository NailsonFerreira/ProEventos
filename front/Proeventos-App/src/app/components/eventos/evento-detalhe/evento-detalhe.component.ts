import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  form!: FormGroup;

  get f():any{
    return this.form.controls;
  }
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.validation();
  }

  public validation():void{
    this.form = this.fb.group({
      local: ['',[Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      dataEvento: ['',[Validators.required]],
      tema: ['',[Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      qtdPessoas: ['',[Validators.required]],
      imagemUrl: ['',[Validators.required]],
      telefone: ['',[Validators.required, Validators.minLength(8),]],
      email: ['',[Validators.required, Validators.email]],
    });
  }

  public resetForm():void{
    this.form.reset();
  }
}
