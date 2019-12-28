using System;
using System.Collections.Generic;
using System.Text;

namespace Superdigital.Domain.Entities
{
    public class Cliente : EntityBase
    {
        public string Nome { get; set; }
        public string Email { get; set; }

        public override bool IsValid(EValidationStage eValidationStage)
        {
            return true;
        }
    }
}