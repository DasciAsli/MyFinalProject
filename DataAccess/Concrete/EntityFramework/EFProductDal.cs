using Core.DataAccess.EntityFramework;
using Core.Entities;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EFProductDal : EFEntityRepositoryBase<Product, NorthwindContext>, IProductDal
    {
      
        //Entity Framework Microsoftun bir ürünü bir ORM ürünüdür.Linq destekli çalışır
        //ORM :Veritabanındaki tabloyu sanki classmış gibi onunla ilişkilendirip operasyonları linq ile yapmak
        //ORM :Veritabanı nesneleriyle kodlar arasında bir bağ kurmayı sağlar.

        //NuGet :Başkalarının kodlarını(paket) kullanabildiğimiz ortam
        public List<ProductDetailDto> GetProductDetails()
        {
            using (NorthwindContext contex = new NorthwindContext())
            {
                var result = from p in contex.Products
                             join j in contex.Categories
                             on p.CategoryId equals j.CategoryId
                             select new ProductDetailDto 
                             {
                                 ProductId = p.ProductId,
                                 ProductName=p.ProductName,
                                 CategoryName=j.CategoryName,
                                 UnitsInStock=p.UnitsInStock 
                             };
                return result.ToList();
            }
            
        }
    }
}
