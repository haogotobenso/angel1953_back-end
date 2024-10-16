using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        private readonly HttpClient _httpClient;

        public PythonService(BackRepository backRepository,BackService backService, HttpClient httpClient)
        {
            _backRepository = backRepository;
            _backService = backService;
            _httpClient = httpClient;
        }
        #region 啟動更新素養連結爬蟲API
        public string UpdateExtLink()
        {
            // Python API 的 URL
            string UpdateExtLinkUrl = "http://localhost:5000/api/some-endpoint";
            // 發送 GET 請求給 Python API，同步操作
            HttpResponseMessage response = _httpClient.GetAsync(UpdateExtLinkUrl).Result;
            // 確保請求成功
            response.EnsureSuccessStatusCode();
            // 讀取回應的內容，同步操作
            string responseData = response.Content.ReadAsStringAsync().Result;
            return responseData;
        }
        #endregion
        
    }
}