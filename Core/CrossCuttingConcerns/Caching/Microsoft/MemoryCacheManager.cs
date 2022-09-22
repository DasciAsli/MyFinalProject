using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching.Microsoft
{
    public class MemoryCacheManager : ICacheManager
    {
        //Adapter Pattern(Adaptasyon desenini uyguladık.Var olan sistemi kendi sistemime uyacak şekilde kullanıyorum)

        IMemoryCache _memoryCache; //Microsoftun Cache ile ilgili yazdığı kütüphanenin içerisindeki bir Interface

        public MemoryCacheManager()
        {
            _memoryCache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
        }

        public void Add(string key, object value, int duration)
        {
            _memoryCache.Set(key, value,TimeSpan.FromMinutes(duration));//Timespan zaman aralığı demek
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public bool IsAdd(string key)
        {
            return _memoryCache.TryGetValue(key,out _);
            //out referans tip için kullanılıyor.
            //Ben bir şey döndürmesini istemiyorsam out _ tekniği kullanarak o parametrenin yerini doldurabilirim
            //Bu sayede sadece o key memoryde var mı yok mu bunun sonucunu bool olarak bize döndürecek
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        //MemoryCache'in dökümantasyonundan aşağıdaki kodları bulabilirsin
        public void RemoveByPattern(string pattern)//Çalışma anında bellekten silmeye yarıyor.Reflection kullanarak yapıyoruz
        {
            var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //Bellekte MemoryCache türündeki dataları EntriesCollectionda tutyor.           
            var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_memoryCache) as dynamic;
            //Bellekte EntriesCollection'ı bul
            
            List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();
            //Her bir cache elemanini gez 
            foreach (var cacheItem in cacheEntriesCollection)
            {
                ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
                cacheCollectionValues.Add(cacheItemValue);
            }

            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //Patternları bu şekilde oluşturuyoruz.
            var keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();
            //regex kuralına uyanları bul
            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
            }
            //Bulduklarını sil bellekten
        }
    }
}
