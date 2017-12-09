using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Globalization;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
        DateTime = 5,
        Dictionary = 6,
        Object = 7

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

        public static bool IsDictionary(this object o)
        {
            Type t = o.GetType();
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>);
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

            else if (obj is DateTime)
                if (justValue) result += JsonWriter.WriteValue(obj, JsonKeyValueType.DateTime, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, obj, JsonKeyValueType.DateTime, format, indentLevel);

            else if (obj.IsNumericType())
                if (justValue) result += JsonWriter.WriteValue(obj, JsonKeyValueType.Numerical, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, obj, JsonKeyValueType.Numerical, format, indentLevel);

            else if (obj.IsArray())
                if (justValue) result += JsonWriter.WriteValue(obj, JsonKeyValueType.Array, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, obj, JsonKeyValueType.Array, format, indentLevel);

            else if (obj.IsDictionary())
                if (justValue) result += JsonWriter.WriteValue(obj, JsonKeyValueType.Dictionary, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, obj, JsonKeyValueType.Dictionary, format, indentLevel);

            else if (obj is Bitmap)
                if (justValue) result += JsonWriter.WriteValue(ImageToText.ConvertImageToText((Bitmap)obj, System.Drawing.Imaging.ImageFormat.Png), JsonKeyValueType.Text, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, ImageToText.ConvertImageToText((Bitmap)obj, System.Drawing.Imaging.ImageFormat.Png), JsonKeyValueType.Text, format, indentLevel);

            else if (obj is Image)
                if (justValue) result += JsonWriter.WriteValue(ImageToText.ConvertImageToText((Image)obj, System.Drawing.Imaging.ImageFormat.Png), JsonKeyValueType.Text, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, ImageToText.ConvertImageToText((Image)obj, System.Drawing.Imaging.ImageFormat.Png), JsonKeyValueType.Text, format, indentLevel);

            else if (obj is Point)
                if (justValue) result += JsonWriter.WriteValue(new PointConverter().ConvertToString(obj), JsonKeyValueType.Text, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, new PointConverter().ConvertToString(obj), JsonKeyValueType.Text, format, indentLevel);

            else if (obj is Size)
                if (justValue) result += JsonWriter.WriteValue(new SizeConverter().ConvertToString(obj), JsonKeyValueType.Text, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, new SizeConverter().ConvertToString(obj), JsonKeyValueType.Text, format, indentLevel);

            else if (obj is Rectangle)
                if (justValue) result += JsonWriter.WriteValue(new RectangleConverter().ConvertToString(obj), JsonKeyValueType.Text, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, new RectangleConverter().ConvertToString(obj), JsonKeyValueType.Text, format, indentLevel);

            else if (obj is Color)
                if (justValue) result += JsonWriter.WriteValue(new ColorConverter().ConvertToString(obj), JsonKeyValueType.Text, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, new ColorConverter().ConvertToString(obj), JsonKeyValueType.Text, format, indentLevel);

            else if (obj is Font)
                if (justValue) result += JsonWriter.WriteValue(new FontConverter().ConvertToString(obj), JsonKeyValueType.Text, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, new FontConverter().ConvertToString(obj), JsonKeyValueType.Text, format, indentLevel);

            else if (obj is char)
                if (justValue) result += JsonWriter.WriteValue(new CharConverter().ConvertToString(obj), JsonKeyValueType.Text, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, new CharConverter().ConvertToString(obj), JsonKeyValueType.Text, format, indentLevel);

            else if (obj is Guid)
                if (justValue) result += JsonWriter.WriteValue(new GuidConverter().ConvertToString(obj), JsonKeyValueType.Text, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, new GuidConverter().ConvertToString(obj), JsonKeyValueType.Text, format, indentLevel);

            else if (justValue) result += JsonWriter.WriteValue(obj, JsonKeyValueType.Object, format, indentLevel); else result += JsonWriter.WriteKeyValuePair(name, obj, JsonKeyValueType.Object, format, indentLevel);

            return result;
        }

        public static string SerializeArray(dynamic obj, JsonFormatting format, int indent)
        {

            // Check if array is multi-dimensional

            if (obj is Array && ((Array)obj).Rank >= 2)
                return SerializeMultiDimensionalArray(obj, format, indent);

            string result = "";
            
            foreach (object element in obj)
                result += Serialize("", element, format, indent, true);

            result = result.TrimEnd().TrimEnd(',');
            if (format == JsonFormatting.Indented)
                result = JsonWriter.Indent(result, indent);

            return result;
        }

        public static string SerializeMultiDimensionalArray(Array obj, JsonFormatting format, int indent)
        {
            string result = "";

            List<List<dynamic>> Arrays = new List<List<dynamic>>();
            List<int> lengths = new List<int>();

            for (int a = 0; a < obj.Rank; a++)
                Arrays.Add(new List<dynamic>());

            int currentRank = 0;
            int i = 0;
            foreach (dynamic dym in obj)
            {
                if (currentRank < Arrays.Count - 1)
                    if ((obj.GetLength(currentRank) - 1) == i)
                    {
                        currentRank++;
                        i = 0;
                        Arrays[currentRank].Add(dym);                    
                    } else {
                        Arrays[currentRank].Add(dym);
                        i++;
                    }               
            }

            foreach (List<dynamic> itm in Arrays)
                result += Serialize("", itm, format, indent - 1, true);

            if (format == JsonFormatting.Indented)
                result = JsonWriter.Indent(result, indent);
            return result;
            // ((Array)obj).GetLength(0)
        }

        public static string SerializeDictionary(dynamic obj, JsonFormatting format, int indent)
        {
            string result = "";

            foreach (dynamic element in obj)
            {
                string value = Serialize("", element.Value, format, indent, true);

                if (format == JsonFormatting.Compact)
                    result += "\"" + element.Key + "\"" + ":" + value;
                else
                    result += "\"" + element.Key + "\"" + ": " + value;
            }

            result = result.TrimEnd().Trim(',');
            if (format == JsonFormatting.Indented)
                result = JsonWriter.Indent(result, indent);

            return result;
        }

        public static string SerializeDateTime(DateTime dtime)
        {
            string result = "";

            TimeSpan offset = TimeZoneInfo.Local.GetUtcOffset(dtime);

            switch (dtime.Kind)
            {
                
                case DateTimeKind.Utc:
                    //bool intoFirstChain = false;

                    //if (dtime.Year != 0) intoFirstChain = true;
                    //if (intoFirstChain) result += CopyIntToString(4, dtime.Year) + "-";

                    //if (dtime.Month != 0) intoFirstChain = true;
                    //if (intoFirstChain) result += CopyIntToString(2, dtime.Month) + "-";

                    //if (dtime.Day != 0) intoFirstChain = true;
                    //if (intoFirstChain) result += CopyIntToString(2, dtime.Day) + "T";

                    //bool intoLastChain = false;

                    //if (dtime.Hour != 0) intoLastChain = true;
                    //if (intoLastChain) result += CopyIntToString(2, dtime.Day) + "-";

                    //if (dtime.Minute != 0) intoLastChain = true;
                    //if (intoLastChain) result += CopyIntToString(2, dtime.Minute) + "-";

                    //if (dtime.Second != 0) intoLastChain = true;
                    //if (intoLastChain) result += CopyIntToString(2, dtime.Second) + ".";

                    //if (dtime.Millisecond != 0) intoLastChain = true;
                    //if (intoLastChain) result += CopyIntToString(3, dtime.Millisecond);

                    //result = result.TrimEnd('.').TrimEnd('-').TrimEnd('T');
                    //result += "Z";

                    if (dtime.Millisecond == 0)
                        result += $"{CopyIntToString(4, dtime.Year)}-{CopyIntToString(2, dtime.Month)}-{CopyIntToString(2, dtime.Day)}T{CopyIntToString(2, dtime.Hour)}:{CopyIntToString(2, dtime.Minute)}:{CopyIntToString(2, dtime.Second)}Z";
                    else
                        result += $"{CopyIntToString(4, dtime.Year)}-{CopyIntToString(2, dtime.Month)}-{CopyIntToString(2, dtime.Day)}T{CopyIntToString(2, dtime.Hour)}:{CopyIntToString(2, dtime.Minute)}:{CopyIntToString(2, dtime.Second)}.{CopyIntToString(3, dtime.Millisecond)}Z";
                    break;
                case DateTimeKind.Unspecified:
                    if (dtime.Millisecond == 0)
                        result += $"{CopyIntToString(4, dtime.Year)}-{CopyIntToString(2, dtime.Month)}-{CopyIntToString(2, dtime.Day)}T{CopyIntToString(2, dtime.Hour)}:{CopyIntToString(2, dtime.Minute)}:{CopyIntToString(2, dtime.Second)}";
                    else
                        result += $"{CopyIntToString(4, dtime.Year)}-{CopyIntToString(2, dtime.Month)}-{CopyIntToString(2, dtime.Day)}T{CopyIntToString(2, dtime.Hour)}:{CopyIntToString(2, dtime.Minute)}:{CopyIntToString(2, dtime.Second)}.{CopyIntToString(3, dtime.Millisecond)}";
                    break;
                case DateTimeKind.Local:
                    string offsetChar = (offset.Ticks >= 0L) ? "+" : "-";
                    if (dtime.Millisecond == 0)
                        result += $"{CopyIntToString(4, dtime.Year)}-{CopyIntToString(2, dtime.Month)}-{CopyIntToString(2, dtime.Day)}T{CopyIntToString(2, dtime.Hour)}:{CopyIntToString(2, dtime.Minute)}:{CopyIntToString(2, dtime.Second)}{offsetChar}{CopyIntToString(2, Math.Abs(offset.Hours))}:{CopyIntToString(2, Math.Abs(offset.Minutes))}";
                    else
                        result += $"{CopyIntToString(4, dtime.Year)}-{CopyIntToString(2, dtime.Month)}-{CopyIntToString(2, dtime.Day)}T{CopyIntToString(2, dtime.Hour)}:{CopyIntToString(2, dtime.Minute)}:{CopyIntToString(2, dtime.Second)}.{CopyIntToString(3, dtime.Millisecond)}{offsetChar}{CopyIntToString(2, Math.Abs(offset.Hours))}:{CopyIntToString(2, Math.Abs(offset.Minutes))}";
                    break;
            }

            return result;
        }

        // Converts a number like "4" into "04"
        /// <summary>
        /// A function to convert an integer like "4" into a string like "04"
        /// </summary>
        /// <param name="digits"></param>
        /// <param name="integer"></param>
        /// <returns></returns>
        private static string CopyIntToString(int digits, int integer)
        {
            string result = "";
            string integerStr = integer.ToString();
            int start;
            if (integerStr.Length >= digits) start = 0;
            else start = digits - integerStr.Length;

            int i = 0;
            while (digits-- != 0)
            {
                if (i >= start) result += integerStr[i - start]; else result += "0";
                i++;   
            }
            return result;
        }
    }
}
