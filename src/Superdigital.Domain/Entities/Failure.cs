using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Superdigital.Domain.Entities
{
    public class Failure
    {
        public Failure()
        {
            this.Extensions = new Dictionary<string, object>();
        }
        public Failure(string errorMessage)
        {
            this.Extensions = new Dictionary<string, object>();
            this.ErrorMessage = errorMessage;
        }
        public Failure(string errorMessage, ExecutionContext context, HttpRequest req)
        {
            this.Extensions = new Dictionary<string, object>();
            this.ErrorMessage = errorMessage;
            this.Instance = req?.HttpContext?.Request?.Path;
            this.Extensions.Add("invocationId", context?.InvocationId);
        }
        public Failure(string propertyName, string errorMessage)
        {
            this.Extensions = new Dictionary<string, object>();
            this.PropertyName = propertyName;
            this.ErrorMessage = errorMessage;
        }
        public Failure(string propertyName, string errorMessage, ExecutionContext context, HttpRequest req)
        {
            this.Extensions = new Dictionary<string, object>();
            this.PropertyName = propertyName;
            this.ErrorMessage = errorMessage;
            this.Instance = req?.HttpContext?.Request?.Path;
            this.Extensions.Add("invocationId", context?.InvocationId);
        }
        public static IList<Failure> GenerateOneFailure(string errorMessage)
        {
            return new List<Failure> { new Failure(errorMessage) };
        }
        public static IList<Failure> GenerateOneFailure(string errorMessage, ExecutionContext context, HttpRequest req)
        {
            return new List<Failure> { new Failure(errorMessage, context, req) };
        }
        public static IList<Failure> GenerateOneFailure(string propertyName, string errorMessage)
        {
            return new List<Failure> { new Failure(propertyName, errorMessage) };
        }
        public static IList<Failure> GenerateOneFailure(string propertyName, string errorMessage, ExecutionContext context, HttpRequest req)
        {
            return new List<Failure> { new Failure(propertyName, errorMessage, context, req) };
        }

        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "instance")]
        public string Instance { get; set; }
        [JsonExtensionData]
        public IDictionary<string, object> Extensions { get; }
    }
}
