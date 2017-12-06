using ABJson.GDISupport;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Yaml.Serialization;
using YAXLib;

namespace QuickJsonSerialize
{
    
    public partial class Form1 : Form
    {
        InheritanceTest inheritancetest = new InheritanceTest()
        {
            normalstr = "Hello world!",
            normalint = 452,
            inherit1 = new InheritanceInherit1()
            {
                x = 52,
                y = 2512,
                zindex = 1,
                clr1 = Color.FromArgb(4, 47, 132),
                part1 = new Point(24, 7534),
                part2 = new Point(24, 7534),
            },
            inherit2 = new InheritanceInherit2()
            {
                x = 253,
                y = 2,
                zindex = 2,
                shapeType = 9
            }
        };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Foo theFoo = new Foo();
            theFoo.aDictionary.Add("Hello1", new string[] { "Key1", "Key2" });
            theFoo.aDictionary.Add("Hello2", new string[] { "Key2-3", "Key2-2" });


            Stopwatch sw = Stopwatch.StartNew();

            //string json = JsonClassConverter.ConvertObjectToJson(theFoo, JsonFormatting.Indented);
            string json = JsonClassConverter.ConvertObjectToJson(inheritancetest, JsonFormatting.Indented);

            sw.Stop();

            Console.WriteLine("TIME ELAPSED ABJSON: " + sw.ElapsedMilliseconds + "ms");

            Stopwatch sw2 = Stopwatch.StartNew();

            string json2 = Newtonsoft.Json.JsonConvert.SerializeObject(theFoo, Newtonsoft.Json.Formatting.Indented);

            sw2.Stop();

            Console.WriteLine("TIME ELAPSED: " + sw2.ElapsedMilliseconds + "ms");
            //MessageBox.Show(Newtonsoft.Json.JsonConvert.SerializeObject(theFoo, Newtonsoft.Json.Formatting.Indented));

            textBox1.Text = json;
            MessageBox.Show(json2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Foo dtest = JsonClassConverter.ConvertJsonToObject<Foo>(textBox2.Text);

            InheritanceTest itest = JsonClassConverter.ConvertJsonToObject<InheritanceTest>(textBox2.Text);

            MessageBox.Show(itest.ToString());
            //Stopwatch sw = Stopwatch.StartNew();

            //Foo dtest = JsonClassConverter.ConvertJsonToObject<Foo>(textBox2.Text);

            //sw.Stop();

            //Console.WriteLine("TIME ELAPSED ABJSON: " + sw.ElapsedMilliseconds + "ms");

            //Stopwatch sw2 = Stopwatch.StartNew();

            //Foo dtest2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Foo>(textBox2.Text);

            //sw2.Stop();

            //Console.WriteLine("TIME ELAPSED: " + sw2.ElapsedMilliseconds + "ms");

            //MessageBox.Show(dtest.ToString());
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            // Hey you, yeah... you! I know this code is a mess but whatever. Oh also, if see this send a screenshot and DM me it kthx.

            string result = "";

            result += "Multiple Serializers test:" + Environment.NewLine;
            result += "Each serializer will be displayed below and the end will be comparisons." + Environment.NewLine;
            result += "This test is WITHOUT DateTimes due to ABJson not being able to Deserialize DateTimes yet because... DO YOU THINK I HAVE ALL DAY? Oh wait, I do have all day to work on it... well, I can't be bothered to add it now!" + Environment.NewLine;
            result += "I will have a new test however with DateTimes!" + Environment.NewLine;

            result += "Oh, also, one final thing. Stay tuned for one of these with inheritance. Anyway, let's see just HOW FAR these serializers can go!" + Environment.NewLine + Environment.NewLine;

            result += "THE TESTS:" + Environment.NewLine;

            result += "We will put each Serializer through the following tests:" + Environment.NewLine + Environment.NewLine;

            result += "1. Make them serialize a real-life situation class WHERE ALL THE VALUES ARE NULL (see below where it shows the objects we will be serializing)" + Environment.NewLine;
            result += "2. Then make them do the same thing with some actual values." + Environment.NewLine;
            result += "3. After that, we will deserialize their results back into that class and ensure that all values are as they should be." + Environment.NewLine;
            result += "4. Now, it's time to see some of the insane stuff these serializers can really do... so... in comes that SerializersTest class! That will see just how far these Serializers can go." + Environment.NewLine;

            result += "Why do we have the same value twice on SerializersTest? Well, it is simply a matter of speed. I believe (may not be true) that Newtonsoft.Json has some kind of caching system and I want to see if that can dramatically make a change on speed vs ABJson... AND I JUST WANT TO THE SERIALIZERS SUFFER!" + Environment.NewLine;

            result += "Final thing and we can start to see stuff happen... each test will be marked with ============= so look out for that - at the end is where it compares all of them in one batch so take a look at that for comparisons." + Environment.NewLine;
            result += "THE OBJECTS WE WILL BE SERIALIZING (in C#)" + Environment.NewLine;

            result += "=============================" + Environment.NewLine;
            result += System.IO.File.ReadAllText("SerializerTest.txt") + Environment.NewLine;
            result += "=============================" + Environment.NewLine + Environment.NewLine;

            result += "Alright, now that you know EXACTLY what's going on here let's ACTUALLY SEE SOME DAMN RESULTS!" + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;

            result += ">>>>>>>>>>>>>>>> TEST 1: REAL-LIFE SITUATION WITH ALL BEING NULL <<<<<<<<<<<<<<<<<" + Environment.NewLine + Environment.NewLine;            

            RealLifeExample test1 = new RealLifeExample();
            
            //csvWrite.Configuration.RegisterClassMap(new RealLifeExampleMap());
            //csvWrite.Configuration.RegisterClassMap(new HouseMap());
            //csvWrite.Configuration.RegisterClassMap(new JobMap());
            //csvWrite.Configuration.RegisterClassMap(new ObjectMap());
            //csvWrite.Configuration.RegisterClassMap(new UpgradeMap());
            //csvWrite.Configuration.RegisterClassMap(new HobbyMap());

            #region Test 1
            #region Newtonsoft.Json JSON
            result += ">>> Newtonsoft.Json JSON: <<<" + Environment.NewLine;

            bool njsonex1 = false;
            Stopwatch JSONNet1 = Stopwatch.StartNew();

            string JSONNET1str = "";
            try { JSONNET1str = Newtonsoft.Json.JsonConvert.SerializeObject(test1); } catch (Exception ex) { njsonex1 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            JSONNet1.Stop();

            result += "SPEED: " + JSONNet1.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + JSONNET1str.Length + Environment.NewLine;
            result += "FAILED (exception was thrown): " + njsonex1.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + JSONNET1str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;

            #endregion

            #region Newtonsoft.Json BSON
            result += ">>> Newtonsoft.Json BSON: <<<" + Environment.NewLine;

            bool nbsonex1 = false;
            Stopwatch BSONNet1 = Stopwatch.StartNew();

            string BSONNET1str = "";
            int BSONNET1size = 0;

            try {
                MemoryStream ms = new MemoryStream();
                byte[] bytes;
                using(BsonWriter writer = new BsonWriter(ms))
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.Serialize(writer, test1);
                    BSONNET1size = (int)ms.Length;
                    bytes = ms.ToArray();
                }

                BSONNet1.Stop();              
                BSONNET1str = Convert.ToBase64String(bytes);
            } catch (Exception ex) { nbsonex1 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            BSONNet1.Stop();

            result += "SPEED: " + BSONNet1.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + BSONNET1str.Length + Environment.NewLine;
            result += "FAILED (exception was thrown): " + nbsonex1.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + BSONNET1str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;

            #endregion

            #region System.Xml XML
            result += ">>> System.Xml XML: <<<" + Environment.NewLine;
            bool xmlex1 = false;
            Stopwatch XML1 = Stopwatch.StartNew();

            string XML1str = "";
            try
            {
                var serializer = new XmlSerializer(test1.GetType());
                using (var writer = new StringWriter())
                {
                    serializer.Serialize(writer, test1);
                    XML1str = writer.ToString();
                }
                
            }
            catch (Exception ex) { xmlex1 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            XML1.Stop();

            result += "SPEED: " + XML1.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + XML1str.Length + Environment.NewLine;
            result += "FAILED (exception was thrown): " + xmlex1.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + XML1str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;
            #endregion

            #region YAXLib
            result += ">>> YAXLib XML: <<<" + Environment.NewLine;
            bool YAXex1 = false;
            Stopwatch YAXLib1 = Stopwatch.StartNew();

            string YAXLib1str = "";
            try
            {
                YAXSerializer serializer = new YAXSerializer(typeof(RealLifeExample));
                YAXLib1str = serializer.Serialize(test1);

            }
            catch (Exception ex) { YAXex1 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            YAXLib1.Stop();

            result += "SPEED: " + YAXLib1.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + YAXLib1str.Length + Environment.NewLine;
            result += "FAILED (exception was thrown): " + YAXex1.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + YAXLib1str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;
            #endregion

            #region CSVHelper
            //result += ">>> CSVHelper CSV: <<<" + Environment.NewLine;
            //bool CSVex1 = false;
            //Stopwatch CSVHelp1 = Stopwatch.StartNew();

            //string CSVHelp1str = "";
            //try
            //{
            //    csvWrite.WriteRecord(test1);
            //    CSVHelp1str = textWriter.ToString();
            //}
            //catch (Exception ex) { CSVex1 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            //CSVHelp1.Stop();

            //result += "SPEED: " + CSVHelp1.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            //result += "SIZE (bytes): " + CSVHelp1str.Length + Environment.NewLine;
            //result += "FAILED (exception was thrown): " + CSVex1.ToString() + Environment.NewLine;
            //result += Environment.NewLine + "================================" + Environment.NewLine + CSVHelp1str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;
            #endregion

            #region YAMLSerializer
            result += ">>> YAMLSerializer YAML: <<<" + Environment.NewLine;
            bool YAMLex1 = false;
            Stopwatch YAML1 = Stopwatch.StartNew();

            string YAML1str = "";
            try
            {
                var serializer = new YamlSerializer();
                YAML1str = serializer.Serialize(test1);
            }
            catch (Exception ex) { YAMLex1 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            YAML1.Stop();

            result += "SPEED: " + YAML1.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + YAML1str.Length + Environment.NewLine;
            result += "FAILED (exception was thrown): " + YAMLex1.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + YAML1str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;
            #endregion

            #region Whoa
            result += ">>> WHOA WHOA: <<<" + Environment.NewLine;
            bool WHOAex1 = false;
            Stopwatch WHOA1 = Stopwatch.StartNew();

            string WHOA1str = "";
            int WHOA1size = 0;
            try
            {
                using (var ms = new MemoryStream())
                {
                    Whoa.Whoa.SerialiseObject<RealLifeExample>(ms, test1, Whoa.SerialisationOptions.NonSerialized);
                    WHOA1size = (int)ms.Length;
                    WHOA1str = BitConverter.ToString(ms.ToArray()).Replace("-", " ");
                }
            }
            catch (Exception ex) { WHOAex1 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            WHOA1.Stop();

            result += "SPEED: " + WHOA1.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + WHOA1size + Environment.NewLine;
            result += "FAILED (exception was thrown): " + WHOAex1.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + WHOA1str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;
            #endregion

            #region BinaryFormatter
            result += ">>> System.Runtime.Serialization.Formatters.Binary.BinaryFormatter BinaryFormat?: <<<" + Environment.NewLine;
            bool BFORMATex1 = false;
            Stopwatch BFORMAT1 = Stopwatch.StartNew();

            string BFORMAT1str = "";
            int BFORMAT1size = 0;
            try
            {
                using (var ms = new MemoryStream())
                {
                    BinaryFormatter bformat = new BinaryFormatter();
                    bformat.Serialize(ms, test1);
                    BFORMAT1size = (int)ms.Length;
                    BFORMAT1str = BitConverter.ToString(ms.ToArray()).Replace("-", " ");
                }
            }
            catch (Exception ex) { BFORMATex1 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            BFORMAT1.Stop();

            result += "SPEED: " + BFORMAT1.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + BFORMAT1size.ToString() + Environment.NewLine;
            result += "FAILED (exception was thrown): " + BFORMATex1.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + BFORMAT1str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;
            #endregion

            #region ABJson
            result += ">>> ABJson JSON: <<<" + Environment.NewLine;
            bool ABJSONex1 = false;
            Stopwatch ABJSON1 = Stopwatch.StartNew();

            string ABJSONstr = "";
            try { ABJSONstr = JsonClassConverter.ConvertObjectToJson(test1, JsonFormatting.Compact); } catch (Exception ex) { ABJSONex1 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            ABJSON1.Stop();

            result += "SPEED: " + ABJSON1.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + ABJSONstr.Length + Environment.NewLine;
            result += "FAILED (exception was thrown): " + ABJSONex1.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + ABJSONstr + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;
            #endregion


            result += "====== TEST 1 RESULTS: ======" + Environment.NewLine + Environment.NewLine;
            result += Environment.NewLine + "FAILED:" + Environment.NewLine + Environment.NewLine;

            if (njsonex1) result += "Newtonsoft.Json" + Environment.NewLine;
            if (nbsonex1) result += "Newtonsoft.Bson" + Environment.NewLine;
            if (xmlex1) result += "System.Xml" + Environment.NewLine;
            if (YAXex1) result += "Yax" + Environment.NewLine;           
            if (YAMLex1) result += "YAMLSerializer" + Environment.NewLine;
            if (WHOAex1) result += "Whoa" + Environment.NewLine;
            if (BFORMATex1) result += "BinaryFormatter" + Environment.NewLine;
            if (ABJSONex1) result += "ABJson" + Environment.NewLine;

            result += Environment.NewLine + "SIZES:" + Environment.NewLine + Environment.NewLine;

            if (!njsonex1) result += "Newtonsoft.Json: " + JSONNET1str.Length + Environment.NewLine;
            if (!nbsonex1) result += "Newtonsoft.Bson: " + BSONNET1size.ToString() + Environment.NewLine;
            if (!xmlex1) result += "System.Xml: " + XML1str.Length + Environment.NewLine;
            if (!YAXex1) result += "Yax: " + YAXLib1str.Length + Environment.NewLine;            
            if (!YAMLex1) result += "YAMLSerializer: " + YAML1str.Length + Environment.NewLine;
            if (!WHOAex1) result += "Whoa: " + WHOA1size + Environment.NewLine;
            if (!BFORMATex1) result += "BinaryFormatter: " + BFORMAT1str.Length + Environment.NewLine;
            if (!ABJSONex1) result += "ABJson: " + ABJSONstr.Length + Environment.NewLine;

            result += Environment.NewLine + "SPEEDS:" + Environment.NewLine + Environment.NewLine;

            if (!njsonex1) result += "Newtonsoft.Json: " + JSONNet1.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!nbsonex1) result += "Newtonsoft.Bson: " + BSONNet1.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!xmlex1) result += "System.Xml: " + XML1.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!YAXex1) result += "Yax: " + YAXLib1.ElapsedMilliseconds + "ms" + Environment.NewLine;        
            if (!YAMLex1) result += "YAMLSerializer: " + YAML1.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!WHOAex1) result += "Whoa: " + WHOA1.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!BFORMATex1) result += "BinaryFormatter: " + BFORMAT1.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!ABJSONex1) result += "ABJson: " + ABJSON1.ElapsedMilliseconds + "ms" + Environment.NewLine;
            #endregion

            result += ">>>>>>>>>>>>>>>> TEST 2: DESERIALIZE RESULTS <<<<<<<<<<<<<<<<<" + Environment.NewLine + Environment.NewLine;

            #region Test2
            #region Newtonsoft.Json JSON
            result += ">>> Newtonsoft.Json JSON: <<<" + Environment.NewLine;

            bool njsonex2 = false;
            Stopwatch JSONNet2 = Stopwatch.StartNew();

            RealLifeExample JSONNET2obj = null;
            try { JSONNET2obj = Newtonsoft.Json.JsonConvert.DeserializeObject<RealLifeExample>(JSONNET1str); } catch (Exception ex) { njsonex2 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            JSONNet2.Stop();

            result += "SPEED: " + JSONNet2.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "FAILED (exception was thrown): " + njsonex2.ToString() + Environment.NewLine + Environment.NewLine;

            #endregion

            #region Newtonsoft.Json BSON
            result += ">>> Newtonsoft.Json BSON: <<<" + Environment.NewLine;

            bool nbsonex2 = false;
            Stopwatch BSONNet2 = Stopwatch.StartNew();

            RealLifeExample BSONNET2obj = null;
            try
            {
                
                byte[] data = Convert.FromBase64String(BSONNET1str);
                MemoryStream ms = new MemoryStream();
                using (BsonReader reader = new BsonReader(ms))
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    BSONNET2obj = serializer.Deserialize<RealLifeExample>(reader);
                }               
            }
            catch (Exception ex) { nbsonex2 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            BSONNet2.Stop();

            result += "SPEED: " + BSONNet2.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "FAILED (exception was thrown): " + nbsonex2.ToString() + Environment.NewLine + Environment.NewLine;

            #endregion

            #region System.Xml XML
            result += ">>> System.Xml XML: <<<" + Environment.NewLine;
            bool xmlex2 = false;
            Stopwatch XML2 = Stopwatch.StartNew();

            object XML2obj = null;
            try
            {
                var serializer = new XmlSerializer(test1.GetType());
                using (var reader = new StringReader(XML1str))
                {
                    serializer.Deserialize(reader);
                    XML2obj = reader.ToString();
                }

            }
            catch (Exception ex) { xmlex2 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            XML2.Stop();

            result += "SPEED: " + XML2.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "FAILED (exception was thrown): " + xmlex2.ToString() + Environment.NewLine + Environment.NewLine;

            #endregion

            #region YAXLib
            result += ">>> YAXLib XML: <<<" + Environment.NewLine;
            bool YAXex2 = false;
            Stopwatch YAXLib2 = Stopwatch.StartNew();

            object YAXLib2obj = null;
            try
            {
                YAXSerializer serializer = new YAXSerializer(typeof(RealLifeExample));
                YAXLib2obj = serializer.Deserialize(YAXLib1str);
            }
            catch (Exception ex) { YAXex2 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            YAXLib2.Stop();

            result += "SPEED: " + YAXLib2.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "FAILED (exception was thrown): " + YAXex2.ToString() + Environment.NewLine + Environment.NewLine;
            #endregion

            #region CSVHelper
            //result += ">>> CSVHelper CSV: <<<" + Environment.NewLine;
            //bool CSVex2 = false;
            //Stopwatch CSVHelp2 = Stopwatch.StartNew();

            //object CSVHelp2obj = null;
            //try
            //{                   
            //    var records = csvRead.GetRecords<dynamic>();
            //    CSVHelp2obj = records;

            //}
            //catch (Exception ex) { CSVex2 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            //CSVHelp2.Stop();

            //result += "SPEED: " + CSVHelp2.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            //result += "FAILED (exception was thrown): " + CSVex2.ToString() + Environment.NewLine + Environment.NewLine;
            #endregion

            #region YAMLSerializer
            result += ">>> YAMLSerializer YAML: <<<" + Environment.NewLine;
            bool YAMLex2 = false;
            Stopwatch YAML2 = Stopwatch.StartNew();

            object YAML2obj = null;
            try
            {
                var serializer = new YamlSerializer();
                YAML2obj = serializer.Deserialize(YAML1str, typeof(RealLifeExample));
            }
            catch (Exception ex) { YAMLex2 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            YAML2.Stop();

            result += "SPEED: " + YAML2.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "FAILED (exception was thrown): " + YAMLex2.ToString() + Environment.NewLine + Environment.NewLine;
            #endregion

            #region Whoa
            result += ">>> WHOA WHOA: <<<" + Environment.NewLine;
            bool WHOAex2 = false;
            Stopwatch WHOA2 = Stopwatch.StartNew();

            object WHOA2obj = null;
            try
            {
                using (var ms = new MemoryStream(Encoding.ASCII.GetBytes(WHOA1str)))
                    WHOA2obj = Whoa.Whoa.DeserialiseObject<RealLifeExample>(ms);

            }
            catch (Exception ex) { WHOAex2 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            WHOA2.Stop();

            result += "SPEED: " + WHOA2.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "FAILED (exception was thrown): " + WHOAex2.ToString() + Environment.NewLine + Environment.NewLine;
            #endregion

            #region BinaryFormatter
            result += ">>> System.Runtime.Serialization.Formatters.Binary.BinaryFormatter BinaryFormat?: <<<" + Environment.NewLine;
            bool BFORMATex2 = false;
            Stopwatch BFORMAT2 = Stopwatch.StartNew();

            object BFORMAT2obj = null;
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter bformat = new BinaryFormatter();
                    BFORMAT2obj = bformat.Deserialize(stream);
                    StreamReader reader = new StreamReader(stream);
                    BFORMAT1str = reader.ReadToEnd();
                }

            }
            catch (Exception ex) { BFORMATex2 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            BFORMAT2.Stop();

            result += "SPEED: " + BFORMAT2.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "FAILED (exception was thrown): " + BFORMATex2.ToString() + Environment.NewLine;
            #endregion

            #region ABJson
            result += ">>> ABJson JSON: <<<" + Environment.NewLine;
            bool ABJSONex2 = false;
            Stopwatch ABJSON2 = Stopwatch.StartNew();

            object ABJSON2obj = null;
            try { ABJSON2obj = JsonClassConverter.ConvertJsonToObject<RealLifeExample>(ABJSONstr); } catch (Exception ex) { ABJSONex2 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            ABJSON2.Stop();

            result += "SPEED: " + ABJSON2.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "FAILED (exception was thrown): " + ABJSONex2.ToString() + Environment.NewLine + Environment.NewLine;
            #endregion

            result += "====== TEST 2 RESULTS: ======" + Environment.NewLine + Environment.NewLine;
            result += Environment.NewLine + "FAILED:" + Environment.NewLine + Environment.NewLine;

            if (njsonex2) result += "Newtonsoft.Json" + Environment.NewLine;
            if (nbsonex2) result += "Newtonsoft.Bson" + Environment.NewLine;
            if (xmlex2) result += "System.Xml" + Environment.NewLine;
            if (YAXex2) result += "Yax" + Environment.NewLine;           
            if (YAMLex2) result += "YAMLSerializer" + Environment.NewLine;
            if (WHOAex2) result += "Whoa" + Environment.NewLine;
            if (BFORMATex2) result += "BinaryFormatter" + Environment.NewLine;
            if (ABJSONex2) result += "ABJson" + Environment.NewLine;

            result += Environment.NewLine + "MATCHES (If the original exactly matches the new):" + Environment.NewLine + Environment.NewLine;

            try { if (JSONNET2obj.Equals(test1)) result += "Newtonsoft.Json" + Environment.NewLine; } catch { }
            try { if (BSONNET2obj.Equals(test1)) result += "Newtonsoft.Bson" + Environment.NewLine; } catch { }
            try { if (XML2obj.Equals(test1)) result += "System.Xml" + Environment.NewLine; } catch { }
            try { if (YAXLib2obj.Equals(test1)) result += "Yax" + Environment.NewLine; } catch { }            
            try { if (YAML2obj.Equals(test1)) result += "YAMLSerializer" + Environment.NewLine; } catch { }
            try { if (WHOA2obj.Equals(test1)) result += "Whoa" + Environment.NewLine; } catch { }
            try { if (BFORMAT2obj.Equals(test1)) result += "BinaryFormatter" + Environment.NewLine; } catch { }
            try { if (ABJSON2obj.Equals(test1)) result += "ABJson" + Environment.NewLine; } catch { }

            result += Environment.NewLine + "SPEEDS:" + Environment.NewLine + Environment.NewLine;

            if (!njsonex2) result += "Newtonsoft.Json: " + JSONNet2.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!nbsonex2) result += "Newtonsoft.Bson: " + BSONNet2.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!xmlex2) result += "System.Xml: " + XML2.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!YAXex2) result += "Yax: " + YAXLib2.ElapsedMilliseconds + "ms" + Environment.NewLine;
            
            if (!YAMLex2) result += "YAMLSerializer: " + YAML2.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!WHOAex2) result += "Whoa: " + WHOA2.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!BFORMATex2) result += "BinaryFormatter: " + BFORMAT2.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!ABJSONex2) result += "ABJson: " + ABJSON2.ElapsedMilliseconds + "ms" + Environment.NewLine;
            #endregion

            RealLifeExample test3 = new RealLifeExample()
            {
                Houses = new List<House>
                {
                    new House()
                    {
                        Location = new Point(2, 3),
                        Height = 60,
                        upgrades = new Dictionary<int, Upgrade>(),
                        Objects = new List<Object>
                        {
                            new Object()
                            {
                                Name = "Book 1",
                                Usage = "The first ever book!!!",
                                color = Color.Aqua
                            },
                            new Object()
                            {
                                Name = "Book 2",
                                Usage = "The second ever book!!!",
                                color = Color.Aquamarine
                            }
                        }
                    },
                    new House()
                    {
                        Location = new Point(8, 5),
                        Height = 90,
                        upgrades = new Dictionary<int, Upgrade>(),
                        Objects = new List<Object>
                        {
                            new Object()
                            {
                                Name = "Computer 1",
                                Usage = "The first ever computer!!!",
                                color = Color.Green
                            },
                            new Object()
                            {
                                Name = "Potato 1",
                                Usage = "Food?!?!",
                                color = Color.Orange
                            }
                        }
                    }
                },
                People = new List<Person>
                {
                    new Person()
                    {
                        Job = new List<Job>
                        {
                            new Job()
                            {
                                Name = "Teaching",
                                Money = 5
                            },
                            new Job()
                            {
                                Name = "Programming",
                                Money = 20
                            }
                        }
                    }
                }
            };

            test3.Houses[0].upgrades.Add(1, new Upgrade() { willExtendWeight = true });

            result += ">>>>>>>>>>>>>>>> TEST 3: REAL-LIFE SITUATION WITH LOTS OF VALUES! <<<<<<<<<<<<<<<<<" + Environment.NewLine + Environment.NewLine;

            #region Test 3
            #region Newtonsoft.Json JSON
            result += ">>> Newtonsoft.Json JSON: <<<" + Environment.NewLine;

            bool njsonex3 = false;
            Stopwatch JSONNet3 = Stopwatch.StartNew();

            string JSONNET3str = "";
            try { JSONNET3str = Newtonsoft.Json.JsonConvert.SerializeObject(test3); } catch (Exception ex) { njsonex3 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            JSONNet3.Stop();

            result += "SPEED: " + JSONNet3.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + JSONNET3str.Length + Environment.NewLine;
            result += "FAILED (exception was thrown): " + njsonex3.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + JSONNET3str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;

            #endregion

            #region Newtonsoft.Json BSON
            result += ">>> Newtonsoft.Json BSON: <<<" + Environment.NewLine;

            bool nbsonex3 = false;
            Stopwatch BSONNet3 = Stopwatch.StartNew();

            string BSONNET3str = "";
            int BSONNET3size = 0;
            try
            {
                byte[] bytes;
                MemoryStream ms = new MemoryStream();
                using (BsonWriter writer = new BsonWriter(ms))
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.Serialize(writer, test1);
                    BSONNET3size = (int)ms.Length;
                    bytes = ms.ToArray();
                }
                BSONNet1.Stop();

                BSONNET3str = Convert.ToBase64String(bytes);
            }
            catch (Exception ex) { nbsonex3 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            

            result += "SPEED: " + BSONNet3.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + BSONNET3str.Length + Environment.NewLine;
            result += "FAILED (exception was thrown): " + nbsonex3.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + BSONNET3str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;

            #endregion

            #region System.Xml XML
            result += ">>> System.Xml XML: <<<" + Environment.NewLine;
            bool xmlex3 = false;
            Stopwatch XML3 = Stopwatch.StartNew();

            string XML3str = "";
            try
            {
                var serializer = new XmlSerializer(test3.GetType());
                using (var writer = new StringWriter())
                {
                    serializer.Serialize(writer, test3);
                    XML3str = writer.ToString();
                }

            }
            catch (Exception ex) { xmlex3 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            XML3.Stop();

            result += "SPEED: " + XML3.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + XML3str.Length + Environment.NewLine;
            result += "FAILED (exception was thrown): " + xmlex3.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + XML3str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;
            #endregion

            #region YAXLib
            result += ">>> YAXLib XML: <<<" + Environment.NewLine;
            bool YAXex3 = false;
            Stopwatch YAXLib3 = Stopwatch.StartNew();

            string YAXLib3str = "";
            try
            {
                YAXSerializer serializer = new YAXSerializer(typeof(RealLifeExample));
                YAXLib3str = serializer.Serialize(test3);
            }
            catch (Exception ex) { YAXex3 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            YAXLib3.Stop();

            result += "SPEED: " + YAXLib3.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + YAXLib3str.Length + Environment.NewLine;
            result += "FAILED (exception was thrown): " + YAXex3.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + YAXLib3str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;
            #endregion

            #region YAMLSerializer
            result += ">>> YAMLSerializer YAML: <<<" + Environment.NewLine;
            bool YAMLex3 = false;
            Stopwatch YAML3 = Stopwatch.StartNew();

            string YAML3str = "";
            try
            {
                var serializer = new YamlSerializer();
                YAML3str = serializer.Serialize(test3);
            }
            catch (Exception ex) { YAMLex3 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            YAML3.Stop();

            result += "SPEED: " + YAML3.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + YAML3str.Length + Environment.NewLine;
            result += "FAILED (exception was thrown): " + YAMLex3.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + YAML3str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;
            #endregion

            #region Whoa
            result += ">>> WHOA WHOA: <<<" + Environment.NewLine;
            bool WHOAex3 = false;
            Stopwatch WHOA3 = Stopwatch.StartNew();

            string WHOA3str = "";
            int WHOA3size = 0;
            try
            {
                using (var ms = new MemoryStream())
                {
                    Whoa.Whoa.SerialiseObject<RealLifeExample>(ms, test3, Whoa.SerialisationOptions.NonSerialized);
                    WHOA3size = (int)ms.Length;
                    WHOA3str = BitConverter.ToString(ms.ToArray()).Replace("-", " ");
                }

            }
            catch (Exception ex) { WHOAex3 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            WHOA3.Stop();

            result += "SPEED: " + WHOA3.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + WHOA3size.ToString() + Environment.NewLine;
            result += "FAILED (exception was thrown): " + WHOAex3.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + WHOA3str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;
            #endregion

            #region BinaryFormatter
            result += ">>> System.Runtime.Serialization.Formatters.Binary.BinaryFormatter BinaryFormat?: <<<" + Environment.NewLine;
            bool BFORMATex3 = false;
            Stopwatch BFORMAT3 = Stopwatch.StartNew();

            string BFORMAT3str = "";
            int BFORMAT3size = 0;
            try
            {
                using (var ms = new MemoryStream())
                {
                    BinaryFormatter bformat = new BinaryFormatter();
                    bformat.Serialize(ms, test3);
                    BFORMAT3size = (int)ms.Length;
                    BFORMAT3str = BitConverter.ToString(ms.ToArray()).Replace("-", " ");
                }
            }
            catch (Exception ex) { BFORMATex3 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            BFORMAT3.Stop();

            result += "SPEED: " + BFORMAT3.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + BFORMAT3size.ToString() + Environment.NewLine;
            result += "FAILED (exception was thrown): " + BFORMATex3.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + BFORMAT3str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;
            #endregion

            #region ABJson
            result += ">>> ABJson JSON: <<<" + Environment.NewLine;
            bool ABJSONex3 = false;
            Stopwatch ABJSON3 = Stopwatch.StartNew();

            string ABJSON3str = "";
            /* try { */ ABJSON3str = JsonClassConverter.ConvertObjectToJson(test3, JsonFormatting.Compact); // } catch (Exception ex) { ABJSONex3 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            ABJSON3.Stop();

            result += "SPEED: " + ABJSON3.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + ABJSON3str.Length + Environment.NewLine;
            result += "FAILED (exception was thrown): " + ABJSONex3.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + ABJSON3str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;
            #endregion


            result += "====== TEST 3 RESULTS: ======" + Environment.NewLine + Environment.NewLine;
            result += Environment.NewLine + "FAILED:" + Environment.NewLine + Environment.NewLine;

            if (njsonex3) result += "Newtonsoft.Json" + Environment.NewLine;
            if (nbsonex3) result += "Newtonsoft.Bson" + Environment.NewLine;
            if (xmlex3) result += "System.Xml" + Environment.NewLine;
            if (YAXex3) result += "Yax" + Environment.NewLine;
            if (YAMLex3) result += "YAMLSerializer" + Environment.NewLine;
            if (WHOAex3) result += "Whoa" + Environment.NewLine;
            if (BFORMATex3) result += "BinaryFormatter" + Environment.NewLine;
            if (ABJSONex3) result += "ABJson" + Environment.NewLine;

            result += Environment.NewLine + "SIZES:" + Environment.NewLine + Environment.NewLine;

            if (!njsonex3) result += "Newtonsoft.Json: " + JSONNET3str.Length + Environment.NewLine;
            if (!nbsonex3) result += "Newtonsoft.Bson: " + BSONNET3size.ToString() + Environment.NewLine;
            if (!xmlex3) result += "System.Xml: " + XML3str.Length + Environment.NewLine;
            if (!YAXex3) result += "Yax: " + YAXLib3str.Length + Environment.NewLine;
            if (!YAMLex3) result += "YAMLSerializer: " + YAML3str.Length + Environment.NewLine;
            if (!WHOAex3) result += "Whoa: " + WHOA3size.ToString() + Environment.NewLine;
            if (!BFORMATex3) result += "BinaryFormatter: " + BFORMAT3size.ToString() + Environment.NewLine;
            if (!ABJSONex3) result += "ABJson: " + ABJSON3str.Length + Environment.NewLine;

            result += Environment.NewLine + "SPEEDS:" + Environment.NewLine + Environment.NewLine;

            if (!njsonex3) result += "Newtonsoft.Json: " + JSONNet3.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!nbsonex3) result += "Newtonsoft.Bson: " + BSONNet3.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!xmlex3) result += "System.Xml: " + XML3.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!YAXex3) result += "Yax: " + YAXLib3.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!YAMLex3) result += "YAMLSerializer: " + YAML3.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!WHOAex3) result += "Whoa: " + WHOA3.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!BFORMATex3) result += "BinaryFormatter: " + BFORMAT3.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!ABJSONex3) result += "ABJson: " + ABJSON3.ElapsedMilliseconds + "ms" + Environment.NewLine;
            #endregion

            result += ">>>>>>>>>>>>>>>> TEST 4: DESERIALIZE RESULTS <<<<<<<<<<<<<<<<<" + Environment.NewLine + Environment.NewLine;

            #region Test4
            #region Newtonsoft.Json JSON
            result += ">>> Newtonsoft.Json JSON: <<<" + Environment.NewLine;

            bool njsonex4 = false;
            Stopwatch JSONNet4 = Stopwatch.StartNew();

            RealLifeExample JSONNET4obj = null;
            try { JSONNET4obj = Newtonsoft.Json.JsonConvert.DeserializeObject<RealLifeExample>(JSONNET3str); } catch (Exception ex) { njsonex4 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            JSONNet4.Stop();

            result += "SPEED: " + JSONNet4.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "FAILED (exception was thrown): " + njsonex4.ToString() + Environment.NewLine + Environment.NewLine;

            #endregion

            #region Newtonsoft.Json BSON
            result += ">>> Newtonsoft.Json BSON: <<<" + Environment.NewLine;

            bool nbsonex4 = false;
            Stopwatch BSONNet4 = Stopwatch.StartNew();

            RealLifeExample BSONNET4obj = null;
            try
            {

                byte[] data = Convert.FromBase64String(BSONNET3str);
                MemoryStream ms = new MemoryStream();
                using (BsonReader reader = new BsonReader(ms))
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    BSONNET4obj = serializer.Deserialize<RealLifeExample>(reader);
                }
            }
            catch (Exception ex) { nbsonex4 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            BSONNet4.Stop();

            result += "SPEED: " + BSONNet4.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "FAILED (exception was thrown): " + nbsonex4.ToString() + Environment.NewLine + Environment.NewLine;

            #endregion

            #region System.Xml XML
            result += ">>> System.Xml XML: <<<" + Environment.NewLine;
            bool xmlex4 = false;
            Stopwatch XML4 = Stopwatch.StartNew();

            object XML4obj = null;
            try
            {
                var serializer = new XmlSerializer(test3.GetType());
                using (var reader = new StringReader(XML3str))
                {
                    serializer.Deserialize(reader);
                    XML4obj = reader.ToString();
                }

            }
            catch (Exception ex) { xmlex4 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            XML4.Stop();

            result += "SPEED: " + XML4.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "FAILED (exception was thrown): " + xmlex4.ToString() + Environment.NewLine + Environment.NewLine;

            #endregion

            #region YAXLib
            result += ">>> YAXLib XML: <<<" + Environment.NewLine;
            bool YAXex4 = false;
            Stopwatch YAXLib4 = Stopwatch.StartNew();

            object YAXLib4obj = null;
            try
            {
                YAXSerializer serializer = new YAXSerializer(typeof(RealLifeExample));
                YAXLib4obj = serializer.Deserialize(YAXLib3str);
            }
            catch (Exception ex) { YAXex4 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            YAXLib4.Stop();

            result += "SPEED: " + YAXLib4.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "FAILED (exception was thrown): " + YAXex4.ToString() + Environment.NewLine + Environment.NewLine;
            #endregion

            #region YAMLSerializer
            result += ">>> YAMLSerializer YAML: <<<" + Environment.NewLine;
            bool YAMLex4 = false;
            Stopwatch YAML4 = Stopwatch.StartNew();

            object YAML4obj = null;
            try
            {
                var serializer = new YamlSerializer();
                YAML2obj = serializer.Deserialize(YAML3str, typeof(RealLifeExample));
            }
            catch (Exception ex) { YAMLex4 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            YAML4.Stop();

            result += "SPEED: " + YAML4.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "FAILED (exception was thrown): " + YAMLex4.ToString() + Environment.NewLine + Environment.NewLine;
            #endregion

            #region Whoa
            result += ">>> WHOA WHOA: <<<" + Environment.NewLine;
            bool WHOAex4 = false;
            Stopwatch WHOA4 = Stopwatch.StartNew();

            object WHOA4obj = null;
            try
            {
                using (var ms = new MemoryStream(Encoding.ASCII.GetBytes(WHOA3str)))                
                    WHOA4obj = Whoa.Whoa.DeserialiseObject<RealLifeExample>(ms);

            }
            catch (Exception ex) { WHOAex4 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            WHOA4.Stop();

            result += "SPEED: " + WHOA4.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "FAILED (exception was thrown): " + WHOAex4.ToString() + Environment.NewLine + Environment.NewLine;
            #endregion

            #region BinaryFormatter
            result += ">>> System.Runtime.Serialization.Formatters.Binary.BinaryFormatter BinaryFormat?: <<<" + Environment.NewLine;
            bool BFORMATex4 = false;
            Stopwatch BFORMAT4 = Stopwatch.StartNew();

            object BFORMAT4obj = null;
            try
            {
                using (var ms = new MemoryStream(Encoding.ASCII.GetBytes(BFORMAT3str)))
                {                  
                    BinaryFormatter bformat = new BinaryFormatter();
                    BFORMAT4obj = bformat.Deserialize(ms);
                }

            }
            catch (Exception ex) { BFORMATex4 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            BFORMAT4.Stop();

            result += "SPEED: " + BFORMAT4.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "FAILED (exception was thrown): " + BFORMATex4.ToString() + Environment.NewLine + Environment.NewLine;
            #endregion

            #region ABJson
            result += ">>> ABJson JSON: <<<" + Environment.NewLine;
            bool ABJSONex4 = false;
            Stopwatch ABJSON4 = Stopwatch.StartNew();

            object ABJSON4obj = null;
            try { ABJSON4obj = JsonClassConverter.ConvertJsonToObject<RealLifeExample>(ABJSON3str); } catch (Exception ex) { ABJSONex4 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            ABJSON4.Stop();

            result += "SPEED: " + ABJSON4.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "FAILED (exception was thrown): " + ABJSONex4.ToString() + Environment.NewLine + Environment.NewLine;
            #endregion

            result += "====== TEST 4 RESULTS: ======" + Environment.NewLine + Environment.NewLine;
            result += Environment.NewLine + "FAILED:" + Environment.NewLine + Environment.NewLine;

            if (njsonex4) result += "Newtonsoft.Json" + Environment.NewLine;
            if (nbsonex4) result += "Newtonsoft.Bson" + Environment.NewLine;
            if (xmlex4) result += "System.Xml" + Environment.NewLine;
            if (YAXex4) result += "Yax" + Environment.NewLine;
            if (YAMLex4) result += "YAMLSerializer" + Environment.NewLine;
            if (WHOAex4) result += "Whoa" + Environment.NewLine;
            if (BFORMATex4) result += "BinaryFormatter" + Environment.NewLine;
            if (ABJSONex4) result += "ABJson" + Environment.NewLine;

            result += Environment.NewLine + "MATCHES (If the original exactly matches the new):" + Environment.NewLine + Environment.NewLine;

            try { if (JSONNET4obj.Equals(test3)) result += "Newtonsoft.Json" + Environment.NewLine; } catch { }
            try { if (BSONNET4obj.Equals(test3)) result += "Newtonsoft.Bson" + Environment.NewLine; } catch { }
            try { if (XML4obj.Equals(test3)) result += "System.Xml" + Environment.NewLine; } catch { }
            try { if (YAXLib4obj.Equals(test3)) result += "Yax" + Environment.NewLine; } catch { }
            try { if (YAML4obj.Equals(test3)) result += "YAMLSerializer" + Environment.NewLine; } catch { }
            try { if (WHOA4obj.Equals(test3)) result += "Whoa" + Environment.NewLine; } catch { }
            try { if (BFORMAT4obj.Equals(test3)) result += "BinaryFormatter" + Environment.NewLine; } catch { }
            try { if (ABJSON4obj.Equals(test3)) result += "ABJson" + Environment.NewLine; } catch { }

            result += Environment.NewLine + "SPEEDS:" + Environment.NewLine + Environment.NewLine;

            if (!njsonex4) result += "Newtonsoft.Json: " + JSONNet4.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!nbsonex4) result += "Newtonsoft.Bson: " + BSONNet4.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!xmlex4) result += "System.Xml: " + XML4.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!YAXex4) result += "Yax: " + YAXLib4.ElapsedMilliseconds + "ms" + Environment.NewLine;

            if (!YAMLex4) result += "YAMLSerializer: " + YAML4.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!WHOAex4) result += "Whoa: " + WHOA4.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!BFORMATex4) result += "BinaryFormatter: " + BFORMAT4.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!ABJSONex4) result += "ABJson: " + ABJSON4.ElapsedMilliseconds + "ms" + Environment.NewLine;
            #endregion

            SerializersTest test5 = new SerializersTest()
            {
                string1 = "FirstString",
                string2 = "FirstString",
                lstofstring = new List<string>() { "ListString-1", "ListString-2" },
                point1 = new Point(45, 32),
                point2 = new Point(45, 32),
                lstofpoint = new List<Point>() { new Point(32, 18), new Point(76, 45) },
                size1 = new Size(32, 67),
                size2 = new Size(32, 67),
                lstofsize = new List<Size>() { new Size(189, 167), new Size(236, 125) },
                rect1 = new Rectangle(45, 32, 89, 65),
                rect2 = new Rectangle(45, 32, 89, 65),
                lstofrect = new List<Rectangle>() { new Rectangle(45346, 6346, 3523, 35235), new Rectangle(45346574, 634636, 3523436, 3365235) },
                clr1 = Color.AliceBlue,
                clr2 = Color.Aquamarine,
                lstofcolor = new List<Color>() { Color.Azure, Color.FromArgb(45, 98, 43), Color.FromArgb(67, 32, 15) },
                strarray1 = new string[] { "Item1", "Item2" },
                strarray2 = new string[] { "Item1", "Item2" },
                lstofarray = new List<string[]> { new string[] { "Item1-1", "Item1-2" }, new string[] { "Item2-1", "Item2-2" } },
                dictionaryintstr1 = new Dictionary<int, string>(),
                dictionaryintstr2 = new Dictionary<int, string>(),
                dictionarystrstr1 = new Dictionary<string, string>(),
                dictionarystrstr2 = new Dictionary<string, string>(),
                dictionarystrarray = new Dictionary<string, string[]>(),
                lstofdictionary = new List<Dictionary<int, string>>() { new Dictionary<int, string>(), new Dictionary<int, string>()},
                lstofarrayofdictionaryintarray = new List<Dictionary<int, string[]>[]>() { new Dictionary<int, string[]>[] { new Dictionary<int, string[]>(), new Dictionary<int, string[]>()}, new Dictionary<int, string[]>[] { new Dictionary<int, string[]>(), new Dictionary<int, string[]>() } }
            };

            test5.dictionaryintstr1.Add(3, "Item1");
            test5.dictionaryintstr1.Add(8, "Item2");
            test5.dictionaryintstr2.Add(3, "Item1");
            test5.dictionaryintstr2.Add(8, "Item2");
            test5.dictionarystrstr1.Add("Key1", "Item1");
            test5.dictionarystrstr1.Add("Key2", "Item2");
            test5.dictionarystrstr2.Add("Key1", "Item1");
            test5.dictionarystrstr2.Add("Key2", "Item2");
            test5.dictionarystrarray.Add("Key1", new string[] { "Item1-1", "Item1-2"});
            test5.lstofdictionary[0].Add(1, "Value1");
            test5.lstofdictionary[0].Add(2, "Value2");
            test5.lstofdictionary[1].Add(3, "Value3");
            test5.lstofdictionary[1].Add(4, "Value4");

            test5.lstofarrayofdictionaryintarray[0][0].Add(1, new string[] { "whew1-1", "whew1-1" });
            test5.lstofarrayofdictionaryintarray[0][0].Add(17, new string[] { "whew2-1", "whew2-2" });
            test5.lstofarrayofdictionaryintarray[0][1].Add(1, new string[] { "whew3-1", "whew3-1" });
            test5.lstofarrayofdictionaryintarray[0][1].Add(67, new string[] { "whew3-1", "whew3-1" });

            test5.lstofarrayofdictionaryintarray[1][0].Add(1, new string[] { "secondary1-1", "secondary1-1" });
            test5.lstofarrayofdictionaryintarray[1][0].Add(17, new string[] { "secondary2-1", "secondary2-2" });
            test5.lstofarrayofdictionaryintarray[1][1].Add(1, new string[] { "secondary3-1", "secondary3-1" });
            test5.lstofarrayofdictionaryintarray[1][1].Add(67, new string[] { "secondary3-1", "secondary3-1" });
            #region Test5
            #region Newtonsoft.Json JSON
            result += ">>> Newtonsoft.Json JSON: <<<" + Environment.NewLine;

            bool njsonex5 = false;
            Stopwatch JSONNet5 = Stopwatch.StartNew();

            string JSONNET5str = "";
            try { JSONNET5str = Newtonsoft.Json.JsonConvert.SerializeObject(test5); } catch (Exception ex) { njsonex5 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            JSONNet5.Stop();

            result += "SPEED: " + JSONNet5.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + JSONNET5str.Length + Environment.NewLine;
            result += "FAILED (exception was thrown): " + njsonex5.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + JSONNET5str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;

            #endregion

            #region Newtonsoft.Json BSON
            result += ">>> Newtonsoft.Json BSON: <<<" + Environment.NewLine;

            bool nbsonex5 = false;
            Stopwatch BSONNet5 = Stopwatch.StartNew();

            string BSONNET5str = "";
            int BSONNET5size = 0;
            try
            {
                byte[] bytes;
                MemoryStream ms = new MemoryStream();
                using (BsonWriter writer = new BsonWriter(ms))
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.Serialize(writer, test5);
                    BSONNET5size = (int)ms.Length;
                    bytes = ms.ToArray();
                }
                BSONNet5.Stop();

                BSONNET5str = Convert.ToBase64String(bytes);
            }
            catch (Exception ex) { nbsonex5 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            result += "SPEED: " + BSONNet5.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + BSONNET5str.Length + Environment.NewLine;
            result += "FAILED (exception was thrown): " + nbsonex5.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + BSONNET5str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;

            #endregion

            #region System.Xml XML
            result += ">>> System.Xml XML: <<<" + Environment.NewLine;
            bool xmlex5 = false;
            Stopwatch XML5 = Stopwatch.StartNew();

            string XML5str = "";
            try
            {
                var serializer = new XmlSerializer(test5.GetType());
                using (var writer = new StringWriter())
                {
                    serializer.Serialize(writer, test5);
                    XML5str = writer.ToString();
                }

            }
            catch (Exception ex) { xmlex5 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            XML5.Stop();

            result += "SPEED: " + XML5.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + XML5str.Length + Environment.NewLine;
            result += "FAILED (exception was thrown): " + xmlex5.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + XML5str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;
            #endregion

            #region YAXLib
            result += ">>> YAXLib XML: <<<" + Environment.NewLine;
            bool YAXex5 = false;
            Stopwatch YAXLib5 = Stopwatch.StartNew();

            string YAXLib5str = "";
            try
            {
                YAXSerializer serializer = new YAXSerializer(typeof(SerializersTest));
                YAXLib5str = serializer.Serialize(test5);
            }
            catch (Exception ex) { YAXex5 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            YAXLib5.Stop();

            result += "SPEED: " + YAXLib5.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + YAXLib5str.Length + Environment.NewLine;
            result += "FAILED (exception was thrown): " + YAXex5.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + YAXLib5str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;
            #endregion

            #region YAMLSerializer
            result += ">>> YAMLSerializer YAML: <<<" + Environment.NewLine;
            bool YAMLex5 = false;
            Stopwatch YAML5 = Stopwatch.StartNew();

            string YAML5str = "";
            try
            {
                var serializer = new YamlSerializer();
                YAML5str = serializer.Serialize(test5);
            }
            catch (Exception ex) { YAMLex5 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            YAML5.Stop();

            result += "SPEED: " + YAML5.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + YAML5str.Length + Environment.NewLine;
            result += "FAILED (exception was thrown): " + YAMLex5.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + YAML5str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;
            #endregion

            #region Whoa
            result += ">>> WHOA WHOA: <<<" + Environment.NewLine;
            bool WHOAex5 = false;
            Stopwatch WHOA5 = Stopwatch.StartNew();

            string WHOA5str = "";
            int WHOA5size = 0;
            try
            {
                using (var ms = new MemoryStream())
                {
                    Whoa.Whoa.SerialiseObject<SerializersTest>(ms, test5, Whoa.SerialisationOptions.NonSerialized);
                    WHOA5size = (int)ms.Length;
                    WHOA5str = BitConverter.ToString(ms.ToArray()).Replace("-", " ");
                }

            }
            catch (Exception ex) { WHOAex5 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            WHOA5.Stop();

            result += "SPEED: " + WHOA5.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + WHOA5size.ToString() + Environment.NewLine;
            result += "FAILED (exception was thrown): " + WHOAex5.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + WHOA5str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;
            #endregion

            #region BinaryFormatter
            result += ">>> System.Runtime.Serialization.Formatters.Binary.BinaryFormatter BinaryFormat?: <<<" + Environment.NewLine;
            bool BFORMATex5 = false;
            Stopwatch BFORMAT5 = Stopwatch.StartNew();

            string BFORMAT5str = "";
            int BFORMAT5size = 0;
            try
            {
                using (var ms = new MemoryStream())
                {
                    BinaryFormatter bformat = new BinaryFormatter();
                    bformat.Serialize(ms, test5);
                    BFORMAT5size = (int)ms.Length;
                    BFORMAT5str = BitConverter.ToString(ms.ToArray()).Replace("-", " ");
                }
            }
            catch (Exception ex) { BFORMATex5 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            BFORMAT5.Stop();

            result += "SPEED: " + BFORMAT3.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + BFORMAT3size.ToString() + Environment.NewLine;
            result += "FAILED (exception was thrown): " + BFORMATex3.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + BFORMAT3str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;
            #endregion

            #region ABJson
            result += ">>> ABJson JSON: <<<" + Environment.NewLine;
            bool ABJSONex5 = false;
            Stopwatch ABJSON5 = Stopwatch.StartNew();

            string ABJSON5str = "";
            /* try { */
            ABJSON5str = JsonClassConverter.ConvertObjectToJson(test5, JsonFormatting.Compact); // } catch (Exception ex) { ABJSONex3 = true; result += "EXCEPTION THROWN! " + ex.Message + Environment.NewLine; }

            ABJSON5.Stop();

            result += "SPEED: " + ABJSON5.ElapsedMilliseconds + "ms!" + Environment.NewLine;
            result += "SIZE (bytes): " + ABJSON5str.Length + Environment.NewLine;
            result += "FAILED (exception was thrown): " + ABJSONex5.ToString() + Environment.NewLine;
            result += Environment.NewLine + "================================" + Environment.NewLine + ABJSON5str + Environment.NewLine + "================================" + Environment.NewLine + Environment.NewLine;
            #endregion


            result += "====== TEST 5 RESULTS: ======" + Environment.NewLine + Environment.NewLine;
            result += Environment.NewLine + "FAILED:" + Environment.NewLine + Environment.NewLine;

            if (njsonex5) result += "Newtonsoft.Json" + Environment.NewLine;
            if (nbsonex5) result += "Newtonsoft.Bson" + Environment.NewLine;
            if (xmlex5) result += "System.Xml" + Environment.NewLine;
            if (YAXex5) result += "Yax" + Environment.NewLine;
            if (YAMLex5) result += "YAMLSerializer" + Environment.NewLine;
            if (WHOAex5) result += "Whoa" + Environment.NewLine;
            if (BFORMATex5) result += "BinaryFormatter" + Environment.NewLine;
            if (ABJSONex5) result += "ABJson" + Environment.NewLine;

            result += Environment.NewLine + "SIZES:" + Environment.NewLine + Environment.NewLine;

            if (!njsonex5) result += "Newtonsoft.Json: " + JSONNET5str.Length + Environment.NewLine;
            if (!nbsonex5) result += "Newtonsoft.Bson: " + BSONNET5size.ToString() + Environment.NewLine;
            if (!xmlex5) result += "System.Xml: " + XML5str.Length + Environment.NewLine;
            if (!YAXex5) result += "Yax: " + YAXLib5str.Length + Environment.NewLine;
            if (!YAMLex5) result += "YAMLSerializer: " + YAML5str.Length + Environment.NewLine;
            if (!WHOAex5) result += "Whoa: " + WHOA5size.ToString() + Environment.NewLine;
            if (!BFORMATex5) result += "BinaryFormatter: " + BFORMAT5size.ToString() + Environment.NewLine;
            if (!ABJSONex5) result += "ABJson: " + ABJSON5str.Length + Environment.NewLine;

            result += Environment.NewLine + "SPEEDS:" + Environment.NewLine + Environment.NewLine;

            if (!njsonex5) result += "Newtonsoft.Json: " + JSONNet5.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!nbsonex5) result += "Newtonsoft.Bson: " + BSONNet5.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!xmlex5) result += "System.Xml: " + XML5.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!YAXex5) result += "Yax: " + YAXLib5.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!YAMLex5) result += "YAMLSerializer: " + YAML5.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!WHOAex5) result += "Whoa: " + WHOA5.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!BFORMATex5) result += "BinaryFormatter: " + BFORMAT5.ElapsedMilliseconds + "ms" + Environment.NewLine;
            if (!ABJSONex5) result += "ABJson: " + ABJSON5.ElapsedMilliseconds + "ms" + Environment.NewLine;
            #endregion

            textBox3.Text = result;
        }
    }

    //public sealed class RealLifeExampleMap : ClassMap<RealLifeExample>
    //{
    //    public RealLifeExampleMap()
    //    {
    //        Map(m => m.Houses).Name("Houses");
    //        Map(m => m.People).Name("People");
    //    }
    //}

    //public sealed class PersonMap : ClassMap<Person>
    //{
    //    public PersonMap()
    //    {
    //        Map(m => m.Job).Name("Job");
    //    }
    //}

    //public sealed class HouseMap : ClassMap<House>
    //{
    //    public HouseMap()
    //    {
    //        Map(m => m.upgrades).Name("upgrades");
    //        Map(m => m.Objects).Name("Objects");
    //        Map(m => m.Location).Name("Location");
    //        Map(m => m.Height).Name("Height");
    //    }
    //}

    //public sealed class UpgradeMap : ClassMap<Upgrade>
    //{
    //    public UpgradeMap()
    //    {
    //        Map(m => m.willExtendWeight).Name("willExtendWeight");
    //    }
    //}

    //public sealed class ObjectMap : ClassMap<Object>
    //{
    //    public ObjectMap()
    //    {
    //        Map(m => m.Name).Name("Name");
    //        Map(m => m.Usage).Name("Usage");
    //    }
    //}

    //public sealed class JobMap : ClassMap<Job>
    //{
    //    public JobMap()
    //    {
    //        Map(m => m.Name).Name("Name");
    //        Map(m => m.Money).Name("Money");
    //    }
    //}

    //public sealed class HobbyMap : ClassMap<Hobby>
    //{
    //    public HobbyMap()
    //    {
    //        Map(m => m.Name).Name("Name");
    //        Map(m => m.Popularity).Name("Popularity");
    //    }
    //}

    public class InheritanceTest
    {
        public string normalstr;

        public InheritanceBase inherit1;
        public InheritanceBase inherit2;

        public int normalint;
    }

    public class InheritanceBase
    {
        public int x;
        public int y;
        public int zindex;
    }

    public class InheritanceInherit1 : InheritanceBase
    {
        public Color clr1;
        public Point part1;
        public Point part2;
    }

    public class InheritanceInherit2 : InheritanceBase
    {
        public int shapeType;
    }

    [Serializable]
    public class SerializersTest
    {
        public string string1 { get; set; }
        public string string2 { get; set; }
        public List<string> lstofstring { get; set; }
        public Point point1 { get; set; }
        public Point point2 { get; set; }
        public List<Point> lstofpoint { get; set; }
        public Size size1 { get; set; }
        public Size size2 { get; set; }
        public List<Size> lstofsize { get; set; }
        public Rectangle rect1 { get; set; }
        public Rectangle rect2 { get; set; }
        public List<Rectangle> lstofrect { get; set; }
        public Color clr1 { get; set; }
        public Color clr2 { get; set; }
        public List<Color> lstofcolor { get; set; }
        public string[] strarray1 { get; set; }
        public string[] strarray2 { get; set; }
        public List<string[]> lstofarray { get; set; }
        public Dictionary<int, string> dictionaryintstr1 { get; set; }
        public Dictionary<int, string> dictionaryintstr2 { get; set; }
        public Dictionary<string, string> dictionarystrstr1 { get; set; }
        public Dictionary<string, string> dictionarystrstr2 { get; set; }
        public Dictionary<string, string[]> dictionarystrarray { get; set; }
        public List<Dictionary<int, string>> lstofdictionary { get; set; }
        public List<Dictionary<int, string[]>[]> lstofarrayofdictionaryintarray { get; set; }

        // Alright, all of the above are what we will be testing for today, so, now some real-life stuff!

        public RealLifeExample rlexample { get; set; }
    }

    [Serializable]
    public class RealLifeExample
    {
        public List<House> Houses { get; set; }
        public List<Person> People { get; set; }
    }

    [Serializable]
    public class Person
    {
        public List<Job> Job { get; set; }
    }

    [Serializable]
    public class House
    {
        public Point Location;
        public int Height { get; set; }
        public List<Object> Objects { get; set; }
        public Dictionary<int, Upgrade> upgrades;
    }

    [Serializable]
    public class Upgrade
    {
        public bool willExtendWeight { get; set; }
    }

    [Serializable]
    public class Object
    {
        public string Name { get; set; }
        public string Usage { get; set; }
        public Color color { get; set; }
    }

    [Serializable]
    public class Job
    {
        public int Money { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class Hobby
    {
        public string Name { get; set; }
        public int Popularity { get; set; }
    }

    public class DeserializeTest
    {
        public string yoy;
        public bool aBool;
        public int anInt;
        public List<string> anArray;
        public Dictionary<int, string[]> aDictionary;
        public List<InsideDeserializeTest> idt;
    }

    public class InsideDeserializeTest
    {
        public string itworks;
    }

    public class Foo
    {
        public string hello = "Hi!";
        public int anInt = 12349;
        public bool anBool = true;
        public string aNull = null;
        public DateTime aDateTime = new DateTime(2005, 5, 3, 3, 7, 9, DateTimeKind.Unspecified);
        public Point aPoint = new Point(3, 4);
        public Size aSize = new Size(30, 40);
        public Rectangle aRectangle = new Rectangle(5, 6, 7, 8);
        //public Image anImage = Properties.Resources.what_you_on_about_1_;
        public Color clr = Color.Black;
        public Dictionary<string, string[]> aDictionary = new Dictionary<string, string[]>();
        public List<Bar> aBar = new List<Bar>()
        {
            new Bar()
            {
                aBarString = "This is a bar ONE"
            },
            new Bar()
            {
                aBarString = "This is a bar TWO"
            }
        };
    }

    public class Bar2 : Bar
    {
        public string aBar2String = "ThisIsABar2ThingOnly";
    }

    public class Bar
    {
        public string aBarString = "";
    }
}
