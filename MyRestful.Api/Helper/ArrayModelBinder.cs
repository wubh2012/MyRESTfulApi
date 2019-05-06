using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyRestful.Api.Helper
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // 只适用于 IEnumerable 类型
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();

            // 空就返回
            if (string.IsNullOrWhiteSpace(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }
            
            var elementType = bindingContext.ModelType.GenericTypeArguments[0];
            var converter = TypeDescriptor.GetConverter(elementType);

            var values = value.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => converter.ConvertFromString(x.Trim())).ToArray();
            var typeValues = Array.CreateInstance(elementType, values.Length);
            values.CopyTo(typeValues, 0);
            bindingContext.Model = typeValues;
            // 成功，把 model 传过去
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;

        }
    }
}
