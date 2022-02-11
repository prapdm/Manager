using System;

namespace Manager.Exeptions
{
    public class NotFoundExeption : Exception
    {
        public NotFoundExeption(string messege) : base(messege) 
        {
        }
    }
}
