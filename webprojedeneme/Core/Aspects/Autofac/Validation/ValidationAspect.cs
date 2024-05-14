using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception
    {
        private Type _validatorType;
        public ValidationAspect(Type validatorType)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new System.Exception("bu bir doğrulama sınıfı değildir");
            }

            _validatorType = validatorType;
        }
        protected override void OnBefore(IInvocation invocation)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);//burada verdiğimiz vadiladortypı örneğin product validatörü newledi
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];// burada tipi bukuruz örneğin product tipi
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType);//burada inovacation method demek
                                                                                      //mesela add methodunun entitytypı biizm belirlediğimiz entitytypa eşit ise yani örnekte product demiştik onun içini gez diyor 
            foreach (var entity in entities)
            {
                ValidationTool.Validate(validator, entity);
            }
        }
    }
}
