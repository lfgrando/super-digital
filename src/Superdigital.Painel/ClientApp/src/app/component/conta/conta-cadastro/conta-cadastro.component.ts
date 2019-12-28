import { Component } from '@angular/core';
import { ContaService } from '../../../service/conta.service';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-conta-cadastro',
  templateUrl: './conta-cadastro.component.html'
})
export class ContaCadastroComponent {
  id: string;
  public cadastroConta: FormGroup;
  submitted = false;

  get f() { return this.cadastroConta.controls; }

  constructor(private formBuilder: FormBuilder,
    private contaService: ContaService,
    private router: Router,
    private toastr: ToastrService,
    private route: ActivatedRoute) {
      this.id = this.route.snapshot.paramMap.get('id');
      this.clearForm();
    }

  add(): void {
    this.submitted = true;

    if (this.cadastroConta.invalid) {
      return;
    }

    if (this.id == null) {
      this.cadastroConta.get('id').setValue(null);
      this.contaService.add(this.cadastroConta.value).subscribe(x => {
        this.toastr.success('Conta cadastrada com sucesso!');
        this.cadastroConta.reset();
        this.submitted = false;
        this.router.navigate(['/Conta/Pesquisa']);
      }, error => {
        error.failures.forEach(item => {
          this.toastr.error(`${item .errorMessage}`, 'An exception has occurred.');
         });
      })
    }
    else{
      this.contaService.edit(this.id, this.cadastroConta.value).subscribe(x => {
        this.toastr.success('Conta alterada com sucesso!');
        this.cadastroConta.reset();
        this.submitted = false;
        this.router.navigate(['/Conta/Pesquisa']);
      }, error => {
        error.failures.forEach(childObj=> {
          this.toastr.error(`${childObj.errorMessage}`, 'An exception has occurred.');
         });
      })
    }
  }

  updateForm(conta: any): void {
    this.cadastroConta.patchValue({
      eContaTipo: conta.eContaTipo,
      numero: conta.numero,
      cliente: {
        nome: conta.cliente.nome,
        email: conta.cliente.email
      }
    });
  }

  clearForm() : void {
    this.cadastroConta = this.formBuilder.group({
      id: this.id,
      eContaTipo: [-1, Validators.compose([Validators.required])],
      numero: ['', Validators.compose([Validators.required])],
      cliente: this.formBuilder.group({
        nome: ['', Validators.compose([Validators.required])],
        email: ['', Validators.compose([Validators.required,Validators.email])],
      })
    });

    if(this.id !== null){
      this.contaService.getById(this.id).subscribe(x => {
        this.updateForm(x.value);
      }, error => {
        error.failures.forEach(item => {
          this.toastr.error(`${item .errorMessage}`, 'An exception has occurred.');
         });
        });
    }
  }

  generateAccountNumber() : void{
    this.contaService.getAccountNumber().subscribe(x => {
      this.cadastroConta.patchValue({
        numero: x.value
      });
    }, error => {
      error.failures.forEach(item => {
        this.toastr.error(`${item .errorMessage}`, 'An exception has occurred.');
       });
      });
  }
}
