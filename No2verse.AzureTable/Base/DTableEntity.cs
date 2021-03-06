
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace No2verse.AzureTable.Base
{
    internal class ObjectUtil
    {

        public static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }


        /// <summary>
        /// 轉成可以擴充的物件
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static dynamic ConvertToDynamic(object obj)
        {

            return JObject.Parse(JsonConvert.SerializeObject(obj));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DTableEntity : TableEntity
    {
       
        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);

            foreach (var thisProperty in
                GetType().GetProperties().Where(thisProperty =>
                    thisProperty.GetType() != typeof(string) &&
                    properties.ContainsKey(thisProperty.Name) &&
                    (properties[thisProperty.Name].PropertyType == EdmType.String || properties[thisProperty.Name].PropertyType == EdmType.DateTime)))
            {

                var t = thisProperty.PropertyType;

                if (t.IsPrimitive || t == typeof(String))
                {
                    Convert.ChangeType(properties[thisProperty.Name].PropertyAsObject, thisProperty.PropertyType);
                }
                else if (t == typeof(DateTime?) || t == typeof(DateTime))
                {
                    if (properties[thisProperty.Name] != null)
                    {
                        thisProperty.SetValue(this, TimeZoneInfo.ConvertTimeFromUtc(properties[thisProperty.Name].DateTime.Value, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfo.Local.Id)));
                    }
                }
                else
                {
                    if (thisProperty.PropertyType.IsGenericType && (thisProperty.PropertyType.GetGenericTypeDefinition() == typeof(List<>)))
                    {
                        var newStr = thisProperty.PropertyType.ToString().Replace("System.Collections.Generic.List`1[", "").Replace("]", "");
                        var type = ObjectUtil.GetType(newStr);
                        Type listType = typeof(List<>).MakeGenericType(new Type[] { type });
                        thisProperty.SetValue(this, JsonConvert.DeserializeObject(properties[thisProperty.Name].StringValue, listType));
                    }
                    else
                    {
                        thisProperty.SetValue(this, JsonConvert.DeserializeObject(properties[thisProperty.Name].StringValue, ObjectUtil.GetType(thisProperty.PropertyType.ToString())));
                    }

                }

            }


        }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var properties = base.WriteEntity(operationContext);

            foreach (var thisProperty in
                GetType().GetProperties().Where(thisProperty =>
                    !properties.ContainsKey(thisProperty.Name) &&
                    typeof(TableEntity).GetProperties().All(p => p.Name != thisProperty.Name)))
            {
                var value = thisProperty.GetValue(this);
                if (value != null)
                {
                    var t = thisProperty.GetType();
                    properties.Add(thisProperty.Name, new EntityProperty(JsonConvert.SerializeObject(value)));

                }

            }

            return properties;
        }


    }
}
