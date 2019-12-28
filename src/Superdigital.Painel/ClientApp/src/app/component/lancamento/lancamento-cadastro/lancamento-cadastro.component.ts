import { Component } from '@angular/core';
import { LancamentoService } from '../../../service/lancamento.service';
import { ContaService } from '../../../service/conta.service';
import { FormGroup, FormControl, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ContaModel } from '../../conta/conta.model';

@Component({
  selector: 'app-lancamento-cadastro',
  templateUrl: './lancamento-cadastro.component.html'
})
export class LancamentoCadastroComponent {
  id: string;
  public cadastroLancamento: FormGroup;
  submitted = false;
  divContaOrigem : boolean
  divContaDestino : boolean
  public contas: ContaModel[]

  get f() { return this.cadastroLancamento.controls; }

  constructor(private formBuilder: FormBuilder,
    private lancamentoService: LancamentoService,
    private contaService: ContaService,
    private router: Router,
    private toastr: ToastrService,
    private route: ActivatedRoute) {
      this.id = this.route.snapshot.paramMap.get('id');
      this.clearForm();
      this.setAccountList();
    }

  add(): void {
    this.submitted = true;

    if (this.cadastroLancamento.invalid) {
      return;
    }

    if (this.id == null) {
      this.cadastroLancamento.get('id').setValue(null);
      this.lancamentoService.add(this.cadastroLancamento.value).subscribe(x => {
        this.toastr.success('Lançamento cadastrado com sucesso!');
        this.cadastroLancamento.reset();
        this.submitted = false;
        this.router.navigate(['/Conta/Pesquisa']);
      }, error => {
        error.failures.forEach(item=> {
          this.toastr.error(`${item.errorMessage}`, 'An exception has occurred.');
         });
        })
    }
  }

  clearForm() : void {
    this.cadastroLancamento = this.formBuilder.group({
      id: this.id,
      eOperacao: [-1, Validators.compose([Validators.required])],
      valor: ['', Validators.compose([Validators.required])],
      idContaOrigem: [-1, Validators.compose([Validators.required])],
      idContaDestino: [-1, Validators.compose([Validators.required])]
    });
    this.divContaOrigem = false
    this.divContaDestino = false
  }

  updateOperation() : void{
    let operacao = this.f.eOperacao.value;
    switch(operacao){
      case 0:
        this.divContaOrigem = false;
        this.divContaDestino = true;
        this.f.idContaOrigem.disable();
        this.f.idContaDestino.enable();
        break;
      case 1:
        this.divContaOrigem = true;
        this.divContaDestino = false;
        this.f.idContaOrigem.enable();
        this.f.idContaDestino.disable();
        break
      case 2:
        this.divContaOrigem = true;
        this.divContaDestino = true;
        this.f.idContaOrigem.enable();
        this.f.idContaDestino.enable();
        break;
      default:
        this.divContaOrigem = false;
        this.divContaDestino = false;
        this.f.idContaOrigem.disable();
        this.f.idContaDestino.disable();
        break
    } 
  }

  setAccountList() : void{
    this.contaService.get().subscribe(x => {
      this.contas = x.value;
    }, error => {
      error.failures.forEach(item => {
        this.toastr.error(`${item.errorMessage}`, 'An exception has occurred.');
       });
      });
  }
}
