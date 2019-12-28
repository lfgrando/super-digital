import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ContaModel } from '../component/conta/conta.model';
import { ResultModel } from '../component/result.model';

@Injectable()
export class ContaService {
  private myApiUrl : string
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl : string)
  {
    this.myApiUrl = this.baseUrl + '/Conta';
  }

  get(): Observable<ResultModel<ContaModel[]>> {
    return this.http.get<ResultModel<ContaModel[]>>(this.myApiUrl)
  }

  getById(id : string): Observable<ResultModel<ContaModel[]>> {
    return this.http.get<ResultModel<ContaModel[]>>(`${this.myApiUrl}/${id}`)
  }

  add(conta: ContaModel): Observable<ResultModel<ContaModel>> {
    return this.http.post<ResultModel<ContaModel>>(`${this.myApiUrl}`, conta)
  }

  edit(id:string, conta: ContaModel, ): Observable<ResultModel<ContaModel>> {
    return this.http.put<ResultModel<ContaModel>>(`${this.myApiUrl}/${id}`, conta)
  }

  delete(id: number, ): Observable<ResultModel<ContaModel>> {
    return this.http.delete<ResultModel<ContaModel>>(`${this.myApiUrl}/${id}`)
  }

  getByName(nome: string): Observable<ResultModel<ContaModel[]>> {
    return this.http.get<ResultModel<ContaModel[]>>(`${this.myApiUrl}/Cliente/Name/${nome}`)
  }

  getAccountNumber(): Observable<any> {
    return this.http.get<any>(`${this.myApiUrl}/Number/Generate`)
  }
}
