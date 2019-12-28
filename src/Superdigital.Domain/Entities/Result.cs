using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Superdigital.Domain.Entities
{
    public class Result
    {
        public HttpStatusCode StatusCode { get; private set; }
        public IList<Failure> Failures { get; private set; }
        public Result(HttpStatusCode statusCode, IList<Failure> failures)
        {
            StatusCode = statusCode;
            Failures = failures;
        }
        public static string GetErrorMessage()
        {
            return string.Empty;
        }
    }
    public class Result<T> : Result
    {
        public T Value { get; set; }

        public Result(T value, HttpStatusCode statusCode, IList<Failure> failures)
            : base(statusCode, failures)
        {
            Value = value;
        }
    }
}