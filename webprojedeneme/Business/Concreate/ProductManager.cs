using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.Validation_Rules.FulentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concreate.InMemory;
using Entities.Concreate;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concreate
{
    //business (iş) kodu Mesela ehliyet elırken alacak kişinin yeterliliklerine bakmak gibi düşün
    //bir e ticaret sayfasında sepete en fazla 10 ürün eklenebilir şartı tarzı kısıtlamaları business katmanı içerisine yazarız
    //validation (doğrula) kodu ise eklemek istediğmiz ürünün yapısal olarak uygun olup olmadığını kontrol eder
    //mesela bir şifre gireceksin burada girdiğin şifre verilen kısıtlamaklara uygun mu o kontrol edilir 
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;
        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }
        [SecuredOperation("product.add , admin")]
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            //if(product.ProductName.Length<2)
            //{
            //    return new ErrorResult(Messages.ProductNameInvalid);
            //} validationd ayazdığımız için buradan silebilriz 


            // bir kategori de en fazla 10 ürün olabilirin kodunu aşağıdaki gibi yazarsan
            // update kısmına da aynı kodu yazman gerkecek ve kendini tekrar eden bi koda dönüşecek biz bunu istemiyoruz
            //var results =  _productDal.GetAll(p=>p.CategoryId==product.CategoryId).Count;
            //if(results>=10)
            //{
            //    return new ErrorResult(Messages.ProductCountOFCategoryError);
            //}

            //bunun yerine oluşturduğumuz CheckIfProductCountOfCategoryCorrect metodunu yazmalıyız şuanlık iş kuralları yok 
            //if(CheckIfProductCountOfCategoryCorrect(product.CategoryId).Success)
            //{
            //    if(CheckIfProductNameExists(product.ProductName).Success)
            //    {
            //        _productDal.Add(product);
            //        return new Result(true, Messages.ProductAdded);
            //    }

            //}
            //return new ErrorResult();

            //iş kuralları oluşturulunca yapıyı böyle kurmalıyız buradaki run içine yazılı methodlar aslında parametrelerimizdir
            //iş kurallarını oluşturma sebebeimiz ilerde bşr iş geldiğinde run içine bu methodu yazıp altta da fonksyonunu oluşturup eklemktir

            IResult result = BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                CheckIfProductNameExists(product.ProductName), CheckIfCategoryLimitExceded());
            //result bıradaki iş kurallarına uymayan methodun sonucudur
            //burada if koşulu ile uymayan kuralı döndürürüz eğer result içi null yani boşsa hepsne uymuş demektir bir şey döndürmeye gerek yok 
            if (result != null)
            {
                return result;
            }
            _productDal.Add(product);
            return new Result(true, Messages.ProductAdded);
        }

        public IDataResult<List<Product>> GetAll()
        {
            /*if (DateTime.Now.Hour == 16)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }*/
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductList);
        }

         public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<Product> GetId(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {

            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        
        // bir kategori de en fazla 10 ürün olabilir
        private IResult CheckIfProductCountOfCategoryCorrect(int Categoryid)
        {
            var results = _productDal.GetAll(p => p.CategoryId == Categoryid).Count;
            if (results >= 10)
            {
                return new ErrorResult(Messages.ProductCountOFCategoryError);
            }
            return new SuccessResult();
        }

        //aynı isimden olan ürünü birdaha ekleyemeyiz bunun kuralı 
        private IResult CheckIfProductNameExists(string productname)
        {
            var result = _productDal.GetAll(p => p.ProductName == productname).Any();//any demek buna uyan kayıt var mı bunu kontrol ediyor
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);

            }
            return new SuccessResult();
        }

        //eğer mevcut kategori sayısı 15 i geçtiyse sisteme ürün eklenemez
        //bunun için categorydalı kullanıp get metodunu çağırmamız lazım fakat şu yanlıştır
        //publicProductManager(IProductDal productDal,ICategoryDal categoryDal)
        //{
        //    _productDal = productDal;
        //    _categoryDal=categoryDal;
        //} bie manager başka entitiesin dalını implement edemez!!!
        //fakat servis kısmını yazabiliriz



        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count > 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccessResult();
        }

        public IResult Update(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
