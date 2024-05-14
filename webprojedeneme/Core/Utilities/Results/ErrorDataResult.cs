using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    
        public class ErrorDataResult<T> : DataResult<T>
        {
            //base dediğimiz şey dataresulttır 

            public ErrorDataResult(T data, string message) : base(data, false, message)
            {

            }
            public ErrorDataResult(T data) : base(data, false)
            {

            }
            // data döndürmeyeceksek eğer default yaparız değer yoksa yani 
            public ErrorDataResult(string message) : base(default, false , message)
            {

            }

            public ErrorDataResult() : base(default, false)
            {

            }
        }
    
}
