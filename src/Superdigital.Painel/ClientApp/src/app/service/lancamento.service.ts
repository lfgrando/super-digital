import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LancamentoModel } from '../component/lancamento/lancamento.model';
import { ResultModel } from '../component/result.model';

@Injectable()
export class LancamentoService {
  private myApiUrl : string
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl : string)
  {
    this.myApiUrl = this.baseUrl + '/Lancamento';
  }

  getByIdConta(id : string): Observable<ResultModel<LancamentoModel[]>> {
    return this.http.get<ResultModel<LancamentoModel[]>>(`${this.myApiUrl}/${id}`)
  }

  add(conta: LancamentoModel, ): Observable<ResultModel<LancamentoModel>> {
    return this.http.post<ResultModel<LancamentoModel>>(`${this.myApiUrl}`, conta)
  }
}