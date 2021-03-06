using System;

namespace MiniShop.Mvc.Models
{
    public class ErrorViewModel
    {
        public ErrorViewModel(string statusCode, string errorMsg)
        {
            string img = "200";

            //TODO
            switch (statusCode)
            {
                case "500":
                case "403":
                case "404":
                    img = statusCode;
                    break;
                default:
                    img = "404";//暂时使用这个图片吧。。
                    break;
            }

            StatusCode = statusCode;
            ErrorImgSrc = $"/admin/images/{img}.svg";
            ErrorMsg = errorMsg;
        }
        public string StatusCode { get; }

        public string ErrorImgSrc { get; }

        public string ErrorMsg { get; }
    }
}
