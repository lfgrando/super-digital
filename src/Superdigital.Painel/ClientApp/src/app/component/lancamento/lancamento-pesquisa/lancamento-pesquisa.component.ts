import { Component } from '@angular/core';
import { LancamentoService } from '../../../service/lancamento.service';
import { LancamentoModel } from '../lancamento.model';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-lancamento-pesquisa',
  templateUrl: './lancamento-pesquisa.component.html'
})

export class LancamentoPesquisaComponent {
  id: string;
  public lancamento: LancamentoModel[]

  constructor(private lancamentoService: LancamentoService,
    private route: ActivatedRoute,
    private toastr: ToastrService) {
    this.id = this.route.snapshot.paramMap.get('id');
    this.getByIdConta(this.id);
  }

  getByIdConta(id : string): void {
    this.lancamentoService.getByIdConta(id).subscribe(x => {
      this.lancamento = x.value;
    }, error => {
      error.failures.forEach(item=> {
        this.toastr.error(`${item.errorMessage}`, 'An exception has occurred.');
       });
      });
  }
}
