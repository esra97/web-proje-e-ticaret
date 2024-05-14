using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.JWT
{ //tokenı bir jeton gibi düşün kullanıcoya jeton bilgisi ve bitiş süresini bu classta tanımlıyoruz
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
