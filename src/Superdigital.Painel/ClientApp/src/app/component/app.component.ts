import { Component } from '@angular/core';
import { AuthenticationService } from '../service/authentication.service';
import { UsuarioLoginModel } from './login/usuario-login.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent {
  title = 'app';
  currentUser: UsuarioLoginModel;

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService
  ) {
      this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
  }
}
