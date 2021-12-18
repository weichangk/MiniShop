namespace System.ComponentModel
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class OperationIdAttribute : Attribute
    {
        private string Default = null;

        /// <summary>
        /// 
        /// </summary>
        public string OperationId { get { return Default; } }

        /// <summary>
        /// 
        /// </summary>
        public OperationIdAttribute()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        public OperationIdAttribute(string description)
        {
            Default = description;
        }
    }
}
