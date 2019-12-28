import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UsuarioLoginModel } from '../component/login/usuario-login.model';
import { ResultModel } from '../component/result.model';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private currentUserSubject: BehaviorSubject<UsuarioLoginModel>;
    private myApiUrl : string
    public currentUser: Observable<UsuarioLoginModel>;
    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl : string) {
        this.currentUserSubject = new BehaviorSubject<UsuarioLoginModel>(JSON.parse(localStorage.getItem('currentUser')));
        this.currentUser = this.currentUserSubject.asObservable();
        this.myApiUrl = this.baseUrl + '/Login';
    }

    public get currentUserValue(): UsuarioLoginModel {
        return this.currentUserSubject.value;
    }

    login(usuario : UsuarioLoginModel) {
        return this.http.post<ResultModel<UsuarioLoginModel>>(`${this.myApiUrl}`, usuario )
            .pipe(map(user => {
                localStorage.setItem('currentUser', JSON.stringify(user.value));
                this.currentUserSubject.next(user.value);
                return user;
            }));
    }

    logout() {
        // remove user from local storage and set current user to null
        localStorage.removeItem('currentUser');
        this.currentUserSubject.next(null);
    }
}