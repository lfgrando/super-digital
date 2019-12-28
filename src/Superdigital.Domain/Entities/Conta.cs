using System;
using System.Net;
using System.Threading.Tasks;

namespace Superdigital.Domain.Entities
{
    public class Conta : EntityBase
    {
        //Default constructor
        public Conta()
        {

        }
        //Define the initial value of balance
        public Conta(decimal saldo)
        {
            Saldo = saldo;
        }
        //This field value can be changed only by deposit, withdraw and transfer operations
        public decimal Saldo { get; private set; }
        public Cliente Cliente { get; set; }
        public string Numero { get; set; }
        public EContaTipo EContaTipo { get; set; }

        public Task<Result<decimal>> IncreaseBalance(decimal valor)
        {
            Saldo += valor;
            return Task.FromResult(new Result<decimal>(Saldo, HttpStatusCode.OK, null));
        }

        public Task<Result<decimal>> DecreaseBalance(decimal valor)
        {
            if (Saldo - valor < 0)
                return Task.FromResult(new Result<decimal>(0, HttpStatusCode.BadRequest, Failure.GenerateOneFailure("Insufficient funds.")));

            Saldo -= valor;
            return Task.FromResult(new Result<decimal>(Saldo, HttpStatusCode.OK, null));
        }

        public override bool IsValid(EValidationStage eValidationStage)
        {
            bool validationResult = true;

            if (Cliente == null)
            {
                validationResult = false;
                this.ValidationErrors.Add(new Failure("Cliente", "Cliente can't be null."));
            }
            if (string.IsNullOrWhiteSpace(Numero))
            {
                validationResult = false;
                this.ValidationErrors.Add(new Failure("Numero", "Numero can't be null."));
            }
            if (Cliente != null && string.IsNullOrWhiteSpace(Cliente.Nome))
            {
                validationResult = false;
                this.ValidationErrors.Add(new Failure("Cliente.Nome", "Cliente.Nome can't be null."));
            }
            if (Cliente != null && string.IsNullOrWhiteSpace(Cliente.Email))
            {
                validationResult = false;
                this.ValidationErrors.Add(new Failure("Cliente.Email", "Cliente.Email can't be null."));
            }
            if (eValidationStage == EValidationStage.Replace)
            {
                if (string.IsNullOrWhiteSpace(Id))
                {
                    validationResult = false;
                    this.ValidationErrors.Add(new Failure("Id", "Id can't be null."));
                }
            }
            if (!Enum.IsDefined(typeof(EContaTipo), EContaTipo))
            {
                validationResult = false;
                this.ValidationErrors.Add(new Failure("EContaTipo", "EContaTipo is invalid."));
            }

            return validationResult;

        }
    }
}
