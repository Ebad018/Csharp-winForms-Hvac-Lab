using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using iTextSharp.text.pdf;
using iTextSharp.text;



namespace Chiller_Testing_Lab_Software
{
    internal static class Program
    {
        public static Form2 Form2Instance;
        public static Form1 Form1Instance;


        private static Form1 form1; // Add a reference to your Form1 instance

        public static void SetForm1(Form1 form)
        {
            form1 = form;
        }
        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form2Instance = new Form2();
            Application.Run(new Form1());
        }
    }
}
