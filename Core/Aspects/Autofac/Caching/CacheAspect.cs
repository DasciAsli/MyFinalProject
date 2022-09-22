using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.Autofac.Caching
{
    public class CacheAspect : MethodInterception
    {
        private int _duration;
        private ICacheManager _cacheManager;

        public CacheAspect(int duration = 60)//Süre vermezsek 60dk boyunca duracak veri cachede ve sonra atılacak
        {
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        public override void Intercept(IInvocation invocation) //OnBefore'da kullanabilirdik farketmez
        {
            //Methodun ismini bul.ReflectedType.FullName demek namespace + classın ismini al
            //Burada key oluşturmaya çalışıyorum
            var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}");
            var arguments = invocation.Arguments.ToList();
            //metodun parametrelerini listeye çevir
            var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})";
            //string.Join birleştirme için kullanılıyor
            if (_cacheManager.IsAdd(key)) //bellekte böyle bir anahtar var mı kontrol et
            {
                invocation.ReturnValue = _cacheManager.Get(key); //metodu çalıştırmadan geri dön
                return;
            }
            invocation.Proceed();
            _cacheManager.Add(key, invocation.ReturnValue, _duration);
        }
    }
}
