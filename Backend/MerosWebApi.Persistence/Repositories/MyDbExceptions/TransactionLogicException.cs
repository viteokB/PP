using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Persistence.Repositories.MyDbExceptions
{
    public class TransactionLogicException : Exception
    {
        public TransactionLogicException(string message) : base(message)
        {
            
        }
    }
}
