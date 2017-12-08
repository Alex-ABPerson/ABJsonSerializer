using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public static bool IsDictionary(this Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        public static object DeserializeValue<T>(string json, bool onlyValue = false)
        {
            return (T)Deserialize(json, typeof(T), onlyValue).value;
        }

        public static JsonKeyValuePair Deserialize(string json, Type typ, bool onlyValue = false, JsonKeyValuePair existing_jkvp = null)
        {
            JsonKeyValuePair ret = new JsonKeyValuePair();

            if (existing_jkvp == null)
                if (onlyValue) ret.value = json; else ret = JsonReader.GetKeyValueData(json);
            else
                ret = existing_jkvp;

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
                    ret.value = DeserializeDictionary(ret.value.ToString(), typ);

                else if (typ == typeof(Bitmap))
                    ret.value = ImageToText.ConvertTextToImage(ret.value.ToString());

                else if (typ == typeof(Image))
                    ret.value = ImageToText.ConvertTextToImage(ret.value.ToString());

                else if (typ == typeof(Point))
                    ret.value = new PointConverter().ConvertFromString(ret.value.ToString());

                else if (typ == typeof(Size))
                    ret.value = new SizeConverter().ConvertFromString(ret.value.ToString());

                else if (typ == typeof(Rectangle))
                    ret.value = new RectangleConverter().ConvertFromString(ret.value.ToString());

                else if (typ == typeof(DateTime))
                    ret.value = DeserializeDateTime(ret.value.ToString());

                else if (typ == typeof(Color))
                    ret.value = new ColorConverter().ConvertFromString(ret.value.ToString());

                else if (typ == typeof(Font))
                    ret.value = new FontConverter().ConvertFromString(ret.value.ToString());

                else if (typ == typeof(char))
                    ret.value = new CharConverter().ConvertFromString(ret.value.ToString());

                else if (typ == typeof(Guid))
                    ret.value = new GuidConverter().ConvertFromString(ret.value.ToString());

                else if (typ != typeof(string))
                    ret.value = DeserializeObject(ret.value.ToString(), typ);

                //else if (Activator.CreateInstance(typ) is Bitmap)
                //    ret.value = ImageToText.ConvertTextToImage(ret.value.ToString());

                //else if (Activator.CreateInstance(typ) is Image)
                //    ret.value = ImageToText.ConvertTextToImage(ret.value.ToString());

                //else if (Activator.CreateInstance(typ) is Point)
                //    ret.value = new Point(int.Parse(ret.value.ToString().Split(',')[0]), int.Parse(ret.value.ToString().Split(',')[1]));
            }
            return ret;
        }

        public static Point DeserializePoint(string json)
        {
            string[] values = json.Split(',');

            return new Point(Convert.ToInt32(values[0]), Convert.ToInt32(values[1]));
        }

        public static Size DeserializeSize(string json)
        {
            string[] values = json.Split(',');

            return new Size(Convert.ToInt32(values[0]), Convert.ToInt32(values[1]));
        }

        public static Rectangle DeserializeRectangle(string json)
        {
            string[] values = json.Split(',');

            return new Rectangle(Convert.ToInt32(values[0]), Convert.ToInt32(values[1]), Convert.ToInt32(values[2]), Convert.ToInt32(values[3]));
        }

        // It is internal because the user SHOULD USE JsonClassConverter.ConvertJsonToObject<T>(json) NOT this, this is only here to parse a type into a function!
        internal static dynamic DeserializeObject(string json, Type typ)
        {
            // Runs JsonClassConverter.ConvertJsonToObject with the type parsed in!
            MethodInfo method = typeof(JsonClassConverter).GetMethod("ConvertJsonToObject");
            MethodInfo generic = method.MakeGenericMethod(typ);
            dynamic value = generic.Invoke(null, new object[] { json, true });

            return value;
        }

        public static dynamic DeserializeArray(string json, Type typ)
        {
            // This function is a mess as it needs to make a List<T> not List<object> and lots of other things like that!
            // The code in the function is not meant to be neat and is pretty much made of a bunch of snippets from stackoverflow.

            Type type;

            if (typ.IsList())
                type = typ.GetListType();
            else
                type = typ.GetElementType();

            json = json.Trim().TrimStart('[').TrimEnd().TrimEnd(']');

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

        public static dynamic DeserializeDictionary(string json, Type typ)
        {
            // This function is also a mess as it needs to make a Dictionary<T, T> not Dictionary<object, object> and lots of other things like that!

            Type[] arguments = typ.GetGenericArguments();
            Type keyType = arguments[0];
            Type valueType = arguments[1];

            json = json.Trim().TrimStart('{').TrimEnd().TrimEnd('}');

            var dictionary = typeof(Dictionary<,>);
            var dictionaryOfType = dictionary.MakeGenericType(keyType, valueType);

            dynamic result = Activator.CreateInstance(dictionaryOfType);

            string[] dictionaryItems = JsonReader.GetAllKeyValues(json);

            foreach (string str in dictionaryItems)
            {
                JsonKeyValuePair jkvp = JsonReader.GetKeyValueData(str);
                dynamic newValue = Deserialize(jkvp.value.ToString(), valueType, true).value;

                bool IsNumeric = keyType.IsNumericType();
                dynamic name = (IsNumeric) ? Convert.ChangeType(Convert.ToDouble(jkvp.name), keyType) : jkvp.name;
                result.Add(name, newValue);
            }

            return result;
        }

        public static dynamic DeserializeDateTime(string dtime)
        {
            // ISO Deserialization (Javascript Ticks coming soon! Don't worry :P )

            // Calculate what kind this is.
            DateTimeKind kind;
            if (dtime.EndsWith("Z")) kind = DateTimeKind.Utc;
            else if (dtime.Contains("+") || dtime.Contains("-")) kind = DateTimeKind.Local;
            else kind = DateTimeKind.Unspecified;

            int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, millisecond = 0;

            dtime = dtime.Replace("Z", ""); // That "Z" is no longer needed! (If it is UTC)

            // For DateTimeKind.Local

            int localOffsetHour = 0, localOffsetMinute = 0;
            long localOffset = 0;
            bool localIsNegative = false;

            // Get the bit before and after the "T".

            // The date (before T) - year, month, day
            string[] parts = dtime.Split('T');

            if (parts.Length <= 0) return null;

            string[] firstparts = parts[0].Split('-');

            if (firstparts.Length > 0) year = int.Parse(firstparts[0]);
            if (firstparts.Length > 1) month = int.Parse(firstparts[1]);
            if (firstparts.Length > 2) day = int.Parse(firstparts[2]);

            // The time (after T) - hour, minute, second, millisecond
            if (parts.Length > 1)
            {
                string[] lastparts;
                lastparts = parts[1].Split('+', '-')[0].Split(':');

                if (lastparts.Length > 0) hour = int.Parse(lastparts[0]);
                if (lastparts.Length > 1) minute = int.Parse(lastparts[1]);
                if (lastparts.Length > 2) {
                    if (lastparts[2].Contains("."))
                    {
                        second = int.Parse(lastparts[2].Split('.')[0]);
                        millisecond = int.Parse(lastparts[2].Split('.')[1]);
                    }
                    else
                        second = int.Parse(lastparts[2]);
                }
            }

            if (kind == DateTimeKind.Local)
            {
                try
                {
                    string[] localparts = parts[1].Split('+', '-')[1].Split(':');

                    localOffsetHour = int.Parse(localparts[0]);
                    localOffsetMinute = int.Parse(localparts[1]);

                    if (!dtime.Contains("+")) localIsNegative = true;

                    localOffset = new TimeSpan(localOffsetHour, localOffsetMinute, 0).Ticks;
                } catch { }
            }
            bool Is24Hour = false;
            if (hour == 24)
            {
                Is24Hour = true;
                hour = 0;
            }

            DateTime d = new DateTime(year, month, day, hour, minute, second, millisecond);

            if (Is24Hour) d.AddDays(1);

            if (localIsNegative)
                return new DateTime(d.Ticks - localOffset, kind);
            else
                return new DateTime(d.Ticks + localOffset, kind);
        }
    }

    public class JsonKeyValuePair
    {
        public object name;
        public object value;
        public JsonKeyValueType type;
    }
}
