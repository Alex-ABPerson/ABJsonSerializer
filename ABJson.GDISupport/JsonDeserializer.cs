using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABJson.GDISupport
{
    public static class JsonDeserializer
    {
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

        public static JsonKeyValuePair Deserialize(string json, Type typ)
        {

            JsonKeyValuePair ret = JsonReader.GetKeyValueData(json);

            if (typ.IsNumericType()) ret.value = Convert.ChangeType(Convert.ToDouble(ret.value.ToString()), typ);

            else if (Type.GetTypeCode(typ) == TypeCode.Boolean) ret.value = (ret.value.ToString().ToLower() == "true") ? true : false;

            // TODO: DATETIME
            else if (typ.IsList()) // A List<>
                ret.value = DeserializeArray(ret.value.ToString()).ToList();

            else if (typ.IsArray()) // An Array - same as list but without .ToList();
                ret.value = DeserializeArray(ret.value.ToString());

            else if (typ.IsDictionary())
                ret.value = DeserializeDictionary(ret.value.ToString());

            //else if (Activator.CreateInstance(typ) is Bitmap)
            //    ret.value = ImageToText.ConvertTextToImage(ret.value.ToString());

            //else if (Activator.CreateInstance(typ) is Image)
            //    ret.value = ImageToText.ConvertTextToImage(ret.value.ToString());

            //else if (Activator.CreateInstance(typ) is Point)
            //    ret.value = new Point(int.Parse(ret.value.ToString().Split(',')[0]), int.Parse(ret.value.ToString().Split(',')[1]));

            return ret;
        }

        public static string[] DeserializeArray(string json)
        {
            return null;
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
