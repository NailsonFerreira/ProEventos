import { AccountService } from './service/account.service';
import { Component } from '@angular/core';
import { User } from './models/identity/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
    this.setCurrentUser();
  }

  public setCurrentUser() {
    let user: User;

    if (this.accountService.hasUserLogged()) {
      user = this.accountService.getCurrentUser();
    } else {
      user = null;
    }

    if (user)
      this.accountService.setCurrentUser(user);
  }
}
