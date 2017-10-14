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
            MessageBox.Show(JsonClassConverter.ConvertObjectToJson((new Foo()), JsonFormatting.Compact));
        }
    }

    public class Foo
    {
        public string hello = "Hi!";
        public int anInt = 12349;
        public bool anBool = true;
        public string aNull = null;
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
