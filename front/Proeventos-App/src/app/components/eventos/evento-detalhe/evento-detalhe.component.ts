import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';
import { EventoService } from '@app/service/evento.service';
import { LoteService } from '@app/service/lote.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})

export class EventoDetalheComponent implements OnInit {

  form!: FormGroup;
  evento = {} as Evento;
  isEditMode: boolean = false;
  eventoId: number;
  modalRef: BsModalRef;
  loteAtual = { id: 0, nome: 0, index: 0 };

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

  get lotes(): FormArray {
    return this.form.get("lotes") as FormArray;
  }

  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activateRoute: ActivatedRoute,
    private router: Router,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toast: ToastrService,
    private loteService: LoteService,
    private modalService: BsModalService) {
    this.localeService.use('pt-br');
  }

  ngOnInit(): void {
    this.carregaEvento();
    this.validation();
    this.setEditMode();
  }

  public carregaEvento(): void {
    this.eventoId = +this.activateRoute.snapshot.paramMap.get('id');

    if (this.eventoId != null || this.eventoId != 0) {
      this.spinner.show();
      this.eventoService.getEventoById(this.eventoId).subscribe(
        {
          next: (evento: Evento) => {
            this.evento = { ...evento }
            this.form.patchValue(this.evento);
            evento.lotes.forEach(lote => {
              this.lotes.push(this.createFormLote(lote));
            });
            console.log(this.evento);
          },
          error: (error) => {
            console.log(error);
            this.spinner.hide();
            this.toast.error("Erro ao carregar evento");
          },
        }
      ).add(() => this.spinner.hide());
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
      lotes: this.fb.array([]),
    });
  }

  adicinaFormLote(): void {

    this.lotes.push(this.createFormLote({ id: 0 } as Lote));
  }

  createFormLote(lote: Lote): FormGroup {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, [Validators.required]],
      preco: [lote.preco, [Validators.required]],
      quantidade: [lote.quantidade, [Validators.required]],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim]
    })
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(form: FormControl | AbstractControl): any {
    return { 'is-invalid': form.errors && form.touched };
  }

  salvarEvento(): void {
    if (this.form.valid) {
      console.log(JSON.stringify('response'));
      this.spinner.show();
      this.evento = { ...this.form.value };
      let methodName: any = 'post';

      if (this.eventoId != null || this.eventoId != 0) {
        this.evento.id = this.eventoId;
        methodName = 'put';
      }

      this.eventoService[methodName](this.evento).subscribe(
        (response: Evento) => {
          console.log(JSON.stringify(response));
          this.router.navigate([`eventos/detalhe/${response.id}`]);
          this.toast.success("Evento salvo com sucesso", "Sucesso");
        },
        (error: any) => {
          console.log(JSON.stringify(error));
          this.toast.error("Erro ao salvar evento");
        },
      ).add(() => this.spinner.hide());
    }
  }

  public salvarLotes(): void {
    if (this.form.controls.lotes.valid) {
      this.spinner.show();
      this.loteService.saveLotes(this.eventoId, this.form.value.lotes).subscribe(
        {
          next: (lotes: Array<Lote>) => {

            this.toast.success('Lotes salvos com sucesso', 'Sucesso');
          },
          error: (error: any) => {
            this.toast.error('Erro ao salvar lotes', 'Erro');

          },
        }
      ).add(() => this.spinner.hide());
    }
  }

  setEditMode(): void {
    if (this.eventoId != null || this.eventoId != 0) {
      this.isEditMode = true;
    }
  }

  public removerLote(index: number, template: TemplateRef<any>): void {
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' })
    this.loteAtual.id = this.lotes.get(index + '.id').value;
    this.loteAtual.nome = this.lotes.get(index + '.nome').value;
    this.loteAtual.index = index;


  }

  confirmLoteDelete(): void {
    this.modalRef.hide();
    this.spinner.show();
    this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe(
      {
        next:(response:any)=>{
          this.toast.show('Lote deletado com sucesso', 'Sucesso');
          this.lotes.removeAt(this.loteAtual.index);
        },
        error:(error:any)=>{
          this.toast.error('Erro ao deletar lote', 'Erro');
          console.error(error);
        }
      }
    ).add(()=>this.spinner.hide());
  }

  cancelaLoteDelete(): void {
    this.modalRef.hide();
  }
}
