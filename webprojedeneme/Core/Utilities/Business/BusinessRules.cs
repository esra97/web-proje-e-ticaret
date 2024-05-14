using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        public static IResult Run (params IResult[] logics) //params yazma nedenşmşz bir sürü IResult tipinde methodları kullanabilmek için
                                                            //parametre ile gönderdiğimiz iş kurallarının başarısız olanlarını logic ile businesse haber veriyoruz
                                                            //
        {
            foreach(var logic in logics) 
            { 
               if(!logic.Success)
                {
                    return logic;
                }
            }
            return null;

        }
    }
}
