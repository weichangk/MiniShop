﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniShop.Model
{
    /// <summary>
    /// 商店
    /// </summary>
    [Table("Shop")]
    public class Shop : EntityBase<Guid>
    {
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime ValidDate { get; set; } = DateTime.Now;

        ////通过override重写，标记NotMapped特性排除基类属性，不生成表字段
        //[NotMapped]
        //public override DateTime CreatedTime { get => base.CreatedTime; set => base.CreatedTime = value; }
        //[NotMapped]
        //public override string OperatorName { get => base.OperatorName; set => base.OperatorName = value; }
    }
}
