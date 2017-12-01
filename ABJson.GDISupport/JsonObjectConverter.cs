using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ABJson.GDISupport
{
    public static class JsonClassConverter
    {
        public static T ConvertJsonToObject<T>(string json)
        {
            return (T)DeserializeObjectInternal(json, typeof(T));
        }

        internal static object DeserializeObjectInternal(string json, Type type)
        {
            object obj = Activator.CreateInstance(type);

            var bindingFlags = BindingFlags.Instance |
                   BindingFlags.NonPublic |
                   BindingFlags.Public;

            List<string> fieldNames = type.GetFields(bindingFlags)
                            .Select(field => field.Name)
                            .ToList();

            List<Type> fieldTypes = type.GetFields(bindingFlags)
                                 .Select(field => field.FieldType)
                                 .ToList();

            for (int a = 0; a < fieldNames.Count; a++) if (fieldNames[a].StartsWith("<")) fieldNames[a] = fieldNames[a].Split('<')[1].Split('>')[0];

            string newJson = json;
            string[] newJsonLines;
            // Strip out the starting "{" and "}"
            newJson = json.Trim().TrimStart('{').TrimEnd().TrimEnd('}');
            // Now get each value seperately
            newJsonLines = JsonReader.GetAllKeyValues(newJson);
            //foreach (string str in newJsonLines) System.Windows.Forms.MessageBox.Show(str);
            if (string.IsNullOrEmpty(newJsonLines[newJsonLines.Length - 1])) Array.Resize(ref newJsonLines, newJsonLines.Length - 1); // Remove the last one if it is blank (essentially if some idiot puts "," at the end of the whole thing!)

            foreach (string str in newJsonLines)
            {

                string name = JsonReader.GetKeyValueData(str).name.ToString();

                //string fname = fieldNames.Find(item => item == JsonReader.GetKeyValueData(str).name);
                for (int i = 0; i < fieldNames.Count; i++)
                {
                    if (fieldNames[i] == name)
                    {
                        JsonKeyValuePair jkvp = JsonDeserializer.Deserialize(str, fieldTypes[i]);
                        try { type.GetField(jkvp.name.ToString(), bindingFlags).SetValue(obj, jkvp.value); } catch { }
                    }
                }
            }

            return obj;
        }

        public static string ConvertObjectToJson(object obj, JsonFormatting format = JsonFormatting.Indented, int identationLevel = 1)
        {            

            string result = "";

            if (format == JsonFormatting.Compact)
                result = "{";
            else if (format == JsonFormatting.CompactReadable)
                result = "{ ";
            else if (format == JsonFormatting.Indented)
                result = $"{{{Environment.NewLine}";

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

            
            for (int a = 0; a < fieldNames.Count; a++) if (fieldNames[a].StartsWith("<")) fieldNames[a] = fieldNames[a].Split('<')[1].Split('>')[0];

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
