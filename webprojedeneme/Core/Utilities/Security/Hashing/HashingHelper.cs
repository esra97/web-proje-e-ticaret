using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Hashing
{
    public class HashingHelper
    {
        //aslında burada oluşturulan şifrenin güvenle tutulması için hashleme işlemi yapıyoruz
        //mesela esraesra gibi şifre oluşturdum onu FTYUSYGG vb şekilde kendi içinde de şifreleyerek oluşturuyor 
        //bunun içinde HMACSHA512 bunu kullansık farklı metodlarda kullanabilirdik bunu tercih ettik
        //out passwordhash deme sebebimiz kullanıcının gönderdiği hash olması
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }


        //burada password hashini doğrulama işlemi yapıyoruz 
        //out kullanmamalıyız çünkü databasedeki password hashi kullanıyoruz dışarıdan almıyoruz 
        //aslında kullanıcı bir şifre girdi onu biz outhash ile hashledik yani şifrenin hash karşılığını kaydettik bunu CreatePaswordHash kısmında yaptık
        //burada da kullanıcının databaseye kaydettiğimiz hash değeri birdaha sisteme giriş yapmak istediği şifre ile aynı mı onu kontrol ediyoruz o yüzden doğrulama kısmı burasıdır
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {

            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))//key istedi yukarıda keyi passwordsalta eşitlediğimiz için bu değeri atadık
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {//giriş kısmında passwordhasini yaptık şifrenin doğrulama kısmında computedhasini yaptık
                 //kaydolan şifre ile giriş yapılmak istenen şifre aynı mı diye kontrol ederken hashleri üzerinden kontrol ediyoruz
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
