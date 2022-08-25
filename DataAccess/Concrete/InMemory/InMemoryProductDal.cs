using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.InMemory
{

    public class InMemoryProductDal : IProductDal
    {
        List<Product> _products;//global değişken
        List<Category> _categories;
        public InMemoryProductDal()
        {
            //Oracle,Sql Server,Postgres,MongoDb gibi veritabanlarından geliyormuş gibi simüle ediyoruz
            _products = new List<Product>
            {
                new Product{ProductId=1,CategoryId=1,ProductName="Bardak",UnitPrice=15,UnitsInStock=15 },
                new Product{ProductId=2,CategoryId=1,ProductName="Kamera",UnitPrice=500,UnitsInStock=3},
                new Product{ProductId=3,CategoryId=2,ProductName="Telefon",UnitPrice=1500,UnitsInStock=2 },
                new Product{ProductId=4,CategoryId=2,ProductName="Klavye",UnitPrice=150,UnitsInStock=65 },
                new Product{ProductId=5,CategoryId=2,ProductName="Fare",UnitPrice=85,UnitsInStock=1 },
            
            
            };//Newlendiği anda bir Product Listesi oluştursun
            _categories = new List<Category>
            {
                new Category{CategoryId=1,CategoryName="X" },
                new Category{CategoryId=2,CategoryName="Y" }
            };
        }
        public void Add(Product product)
        {
            _products.Add(product);
        }

        public void Delete(Product product)
        {

            Product productToDelete = null;
            //LINQ-Language Integrated Query(Dile Gömülü Sorgulama)

            //Linq kullanılmadan yapılma şekli
            //foreach (var p in _products)
            //{
            //    if (product.ProductId==p.ProductId)
            //    {
            //        productToDelete = p;
            //    }
            //}

            //Linq kullanılarak yapılma şekli
            productToDelete = _products.SingleOrDefault(p=>p.ProductId==product.ProductId);//SingleOrDefault tek bir eleman bulmaya yarar
            _products.Remove(productToDelete);
        }      

        public void Update(Product product)
        {
            //Gönderdiğim ürün idsine sahip olan listedeki ürünü bul demek
            Product productToUpdate = _products.SingleOrDefault(p => p.ProductId == product.ProductId);
            productToUpdate.ProductName = product.ProductName;
            productToUpdate.CategoryId = product.CategoryId;
            productToUpdate.UnitPrice = product.UnitPrice;
            productToUpdate.UnitsInStock = product.UnitsInStock;
  
        }       

        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            return _products;
        }

        public Product Get(Expression<Func<Product, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}
