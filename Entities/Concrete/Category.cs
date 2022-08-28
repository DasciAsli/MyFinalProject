using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    //Çıplak Class Kalmasın
    //Eğer bir class herhangi bir inheritence ya da interface implementasyonu almıyorsa ilerde problem yaşama olasılığı yüksektir.
    public class Category:IEntity
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
 