using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Superdigital.Infra.CrossCutting.Extension
{
    [ExcludeFromCodeCoverage]
    public static class DateTimeExtension
    {
        public static string FormatValue(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy");
        }
    }
}