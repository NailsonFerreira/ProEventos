import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  constructor(private http: HttpClient) { }

  public eventos: any = [];
  public eventosFilter: any = [];
  widthImg = 100;
  marginImg = 2;
  showImage = true;
  private _search: string = '';

  public get search(): string {
    return this._search;
  }

  public set search(value: string) {
    this._search = value;
    this.eventosFilter = this._search ? this.filterEvents(value) : this.eventos;
  }

  ngOnInit(): void {
    this.getEventos();
  }

  public getEventos(): void {
    this.http.get("http://localhost:5000/evento").subscribe(
      (response) => {
        this.eventos = response;
        this.eventosFilter = response;
        console.log(response);
      },
      error => console.log(error),
    )
  }

  public setShowImages(): void {
    this.showImage = !this.showImage;
  }

  public filterEvents(value: string) {
    value = value.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: { tema: string, local:string; }) => evento.tema.toLocaleLowerCase().indexOf(value) !== -1 ||
      evento.local.toLocaleLowerCase().indexOf(value) !== -1
    );
  }
}
