import { Component, ViewChild, ElementRef } from '@angular/core';
import { ContaService } from '../../../service/conta.service';
import { ContaModel } from '../conta.model';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-conta-pesquisa',
  templateUrl: './conta-pesquisa.component.html'
})

export class ContaPesquisaComponent {
  @ViewChild('nome', { static: false }) nomeInput: ElementRef;
  public conta: ContaModel[];

  constructor(private contaService: ContaService, 
    private toastr: ToastrService) {
    this.get();
  }

  get(): void {
    let nome: string = "null";
    if ((document.getElementById("nome") as HTMLInputElement) != undefined && (document.getElementById("nome") as HTMLInputElement).value != '') {
      nome = (document.getElementById("nome") as HTMLInputElement).value;
      this.contaService.getByName(nome).subscribe(x => {
        this.conta = x.value;
      }, error => {
        error.failures.forEach(item => {
          this.toastr.error(`${item.errorMessage}`, 'An exception has occurred.');
         });
        });
    }
    else{
      this.contaService.get().subscribe(x => {
        this.conta = x.value;
      }, error => {
        error.failures.forEach(item => {
          this.toastr.error(`${item.errorMessage}`, 'An exception has occurred.');
         });
        });
    }
  }

  deleteAccount(id : number): void{
    Swal.fire({
      title: 'Você tem certeza?',
      text: "Você não poderá desfazer esta ação.",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sim, Excluir!',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.value) {
        this.contaService.delete(id).subscribe(x => {
          this.toastr.success('Conta excluída com sucesso!');
          this.get();
        }, error => {
          error.failures.forEach(item => {
            this.toastr.error(`${item.errorMessage}`, 'An exception has occurred.');
           });
        });
      }
    })
  }
}
