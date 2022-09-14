using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Encryption
{
    public class SigningCredentialsHelper
    {
        //JWT oluşturulabilmesi için  gerekli olan Keyi vereceğiz ve bu method bize imzalama görevi sunacak 
        public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey)        
        {
            //Asp.net'e hashing işlemi yaparken anahtar olarak bu securityKey'i kullan,şifreleme olarak da HmacSha512 kullan diyoruz.
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        }
    }
}
