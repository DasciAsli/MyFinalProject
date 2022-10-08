using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Business.Abstract;
using Business.Concrete;
using Business.DependencyResolvers.Autofac;
using Core.DependencyResolvers;
using Core.Extensions;
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
        builder.Services.AddDependencyResolvers(new ICoreModule[] {
          new CoreModule()
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();



        //Autofac,Ninject,CastleWindsor,StructureMap,LightInject,DryInject bunlarýn hepsi  IoC yapýsý yokken IoC yerine alt yapý sunuyordu
        //Biz Aop yapmayý planlýyoruz.AOP bir metodun önünde,sonunda,metod hata verdiðinde çalýþan kod parçacýklarýný yazmamýzý saðlar
        //Autofac bize Aop imkaný sunuyor.O yüzden Autofac'i enjekte edeceðiz daha sonra
        //builder.Services.AddSingleton<IProductService,ProductManager>(); //Bana arka planda bir referans oluþtur.Yani IoC'ler bizim yerimize newliyor.Bellekte bir tane ProductManager oluþturuyor.Ne kadar client gelirse gelsin hepsine ayný ProductManager'ý veriyor.Ancak içerisinde data olmamalý
        //builder.Services.AddSingleton<IProductDal, EFProductDal>();

        //CORS injectioný yapýldý Apide
        builder.Services.AddCors();


        //Autofac için gerekli konfigürasyon
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
        app.ConfigureCustomExceptionMiddleware();

        app.UseCors(builder=>builder.WithOrigins("http://localhost:5100").AllowAnyHeader());//Bunun yazýlma sýrasý önemli.Bu adresten gelen isteklere izin ver

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();

        //Token ile authorize edilecek kullanici
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}