using Entities.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    //Generic Repository Design Pattern
    //Generic Constraint
    //class : referans tip olmalı
    //IEntity :Ya IEntity olabiir ya da IEntity implemente eden bir nesne olabilir
    //new()   :new'lenebilir olmalı
    public interface IEntityRepository<T> where T : class,IEntity,new()//referans tip olmak zorunda diye kısıtlama koyduk T'ye 
    {

        List<T> GetAll(Expression<Func<T,bool>> filter= null);//interface metodları default olarak publictir.
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
