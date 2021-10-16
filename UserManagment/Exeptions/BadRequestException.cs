using System;

namespace WebAPI.Exeptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {

        }

    }
}
