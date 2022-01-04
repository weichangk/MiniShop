using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniShop.Model
{
    /// <summary>
    /// 用户
    /// </summary>
    [Table("User")]
    public class User : EntityBase<int>
    {
        /// <summary>
        /// 门店ID
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// 门店
        /// </summary>
        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public EnumRole Role { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        [NotMapped]
        public string RoleDes => Role.ToDescription();

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; } = true;

        ////通过override重写，标记NotMapped特性排除基类属性，不生成表字段
        //[NotMapped]
        //public override DateTime CreatedTime { get => base.CreatedTime; set => base.CreatedTime = value; }
        //[NotMapped]
        //public override string OperatorName { get => base.OperatorName; set => base.OperatorName = value; }
    }
}
