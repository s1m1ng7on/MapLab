using Microsoft.AspNetCore.Mvc;

namespace MapLab.Web.Infrastructure.ModelBinding
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class NullIfEmptyBinderAttribute : ModelBinderAttribute
    {
        public NullIfEmptyBinderAttribute()
        {
            BinderType = typeof(NullIfEmptyModelBinder);
        }
    }
}
