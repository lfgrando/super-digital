using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Superdigital.Infra.CrossCutting.Extension
{
    [ExcludeFromCodeCoverage]
    public static class HttpStatusCodeExtension
    {
        public static bool IsSuccessStatusCode(this HttpStatusCode value)
        {
            int statusCode = (int)value;
            return (statusCode >= 200 && statusCode < 300);
        }
    }
}
