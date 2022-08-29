using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public static class Messages //Newlemeye gerek kalmaması için static yaptık.
    {
        //public değişkenler PascalCase yazılır.
        public static string ProductAdded = "Ürün eklendi";
        public static string ProductNameInvalid = "Ürün ismi geçersiz";
        public static string MaintenanceTime = "Sistem bakımda";
        public static string ProductsListed ="Ürünler listelendi";


        //Her projede sonuçta bu hata mesajları kullanılmayacak.
        //O yüzden business katmanına yazdık bu mesajları
    }
}
