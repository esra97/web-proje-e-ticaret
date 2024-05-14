
using Business.Abstract;
using Business.Concreate;
using DataAccess.Concreate.EntityFrameWork;
using DataAccess.Concreate.InMemory;
using Entities.Concreate;
using System;


namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
          // CategoryTest();
           ProductTest();



        }

        //private static void CategoryTest()
        //{
        //    CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());
        //    foreach (var category in categoryManager.GetAll)
        //    {

        //        Console.WriteLine(category.CategoryName);
        //    }
        //}

        private static void ProductTest()
        {
            ProductManager productmanager = new ProductManager(new EfProductDal(), new CategoryManager(new EfCategoryDal()));

            var result = productmanager.GetProductDetails();

            if(result.Success == true)
            {
                foreach (var product in result.Data )
                {
                    Console.WriteLine(product.ProductName + "/" + product.CategoryName);
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }
           
        }
    }
}