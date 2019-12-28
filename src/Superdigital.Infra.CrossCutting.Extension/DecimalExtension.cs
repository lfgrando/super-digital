using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Superdigital.Infra.CrossCutting.Extension
{
    [ExcludeFromCodeCoverage]
    public static class DecimalExtension
    {
        public static string FormatValue(this decimal value)
        {
            return
                $"R$ {value.ToString("n2")}" ;
        }
    }
}
