using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Token_Ifinsys.Interfaces;
using Token_Ifinsys.Models;

namespace Token_Ifinsys
{
    public class TokenAuthenticationService : IAuthenticateService
    {

        private readonly IUserManagementService _userManagementService;
        private readonly TokenManagement _tokenManagement;
        private string _tokenSecretModule;

        public TokenAuthenticationService(IUserManagementService service, IOptions<TokenManagement> tokenManagement)
        {
            _userManagementService = service;
            _tokenManagement = tokenManagement.Value;
            //_connectionModule = configuration.GetConnectionString("tokenManagementLos");
        }
        public bool IsAuthenticated(TokenRequest request, out string token, out int status, out string message, out DataTable datalist)
        {

            token = string.Empty;
            message = string.Empty;
            status = 0;
            datalist = null;

            if (!_userManagementService.IsValidUser(request.Username, request.Password, out string messageEx, out DataTable dataList)) //request.Imei
            {
                status = 0;
                message = messageEx;
                token = "";
                datalist = null;
                return true;
            }


            //if (request.ModuleName.Equals("ifinsys"))
            //{
            //    _tokenSecretModule = _tokenManagement.Secret;
            //}
            //else if (request.ModuleName.Equals("ifinlos"))
            //{
            //    _tokenSecretModule = _tokenManagement.SecretLos;
            //}
               

            var claim = new[]
            {
                new Claim(ClaimTypes.Name, request.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSecretModule));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                _tokenManagement.Issuer,
                _tokenManagement.Audience,
                claim,
                expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                signingCredentials: credentials
            );
            status = 1;
            message = "Success!";
            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            datalist = dataList;
            return true;

        }

    }
}
