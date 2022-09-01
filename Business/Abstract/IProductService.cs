using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductService
    {
        IDataResult<List<Product>> GetAll();
        IDataResult<List<Product>> GetAllByCategoryId(int id);
        IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max);

        IDataResult<List<ProductDetailDto>> GetProductDetails();

        IResult Add(Product product);//Burada IResult döndürmek istediğimi belirtiyorum

        IDataResult<Product> GetById(int productId);



        //Restful yapılar Http protokolü üzerinden geliyor.
        //Http Protokolü:Bir kaynağa ulaşmak için izlediğimiz yol
    }
}
