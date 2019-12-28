using System;

namespace Superdigital.Domain.Entities
{
    public class Lancamento : EntityBase
    {
        public decimal Valor { get; set; }
        public string IdContaOrigem { get; set; }
        public Conta ContaOrigem { get; set; }
        public string IdContaDestino { get; set; }
        public Conta ContaDestino { get; set; }
        public EOperacao EOperacao { get; set; }

        public override bool IsValid(EValidationStage eValidationStage)
        {
            bool validationResult = true;

            if (Valor <= 0)
            {
                validationResult = false;
                this.ValidationErrors.Add(new Failure("IdContaOrigem", "Valor must be highter than R$ 0,00."));
            }
            if (!Enum.IsDefined(typeof(EOperacao), EOperacao))
            {
                validationResult = false;
                this.ValidationErrors.Add(new Failure("EOperacao", "EOperacao is invalid."));
            }
            if (EOperacao == EOperacao.Deposito)
            {
                if (string.IsNullOrWhiteSpace(IdContaDestino))
                {
                    validationResult = false;
                    this.ValidationErrors.Add(new Failure("IdContaDestino", "IdContaDestino can't be null."));
                }
            }
            if (EOperacao == EOperacao.Saque)
            {
                if (string.IsNullOrWhiteSpace(IdContaOrigem))
                {
                    validationResult = false;
                    this.ValidationErrors.Add(new Failure("IdContaOrigem", "IdContaOrigem can't be null."));
                }
            }
            if (EOperacao == EOperacao.Transferencia)
            {
                if (string.IsNullOrWhiteSpace(IdContaOrigem))
                {
                    validationResult = false;
                    this.ValidationErrors.Add(new Failure("IdContaOrigem", "IdContaOrigem can't be null."));
                }
                if (string.IsNullOrWhiteSpace(IdContaDestino))
                {
                    validationResult = false;
                    this.ValidationErrors.Add(new Failure("IdContaDestino", "IdContaDestino can't be null."));
                }
                if (!string.IsNullOrWhiteSpace(IdContaOrigem) && !string.IsNullOrWhiteSpace(IdContaDestino))
                {
                    if (IdContaOrigem == IdContaDestino)
                    {
                        validationResult = false;
                        this.ValidationErrors.Add(new Failure("IdContaOrigem", "IdContaOrigem can't be the same that IdContaDestino."));
                    }
                }
            }
            return validationResult;
        }
    }
}