using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;

public class NullIfEmptyModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        var model = Activator.CreateInstance(bindingContext.ModelType);
        var modelType = bindingContext.ModelType;

        bool anyPropertySet = false;

        foreach (var property in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.CanWrite)
                continue;

            var valueResult = bindingContext.ValueProvider.GetValue(property.Name);
            if (valueResult == ValueProviderResult.None)
                continue;

            var rawValue = valueResult.FirstValue;
            if (string.IsNullOrWhiteSpace(rawValue))
                continue;

            try
            {
                var converter = TypeDescriptor.GetConverter(property.PropertyType);
                var converted = converter.ConvertFromString(rawValue);
                property.SetValue(model, converted);
                anyPropertySet = true;
            }
            catch { }
        }

        bindingContext.Result = anyPropertySet
            ? ModelBindingResult.Success(model)
            : ModelBindingResult.Success(null);

        await Task.CompletedTask;
    }
}
