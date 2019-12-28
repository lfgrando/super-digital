using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Superdigital.Infra.CrossCutting.Extension
{
    [ExcludeFromCodeCoverage]
    public static class EnumExtension
    {
        public static string GetDescription(this Enum value)
        {
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

                if (attributes != null && attributes.Any())
                {
                    return attributes.First().Description;
                }

                return value.ToString();
            }
            catch (Exception)
            {
                return value.ToString();
            }
        }
    }
}
