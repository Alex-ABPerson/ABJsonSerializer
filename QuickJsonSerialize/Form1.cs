using ABJson.GDISupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            Foo theFoo = new Foo();
            theFoo.aDictionary.Add("Hello1", new string[] { "Key1", "Key2" });
            theFoo.aDictionary.Add("Hello2", new string[] { "Key2-3", "Key2-2" });
            MessageBox.Show(Newtonsoft.Json.JsonConvert.SerializeObject(theFoo, Newtonsoft.Json.Formatting.Indented));
            textBox1.Text = JsonClassConverter.ConvertObjectToJson(theFoo, JsonFormatting.Indented);
        }
    }

    public class Foo
    {
        public string hello = "Hi!";
        public int anInt = 12349;
        public bool anBool = true;
        public string aNull = null;
        public DateTime aDateTime = new DateTime(2005, 5, 3, 5, 2, 3, DateTimeKind.Utc);
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

    public class Bar
    {
        public string aBarString = "";
    }
}
