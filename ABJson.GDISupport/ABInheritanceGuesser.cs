using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ABJson.GDISupport
{
	// ABInheritanceGuesser Process:
	// 0? If the user just has string[] and type[] convert them into an array of ABInheritanceGuesser
	// 1. Get all of the things that inherit from the base class and loop through them.
	// 		2. Run CompareClassAgainstObjects to get a "InheritanceGuesserMatch" for each value - see below
	//		3. Get a summary which is just the amount of ones that were perfect, nameoff etc.
	//		4. Check if the amount that are incorrect (NameOff, TypeOff and NoCorrelation) is 0, if so, end the loop and just return that type.
	//
	// [End of loop]
	//
	// If we haven't returned a type yet then that means that the user could be missing some values, so we need to find the best possible match from all our summaries.
	// 6. Loop through all the inherited classes again
	//		7. If the "best" is null then set the Best to the current one.
	//		8. Compare the TypesOff of the current one to the best. If there are more types off on the current one then "TypeWorse = true;"
	//		9. Compare the NamesOff of the current one to the best. If there are more names off on the current one then "NameWorse = true;"
	//		10. Check if there are more names worse. If there aren't then this class has passed.
	//			11. If there are more names off, take a look at the types. If the types are more correct then the best and overall there are less problems with this class it has passed!
	//
	//		12. If it has "passed" then if the types aren't worse we will make it the new best!
	//
	// [End of loop]
	//
	// 13. Return the new best.
	
	
    public enum InheritanceGuesserMatch
    {
        /// <summary>
        /// The result of this value is perfect - all names match along with their types!
        /// </summary>
        Perfect,
        /// <summary>
        /// The name of this value is off - however, the type is correct!
        /// </summary>
        NameOff,
        /// <summary>
        /// The type of this value is off - however, the name is correct!
        /// Note: The type being off is considered to be worse then the name being off.
        /// </summary>
        TypeOff,
        /// <summary>
        /// Nor the type or name match.
        /// </summary>
        NoCorrelation,
        /// <summary>
        /// Hasn't been processed yet.
        /// </summary>
        Unknown
    }

    public class ABInheritanceGuesserSummary
    {
        public int TotalAmountPerfect;
        public int TotalAmountOff;
        public int NamesOff;
        public int TypesOff;
        public int CompleteOff;
    }

    public class ABInheritanceGuesserObject
    {
        public ABInheritanceGuesserObject(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public string Name = "";
        public Type Type;
        public ABInheritanceGuesserSummary summary;
    }

    public static class ABInheritanceGuesser
    {
        public static Type GetBestInheritance<T>(string[] names, Type[] types)
        {
            return GetBestInheritanceInternal(typeof(T), names, types);
        }

        public static Type GetBestInheritance<T>(List<ABInheritanceGuesserObject> lstofobjects)
        {
            return GetBestInheritanceInternal(typeof(T), lstofobjects);
        }

        internal static Type GetBestInheritanceInternal(Type baseType, string[] names, Type[] types)
        {
            // Convert the result into a nicely grouped array.
            List<ABInheritanceGuesserObject> objects = new List<ABInheritanceGuesserObject>();
            for (int i = 0; i < names.Length; i++) objects.Add(new ABInheritanceGuesserObject(names[i], types[i]));

            return GetBestInheritanceInternal(baseType, objects);
        }

        internal static Type GetBestInheritanceInternal(Type baseType, List<ABInheritanceGuesserObject> objects)
        {
            // Get all the avalible types
            MethodInfo method = typeof(ABInheritanceGuesser).GetMethod("GetAllInheriters")
                             .MakeGenericMethod(new Type[] { baseType });
            dynamic inheriters = method.Invoke(typeof(ABInheritanceGuesser), new object[] { });

            int i = 0;

            List<ABInheritanceGuesserSummary> summaries = new List<ABInheritanceGuesserSummary>();
            foreach (object obj in inheriters)
            {
                List<InheritanceGuesserMatch> matches = CompareClassAgainstObjects(obj, objects);

                ABInheritanceGuesserSummary summary = GetSummary(matches);
                summaries.Add(summary);
                
                if (summaries[i].TotalAmountOff == 0 && summaries[i].TotalAmountPerfect > 0)                  
                    return obj.GetType(); // This means it is perfect - same amount of types and everything! Use this!

                Console.WriteLine("nyah " + obj.GetType());

                i++;
            }

            // If we are still here it means none of the types were "perfect" and we have to determine the best one out of what we have.

            int a = 0;

            Type BestType = null;
            ABInheritanceGuesserSummary BestSummary = new ABInheritanceGuesserSummary();

            foreach (object obj in inheriters)
            {
                if (BestType == null) { BestType = obj.GetType(); BestSummary = summaries[a]; }
                bool TypeWorse = false;
                bool NameWorse = false;

                if (summaries[a].TypesOff > BestSummary.TypesOff)
                    TypeWorse = true;

                if (summaries[a].NamesOff > BestSummary.NamesOff)
                    NameWorse = true;

                bool passed = false;
                if (NameWorse)
                {
                    if (!TypeWorse)
                    {
                        if (summaries[a].CompleteOff < BestSummary.CompleteOff) // If the best one is worse!
                            passed = true;
                    }
                } else passed = true;

                if (passed) {
                    if (!TypeWorse)
                    {
                        BestType = obj.GetType();
                        BestSummary = summaries[a];
                    }
                }

                a++;
            }

            if (BestType != null) return BestType;

            return baseType; // If it failed to return - it means that none of the inherited types are working, so it must be the original here.
        }

        public static List<InheritanceGuesserMatch> CompareClassAgainstObjects(object baseType, List<ABInheritanceGuesserObject> objects)
        {
            var bindingFlags = BindingFlags.Instance |
                    BindingFlags.Static |
                    BindingFlags.NonPublic |
                    BindingFlags.Public;

            List<string> fieldNames = baseType.GetType().GetFields(bindingFlags)
                        .Select(field => field.Name)
                        .ToList();

            fieldNames.AddRange(baseType.GetType().BaseType.GetFields(bindingFlags)
                        .Select(field => field.Name)
                        .ToList());

            List<Type> fieldTypes = baseType.GetType().GetFields(bindingFlags)
                        .Select(field => field.FieldType)
                        .ToList();

            fieldTypes.AddRange(baseType.GetType().BaseType.GetFields(bindingFlags)
                        .Select(field => field.FieldType)
                        .ToList());

            List<InheritanceGuesserMatch> matches = new List<InheritanceGuesserMatch>();
            
            int i = 0;
            foreach (ABInheritanceGuesserObject obj in objects)
            {
                InheritanceGuesserMatch currentmatch = InheritanceGuesserMatch.Unknown;

                bool NameOff = false;
                bool TypeOff = false;

                try
                {
                    if (fieldNames.Contains(obj.Name))
                    {
                        if (fieldTypes[fieldNames.FindIndex(item => item == obj.Name)] != obj.Type) TypeOff = true;
                    } else NameOff = true;
                } catch { NameOff = true; TypeOff = true; } // If it fails we have gone beyond all the items it has! Meaning this type will be NoCorrelation!

                if (NameOff) currentmatch = InheritanceGuesserMatch.NameOff;
                if (TypeOff) currentmatch = InheritanceGuesserMatch.TypeOff;

                if (NameOff && TypeOff) currentmatch = InheritanceGuesserMatch.NoCorrelation;
                if (currentmatch == InheritanceGuesserMatch.Unknown) currentmatch = InheritanceGuesserMatch.Perfect;

                matches.Add(currentmatch);
                i++;
            }

            return matches;
        }

        public static ABInheritanceGuesserSummary GetSummary(List<InheritanceGuesserMatch> matches)
        {
            ABInheritanceGuesserSummary ret = new ABInheritanceGuesserSummary();
            foreach (InheritanceGuesserMatch match in matches)
                switch (match)
                {
                    case InheritanceGuesserMatch.Perfect:
                        ret.TotalAmountPerfect += 1;
                        break;
                    case InheritanceGuesserMatch.NoCorrelation:
                        ret.CompleteOff += 1;
                        ret.TotalAmountOff += 1;
                        break;
                    case InheritanceGuesserMatch.NameOff:
                        ret.NamesOff += 1;
                        ret.TotalAmountOff += 1;
                        break;
                    case InheritanceGuesserMatch.TypeOff:
                        ret.TypesOff += 1;
                        ret.TotalAmountOff += 1;
                        break;
                }

            return ret;
        }

        public static IEnumerable<T> GetAllInheriters<T>() where T : class
        {
            return typeof(T).Assembly.GetTypes()
                        .Where(t => t.IsSubclassOf(typeof(T)) && t.GetConstructor(Type.EmptyTypes) != null)
                        .Select(t => Activator.CreateInstance(t) as T);
        }
    }
}
