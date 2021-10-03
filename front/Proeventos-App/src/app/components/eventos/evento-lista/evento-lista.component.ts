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
          this.spinner.hide();
          this.toastr.error('Erro ao carregar eventos', "Erro");

          console.error(`ERROR GET EVENTOS${JSON.stringify(error)}`);
        },
        complete: () => {
          this.spinner.hide();
        }
      }
    );

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

  openModal(template: TemplateRef<any>): void {
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });


  }

  confirm(): void {
    this.modalRef?.hide();
    this.toastr.success('Hello world!', 'Toastr fun!');
  }

  decline(): void {
    this.modalRef?.hide();
  }

  detalheEvento(id: number):void{
      this.router.navigate([`eventos/detalhe/${id}`]);
  }
}
