using ABJson.GDISupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickJsonSerialize
{
    
    public partial class Form1 : Form
    {
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

            string json = JsonClassConverter.ConvertObjectToJson(theFoo, JsonFormatting.Indented);

            sw.Stop();

            Console.WriteLine("TIME ELAPSED ABJSON: " + sw.ElapsedMilliseconds + "ms");

            Stopwatch sw2 = Stopwatch.StartNew();

            string json2 = Newtonsoft.Json.JsonConvert.SerializeObject(theFoo, Newtonsoft.Json.Formatting.Indented);

            sw2.Stop();

            Console.WriteLine("TIME ELAPSED: " + sw2.ElapsedMilliseconds + "ms");
            //MessageBox.Show(Newtonsoft.Json.JsonConvert.SerializeObject(theFoo, Newtonsoft.Json.Formatting.Indented));

            textBox1.Text = json;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();

            DeserializeTest dtest = JsonClassConverter.ConvertJsonToObject<DeserializeTest>(textBox2.Text);

            sw.Stop();

            Console.WriteLine("TIME ELAPSED ABJSON: " + sw.ElapsedMilliseconds + "ms");

            Stopwatch sw2 = Stopwatch.StartNew();

            DeserializeTest dtest2 = Newtonsoft.Json.JsonConvert.DeserializeObject<DeserializeTest>(textBox2.Text);

            sw2.Stop();

            Console.WriteLine("TIME ELAPSED: " + sw2.ElapsedMilliseconds + "ms");

            MessageBox.Show(dtest.yoy);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            JsonReader.GetKeyValueData("'lol': 'How are you today?'");
        }
    }

    public class DeserializeTest
    {
        public string yoy;
        public bool aBool;
        public int anInt;
        public List<string> anArray;
        public Dictionary<int, string> aDictionary;
    }

    public class Foo
    {
        public string hello = "Hi!";
        public int anInt = 12349;
        public bool anBool = true;
        public string aNull = null;
        public DateTime aDateTime = new DateTime(2005, 5, 3, 5, 2, 3, DateTimeKind.Local);
        public Point aPoint = new Point(3, 4);
        public Size aSize = new Size(30, 40);
        public Rectangle aRectangle = new Rectangle(5, 6, 7, 8);
        //public Image anImage = Properties.Resources.what_you_on_about_1_;
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
