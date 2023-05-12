using RithmicSoulSharedLibrary.Extensions;

namespace RithmicSoul.Admin.Client.Utilities;

public static class ModelUtilities
{
    public static IList<T1>? MapToModelWithCollection<T1, T>(T model)
    {
        IList<T1>? collection = new List<T1>();
        var modelProperties = model.GetType().GetProperties();
        var genericList = modelProperties.FirstOrDefault(m => m.PropertyType.UnderlyingSystemType.IsGenericList());

        var values = genericList?.GetValue(model) as IEnumerable<object>;
        if (values is null || !values.Any()) return null;

        foreach (var value in values)
        {
            var mapToModel = (T1)Activator.CreateInstance(typeof(T1)) ?? throw new InvalidCastException("Cannot create instance of mapped model type");
            var mapToTypeProperties = mapToModel.GetType().GetProperties();
            var propertyToMapTo = mapToTypeProperties.FirstOrDefault(p => !modelProperties.Select(m => m.Name).Contains(p.Name));
            propertyToMapTo?.SetValue(mapToModel, value);

            foreach (var property in modelProperties)
            {
                var propertyValue = property.GetValue(model);
                var mapToTypeProperty = mapToTypeProperties.FirstOrDefault(m => m.Name == property.Name);
                mapToTypeProperty?.SetValue(mapToModel, propertyValue);
            }



            collection.Add(mapToModel);
        }


        return collection;
    }
}