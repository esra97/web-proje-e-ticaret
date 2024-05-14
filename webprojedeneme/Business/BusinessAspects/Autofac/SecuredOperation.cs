using Business.Constants;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using Core.Extensions;


namespace Business.BusinessAspects.Autofac
{
    //yetkilendirme olayı için yaptık mesela işte ürün ekleme yetkisi var mı gibi yönlendirme olayında kullanmak için oluşturduk 
    //JWT için oluşturduk
    public class SecuredOperation : MethodInterception
    {
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor;//her istek için bir thread oluşturur

        public SecuredOperation(string roles)
        {
            _roles = roles.Split(',');//roller virgül ile ayrılarak belirlenir(admin,product.add vb)yani buradakilerin yetkisi var demek oluyor 
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            //autofacte yaptığımız injectionun değerini alıyor 

        }

        protected override void OnBefore(IInvocation invocation)
        {
            //onbefore demek eklediğimiz yerin önünde çalışan method mesela product add kısmının önüne ekleyince burada add kısmından bahsetmiş oluyoruz
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();//bu kullanıcının claim rollerini bul 
            foreach (var role in _roles)//rollerin içinde gez
            {
                if (roleClaims.Contains(role))//eğer ilgili rol varsa return et yani methodu çalıştırmaya devam et
                {
                    return;
                }
            }
            throw new Exception(Messages.AuthorizationDenied);//yoksa hata mesajı ver
        }
    }
}
