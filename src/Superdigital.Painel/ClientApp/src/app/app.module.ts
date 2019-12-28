import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './component/app.component';
import { HttpModule } from '@angular/http';
import { ContaService } from './service/conta.service';
import { ContaCadastroComponent } from './component/conta/conta-cadastro/conta-cadastro.component';
import { ContaPesquisaComponent } from './component/conta/conta-pesquisa/conta-pesquisa.component';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { NgxMaskModule } from 'ngx-mask';
import { NgxCurrencyModule } from 'ngx-currency';
import { LancamentoCadastroComponent } from './component/lancamento/lancamento-cadastro/lancamento-cadastro.component';
import { LancamentoPesquisaComponent } from './component/lancamento/lancamento-pesquisa/lancamento-pesquisa.component';
import { LancamentoService } from './service/lancamento.service';
import { JwtInterceptor } from './helpers/jwt.interceptor';
import { ErrorInterceptor } from './helpers/error.interceptor';
import { LoginComponent } from './component/login/login.component';
import { AuthGuard } from './helpers/auth.guard';
import { SecureComponent, SECURE_ROUTES } from './component/secure/secure-routes.component';
import { PublicComponent, PUBLIC_ROUTES } from './component/public/public-routes.component';

@NgModule({
  declarations: [
    AppComponent,
    SecureComponent,
    PublicComponent,
    ContaCadastroComponent,
    ContaPesquisaComponent,
    LancamentoCadastroComponent,
    LancamentoPesquisaComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    HttpModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    NgxMaskModule.forRoot(),
    NgxCurrencyModule,
    RouterModule.forRoot([
      { path: '', component: SecureComponent, canActivate: [AuthGuard], data: { title: 'Secure Views' }, children: SECURE_ROUTES },
      { path: '', component: PublicComponent, data: { title: 'Public Views' }, children: PUBLIC_ROUTES }
    ])
  ],
  providers: [
    ContaService, 
    LancamentoService,
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
