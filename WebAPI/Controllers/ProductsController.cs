using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]   //İsteği yaparken insanların nasıl ulaşacağına dair bilgi verir.
    [ApiController]               //ATTRIBUTE(Bir class ile ilgili bilgi vermek için kullanılır)
    public class ProductsController : ControllerBase
    {
        //Loosely Coupled (Gevşek bağımlılık yani soyuta bağımlılık var.Managerı değiştirirsek herhangi bir problemle karşılaşmayacağız )
        //_productService yazım şekli bir naming convention.Bunu bir standart olarak belirleyebilirsin
        //IoC Container :Inversion of Control(Değişimin kontrolü demek.Bellekteki bir kutu gibi düşün ve içine istediğim referansları verelim demek.Böylelikle _productService neyi referans edebileceğini kutu içerisinden seçebilecek )
        IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            //Dependency Chain
            var result = _productService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
            //Gerçek hayatta apiyi geliştirenler bu apinin ne verdiğiyle ilgili dökümentasyon sağlarlar.
            //Bunun için hazır dökümantasyon imkanı sunan Swagger gibi ürünler vardır
        }


        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _productService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);

            //Apini kullanan kişide kafa karışıklığı oluşturmamak için standart bir transaction kullanman daha iyi olur.
        }


        [HttpPost("add")]
        public IActionResult Add(Product product)
        {

            var result = _productService.Add(product);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        
        }



    }
}
