using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        //Configurasyon yapısını enjekte ettik buraya (appsettings.json dosyamızı okumaya yarar)     
        public IConfiguration Configuration { get; }
        //yukarıda okuduğum değerleri atabileceğimiz bir nesne(Token'ın değerleri)
        private TokenOptions _tokenOptions;
        //Access Token ne zaman geçersiz hale gelecek?
        private DateTime _accessTokenExpiration;
        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            //GetSection appsettings.jsondaki herbir bölüm olarak düşünebilirsin.Onların arasından TokenOptions bölümünü al
            //ve oradaki değerleri TokenOptions sınıfı ile maple.Tek tek atamalarını yap değerlerin.
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

        }
        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
        {
            //Bu token ne zaman bitecek?
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            //appsettings.json dosyamızdaki TokenOptionstaki securitykeyi kullanarak CreateSecurityKey methodu ile SecurityKey formatımızı çevir
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            //Hangi anahtarı ve hangi algoritmayı kullansın?
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            //JWT'yi üret
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };

        }

        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials, List<OperationClaim> operationClaims)
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,//Şuandan önceki bir değer verilemez
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
            );
            return jwt;
        }

        private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());

            return claims;
        }
    }
}
