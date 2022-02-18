using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;

namespace MiniShop.Model.Code
{
    public static class EnumExtensions
    {
        private static readonly ConcurrentDictionary<string, string> DescriptionCache = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 获取枚举类型的Description说明
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescription(this Enum value)
        {
            var type = value.GetType();
            var info = type.GetField(value.ToString());
            var key = type.FullName + info.Name;
            if (!DescriptionCache.TryGetValue(key, out string desc))
            {
                var attrs = info.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs.Length < 1)
                    desc = string.Empty;
                else
                    desc = attrs[0] is DescriptionAttribute
                        descriptionAttribute
                        ? descriptionAttribute.Description
                        : value.ToString();

                DescriptionCache.TryAdd(key, desc);
            }

            return desc;
        }

        public static Dictionary<int, string> ToValueAndDesDictionary<T>()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string desc = string.Empty;
            if (!typeof(T).IsEnum)
            {
                return dic;
            }
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                var attrs = item.GetType().GetField(item.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    DescriptionAttribute descAttr = attrs[0] as DescriptionAttribute;
                    desc = descAttr.Description;
                }
                dic.Add(Convert.ToInt32(item), desc);
            }
            return dic;
        }

        public static Dictionary<string, string> ToNameAndDesDictionary<T>()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string desc = string.Empty;
            if (!typeof(T).IsEnum)
            {
                return dic;
            }
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                var attrs = item.GetType().GetField(item.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    DescriptionAttribute descAttr = attrs[0] as DescriptionAttribute;
                    desc = descAttr.Description;
                }
                dic.Add(item.ToString(), desc);
            }
            return dic;
        }

        public static List<string> ToDesList<T>()
        {
            List<string> list = new List<string>();
            string desc = string.Empty;
            if (!typeof(T).IsEnum)
            {
                return list;
            }
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                var attrs = item.GetType().GetField(item.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    DescriptionAttribute descAttr = attrs[0] as DescriptionAttribute;
                    desc = descAttr.Description;
                }
                list.Add(desc);
            }
            return list;
        }
    }
}
