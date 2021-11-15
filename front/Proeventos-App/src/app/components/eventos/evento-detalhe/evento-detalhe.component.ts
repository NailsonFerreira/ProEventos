import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/service/evento.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  form!: FormGroup;
  evento = {} as Evento;

  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false
    };
  }
  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private route: ActivatedRoute,
    private router: Router,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toast: ToastrService) {
    this.localeService.use('pt-br');
  }

  ngOnInit(): void {
    this.carregaEvento();
    this.validation();
  }

  public carregaEvento(): void {
    const eventoIdParam = this.route.snapshot.paramMap.get('id');
      console.log(`Parametro ${eventoIdParam}`);
    if (eventoIdParam != null) {
      this.spinner.show();
      this.eventoService.getEventoById(+eventoIdParam).subscribe(
        {
          next: (evento:Evento) => {
            this.evento = {...evento}
            this.form.patchValue(this.evento);
            console.log(this.evento);
           },
          error: (error) => {
              console.log(error);
              this.spinner.hide();
              this.toast.error("Erro ao carregar evento");
            },
        }
      ).add(()=>this.spinner.hide());
    }
  }

  public validation(): void {
    this.form = this.fb.group({
      local: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      dataEvento: ['', [Validators.required]],
      tema: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      qtdPessoas: ['', [Validators.required]],
      imagemUrl: ['', [Validators.required]],
      telefone: ['', [Validators.required, Validators.minLength(8)]],
      email: ['', [Validators.required, Validators.email]],
    });
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(form: FormControl): any {
    return { 'is-invalid': form.errors && form.touched };
  }

  salvaAlteracao():void{
    if(this.form.valid){
      console.log(JSON.stringify('response'));
      this.spinner.show();
      const eventoIdParam = this.route.snapshot.paramMap.get('id');
      this.evento = {...this.form.value};
      let methodName :any= 'post';

      if(eventoIdParam!=null){
        this.evento.id = +eventoIdParam;
        methodName = 'put';
      }

        this.eventoService[methodName](this.evento).subscribe(
          (response:any)=>{
          console.log(JSON.stringify(response));
          this.router.navigate([`eventos/lista/`]);
          this.toast.success("Evento salvo com sucesso", "Sucesso");
        },
         (error:any)=>{
          console.log(JSON.stringify(error));
          this.toast.error("Erro ao salvar evento");
        },
      ).add(()=>this.spinner.hide());


    }
  }
}
