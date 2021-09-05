import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from '../../models/Evento';
import { EventoService } from '../../service/evento.service';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
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
          console.error(error);
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
}
