using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EFProductDal : IProductDal
    {

        //Entity Framework Microsoftun bir ürünü bir ORM ürünüdür.Linq destekli çalışır
        //ORM :Veritabanındaki tabloyu sanki classmış gibi onunla ilişkilendirip operasyonları linq ile yapmak
        //ORM :Veritabanı nesneleriyle kodlar arasında bir bağ kurmayı sağlar.

        //NuGet :Başkalarının kodlarını(paket) kullanabildiğimiz ortam
        public void Add(Product entity)
        {
            //IDisposable pattern impementation of c#
            using (NorthwindContext context= new NorthwindContext())//NorthwindContext bellekten işi bitince kaldırılması için using kullanıyoruz.
            {
                var addedEntity = context.Entry(entity);//veri kaynağımla ilişkilendir.Referansı yakala
                addedEntity.State = EntityState.Added;//O eklenecek bir nesne
                context.SaveChanges();//İşlemleri gerçekleştir.

            }
        }

        public void Delete(Product entity)
        {
            using (NorthwindContext context = new NorthwindContext())//NorthwindContext bellekten işi bitince kaldırılması için using kullanıyoruz.
            {
                var deletedEntity = context.Entry(entity);//veri kaynağımla ilişkilendir.Referansı yakala
                deletedEntity.State = EntityState.Deleted;//O silinecek bir nesne
                context.SaveChanges();//İşlemleri gerçekleştir.

            }

        }

        public Product Get(Expression<Func<Product, bool>> filter)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                return context.Set<Product>().SingleOrDefault(filter);
            }
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                return filter == null ? context.Set<Product>().ToList() :context.Set<Product>().Where(filter).ToList();
            }
        }

        public void Update(Product entity)
        {
            using (NorthwindContext context = new NorthwindContext())//NorthwindContext bellekten işi bitince kaldırılması için using kullanıyoruz.
            {
                var updatedEntity = context.Entry(entity);//veri kaynağımla ilişkilendir.Referansı yakala
                updatedEntity.State = EntityState.Modified;//O silinecek bir nesne
                context.SaveChanges();//İşlemleri gerçekleştir.

            }
        }
    }
}
