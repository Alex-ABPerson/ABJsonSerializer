using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ABJson.GDISupport
{
    public static class JsonDeserializer
    {
        internal static Type GetListType(this Type type)
        {
            //use type.GenericTypeArguments if exist 
            if (type.GenericTypeArguments.Any())
                return type.GenericTypeArguments.First();

            return type.GetRuntimeProperty("Item").PropertyType;
        }

        public static bool IsList(this Type o)
        {
            try
            {
                return (o.GetGenericTypeDefinition() == typeof(List<>));
            } catch { return false; }
        }

        public static bool IsNumericType(this Type o)
        {
            if (o != null)
                switch (Type.GetTypeCode(o))
                {
                    case TypeCode.Byte:
                    case TypeCode.SByte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Single:
                        return true;
                    default:
                        return false;
                }
            else return false;
        }

        public static object DeserializeValue<T>(string json, bool onlyValue = false)
        {
            return (T)Deserialize(json, typeof(T), onlyValue).value;
        }

        public static JsonKeyValuePair Deserialize(string json, Type typ, bool onlyValue = false)
        {

            JsonKeyValuePair ret = new JsonKeyValuePair();

            if (onlyValue) ret.value = json; else ret = JsonReader.GetKeyValueData(json);

            if (ret.value.ToString() == "null")
                ret.value = null;
            else
            {
                if (typ.IsNumericType()) ret.value = Convert.ChangeType(Convert.ToDouble(ret.value.ToString()), typ);

                else if (Type.GetTypeCode(typ) == TypeCode.Boolean) ret.value = (ret.value.ToString().ToLower() == "true") ? true : false;

                // TODO: DATETIME
                else if (typ.IsList()) // A List<>
                    ret.value = DeserializeArray(ret.value.ToString(), typ);

                else if (typ.IsArray) // An Array - same as list but without .ToList();
                    ret.value = DeserializeArray(ret.value.ToString(), typ).ToArray();

                else if (typ.IsDictionary())
                    ret.value = DeserializeDictionary(ret.value.ToString());

                //else if (Activator.CreateInstance(typ) is Bitmap)
                //    ret.value = ImageToText.ConvertTextToImage(ret.value.ToString());

                //else if (Activator.CreateInstance(typ) is Image)
                //    ret.value = ImageToText.ConvertTextToImage(ret.value.ToString());

                //else if (Activator.CreateInstance(typ) is Point)
                //    ret.value = new Point(int.Parse(ret.value.ToString().Split(',')[0]), int.Parse(ret.value.ToString().Split(',')[1]));
            }
            return ret;
        }

        public static dynamic DeserializeArray(string json, Type typ)
        {
            // This function is a mess as it needs to make List<string> not List<object> and lots of other things like that!
            // The code in the function is not meant to be neat and is pretty much made of a bunch of snippets from stackoverflow.

            Type type;

            if (typ.IsList())
                type = typ.GetListType();
            else
                type = typ.GetElementType();



            json = json.Remove(0, 1).Remove(json.LastIndexOf(']') - 1);

            var list = typeof(List<>);
            var listOfType = list.MakeGenericType(type);

            dynamic result = Activator.CreateInstance(listOfType);

            string[] arrayItems = JsonReader.GetAllValuesInArray(json);

            foreach (string str in arrayItems)
            {
                MethodInfo method = typeof(JsonDeserializer).GetMethod("DeserializeValue");
                MethodInfo generic = method.MakeGenericMethod(type);
                dynamic value = generic.Invoke(null, new object[] { str, true });

                result.Add(value);
            }

            return result;
        }

        public static string[] DeserializeDictionary(string json)
        {
            return null;
        }
    }

    public class JsonKeyValuePair
    {
        public string name;
        public object value;
        public JsonKeyValueType type;
    }
}
