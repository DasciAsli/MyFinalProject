using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework
{
    public class EFEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public void Add(TEntity entity)
        {
            //IDisposable pattern impementation of c#
            using (TContext context = new TContext())//NorthwindContext bellekten işi bitince kaldırılması için using kullanıyoruz.
            {
                var addedEntity = context.Entry(entity);//veri kaynağımla ilişkilendir.Referansı yakala
                addedEntity.State = EntityState.Added;//O eklenecek bir nesne
                context.SaveChanges();//İşlemleri gerçekleştir.

            }
        }

        public void Delete(TEntity entity)
        {
            using (TContext context = new TContext())//NorthwindContext bellekten işi bitince kaldırılması için using kullanıyoruz.
            {
                var deletedEntity = context.Entry(entity);//veri kaynağımla ilişkilendir.Referansı yakala
                deletedEntity.State = EntityState.Deleted;//O silinecek bir nesne
                context.SaveChanges();//İşlemleri gerçekleştir.

            }

        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (TContext context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(filter);
            }
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext context = new TContext())
            {
                return filter == null ? context.Set<TEntity>().ToList() : context.Set<TEntity>().Where(filter).ToList();
            }
        }

        public void Update(TEntity entity)
        {
            using (TContext context = new TContext())//NorthwindContext bellekten işi bitince kaldırılması için using kullanıyoruz.
            {
                var updatedEntity = context.Entry(entity);//veri kaynağımla ilişkilendir.Referansı yakala
                updatedEntity.State = EntityState.Modified;//O silinecek bir nesne
                context.SaveChanges();//İşlemleri gerçekleştir.

            }
        }
    }
}

