import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from '../login/login.component';
import { Component } from '@angular/core';

@Component({
  selector: 'app-public',
  templateUrl: './public-routes.component.html'
})
export class PublicComponent {
}

/** public.routes component*/
export const PUBLIC_ROUTES: Routes = [
  { path: 'Login', component: LoginComponent, pathMatch: 'full' }
]