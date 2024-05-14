using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.JWT
{
    //kullanıcı kullanıcıadı ve şifrsini doğru girdi diyelim kullanıcı ekranından APİ ye gelecek ve CreateToken operasyonumuzu çalıştıracak
    //burada ilgili kullanıcı için veritabanına gidecek oradan bu kullanıcının claimlerini bulacak
    //ve orada bu bilgileri barındıran (yani operationclaim) JWB oluşturacak ve kullanıcıya verecek 
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<OperationClaim>operationclaims);
    }
}
