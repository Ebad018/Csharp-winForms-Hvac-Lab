using iTextSharp.text.pdf;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chiller_Testing_Lab_Software
{
    public partial class Form2 : Form
    {
        
        public string engname { get; private set; }
        public  string unitlab { get; private set; }
        public string complab { get; private set; }
        public string coillab { get; private set; }
        public string caplab { get; private set; }
        public string fanlab { get; private set; }
        public string gaslab { get; private set; }
        public string motolab { get; private set; }
        public string invlab { get; private set; }
        
        
        
            private int YourInitialFormWidth;
            private int YourInitialFormHeight;

            public Form2()
            {
                InitializeComponent();

                // Store the initial dimensions of the form
                YourInitialFormWidth = Width;
                YourInitialFormHeight = Height;

                // Subscribe to the Resize event
                Resize += MainForm_Resize;
            }

            private void MainForm_Resize(object sender, EventArgs e)
            {
                // Adjust font size based on the form's size
                float scaleFactor = (float)(Width + Height) / (float)(YourInitialFormWidth + YourInitialFormHeight);

                // Apply the scale factor to all relevant controls (e.g., labels, textboxes)
                AdjustControlFontSizes(this, scaleFactor);
            }

            private void AdjustControlFontSizes(Control control, float scaleFactor)
            {
                foreach (Control childControl in control.Controls)
                {
                    // You can customize this based on the type of controls you have
                    if (childControl is Label || childControl is TextBox)
                    {
                        childControl.Font = new Font(childControl.Font.FontFamily, childControl.Font.Size * scaleFactor);
                    }

                    // Recursively adjust font sizes for nested controls
                    AdjustControlFontSizes(childControl, scaleFactor);
                }
            }
       

        private void SetS5()
        {
            string cfmValue = textBoxS5.Text;
            if (Program.Form1Instance !=null)
            {
                Program.Form1Instance.TextBoxS5Value = cfmValue;
            }
            string engname = EngineerNameTextBox.Text;
            if (Program.Form1Instance != null)
            {
                Program.Form1Instance.textBoxNamelab.Text = engname;
            }
            string unitlab = UnitNameTextBox.Text;
            if (Program.Form1Instance != null)
            {
                Program.Form1Instance.textBoxUnitlab.Text = unitlab;

            }
            string complab = CompressorTextBox.Text;
            if(Program.Form1Instance != null)
            {
                Program.Form1Instance.textBoxComplab.Text = complab;
            }
            string coillab = CoilSizeTextBox.Text;
            if (Program.Form1Instance != null)
            {
                Program.Form1Instance.textBoxCoillab.Text = coillab;
            }
            string caplab = CapacityTextBox.Text;
            if (Program.Form1Instance !=null)
            {
                Program.Form1Instance.textBoxCaplab.Text = caplab;
            }
            string fanlab = FanDetailsTextBox.Text;
            if (Program.Form1Instance !=null)
            {
                Program.Form1Instance.textBoxFanlab.Text = fanlab;
            }
            string gaslab = textBoxGas.Text;
            if(Program.Form1Instance !=null)
            {
                Program.Form1Instance.textBoxGas.Text = gaslab;
            }
            string motolab = textBoxMotor.Text;
            if(Program.Form1Instance !=null)
            {
                Program.Form1Instance.textBoxMotor.Text = motolab;
            }
            string invlab = textBoxInverter.Text;
                if(Program.Form1Instance !=null)
            {
                Program.Form1Instance.textBoxInverter.Text = invlab;
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }



        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void UnitName_TextChanged(object sender, EventArgs e)
        {

        }

        private void EngineerName_TextChanged(object sender, EventArgs e)
        {

        }

        private void Compressor_TextChanged(object sender, EventArgs e)
        {

        }

        private void CoilSize_TextChanged(object sender, EventArgs e)
        {

        }

        private void Capacity_TextChanged(object sender, EventArgs e)
        {

        }

        private void Fan_TextChanged(object sender, EventArgs e)
        {

        }
        public void SetForm2Data( string motolab,string gaslab, string engname, string unitlab, string complab, string coillab, string caplab, string fanlab, string cfmValue, string invlab)
        {
            // Set the properties of Form2
            
            this.fanlab = fanlab;
            this.engname = engname;
            this.unitlab = unitlab;
            this.complab = complab;
            this.coillab = coillab;
            this.caplab = caplab;
            this.gaslab = gaslab;
            this.motolab = motolab;
            this.invlab = invlab;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetS5();
            
            // Retrieve the data entered in the textboxes
            
            string cfmValue = textBoxS5.Text;
            string engname = EngineerNameTextBox.Text;
            string unitlab = UnitNameTextBox.Text;
            string complab = CompressorTextBox.Text;
            string caplab = CapacityTextBox.Text;
            string fanlab = FanDetailsTextBox.Text;
            string coillab = CoilSizeTextBox.Text;
            string gaslab = textBoxGas.Text;
            string motolab = textBoxMotor.Text;
            string invlab = textBoxInverter.Text;


            




            // Check if Form2Instance is already created
            if (Program.Form1Instance == null)
            {
                // Create a new instance of Form1 if not created
                Program.Form1Instance = new Form1();
                Program.Form1Instance.FormClosed += (s, args) => Program.Form1Instance = null;
            }

            


            Program.Form1Instance.TextBoxS5Value = cfmValue;
            Program.Form1Instance.TextBoxNamelab = engname;
            Program.Form1Instance.TextBoxUnitlab = unitlab;
            Program.Form1Instance.TextBoxComplab = complab;
            Program.Form1Instance.TextBoxCoillab = coillab;
            Program.Form1Instance.TextBoxCaplab = caplab;
            Program.Form1Instance.TextBoxFanlab = fanlab;
            Program.Form1Instance.TextBoxMotorlab = motolab;
            Program.Form1Instance.TextBoxGaslab = gaslab;
            Program.Form1Instance.TextBoxInvlab = invlab;

            // Show Form1
            Program.Form1Instance.Show();
            
                this.Hide();
            
        }

        private void textBoxS5_TextChanged(object sender, EventArgs e)
        {

        }
    }
    }

