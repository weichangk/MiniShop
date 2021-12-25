using System.ComponentModel.DataAnnotations.Schema;

namespace MiniShop.Model
{
    /// <summary>
    /// 供应商
    /// </summary>
    [Table("Supplier")]
    public class Supplier : EntityBase<int>
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
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public EnumSupplierStatus State { get; set; }

        /// <summary>
        /// 状态描述
        /// </summary>
        [NotMapped]
        public string StateDes => State.ToDescription();

    }
}
