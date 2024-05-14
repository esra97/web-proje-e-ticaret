using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Utilities.Results
{
    public class SuccessDataResult<T> : DataResult<T>
    {
       //base dediğimiz şey dataresulttır 

        public SuccessDataResult(T data,  string message) : base(data,true, message)
        {
           
        }
        public SuccessDataResult(T data) : base(data,true)
        {
           
        }
        // data döndürmeyeceksek eğer default yaparız değer yoksa yani 
        public SuccessDataResult(string message): base(default,true,message)
        {
                
        }

        public SuccessDataResult() : base(default,true)
        {
                
        }
    }
}
