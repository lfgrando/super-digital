import { Routes, RouterModule, Router } from '@angular/router';
import { ContaCadastroComponent } from '../conta/conta-cadastro/conta-cadastro.component';
import { ContaPesquisaComponent } from '../conta/conta-pesquisa/conta-pesquisa.component';
import { LancamentoPesquisaComponent } from '../lancamento/lancamento-pesquisa/lancamento-pesquisa.component';
import { LancamentoCadastroComponent } from '../lancamento/lancamento-cadastro/lancamento-cadastro.component';
import { AuthGuard } from '../../helpers/auth.guard';
import { Component } from '@angular/core';
import { AuthenticationService } from 'src/app/service/authentication.service';

@Component({
  selector: 'app-secure',
  templateUrl: './secure-routes.component.html'
})
export class SecureComponent {
  constructor(
    private router: Router,
    private authenticationService: AuthenticationService
  ) {
  }

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['/Login']);
  }
}
export const SECURE_ROUTES: Routes = [
  { path: '', component: ContaPesquisaComponent, pathMatch: 'full',  canActivate: [AuthGuard] },
  { path: 'Conta/Pesquisa', component: ContaPesquisaComponent, pathMatch: 'full',  canActivate: [AuthGuard] },
  { path: 'Conta/Cadastro', component: ContaCadastroComponent, pathMatch: 'full',  canActivate: [AuthGuard] },
  { path: 'Conta/Alterar/:id', component: ContaCadastroComponent, pathMatch: 'full',  canActivate: [AuthGuard] },
  { path: 'Lancamento/Cadastro', component: LancamentoCadastroComponent, pathMatch: 'full',  canActivate: [AuthGuard] },
  { path: 'Lancamento/Pesquisa/:id', component: LancamentoPesquisaComponent, pathMatch: 'full',  canActivate: [AuthGuard] }
];