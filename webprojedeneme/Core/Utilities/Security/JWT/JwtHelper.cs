using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }//IConfiguration bizim APİ mizdeki apsettings.json dosyasındaki değrrleri okumamıza yarıyor 
        private TokenOptions _tokenOptions;//IConfigurationun okuduğu değerleri bir nesneye atar
        private DateTime _accessTokenExpiration;//Tokenın bitme süresi bunu api deki apsetting.jsonda 10 olarak belirtmiştik
        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();//Burada configurationstaki tokenoptions kısmına gidiyor
                                                                               //içindeki değerleri token optionsta oluşturduğumuz nesnelere eşitliyor 

        }
        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
        {//burada kullanıcı için token oluşturuyoruz

            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);//tokenın bitme süresi
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);//tokenı oluşturacak güvenlik anahtarı
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);//anahtar nedir ve hangi algoritmayı kullanmalıyım
                                                                                                    //kısmını burada oluşturduk 

            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };

        }

        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials, List<OperationClaim> operationClaims)
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
            );
            return jwt;
        }

        private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
        {
            var claims = new List<Claim>();//bu .nette zaten var olan bi class biz buna kendi metrodlarımızı eklemek istiyoruz (AddEmail,AddName vb)
                                           //bunun için core extensions dosyası oluşturduk ve içine claimextensions classını oluşturduk
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());

            return claims;
        }
    }
}
