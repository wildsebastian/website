using Microsoft.AspNetCore.Mvc.ModelBinding;
using NodaTime;

namespace Website.Database.ModelBinder
{
    public class InstantModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ArgumentNullException.ThrowIfNull(bindingContext);

            string modelName = bindingContext.ModelName;
            ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
            string? value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
                return Task.CompletedTask;

            if (DateTime.TryParse(value, out var dateTime))
            {
                DateTime dateTimeUtc = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                Instant instant = Instant.FromDateTimeUtc(dateTimeUtc);
                bindingContext.Result = ModelBindingResult.Success(instant);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(
                    modelName,
                    "Invalid datetime format"
                );
            }

            return Task.CompletedTask;
        }
    }
}