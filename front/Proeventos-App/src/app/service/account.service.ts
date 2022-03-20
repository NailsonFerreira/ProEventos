import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '@app/models/identity/user';
import { UserUpdate } from '@app/models/identity/user-update';
import { Observable, ReplaySubject } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from './../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  baseUrl = `${environment.apiURL}api/account/`

  constructor(private http: HttpClient) { }

  public login(model: any): Observable<void> {
    return this.http.post<User>(`${this.baseUrl}login`, model).pipe(
      take(1),
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }
  public register(model:any):Observable<void>{
    return this.http.post<User>(`${this.baseUrl}register`, model).pipe(
      take(1),
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  public setCurrentUser(user: User): void {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  public logout():void{
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.currentUserSource.complete();
  }

  public hasUserLogged():boolean{
    return localStorage.getItem("user")!=null;
  }

  public getCurrentUser():User{
    let user = JSON.parse(localStorage.getItem("user")) as User;
    return user;
  }

  public getUser():Observable<UserUpdate>{
    return this.http.get<UserUpdate>(this.baseUrl+"getUser").pipe(take(1));
  }

  public updateUser(user:UserUpdate):Observable<void>{
    return this.http.put<UserUpdate>(this.baseUrl+"update", user).pipe(
      take(1),
      map((user: UserUpdate)=>{
        this.setCurrentUser(user);
      })
    )
  }
}

