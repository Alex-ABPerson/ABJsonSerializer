using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ABJson.GDISupport
{
    public enum JsonFormatting
    {
        Compact = 0,
        CompactReadable = 1,
        Indented = 2
    }
    public enum JsonKeyValueType
    {
        Text = 0,
        Numerical = 1,
        Boolean = 2,
        Null = 3,
        Array = 4,
        Object = 5

    }

    public static class JsonSerializer
    {

        public static bool IsArray(this object o)
        {
            try
            {
                Type valueType = o.GetType();
                if (valueType.IsArray)
                    return true;

                return o.GetType().GetGenericTypeDefinition() == typeof(List<>);
            }
            catch { return false; }
        }

        public static bool IsNumericType(this object o)
        {
            if (o != null)
                switch (Type.GetTypeCode(o.GetType()))
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

        public static string Serialize(string name, object obj, JsonFormatting format, int indentLevel, bool justValue = false)
        {
            string result = "";

            if (obj == null)
                if (justValue) result += JsonWriter.WriteValue(obj, JsonKeyValueType.Null, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, obj, JsonKeyValueType.Null, format, indentLevel);

            else if (obj is bool)
                if (justValue) result += JsonWriter.WriteValue(obj, JsonKeyValueType.Boolean, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, obj, JsonKeyValueType.Boolean, format, indentLevel);

            else if (obj is string)
                if (justValue) result += JsonWriter.WriteValue(obj, JsonKeyValueType.Text, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, obj, JsonKeyValueType.Text, format, indentLevel);

            else if (obj.IsNumericType())
                if (justValue) result += JsonWriter.WriteValue(obj, JsonKeyValueType.Numerical, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, obj, JsonKeyValueType.Numerical, format, indentLevel);

            else if (obj.IsArray())
                if (justValue) result += JsonWriter.WriteValue(obj, JsonKeyValueType.Array, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, obj, JsonKeyValueType.Array, format, indentLevel);

            else if (obj is Bitmap)
                if (justValue) result += JsonWriter.WriteValue(ImageToText.ConvertImageToText((Bitmap)obj, System.Drawing.Imaging.ImageFormat.Png), JsonKeyValueType.Text, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, ImageToText.ConvertImageToText((Bitmap)obj, System.Drawing.Imaging.ImageFormat.Png), JsonKeyValueType.Text, format, indentLevel);

            else if (obj is Image) result += JsonWriter.WriteKeyValuePair(name, ImageToText.ConvertImageToText((Image)obj, System.Drawing.Imaging.ImageFormat.Png), JsonKeyValueType.Text, format, indentLevel);

            else if (justValue) result += JsonWriter.WriteValue(obj, JsonKeyValueType.Object, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, obj, JsonKeyValueType.Object, format, indentLevel);

            return result;
        }

        public static string SerializeArray(object[] obj, JsonFormatting format, int indent)
        {
            string result = "";

            
            foreach (object element in obj)
                result += Serialize("", element, format, indent, true);

            result = result.Remove(result.LastIndexOf(','), 1);
            if (format == JsonFormatting.Indented)
                result = JsonWriter.Indent(result, indent);

            return result;
        }
    }
}
