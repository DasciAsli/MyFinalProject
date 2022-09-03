using Autofac;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)//Uygulama yayınlandığında çalışır.
        {
            builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance();
            //Birisi IProductService isterse ona bir tane ProductManager instance'ı ver
            builder.RegisterType<EFProductDal>().As<IProductDal>().SingleInstance();

        }
    }
}
