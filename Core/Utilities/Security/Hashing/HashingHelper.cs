using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Hashing
{
    public class HashingHelper
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) //Verdiğimiz string passwordun hashini oluşturacak.Hashi de saltı da oluşturacak
        {
            //.Net'in Cryptography sınıfından yararlanacağız
            //Algoritmamızı seçip ona göre hash değeri oluşturacağız
            //Bunu da dispossible pattern ile yapacağız
            //HMACSHA512 algoritmasını kullanacağız
            using (var hmac = new System.Security.Cryptography.HMACSHA512())//hmac aslında cryptography sınıfında kullandığımız class'a karşılık geliyor
            {
                passwordSalt = hmac.Key; // Her kullanıcı için başka bir key oluşturur
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));//Bir stringin byte karşılığını Encoding.UTF8.GetBytes(password) ile alıyoruz
            }

        }

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) //PasswordHashi doğrulamak için oluşturduk
        {
            //Buradaki password kullanıcının tekrar girerken verdiği parola
            //Veritabanımızdaki hash ile kullanıcı giriş yaptığında oluşan hasin karşılaştırmasını yapıyoruz
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                //Burada computedHash (hesaplanan Hash) passwordSalt aracılığıyla yapılıyor
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }

            }
            return true;
        }

    }
}
