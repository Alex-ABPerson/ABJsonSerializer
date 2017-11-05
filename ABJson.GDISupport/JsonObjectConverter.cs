using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;

namespace ABJson.GDISupport
{
    public static class JsonClassConverter
    {
        
        public static string ConvertObjectToJson(object obj, JsonFormatting format = JsonFormatting.Indented, int identationLevel = 1)
        {            

            string result = "";

            if (format == JsonFormatting.Compact)
                result = "{";
            else if (format == JsonFormatting.CompactReadable)
                result = "{ ";
            else if (format == JsonFormatting.Indented)
                result = "{\n";

            var bindingFlags = BindingFlags.Instance |
                   BindingFlags.NonPublic |
                   BindingFlags.Public;

            var fieldNames = obj.GetType().GetFields(bindingFlags)
                            .Select(field => field.Name)
                            .ToList();

            var fieldValues = obj.GetType()
                                 .GetFields(bindingFlags)
                                 .Select(field => field.GetValue(obj))
                                 .ToList();

            int i = 0;
            foreach (string fieldName in fieldNames) 
            {
                if (format == JsonFormatting.Indented)
                    result += JsonWriter.Indent(JsonSerializer.Serialize(fieldName, fieldValues[i], format, identationLevel), identationLevel);
                else
                    result += JsonSerializer.Serialize(fieldName, fieldValues[i], format, identationLevel);
                i++;
            }

            result = result.Remove(result.LastIndexOf(','), 1);

            if (format == JsonFormatting.Compact)
                result += "}";
            else if (format == JsonFormatting.CompactReadable)
                result += " }";
            else if (format == JsonFormatting.Indented)
                result += "}";

            return result;
        }
    }
}
