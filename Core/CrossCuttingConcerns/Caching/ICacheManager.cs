using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching
{
    public interface ICacheManager //Teknoloji bağımsız bir interface,istediğin cache yöntemini daha sonra entegre edebilmek için oluşturduk
    {
        T Get<T>(string key);
        //Cacheden getirirken hangi tiple çalıştığımızı ve hangi tipe dönüştürmek istedimizi belirtebiliriz generic yapı ile.
        //Bu metod ile demek istediğimiz verdiğimiz keye karşılık gelen datayi istemek
        object Get(string key);
        //Yukarıdaki metodun generic olmayan hali.Kullanıcı ikisinden hangisini istiyorsa kulanabilir.
        void Add(string key, object value,int duration); //value herşey olabilir bu nedenle object tanımlandı.duration :veri ne kdr cachede duracak?

        bool IsAdd(string key);//Veri cachede var mı kontrolü için kullanılacak.Yoksa ekleyecek
        void Remove(string key);//cacheden silmek için keyi
        void RemoveByPattern(string pattern); //içinde get olan kategori olanları vs sil gibi kullabilmek için oluşturduk
    }
}
