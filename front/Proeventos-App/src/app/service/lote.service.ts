import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Lote } from '@app/models/Lote';
import { Observable } from 'rxjs';
import { take } from 'rxjs/internal/operators/take';

@Injectable()
export class LoteService {



private baseUrl = "http://localhost:5000/lote";
  constructor(private http: HttpClient) { }

  getLotesByEventoId(id: number): Observable<Lote> {
    return this.http.get<Lote>(`${this.baseUrl}/${id}`).pipe(take(1));
  }

  saveLotes(eventoId: number, lotes: Lote[]):Observable<Lote[]>{
    return this.http.put<Lote[]>(`${this.baseUrl}?eventoId=${eventoId}`, lotes).pipe(take(1));
  }

  deleteLote(eventoId: number, loteId: number): Observable<any> {
    console.log(`${this.baseUrl}?eventoId=${eventoId}&loteId=${loteId}`);
    return this.http.delete<any>(`${this.baseUrl}?eventoId=${eventoId}&loteId=${loteId}`).pipe(take(1));
  }
}
