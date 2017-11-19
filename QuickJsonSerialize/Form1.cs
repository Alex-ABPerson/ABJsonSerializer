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

            Foo dtest = JsonClassConverter.ConvertJsonToObject<Foo>(textBox2.Text);

            sw.Stop();

            Console.WriteLine("TIME ELAPSED ABJSON: " + sw.ElapsedMilliseconds + "ms");

            Stopwatch sw2 = Stopwatch.StartNew();

            Foo dtest2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Foo>(textBox2.Text);

            sw2.Stop();

            Console.WriteLine("TIME ELAPSED: " + sw2.ElapsedMilliseconds + "ms");

            MessageBox.Show(dtest.ToString());
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string result = "";

            result += "Multiple Serializers test:" + Environment.NewLine;
            result += "Each serializer will be displayed below and the end will be comparisons." + Environment.NewLine;
            result += "This test is WITHOUT DateTimes due to ABJson not being able to Deserialize DateTimes yet because... DO YOU THINK I HAVE ALL DAY? Oh wait, I do have all day to work on it... well, I can't be bothered to add it now!" + Environment.NewLine;
            result += "I will have a new test however with DateTimes!" + Environment.NewLine;

            result += "THE OBJECT WE WILL BE SERIALIZING (in C#)" + Environment.NewLine;

            result += "Newtonsoft.Json JSON:" + Environment.NewLine;

            Stopwatch JSONNet = Stopwatch.StartNew();

            string JSONNETstr = Newtonsoft.Json.JsonConvert.SerializeObject(new SerializersTest());

            JSONNet.Stop();

        }
    }

    public class SerializersTest
    {
        public string string1;
        public string string2;
        public List<string> lstofstring;
        public Point point1;
        public Point point2;
        public List<Point> lstofpoint;
        public Size size1;
        public Size size2;
        public List<Size> lstofsize;
        public Rectangle rect1;
        public Rectangle rect2;
        public List<Rectangle> lstofrect;
        public Color clr1;
        public Color clr2;
        public List<Color> lstofcolor;
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
        //public DateTime aDateTime = new DateTime(2005, 5, 3, 5, 2, 3, DateTimeKind.Local);
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
