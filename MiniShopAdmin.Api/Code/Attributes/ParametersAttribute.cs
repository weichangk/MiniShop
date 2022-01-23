using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ParametersAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string param { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ParametersAttribute()
        {
        }
    }
}
