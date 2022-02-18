using MiniShop.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniShop.Model
{
    /// <summary>
    /// 供应商
    /// </summary>
    [Table("Supplier")]
    public class Supplier : EntityBaseNoStoreId<int>
    {
        /// <summary>
        /// 供应商编码
        /// </summary>
        public int Code { get; set; }
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
    }
}
