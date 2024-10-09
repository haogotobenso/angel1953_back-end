using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace angel1953_backend.Dtos
{
    public class RecoveryDetailDto
    {
        public string Question { get; set; }           // 題目內容
        public string CorrectAnswer { get; set; }      // 正確答案
        public string? UserAnswer { get; set; }         // 使用者作答
        public List<OptionDetail> Options { get; set; }  // 題目選項清單
        public class OptionDetail
        {
            public string Text { get; set; }               // 選項文字
            public bool IsCorrect { get; set; }            // 該選項是否為正確答案
            public bool IsUserAnswer { get; set; }         // 使用者是否選擇了這個選項
        }
    }
}