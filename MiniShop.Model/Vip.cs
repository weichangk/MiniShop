using MiniShop.Model.Code;
using MiniShop.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniShop.Model
{
    /// <summary>
    /// 会员
    /// </summary>
    [Table("Vip")]
    public class Vip : EntityBaseNoStoreId<int>
    {
        /// <summary>
        /// 会员类型
        /// </summary>
        public EnumVipType VipType { get; set; }

        /// <summary>
        /// 会员类型描述
        /// </summary>
        [NotMapped]
        public string VipTypeDes => VipType.ToDescription();

        /// <summary>
        /// 卡号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public EnumSex Sex { get; set; }

        /// <summary>
        /// 性别描述
        /// </summary>
        [NotMapped]
        public string SexDes => Sex.ToDescription();

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; } = DateTime.Now;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime ValidDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 状态
        /// </summary>
        public EnumVipStatus State { get; set; }

        /// <summary>
        /// 状态描述
        /// </summary>
        [NotMapped]
        public string StateDes => State.ToDescription();
    }
}
