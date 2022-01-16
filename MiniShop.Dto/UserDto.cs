using MiniShop.Model.Code;
using MiniShop.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class UserDto
    {
        [Display(Name = "用户ID")]
        public string Id { get; set; }

        [Display(Name = "商店ID")]
        public Guid ShopId { get; set; }

        [Display(Name = "商店名")]
        public string ShopName { get; set; }

        [Display(Name = "门店ID")]
        public Guid StoreId { get; set; }

        [Display(Name = "门店名")]
        public string StoreName { get; set; }

        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name = "手机号")]
        public string PhoneNumber { get; set; }

        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Display(Name = "职称")]
        public EnumRole Rank { get; set; }

        [Display(Name = "职称描述")]
        public string RankDes => Rank.ToDescription();

        [Display(Name = "是否冻结")]
        public bool IsFreeze { get; set; }

        [Display(Name = "注册时间")]
        public DateTime CreatedTime { get; set; }
    }
}
