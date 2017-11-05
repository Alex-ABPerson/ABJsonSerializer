using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABJson.GDISupport
{
    public static class JsonWriter
    {
        public static string WriteName(string name, JsonFormatting format)
        {
            string result = "";

            result += "\"" + name + "\"";

            return result;
        }

        public static string WriteValue(object obj, JsonKeyValueType type, JsonFormatting format, int indent)
        {
            string result = "";

            switch (type)
            {
                case JsonKeyValueType.Text:
                    if (format == JsonFormatting.Compact)
                        result += "\"" + obj.ToString() + "\",";
                    else if (format == JsonFormatting.CompactReadable)
                        result += "\"" + obj.ToString() + "\", ";
                    else if (format == JsonFormatting.Indented)
                        result += "\"" + obj.ToString() + "\", \n";

                    break;
                case JsonKeyValueType.Boolean:
                case JsonKeyValueType.Numerical:
                    if (format == JsonFormatting.Compact)
                        result += obj.ToString().ToLower() + ",";
                    else if (format == JsonFormatting.CompactReadable)
                        result += obj.ToString().ToLower() + ", ";
                    else if (format == JsonFormatting.Indented)
                        result += obj.ToString().ToLower() + ", \n";

                    break;
                case JsonKeyValueType.Null:
                    if (format == JsonFormatting.Compact)
                        result += "null,";
                    else if (format == JsonFormatting.CompactReadable)
                        result += "null, ";
                    else if (format == JsonFormatting.Indented)
                        result += "null, \n";

                    break;
                case JsonKeyValueType.Array:

                    if (format == JsonFormatting.Compact)
                        result += "[" + JsonSerializer.SerializeArray(((dynamic)obj).ToArray(), format, indent + 1) + "],";
                    else if (format == JsonFormatting.CompactReadable)
                        result += "[" + JsonSerializer.SerializeArray(((dynamic)obj).ToArray(), format, indent + 1) + "], ";
                    else if (format == JsonFormatting.Indented)
                        result += "[\n" + JsonSerializer.SerializeArray(((dynamic)obj).ToArray(), format, indent + 1) + "], \n";

                    break;
                case JsonKeyValueType.Object:
                    if (format == JsonFormatting.Compact)
                        result += JsonClassConverter.ConvertObjectToJson(obj, format, indent + 1) + ",";
                    else if (format == JsonFormatting.CompactReadable)
                        result += JsonClassConverter.ConvertObjectToJson(obj, format, indent + 1) + ", ";
                    else if (format == JsonFormatting.Indented)
                        result += JsonClassConverter.ConvertObjectToJson(obj, format, indent + 1) + ", \n";

                    break;                   
            }
            return result;
        }

        public static string WriteKeyValuePair(string name, object obj, JsonKeyValueType type, JsonFormatting format, int indent)
        {
            string result = "";

            if (format == JsonFormatting.Compact)
                result = WriteName(name, format) + ":" + WriteValue(obj, type, format, indent);
            else if (format == JsonFormatting.CompactReadable)
                result = WriteName(name, format) + ": " + WriteValue(obj, type, format, indent);
            else if (format == JsonFormatting.Indented)
                result = WriteName(name, format) + ": " + WriteValue(obj, type, format, indent);

            return result;
        }

        public static string Indent(string value, int size)
        {
            string str = "";
            var strArray = value.Split('\n');
            foreach (var s in strArray)
                if (s != "")
                    str += new string(' ', size * 4) + s + "\n";
            return str;
        }
    }
}

