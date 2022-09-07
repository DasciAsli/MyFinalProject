using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        //İş kodları
        //Kural : Bir iş sınıfı başka sınıfları newlemez

        //Bir EntityManager kendisi hariç başka bir dalı enjekte edemez.
        IProductDal _productDal;
        ICategoryService _categoryService;
       
        public ProductManager(IProductDal productDal,ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        [ValidationAspect(typeof(ProductValidator))] //Add metodunu ProductValidatordeki kurallara göre doğrula
        public IResult Add(Product product)
        {

            //İş kurallarımızı uyguladık yazdığımız iş motoru yardımıyla
            IResult result=  BusinessRules.Run
                (
                CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                CheckIfProductNameExists(product.ProductName),
                CheckIfCategoryLimitExceded()
                );

            if (result != null)
            {
                return result;
            }

            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);         

            //Business codelar burada yazılır.
            //Antipattern : Kötü kullanım demek
            //Magic strings:Stringleri ayrı ayrı her yerde yazmak.

            //loglama
            //cacheremove
            //performance
            //transaction
            //authorization
            //yukarıdaki işlemleri burada yapmak yerine [] (attributeler) ile bunları yapacağız

        }

        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour == 24)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(),Messages.ProductsListed);

        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id)) ;
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }


        [ValidationAspect(typeof(ProductValidator))]
        public IResult Update(Product product)
        {
             _productDal.Update(product);
             return new SuccessResult(Messages.ProductUpdated);
        }



        //İş kuralı1- Bir kategoride en fazla 10 ürün olabilir.
        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            List<Product> productList = _productDal.GetAll(p => p.CategoryId == categoryId);
            var urunAdeti = productList.Count;
            if (urunAdeti > 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);

            }

            return new SuccessResult();
        }


        //İş kuralı2-Aynı isimde ürün eklenemez.
        private IResult CheckIfProductNameExists(string productName) 
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }

        //İş kuralı3-Eğer mevcut kategori sayısı 15i geçtiyse sisteme yeni ürün eklenemez.
        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count > 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccessResult();
        }
    }
}
