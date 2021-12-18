using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniShop.Model
{
    [Table("User")]
    public class User : EntityBase<int>
    {
        /// <summary>
        /// 商店Id
        /// </summary>
        [Required]
        public Guid ShopId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string Name { get; set; }

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
        public string RoleName => Role.ToDescription();

        ////通过override重写，标记NotMapped特性排除基类属性，不生成表字段
        //[NotMapped]
        //public override DateTime CreatedTime { get => base.CreatedTime; set => base.CreatedTime = value; }
        //[NotMapped]
        //public override string OperatorName { get => base.OperatorName; set => base.OperatorName = value; }
    }
}
