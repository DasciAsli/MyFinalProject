using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();



        //Autofac,Ninject,CastleWindsor,StructureMap,LightInject,DryInject bunlarýn hepsi  IoC yapýsý yokken IoC yerine alt yapý sunuyordu
        //Biz Aop yapmayý planlýyoruz.AOP bir metodun önünde,sonunda,metod hata verdiðinde çalýþan kod parçacýklarýný yazmamýzý saðlar
        //Autofac bize Aop imkaný sunuyor.O yüzden Autofac'i enjekte edeceðiz daha sonra
        builder.Services.AddSingleton<IProductService,ProductManager>(); //Bana arka planda bir referans oluþtur.Yani IoC'ler bizim yerimize newliyor.Bellekte bir tane ProductManager oluþturuyor.Ne kadar client gelirse gelsin hepsine ayný ProductManager'ý veriyor.Ancak içerisinde data olmamalý
        builder.Services.AddSingleton<IProductDal, EFProductDal>();
        var app = builder.Build();  

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}