using System;

namespace MiniShopAdmin.Api.Code.Attributes
{
    /// <summary>
    /// Swagger：隐藏属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnorePropertyAttribute : Attribute
    {
    }
}
