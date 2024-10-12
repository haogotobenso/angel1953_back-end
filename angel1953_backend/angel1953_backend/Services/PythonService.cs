using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using angel1953_backend.Dtos;
using angel1953_backend.Models;
using angel1953_backend.Repository;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using OfficeOpenXml.Utils.TypeConversion;

namespace angel1953_backend.Services
{
    public class PythonService
    {
        private readonly BackRepository _backRepository;
        private readonly BackService _backService;

        public PythonService(BackRepository backRepository,BackService backService)
        {
            _backRepository = backRepository;
            _backService = backService;
        }
        #region 啟動更新素養連結爬蟲API
        public string UpdateExtLink()
        {
            return "連結已更新";
        }
        #endregion
        
    }
}