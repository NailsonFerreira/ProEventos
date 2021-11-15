import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/service/evento.service';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {
  public eventoId: number = 0;

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) { }

  public eventos: Evento[] = [];
  public eventosFilter: Evento[] = [];
  public widthImg = 100;
  public marginImg = 2;
  public showImage = true;
  private _search: string = '';
  modalRef?: BsModalRef;

  public get search(): string {
    return this._search;
  }

  public set search(value: string) {
    this._search = value;
    this.eventosFilter = this._search ? this.filterEvents(value) : this.eventos;
  }

  public ngOnInit(): void {
    this.spinner.show();
    this.getEventos();
  }

  public getEventos(): void {
    this.eventoService.getEventos().subscribe(
      {
        next: (eventos: Evento[]) => {
          this.eventos = eventos;
          this.eventosFilter = eventos;
          console.log(eventos);
        },
        error: (error: any) => {
          this.toastr.error('Erro ao carregar eventos', "Erro");
          console.error(`ERROR GET EVENTOS${JSON.stringify(error)}`);
        }
      }
    ).add(()=>this.spinner.hide());

  }

  public setShowImages(): void {
    this.showImage = !this.showImage;
  }

  public filterEvents(value: string): Evento[] {
    value = value.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: { tema: string, local: string; }) => evento.tema.toLocaleLowerCase().indexOf(value) !== -1 ||
        evento.local.toLocaleLowerCase().indexOf(value) !== -1
    );
  }

  openModal(event:any, template: TemplateRef<any>, evento:Evento): void {
    this.eventoId = evento.id;
    event.stopPropagation();
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();
    this.eventoService.deleteEvento(this.eventoId).subscribe(
      (response:any)=>{
        if(response.message=='Deletado'){
          this.toastr.success("Evento deletado com sucesso", 'Deletado');
          this.getEventos();
        }
      },
      (error:any)=>{
        console.log(`ERO DELETAR ${JSON.stringify(error)}`);
        this.toastr.error(`Erro ao deletar evento de id ${this.eventoId}`, 'Erro');
      },
    ).add(()=>this.spinner.hide());
  }



  decline(): void {
    this.modalRef?.hide();
  }

  detalheEvento(id: number):void{
      this.router.navigate([`eventos/detalhe/${id}`]);
  }
}
