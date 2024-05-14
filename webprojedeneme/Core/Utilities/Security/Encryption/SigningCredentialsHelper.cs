using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Encryption
{
    //credential bir sisteme girebilmek için elimizde olanlardır (şifre kullanıcıadı vb)
    //burada bizim anahtarımız securitykey olduğu için parametre olarak onu veririz
    public class SigningCredentialsHelper
    {
        public static SigningCredentials CreateSigningCredentials (SecurityKey securityKey)
        {
            //Burada güvenlik anahtarı ve şifreleme algoritmasını da yazmış bulunduk
            return new SigningCredentials (securityKey,SecurityAlgorithms.HmacSha512Signature);
        }
    }
}
