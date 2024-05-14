using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concreate;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concreate.EntityFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolvers.Autofac
{ //bağımlılığı yok etmek için kullanırız interfaceler arası bağımlılık classlar arası bağımlılık
  //bir interface newlenemez olduğu için biz zayıf bağımlılık kullanıyoruk
  //(_) ve constrocter yapısı ile ama autofac ile bu zayıf bağımlılığı da ortadan kaldırmak istiyoruz
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        { //yaptığımız işlemler aslında birisi ıproductdal isterse efproductdal ver demek gibi oluyor
          //WebAPI nin program kısmı veya startup kısmında yaptığımızı burada yapıyoruz yani API ninIOC yapılandırmasını kullanmıyoruz 
          //kendimiz IOC yapısı olarak Autofac kullanıyoruz bunun nedeni AOP yapabilmek
          //bu projeyi farklıdatabase kullanan müşterilere sattığını düşün bu method sayesinde database farkında bile çalışır
          //çünkü neyin neye karşılık geldiğini yazdık
          
            builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance();
            builder.RegisterType<EfProductDal>().As<IProductDal>();

            builder.RegisterType<CategoryManager>().As<ICategoryService>().SingleInstance();
            builder.RegisterType<EfCategoryDal>().As<ICategoryDal>();

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}
