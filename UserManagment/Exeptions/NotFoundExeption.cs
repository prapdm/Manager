using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Exeptions
{
    public class NotFoundExeption : Exception
    {
        public NotFoundExeption(string messege) : base(messege) 
        {
        }
    }
}
