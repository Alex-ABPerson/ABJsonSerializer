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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Type typ = ABJson.GDISupport.ABInheritanceGuesser.GetBestInheritance<BaseClass>(new string[] { "lol3", "hmmm7" }, new Type[] { typeof(int), typeof(string) });
            Console.WriteLine(typ.Name);
        }
    }

    public class BaseClass
    {
        public static string lolhi;
    }

    public class InheritedClass : BaseClass
    {
        public static int lol1;
        public static bool hmmm1;
    }

    public class InheritedClass2 : BaseClass
    {
        public static int lol3;
        public static string hmmm7;
    }
}
