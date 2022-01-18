using System;

namespace Immo.Models
{
    public class CustomException : Exception
    {
        //public CustomException() { }
        public CustomException(string message) : base(message) { }
    }
}
