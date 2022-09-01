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



        //Autofac,Ninject,CastleWindsor,StructureMap,LightInject,DryInject bunlar�n hepsi  IoC yap�s� yokken IoC yerine alt yap� sunuyordu
        //Biz Aop yapmay� planl�yoruz.AOP bir metodun �n�nde,sonunda,metod hata verdi�inde �al��an kod par�ac�klar�n� yazmam�z� sa�lar
        //Autofac bize Aop imkan� sunuyor.O y�zden Autofac'i enjekte edece�iz daha sonra
        builder.Services.AddSingleton<IProductService,ProductManager>(); //Bana arka planda bir referans olu�tur.Yani IoC'ler bizim yerimize newliyor.Bellekte bir tane ProductManager olu�turuyor.Ne kadar client gelirse gelsin hepsine ayn� ProductManager'� veriyor.Ancak i�erisinde data olmamal�
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