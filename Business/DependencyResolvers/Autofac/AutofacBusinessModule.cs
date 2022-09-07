using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
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
            //Autofac IoC
            builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance();
            //Birisi IProductService isterse ona bir tane ProductManager instance'ı ver
            builder.RegisterType<EFProductDal>().As<IProductDal>().SingleInstance();
            builder.RegisterType<CategoryManager>().As<ICategoryService>().SingleInstance();
            //Birisi IProductService isterse ona bir tane ProductManager instance'ı ver
            builder.RegisterType<EFCategoryDal>().As<ICategoryDal>().SingleInstance();

            //Autofac Aspect
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();

        }
    }
}
