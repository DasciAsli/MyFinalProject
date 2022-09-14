using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Business.Abstract;
using Business.Concrete;
using Business.DependencyResolvers.Autofac;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();




        //API'ye jwt kullanilacagini bildiriliyor
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                };
            });
        ServiceTool.Create(builder.Services);






        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();



        //Autofac,Ninject,CastleWindsor,StructureMap,LightInject,DryInject bunlar�n hepsi  IoC yap�s� yokken IoC yerine alt yap� sunuyordu
        //Biz Aop yapmay� planl�yoruz.AOP bir metodun �n�nde,sonunda,metod hata verdi�inde �al��an kod par�ac�klar�n� yazmam�z� sa�lar
        //Autofac bize Aop imkan� sunuyor.O y�zden Autofac'i enjekte edece�iz daha sonra
        //builder.Services.AddSingleton<IProductService,ProductManager>(); //Bana arka planda bir referans olu�tur.Yani IoC'ler bizim yerimize newliyor.Bellekte bir tane ProductManager olu�turuyor.Ne kadar client gelirse gelsin hepsine ayn� ProductManager'� veriyor.Ancak i�erisinde data olmamal�
        //builder.Services.AddSingleton<IProductDal, EFProductDal>();




        //AOP'ye ge�tikten sonra Businessda i�lemlerimizi yapt�k
        //Autofac i�in gerekli konfig�rasyon
         builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
         .ConfigureContainer<ContainerBuilder>(builder =>
         {
            builder.RegisterModule(new AutofacBusinessModule());
         });


    




        var app = builder.Build();  

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        //Token ile authorize edilecek kullanici
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}