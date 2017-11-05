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
                        result += "\"" + obj.ToString() + $"\",{Environment.NewLine}";

                    break;
                case JsonKeyValueType.DateTime:
                    if (format == JsonFormatting.Compact)
                        result += "\"" + JsonSerializer.SerializeDateTime((DateTime)obj) + "\",";
                    else if (format == JsonFormatting.CompactReadable)
                        result += "\"" + JsonSerializer.SerializeDateTime((DateTime)obj) + "\", ";
                    else if (format == JsonFormatting.Indented)
                        result += "\"" + JsonSerializer.SerializeDateTime((DateTime)obj) + $"\",{Environment.NewLine}";

                    break;
                case JsonKeyValueType.Boolean:
                case JsonKeyValueType.Numerical:
                    if (format == JsonFormatting.Compact)
                        result += obj.ToString().ToLower() + ",";
                    else if (format == JsonFormatting.CompactReadable)
                        result += obj.ToString().ToLower() + ", ";
                    else if (format == JsonFormatting.Indented)
                        result += obj.ToString().ToLower() + $",{Environment.NewLine}";

                    break;
                case JsonKeyValueType.Null:
                    if (format == JsonFormatting.Compact)
                        result += "null,";
                    else if (format == JsonFormatting.CompactReadable)
                        result += "null, ";
                    else if (format == JsonFormatting.Indented)
                        result += $"null, {Environment.NewLine}";

                    break;
                case JsonKeyValueType.Array:
                    if (format == JsonFormatting.Compact)
                        if (obj.GetType().IsArray) result += "[" + JsonSerializer.SerializeArray(((dynamic)obj), format, indent + 1) + "],"; else result += "[" + JsonSerializer.SerializeArray(((dynamic)obj).ToArray(), format, indent + 1) + "],";
                    else if (format == JsonFormatting.CompactReadable)
                        if (obj.GetType().IsArray) result += "[" + JsonSerializer.SerializeArray(((dynamic)obj), format, indent + 1) + "], "; else result += "[" + JsonSerializer.SerializeArray(((dynamic)obj).ToArray(), format, indent + 1) + "], ";
                    else if (format == JsonFormatting.Indented)
                        if (obj.GetType().IsArray) result += $"[{Environment.NewLine}" + JsonSerializer.SerializeArray(((dynamic)obj), format, indent + 1) + "],"; else result += $"[{Environment.NewLine}" + JsonSerializer.SerializeArray(((dynamic)obj).ToArray(), format, indent + 1) + $"],{Environment.NewLine}";

                    break;
                case JsonKeyValueType.Dictionary:
                    if (format == JsonFormatting.Compact)
                        result += "{" + JsonSerializer.SerializeDictionary(((dynamic)obj), format, indent + 1) + "},";
                    else if (format == JsonFormatting.CompactReadable)
                        result += "{" + JsonSerializer.SerializeDictionary(((dynamic)obj), format, indent + 1) + "}, ";
                    else if (format == JsonFormatting.Indented)
                        result += $"{{{Environment.NewLine}" + JsonSerializer.SerializeDictionary(((dynamic)obj), format, indent + 1) + $"}},{Environment.NewLine}";

                    break;
                case JsonKeyValueType.Object:
                    if (format == JsonFormatting.Compact)
                        result += JsonClassConverter.ConvertObjectToJson(obj, format, indent + 1) + ",";
                    else if (format == JsonFormatting.CompactReadable)
                        result += JsonClassConverter.ConvertObjectToJson(obj, format, indent + 1) + ", ";
                    else if (format == JsonFormatting.Indented)
                        result += JsonClassConverter.ConvertObjectToJson(obj, format, indent + 1) + $",{Environment.NewLine}";

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
                    str += new string(' ', size * 4) + s + $"{Environment.NewLine}";
            return str;
        }
    }
}

