using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MusicFestivalDeserializer
{
    public static class ObjectFlattener
    {
        public static List<ItemRow> Flatten(object data)
        {
            List<ItemRow> rows = new List<ItemRow>();
            AddRows(data, rows, string.Empty);
            return rows;
        }

        private static void AddRows(object data, List<ItemRow> rows, string prefix)
        {
            if (data == null)
            {
                rows.Add(new ItemRow(prefix, "null"));
                return;
            }

            Type type = data.GetType();

            if (IsSimpleType(type))
            {
                rows.Add(new ItemRow(prefix, data.ToString()));
                return;
            }

            if (data is IEnumerable && !(data is string))
            {
                IEnumerable collection = (IEnumerable)data;
                int index = 0;

                foreach (object item in collection)
                {
                    string nextPrefix = GetCollectionPrefix(prefix, index);
                    AddRows(item, rows, nextPrefix);
                    index = index + 1;
                }

                if (index == 0)
                {
                    rows.Add(new ItemRow(prefix, "Пусто"));
                }

                return;
            }

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            int i;
            for (i = 0; i < properties.Length; i = i + 1)
            {
                PropertyInfo property = properties[i];
                object value = property.GetValue(data, null);
                string nextPrefix = GetPropertyPrefix(prefix, property.Name);

                if (value == null)
                {
                    rows.Add(new ItemRow(nextPrefix, "null"));
                }
                else if (IsSimpleType(property.PropertyType))
                {
                    rows.Add(new ItemRow(nextPrefix, value.ToString()));
                }
                else
                {
                    AddRows(value, rows, nextPrefix);
                }
            }
        }

        private static string GetCollectionPrefix(string prefix, int index)
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                return "[" + index + "]";
            }

            return prefix + "[" + index + "]";
        }

        private static string GetPropertyPrefix(string prefix, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                return propertyName;
            }

            return prefix + "." + propertyName;
        }

        private static bool IsSimpleType(Type type)
        {
            Type actualType = Nullable.GetUnderlyingType(type);
            if (actualType == null)
            {
                actualType = type;
            }

            if (actualType.IsPrimitive)
            {
                return true;
            }

            if (actualType.IsEnum)
            {
                return true;
            }

            if (actualType == typeof(string))
            {
                return true;
            }

            if (actualType == typeof(decimal))
            {
                return true;
            }

            if (actualType == typeof(DateTime))
            {
                return true;
            }

            if (actualType == typeof(Guid))
            {
                return true;
            }

            if (actualType == typeof(double))
            {
                return true;
            }

            if (actualType == typeof(float))
            {
                return true;
            }

            return false;
        }
    }

    public class ItemRow
    {
        public ItemRow()
        {
            Property = string.Empty;
            Value = string.Empty;
        }

        public ItemRow(string property, string value)
        {
            if (property == null)
            {
                Property = string.Empty;
            }
            else
            {
                Property = property;
            }

            if (value == null)
            {
                Value = string.Empty;
            }
            else
            {
                Value = value;
            }
        }

        public string Property { get; set; }
        public string Value { get; set; }
    }
}
