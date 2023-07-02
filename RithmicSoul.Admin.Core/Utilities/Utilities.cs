using RithmicSoulSharedLibrary.Extensions;

namespace RithmicSoul.Admin.Core.Utilities;

public static class Utilities
{
    public static IList<TToDto>? MapToModelWithCollection<TToDto, TFromDto>(TFromDto model)
    {
        IList<TToDto>? collection = new List<TToDto>();
        var modelProperties = model?.GetType().GetProperties();
        var genericList = modelProperties?.FirstOrDefault(m => m.PropertyType.UnderlyingSystemType.IsGenericList());

        if (genericList?.GetValue(model) is not IEnumerable<object> values || !values.Any()) return null;

        foreach (var value in values)
        {
            var isValueGenericList = value.GetType().IsGenericList();
            var mapToModel = (TToDto)Activator.CreateInstance(typeof(TToDto)) ?? throw new InvalidCastException("Cannot create instance of mapped model type");
            var mapToTypeProperties = mapToModel.GetType().GetProperties();
            var propertyToMapTo = mapToTypeProperties.FirstOrDefault(p => !modelProperties.Select(m => m.Name).Contains(p.Name));
            propertyToMapTo?.SetValue(mapToModel, value);

            //foreach (var property in modelProperties)
            //{
            //    var propertyValue = property.GetValue(model);
            //    var mapToTypeProperty = mapToTypeProperties.FirstOrDefault(m => m.Name == property.Name);
            //    mapToTypeProperty?.SetValue(mapToModel, propertyValue);
            //}

            collection.Add(mapToModel);
        }

        return collection;
    }

    public static string? GetMimeTypeString(string dataString)
    {
        if (string.IsNullOrEmpty(dataString)) return null;
        if (!dataString.Contains('/')) return null;

        var s = dataString.Split("/")[0];
        if (!s.Contains("data:")) return null;

        return s.Split(":")[1];
    }
}