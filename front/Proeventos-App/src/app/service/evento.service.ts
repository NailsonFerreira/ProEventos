import { HttpClient } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Evento } from '../models/Evento';

@Injectable({
  providedIn: 'root'
})

export class EventoService {
  private baseUrl = "http://localhost:5000/evento";
  constructor(private http: HttpClient) { }



  getEventos(): Observable<Evento[]> {
    return this.http.get<Evento[]>(this.baseUrl).pipe(take(1));
  }

  getEventosByTema(tema: string): Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseUrl}/${tema}/tema`).pipe(take(1));
  }

  getEventoById(id: number): Observable<Evento> {
    return this.http.get<Evento>(`${this.baseUrl}/${id}`).pipe(take(1));
  }

  post(evento: Evento):Observable<Evento>{
    return this.http.post<Evento>(this.baseUrl, evento).pipe(take(1));
  }

  put(evento: Evento):Observable<Evento>{
    return this.http.put<Evento>(`${this.baseUrl}?id=${evento.id}`, evento);//.pipe(take(1));
  }

  deleteEvento(id: number): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}?id=${id}`).pipe(take(1));
  }
}
