using Entities.Concreate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Ürün eklendi";
        public static string ProductNameInvalid = "Ürün ismi geçersiz";
        public static string MaintenanceTime = "Sistem bakımda";
        public static string ProductList = "Ürün listelendi";
        public static string ProductCountOFCategoryError = "Ürün adedi en fazla 10 olmalıdır";
        public static string ProductNameAlreadyExists = "Bu ürün zaten mevcut";
        public static string CategoryLimitExceded = "Category limiti 15 den fazla olduğu için ürün ekleyemiyoruz";
        public static string AuthorizationDenied = "Yetkiniz yok";
    }
}
