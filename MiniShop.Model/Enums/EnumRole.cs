using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiniShop.Model.Enums
{
    public enum EnumRole
    {
        [Description("店长")]
        [Display(Name = "店长")]
        ShopManager,
        [Description("管理员")]
        [Display(Name = "管理员")]
        Admin,
        [Description("操作员")]
        [Display(Name = "操作员")]
        Operator,
        [Description("收银员")]
        [Display(Name = "收银员")]
        Cashier,
    }
}
