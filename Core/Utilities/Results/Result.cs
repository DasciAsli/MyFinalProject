using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    //IResult'ın somut sınıfını yazıyoruz.
    public class Result : IResult
    {
        //get ReadOnlydir.ReadOnlyler constructorlarda set edilebilirler.
        //Constructor dışında set etmeyeceğiz

        //Constructorın kendi içinde farklı yapılarla çalışmasına örnek
        public Result(bool success, string message):this(success) //Bu şekilde constructorı çağırısan this ile içinde success parametresi olan constructor da çağırmış olursun
        {
            Message = message;           
        }
        public Result(bool success)//Overloading(Aşırı yükleme)
        {
            Success = success;
        }
        public bool Success { get; }

        public string Message { get; }
    }
}
