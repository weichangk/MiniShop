using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Orm.Core;

namespace MiniShop.Model
{

    /// <summary>
    /// 不包含软删除的实体基类
    /// </summary>
    /// <typeparam name="TKey">主键类型</typeparam>
    public class EntityBaseNoDeleted<TKey> : EntityBase<TKey> where TKey : struct
    {
        [NotMapped]
        public override int Deleted { get => base.Deleted; set => base.Deleted = value; }
    }

    /// <summary>
    /// 不包含商店ID的实体基类
    /// </summary>
    /// <typeparam name="TKey">主键类型</typeparam>
    public class EntityBaseNoShopId<TKey> : EntityBase<TKey> where TKey : struct
    {
        [NotMapped]
        public override Guid ShopId { get => base.ShopId; set => base.ShopId = value; }
    }

    /// <summary>
    /// 不包含门店ID的实体基类
    /// </summary>
    /// <typeparam name="TKey">主键类型</typeparam>
    public class EntityBaseNoStoreId<TKey> : EntityBase<TKey> where TKey : struct
    {
        [NotMapped]
        public override Guid StoreId { get => base.StoreId; set => base.StoreId = value; }
    }

    /// <summary>
    /// 不包含软删除、商店ID、门店ID的实体基类
    /// </summary>
    /// <typeparam name="TKey">主键类型</typeparam>
    public class EntityBaseNoDeletedShopIdStoreId<TKey> : EntityBase<TKey> where TKey : struct
    {
        [NotMapped]
        public override int Deleted { get => base.Deleted; set => base.Deleted = value; }
        [NotMapped]
        public override Guid ShopId { get => base.ShopId; set => base.ShopId = value; }
        [NotMapped]
        public override Guid StoreId { get => base.StoreId; set => base.StoreId = value; }
    }

    /// <summary>
    /// 不包含软删除、商店ID的实体基类
    /// </summary>
    /// <typeparam name="TKey">主键类型</typeparam>
    public class EntityBaseNoDeletedShopId<TKey> : EntityBase<TKey> where TKey : struct
    {
        [NotMapped]
        public override int Deleted { get => base.Deleted; set => base.Deleted = value; }
        [NotMapped]
        public override Guid ShopId { get => base.ShopId; set => base.ShopId = value; }
    }

    /// <summary>
    /// 不包含软删除、门店ID的实体基类
    /// </summary>
    /// <typeparam name="TKey">主键类型</typeparam>
    public class EntityBaseNoDeletedStoreId<TKey> : EntityBase<TKey> where TKey : struct
    {
        [NotMapped]
        public override int Deleted { get => base.Deleted; set => base.Deleted = value; }
        [NotMapped]
        public override Guid StoreId { get => base.StoreId; set => base.StoreId = value; }
    }

    /// <summary>
    /// 不包含商店ID、门店ID的实体基类
    /// </summary>
    /// <typeparam name="TKey">主键类型</typeparam>
    public class EntityBaseNoShopIdStoreId<TKey> : EntityBase<TKey> where TKey : struct
    {
        [NotMapped]
        public override Guid ShopId { get => base.ShopId; set => base.ShopId = value; }
        [NotMapped]
        public override Guid StoreId { get => base.StoreId; set => base.StoreId = value; }
    }

    /// <summary>
    /// 包含通用模型实体基类
    /// </summary>
    /// <typeparam name="TKey">主键类型</typeparam>
    public class EntityBase<TKey> : IEntity where TKey : struct
    {
        [Key]
        public TKey Id { get; set; }

        /// <summary>
        /// 商店Id
        /// </summary>
        public virtual Guid ShopId { get; set; }

        /// <summary>
        /// 门店Id
        /// </summary>
        public virtual Guid StoreId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreatedTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime ModifiedTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 操作人名称
        /// </summary>
        public virtual string OperatorName { get; set; }
        /// <summary>
        /// 软删除
        /// </summary>
        [DefaultValue(0)]
        public virtual int Deleted { get; set; }
    }
}
