using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chiller_Testing_Lab_Software
{

    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer errorBlinkTimer = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer errorClearTimer = new System.Windows.Forms.Timer();

        private bool isErrorBlinking = false;
        private SerialPort serialPort = new SerialPort();
        private byte[] dataBuffer = new byte[256]; // Move dataBuffer to Form1
        private bool stop = false; // Move stop to Form1
        private double s1 = double.NaN;
        private double s2 = double.NaN;
        private double s3 = double.NaN;
        private double s4 = double.NaN;
        private double sh = double.NaN;
        private double sc = double.NaN;
        private double TwFahrenheit;
        private double sBtu = 0.0;
        private double tBtu = 0.0;
        private double latentCapacity = 0.0;
        private bool heatingMode = true;
        private double s5;
        static double rh = double.NaN;
        static double RH1 = double.NaN;
        static double RH2 = double.NaN;
        private System.Windows.Forms.Timer blinkTimer = new System.Windows.Forms.Timer();
        private bool isRedLightOn = false;
        private double us2 = double.NaN;
        private System.Windows.Forms.Timer autoRequestTimer = new System.Windows.Forms.Timer();
        private int currentSlave = 3; // Start with Slave 3
        private bool isAutoRequestEnabled = false;
        private System.Windows.Forms.Timer autoUpdateTimer;
        //double s1Offset = Properties.Settings.Default.S1OffSet;
        //double s2Offset = Properties.Settings.Default.S2OffSet;
        //double s3Offset = Properties.Settings.Default.S3OffSet;
        //double s4Offset = Properties.Settings.Default.S4OffSet;
        //double scOffset = Properties.Settings.Default.ScOffSet;
        //double shOffset = Properties.Settings.Default.ShOffSet;
        //double dlOffset = Properties.Settings.Default.DlOffSet;
        //double llOffset = Properties.Settings.Default.LlOffSet;
        //double a1Offset = Properties.Settings.Default.A1OffSet;
        //double a2Offset = Properties.Settings.Default.A2OffSet;
        //double a3Offset = Properties.Settings.Default.A3OffSet;
        //double a4Offset = Properties.Settings.Default.A4OffSet;
        //double a5Offset = Properties.Settings.Default.A5OffSet;
        //double a6Offset = Properties.Settings.Default.A6OffSet;
        //double a7Offset = Properties.Settings.Default.A7OffSet;
        //double a8Offset = Properties.Settings.Default.A8OffSet;






        private double[] sensorDataArray = new double[6];
        private Form2 form2Instance;
        private Form3 form3Instance;

        private double a1, a2, a3, a4, a5, a6, a7, a8, bs, ciwb;
        private int YourInitialFormWidth;
        private int YourInitialFormHeight;




        public Form1()
        {
            InitializeComponent();
            RefreshComPorts();
            InitializeAutoUpdateTimer();

            // Store the initial dimensions of the form
            YourInitialFormWidth = Width;
            YourInitialFormHeight = Height;

            // Subscribe to the Resize event
            Resize += MainForm_Resize;

            blinkTimer.Interval = 500; // Set the interval for blinking (in milliseconds)
            blinkTimer.Tick += BlinkTimer_Tick;

            autoRequestTimer.Interval = 1000; // Set the interval to 1000 milliseconds (1 seconds)
            autoRequestTimer.Tick += AutoRequestTimer_Tick;

            errorClearTimer.Interval = 5000; // 5 seconds
            errorClearTimer.Tick += ErrorClearTimer_Tick;


            FormClosed += (s, e) =>
            {
                autoRequestTimer.Stop(); //Stop the Modbus requests when form is closed
                Application.Exit();  //makes sure the app isn't running in the background when the form is closed
                CloseSerialPort(); //Makes Sure to close the port when form is closed   


            };
            // Event handler to detect unexpected port closure
            serialPort.DataReceived += (s, e) =>
            {
                // Check if the port is closed
                if (!serialPort.IsOpen)
                {
                    autoRequestTimer.Stop();
                    // Optionally, you can display a message to inform the user
                    this.Invoke((MethodInvoker)delegate
                    {
                        textBoxError.ForeColor = Color.Red;
                        textBoxError.Text = $"Port Error:";
                        errorClearTimer.Stop();   // Restart the timer in case it's already running
                        errorClearTimer.Start();
                    });

                }
            };
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            // Calculate the scale factor based on the change in width and height
            float widthScaleFactor = (float)Width / (float)YourInitialFormWidth;
            float heightScaleFactor = (float)Height / (float)YourInitialFormHeight;

            // Choose the larger scale factor to maintain proportional scaling
            float scaleFactor = Math.Max(widthScaleFactor, heightScaleFactor);

            // Apply the scale factor to all relevant controls (e.g., labels, textboxes)
            AdjustControlFontSizes(this, scaleFactor);
        }

        private void AdjustControlFontSizes(Control control, float scaleFactor)
        {
            foreach (Control childControl in control.Controls)
            {
                // You can customize this based on the type of controls you have
                if (childControl is Label || childControl is System.Windows.Forms.TextBox || childControl is System.Windows.Forms.Button)
                {
                    // Adjust the font size based on the scale factor
                    childControl.Font = new System.Drawing.Font(childControl.Font.FontFamily, childControl.Font.Size * scaleFactor);
                }

                // Recursively adjust font sizes for nested controls
                AdjustControlFontSizes(childControl, scaleFactor);
            }
        }

        private void InitializeAutoUpdateTimer()
        {
            System.Windows.Forms.Timer autoUpdateTimer = new System.Windows.Forms.Timer();
            autoUpdateTimer.Interval = 1000; // 1 second interval
            autoUpdateTimer.Tick += (sender, e) => absolute_conditions();
            autoUpdateTimer.Start();
        }
        public string TextBoxS5Value
        {
            get { return textBoxS5.Text; }
            set
            {
                textBoxS5.Text = value;

                // Assuming s5 is a double, you need to parse the string value
                if (double.TryParse(value, out double parsedValue))
                {
                    s5 = parsedValue;
                }
            }
        }

        public string TextBoxNamelab
        {
            get { return textBoxNamelab.Text; }
            set { textBoxNamelab.Text = value; }


        }
        public string TextBoxUnitlab
        {
            get { return textBoxUnitlab.Text; }
            set { textBoxUnitlab.Text = value; }

        }
        public string TextBoxComplab
        {
            get { return textBoxComplab.Text; }
            set { textBoxComplab.Text = value; }
        }
        public string TextBoxCoillab
        {
            get { return textBoxCoillab.Text; }
            set { textBoxCoillab.Text = value; }
        }
        public string TextBoxCaplab
        {
            get { return textBoxCaplab.Text; }
            set { textBoxCaplab.Text = value; }
        }
        public string TextBoxFanlab
        {
            get { return textBoxFanlab.Text; }
            set { textBoxFanlab.Text = value; }
        }
        public string TextBoxMotorlab
        {
            get { return textBoxMotor.Text; }
            set { textBoxMotor.Text = value; }
        }
        public string TextBoxGaslab
        {
            get { return textBoxGas.Text; }
            set { textBoxGas.Text = value; }
        }

        public string TextBoxInvlab
        {
            get { return textBoxInverter.Text; }
            set { textBoxInverter.Text = value; }
        }

        //Enthaply chart
        static Dictionary<double, double> enthalpyLookup = new Dictionary<double, double>
    {
 {35.0 , 13.01},
 {35.1 , 13.05},{35.2 , 13.09},{35.3, 13.4 }, {35.4 , 13.18},{35.5 , 13.22},{35.6 , 13.27},{35.7 , 13.31},{35.8 , 13.35},{35.9 , 13.39},{36.0 , 13.44},{36.1 , 13.48},{36.2 , 13.52},{36.3 , 13.57},
 {36.4 , 13.62},{36.5 , 13.66},{36.6 , 13.70},{36.7 , 13.74},{36.8 , 13.79},{36.9 , 13.83},{37.0 , 13.87},{37.1 , 13.92},{37.2 , 13.96},{37.3 , 14.01},{37.4 , 14.05},{37.5 , 14.10},{37.6 , 14.14},{37.7 , 14.19},
 {37.8 , 14.23},{37.9 , 14.27},{38.0 , 14.32},{38.1 , 14.36},{38.2 , 14.41},{38.3 , 14.45},{38.4 , 14.50},{38.5 , 14.54},{38.6 , 14.59},{38.7 , 14.63},{38.8 , 14.68},{38.9 , 14.73},
 {39.0 , 14.77},{39.1 , 14.82},{39.2 , 14.86},{39.3 , 14.91},{39.4 , 14.95},{39.5 , 15.00},{39.6 , 15.05},{39.7 , 15.09},{39.8 , 15.14},{39.9 , 15.18},{40.0 , 15.23},{40.1 , 15.28},
 {40.2 , 15.32},{40.3 , 15.37},{40.4 , 15.42},{40.5 , 15.46},{40.6 , 15.51},{40.7 , 15.56},{40.8 , 15.60},{40.9 , 15.65},{41.0 , 15.70},{41.1 , 15.74},{41.2 , 15.79},{41.3 , 15.84},{41.4 , 15.89},
 {41.5 , 15.93},{41.6 , 15.98},{41.7 , 16.03},{41.8 , 16.08},{41.9 , 16.12},{42.0 , 16.17},{42.1 , 16.22},{42.2 , 16.27},{42.3 , 16.32},{42.4 , 16.37},{42.5 , 16.41},{42.6 , 16.46},
 {42.7 , 16.51},{42.8 , 16.56},{42.9 , 16.61},{43.0 , 16.66},{43.1 , 16.71},{43.2 , 16.75},{43.3 , 16.80},{43.4 , 16.85},{43.5 , 16.90},{43.6 , 16.95},{43.7 , 17.00},{43.8 , 17.05},{43.9 , 17.10},
 {44.0 , 17.15},{44.1 , 17.20},{44.2 , 17.25},{44.3 , 17.30},{44.4 , 17.35},{44.5 , 17.40},{44.6 , 17.45},{44.7 , 17.50},{44.8 , 17.55},{44.9 , 17.60},{45.0 , 17.65},{45.1 , 17.70},
 {45.2 , 17.75},{45.3 , 17.80},{45.4 , 17.85},{45.5 , 17.91},{45.6 , 17.96},{45.7 , 18.01},{45.8 , 18.06},{45.9 , 18.11},{46.0 , 18.16},{46.1 , 18.21},{46.2 , 18.26},{46.3 , 18.32},{46.4 , 18.37},{46.5 , 18.42},
 {46.6 , 18.47},{46.7 , 18.52},{46.8 , 18.58},{46.9 , 18.63},{47.0 , 18.68},{47.1 , 18.73},{47.2 , 18.79},{47.3 , 18.84},{47.4 , 18.89},{47.5 , 18.95},{47.6 , 19.00},{47.7 , 19.05},{47.8 , 19.10},{47.9 , 19.16},
 {48.0 , 19.21},{48.1 , 19.26},{48.2 , 19.32},{48.3 , 19.37},{48.4 , 19.43},{48.5 , 19.48},{48.6 , 19.53},{48.7 , 19.59},{48.8 , 19.64},{48.9 , 19.70},{49.0 , 19.75},{49.1 , 19.81},{49.2 , 19.86},{49.3 , 19.92},
 {49.4 , 19.97},{49.5 , 20.03},{49.6 , 20.08},{49.7 , 20.14},{49.8 , 20.19},{49.9 , 20.25},{50.0 , 20.30},{50.1 , 20.36},{50.2 , 20.41},{50.3 , 20.47},{50.4 , 20.52},{50.5 , 20.58},{50.6 , 20.64},{50.7 , 20.69},{50.9 , 20.81},
 {51.0 , 20.86},{51.1 , 20.92},{51.2 , 20.98},{51.3 , 21.03},{51.4 , 21.09},{51.5 , 21.15},{51.6 , 21.21},{51.7 , 21.26},{51.8 , 21.32},{51.9 , 21.38},{52.0 , 21.44},{52.1 , 21.49},{52.2 , 21.55},{52.3 , 21.61},{52.4 , 21.67},{52.5 , 21.73},{52.6 , 21.79},{52.7 , 21.84},{52.8 , 21.90},
 {52.9 , 21.96},{53.0 , 21.02},{53.1 , 22.08},{53.2 , 22.14},{53.3 , 22.20},{53.4 , 22.26},{53.5 , 22.32},{53.6 , 22.38},{53.7 , 22.44},{53.8 , 22.50},{53.9 , 22.56},{54.0 , 22.61},{54.1 , 22.68},{54.2 , 22.74},{54.3 , 22.80},{54.4 , 22.86},{54.5 , 22.92},{54.6 , 22.98},{54.7 , 22.04},{54.8 , 23.10},
 {54.9 , 23.16},{55.0 , 23.22},{55.1 , 23.28},{55.2 , 23.34},{55.3 , 23.41},{55.4 , 23.47},{55.5 , 23.53},{55.6 , 23.59},{55.7 , 23.65},{55.8 , 23.72},{55.9 , 23.78},{56.0 , 23.84},{56.1 , 23.90},{56.2 , 23.97},{56.3 , 23.03},{56.4 , 24.10},
 {56.5 , 24.16},{56.6 , 24.22},{56.7 , 24.29},{56.8 , 24.35},{56.9 , 24.42},{57.0 , 24.48},{57.1 , 24.54},{57.2 , 24.61},{57.3 , 24.67},{57.4 , 24.74},{57.5 , 24.80},{57.6 , 24.86},
 {57.7 , 24.93},{57.8 , 24.99},{57.9 , 24.06},{58.0 , 24.12},{58.1 , 25.19},{58.2 , 25.25},{58.3 , 25.32},{58.4 , 25.38},{58.5 , 25.45},{58.6 , 25.52},{58.7 , 25.58},{58.8 , 25.65},{58.9 , 25.71},{59.0 , 25.78},{59.1 , 25.85},{59.2 , 25.92},
 {59.3 , 25.98},{59.4 , 25.05},{59.5 , 25.12},{59.6 , 26.19},{59.7 , 26.26},{59.8 , 26.32},{59.9 , 26.39},{60.0 , 26.46},{60.1 , 26.53},{60.2 , 26.67},{60.3 , 26.74},{60.4 , 26.80},{60.5 , 26.87},{60.6 , 26.87},{60.7 , 26.94},{60.8 , 27.01},
 {60.9 , 27.08},{61.0 , 27.15},{61.1 , 27.22},{61.2 , 27.29},{61.3 , 27.36},{61.4 , 27.43},{61.5 , 27.50},{61.6 , 27.57},{61.7 , 27.64},{61.8 , 27.71},{61.9 , 27.78},{62.0 , 27.85},{62.1 , 27.92},{62.2 , 27.99},
 {62.3 , 28.07},{62.4 , 28.14},{62.5 , 28.21},{62.6 , 28.28},{62.7 , 28.35},{62.8 , 28.43},{62.9 , 28.50},{63.0 , 28.57},{63.1 , 28.64},{63.2 , 28.72},{63.3 , 28.79},{63.4 , 28.87},{63.5 , 28.94},{63.6 , 29.01},{63.7 , 29.09},{63.8 , 29.16},
 {63.9 , 29.24},{64.0 , 29.31},{64.1 , 29.38},{64.2 , 29.46},{64.3 , 29.53},{64.4 , 29.61},{64.5 , 29.68},{64.6 , 29.76},{64.7 , 29.83},{64.8 , 29.91},{64.9 , 29.98},{65.0 , 30.06},{65.1 , 30.14},{65.2 , 30.21},{65.3 , 30.29},{65.4 , 30.37},
 {65.5 , 30.44},{65.6 , 30.52},{65.7 , 30.60},{65.8 , 30.68},{65.9 , 30.75},{66.0 , 30.83},{66.1 , 30.91},{66.2 , 30.99},{66.3 , 31.07},{66.4 , 31.15},{66.5 , 31.22},{66.6 , 31.30},{66.7 , 32.38},{66.8 , 32.46},{66.9 , 31.54},{67.0 , 31.62},
 {67.1 , 31.70},{67.2 , 31.78},{67.3 , 31.86},{67.4 , 31.94},{67.5 , 32.02},{67.6 , 32.10},{67.7 , 32.18},{67.8 , 32.26},{67.9 , 32.34},{68.0 , 32.42},{68.1 , 32.50},{68.2 , 32.59},{68.3 , 32.67},{68.4 , 32.75},{68.5 , 32.83},{68.6 , 32.92},
 {68.7 , 33.00},{68.8 , 33.08},{68.9 , 33.17},{69.0 , 33.25},{69.1 , 33.33},{69.2 , 33.42},{69.3 , 33.50},{69.4 , 33.59},{69.5 , 33.67},{69.6 , 33.75},{69.7 , 33.84},{69.8 , 33.92},{69.9 , 34.00},{70.0 , 34.09},{70.1 , 34.18},{70.2 , 34.26},{70.3 , 34.35},{70.4 , 34.43},{70.5 , 34.52},{70.6 , 34.61},
 {70.7 , 34.69},{70.8 , 34.79},{70.9 , 34.86},{71.0 , 34.95},{71.1 , 35.04},{71.2 , 35.13},{71.3 , 35.21},{71.4 , 35.30},{71.5 , 35.39},{71.6 , 35.48},{71.7 , 35.57},{71.8 , 35.65},{71.9 , 35.74},{72.0 , 35.83},{72.1 , 35.92},{72.2 , 36.01},
 {72.3 , 36.10},{72.4 , 36.19},{72.5 , 36.28},{72.6 , 36.38},{72.7 , 36.47},{72.8 , 36.56},{72.9 , 36.65},{73.0 , 36.74},{73.1 , 36.83},{73.2 , 36.92},{73.3 , 37.02},{73.4 , 37.11},{73.5 , 37.20},{73.6 , 37.29},{ 73.7 , 37.38},
 {73.8 , 37.48},{ 73.9 , 37.57},{ 74.0 , 37.66},{ 74.1 , 37.75},{ 74.2 , 37.85},{ 74.3 , 38.94},{ 74.4 , 38.04},{ 74.5 , 38.13},{ 74.6 , 38.23},{ 74.7 , 38.32},{ 74.8 , 38.42},{ 74.9 , 38.51},{ 75.0 , 38.61},{ 75.1 , 38.71},{ 75.2 , 38.80},{ 75.3 , 38.90},{ 75.4 , 39.00},{ 75.5 , 39.09},
 { 75.6 , 39.19},{ 75.7 , 39.28},{ 75.8 , 39.38},{ 75.9 , 39.47},{ 76.0 , 39.57},{ 76.1 , 39.67},{ 76.2 , 39.77},{ 76.3 , 39.87},
 { 76.4 , 39.98},{ 76.5 , 40.07},{ 76.6 , 40.17},{ 76.7 , 40.27},{ 76.8 , 40.37},{ 76.9 , 40.47},{ 77.0 , 40.57},{ 77.1 , 40.67},{ 77.2 , 40.77},{ 77.3 , 40.87},{ 77.4 , 40.97},{ 77.5 , 41.07},{ 77.6 , 41.18},
 { 77.7 , 41.28},{ 77.8 , 41.38},{ 77.9 , 41.48},{ 78.0 , 41.58},{ 78.1 , 41.68},{ 78.2 , 41.79},
 { 78.3 , 41.89},{ 78.4 , 42.00},{ 78.5 , 42.10},{ 78.6 , 42.20},{ 78.7 , 42.31},{ 78.8 , 42.41},{ 78.9 , 42.52},{ 79.0 , 42.62},{ 79.1 , 42.73},{ 79.2 , 42.83},{ 79.3 , 42.94},{ 79.4 , 43.05},{ 79.5 , 43.15},{ 79.6 , 43.26},{ 79.7 , 43.37},{ 79.8 , 43.48},
 { 79.9 , 43.58},{ 80.0 , 43.69},{ 80.1 , 43.80},{ 80.2 , 43.91},{ 80.3 , 44.02},{ 80.4 , 44.13},{ 80.5 , 44.23},{ 80.6 , 44.34},{ 80.7 , 44.45},{ 80.8 , 44.56},{ 80.9 , 44.67},{ 81.0 , 44.78},{ 81.1 , 44.89},{ 81.2 , 45.00},{ 81.3 , 45.12},{ 81.4 , 45.23},{ 81.5 , 45.34},{ 81.6 , 45.45},
 { 81.7 , 45.56},{ 81.8 , 45.68},{ 81.9 , 45.79},{ 82.0 , 45.90},{ 82.1 , 46.01},{ 82.2 , 46.13},{ 82.3 , 46.24},{ 82.4 , 46.36},{ 82.5 , 46.47},{ 82.6 , 46.58},{ 82.7 , 46.70},{ 82.8 , 46.81},{ 82.9 , 46.90},{ 83.0 , 47.04},{ 83.1 , 47.16},{ 83.2 , 47.28},{ 83.3 , 47.39},{ 83.4 , 47.51},
 { 83.5 , 47.63},{ 83.6 , 47.75},{ 83.7 , 47.87},{ 83.8 , 47.89},{ 83.9 , 48.10},{ 84.0 , 48.22},{ 84.1 , 48.34},{ 84.2 , 48.46},{ 84.3 , 48.58},{ 84.4 , 48.70},{ 84.5 , 48.82},{ 84.6 , 48.95},{ 84.7 , 49.07},{ 84.8 , 49.19},{ 84.9 , 49.31},{ 85.0 , 49.43},
 { 85.1 , 49.55},{ 85.2 , 49.68},{ 85.3 , 49.80},{ 85.4 , 49.92},{ 85.5 , 50.04},{ 85.6 , 50.17},{ 85.7 , 50.29},{ 85.8 , 50.41},{ 85.9 , 59.54}
     };

        // CRC table
        private readonly ushort[] crcTable = new ushort[]
        {
        0x0000, 0xC0C1, 0xC181, 0x0140, 0xC301, 0x03C0, 0x0280, 0xC241, 0xC601, 0x06C0, 0x0780,
        0xC741, 0x0500, 0xC5C1, 0xC481, 0x0440, 0xCC01, 0x0CC0, 0x0D80, 0xCD41, 0x0F00, 0xCFC1,
        0xCE81, 0x0E40, 0x0A00, 0xCAC1, 0xCB81, 0x0B40, 0xC901, 0x09C0, 0x0880, 0xC841, 0xD801,
        0x18C0, 0x1980, 0xD941, 0x1B00, 0xDBC1, 0xDA81, 0x1A40, 0x1E00, 0xDEC1, 0xDF81, 0x1F40,
        0xDD01, 0x1DC0, 0x1C80, 0xDC41, 0x1400, 0xD4C1, 0xD581, 0x1540, 0xD701, 0x17C0, 0x1680,
        0xD641, 0xD201, 0x12C0, 0x1380, 0xD341, 0x1100, 0xD1C1, 0xD081, 0x1040, 0xF001, 0x30C0,
        0x3180, 0xF141, 0x3300, 0xF3C1, 0xF281, 0x3240, 0x3600, 0xF6C1, 0xF781, 0x3740, 0xF501,
        0x35C0, 0x3480, 0xF441, 0x3C00, 0xFCC1, 0xFD81, 0x3D40, 0xFF01, 0x3FC0, 0x3E80, 0xFE41,
        0xFA01, 0x3AC0, 0x3B80, 0xFB41, 0x3900, 0xF9C1, 0xF881, 0x3840, 0x2800, 0xE8C1, 0xE981,
        0x2940, 0xEB01, 0x2BC0, 0x2A80, 0xEA41, 0xEE01, 0x2EC0, 0x2F80, 0xEF41, 0x2D00, 0xEDC1,
        0xEC81, 0x2C40, 0xE401, 0x24C0, 0x2580, 0xE541, 0x2700, 0xE7C1, 0xE681, 0x2640, 0x2200,
        0xE2C1, 0xE381, 0x2340, 0xE101, 0x21C0, 0x2080, 0xE041, 0xA001, 0x60C0, 0x6180, 0xA141,
        0x6300, 0xA3C1, 0xA281, 0x6240, 0x6600, 0xA6C1, 0xA781, 0x6740, 0xA501, 0x65C0, 0x6480,
        0xA441, 0x6C00, 0xACC1, 0xAD81, 0x6D40, 0xAF01, 0x6FC0, 0x6E80, 0xAE41, 0xAA01, 0x6AC0,
        0x6B80, 0xAB41, 0x6900, 0xA9C1, 0xA881, 0x6840, 0x7800, 0xB8C1, 0xB981, 0x7940, 0xBB01,
        0x7BC0, 0x7A80, 0xBA41, 0xBE01, 0x7EC0, 0x7F80, 0xBF41, 0x7D00, 0xBDC1, 0xBC81, 0x7C40,
        0xB401, 0x74C0, 0x7580, 0xB541, 0x7700, 0xB7C1, 0xB681, 0x7640, 0x7200, 0xB2C1, 0xB381,
        0x7340, 0xB101, 0x71C0, 0x7080, 0xB041, 0x5000, 0x90C1, 0x9181, 0x5140, 0x9301, 0x53C0,
        0x5280, 0x9241, 0x9601, 0x56C0, 0x5780, 0x9741, 0x5500, 0x95C1, 0x9481, 0x5440, 0x9C01,
        0x5CC0, 0x5D80, 0x9D41, 0x5F00, 0x9FC1, 0x9E81, 0x5E40, 0x5A00, 0x9AC1, 0x9B81, 0x5B40,
        0x9901, 0x59C0, 0x5880, 0x9841, 0x8801, 0x48C0, 0x4980, 0x8941, 0x4B00, 0x8BC1, 0x8A81,
        0x4A40, 0x4E00, 0x8EC1, 0x8F81, 0x4F40, 0x8D01, 0x4DC0, 0x4C80, 0x8C41, 0x4400, 0x84C1,
        0x8581, 0x4540, 0x8701, 0x47C0, 0x4680, 0x8641, 0x8201, 0x42C0, 0x4380, 0x8341, 0x4100,
        0x81C1, 0x8081, 0x4040
        };




        private void ReadAndUpdateData()
        {
            int dataIndex = 0;
            int dataLength = 0;
            bool isCRC = false;

            while (!stop)
            {
                try
                {
                    int newData = serialPort.ReadByte();

                    if (dataLength < dataBuffer.Length) // <-- Bounds checking
                    {
                        if (isCRC)
                        {
                            dataBuffer[dataLength++] = (byte)newData;
                            if (dataLength == 2)
                            {
                                dataLength = 0;
                                isCRC = false;
                                dataIndex = 0;
                            }
                        }
                        else
                        {
                            dataBuffer[dataLength++] = (byte)newData;
                            if (dataLength >= 3)
                            {
                                byte slaveID = dataBuffer[dataIndex];
                                byte command = dataBuffer[dataIndex + 1];
                                byte numBytes = dataBuffer[dataIndex + 2];

                                if (dataLength >= 3 + numBytes)
                                {
                                    if (slaveID == 0x01)
                                    {
                                        // Process data for first slave
                                        HandleDataForSlave1(dataIndex, numBytes);
                                    }
                                    else if (slaveID == 0x02)
                                    {
                                        // Process data for second slave
                                        HandleDataForSlave2(dataIndex, numBytes);
                                    }
                                    else if (slaveID == 0x05)
                                    {
                                        //Process data for third slave
                                        HandleDataForSlave3(dataIndex, numBytes);
                                    }
                                    else if (slaveID == 0x06 && command == 0x04)
                                    {
                                        // Call a method to handle data for Slave 06 with specific command
                                        HandleDataForSlave06(dataBuffer, dataIndex, numBytes);
                                    }
                                    else if (slaveID == 0x03)
                                    {
                                        HandleDataForSlave05(dataIndex, numBytes);
                                    }
                                    isCRC = true;
                                    dataIndex += 3 + numBytes;
                                    dataLength = 0;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        textBoxError.ForeColor = Color.Red;
                        textBoxError.Text = $"Data Read Error: {ex.Message}";
                        errorClearTimer.Stop();   // Restart the timer in case it's already running
                        errorClearTimer.Start();
                    });
                }
            }
        }
        private void AutoRequestTimer_Tick(object sender, EventArgs e)
        {
            // Check if the serial port is open
            if (!serialPort.IsOpen)
            {
                // Stop the auto request timer
                autoRequestTimer.Stop();

                // Optionally, you can display a message to inform the user
                this.Invoke((MethodInvoker)delegate
                {
                    textBoxError.ForeColor = Color.Red;
                    textBoxError.Text = $"Port Closed";
                    errorClearTimer.Stop();   // Restart the timer in case it's already running
                    errorClearTimer.Start();
                });
                return;
            }
            // Perform the Modbus request based on the current slave
            switch (currentSlave)
            {
                case 5:
                    SendModbusCommandToFivethSlave();
                    break;
                case 4:
                    // SendModbusCommandToFourthSlave();
                    break;
                case 3:

                    SendModbusCommandToThirdSlave();
                    break;
                case 2:
                    SendModbusCommandToSecondSlave();
                    break;
                case 1:
                    SendModbusCommand();
                    break;
                // Add more cases if you have additional slaves

                default:
                    break;
            }

            // Update the current slave for the next cycle
            currentSlave--;

            // If all slaves have been queried, reset to the highest slave number
            if (currentSlave < 1)
            {
                currentSlave = 5; // Change this value if you have more than 3 slaves
            }
        }
        private void HandleDataForSlave3(int dataIndex, byte numbyte)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                int numBytes = dataBuffer[dataIndex + 2];
                // Extract data for each sensor and update sensorDataArray
                for (int i = dataIndex + 3; i < dataIndex + 3 + numBytes; i += 2)
                {
                    int sensorData = (dataBuffer[i] << 8) + dataBuffer[i + 1];

                    int sensorIndex = (i - dataIndex - 3) / 2;

                    // Update sensorDataArray
                    if (sensorIndex >= 0 && sensorIndex < sensorDataArray.Length)
                    {
                        sensorDataArray[sensorIndex] = sensorData;
                    }

                    // Update corresponding TextBox based on sensorIndex
                    switch (sensorIndex)
                    {
                        case 0:
                            textBoxP1.Text = $"{sensorData} PSI";
                            //p1 = sensorData;
                            break;
                        case 1:
                            textBoxP2.Text = $"{sensorData} PSI";
                            //p2 = sensorData;
                            break;
                        case 2:
                            textBoxUS1.Text = $"{sensorData}";
                            //US1 = sensorData;
                            break;
                        case 3:
                            textBoxUS2.Text = $"{sensorData}";
                            UpdateTextBoxML(sensorData);
                            break;
                        case 4:
                            // Skip handling data for this sensor
                            break;
                        case 5:
                            textBoxRH1.Text = $"{sensorData}%";
                            //rh = sensorData;
                            break;

                            // Add more cases if you have additional sensors
                    }

                }
            });




        }

        private void HandleDataForSlave05(int dataIndex, byte numbyte)

        {
            BeginInvoke((MethodInvoker)delegate
            {
                int numBytes = dataBuffer[dataIndex + 2];
                // Extract data for each sensor and update sensorDataArray
                for (int i = dataIndex + 3; i < dataIndex + 3 + numBytes; i += 2)
                {
                    int sensorData = (dataBuffer[i] << 8) + dataBuffer[i + 1];

                    int sensorIndex = (i - dataIndex - 3) / 2;

                    // Update sensorDataArray
                    if (sensorIndex >= 0 && sensorIndex < sensorDataArray.Length)
                    {
                        sensorDataArray[sensorIndex] = sensorData;
                    }

                    // Update corresponding TextBox based on sensorIndex
                    switch (sensorIndex)
                    {
                        case 0:
                            //textBoxS3.Text = $"{sensorData} C / {CelsiusToFahrenheit(sensorData):F} ";
                            //s3= CelsiusToFahrenheit(sensorData);
                            break;
                        case 1:
                            textBoxRH1.Text = $"{sensorData - 2} %";
                            RH1 = (sensorData) - 2;
                            break;
                        case 2:
                            textBoxB4WB1.Text = $"{sensorData + 1}C";
                            s1 = (sensorData + 1);
                            break;
                        case 3:
                            textBoxRH2.Text = $"{sensorData - 2} %";
                            RH2 = (sensorData) - 2;

                            break;
                        case 4:
                            textBoxB4WB2.Text = $"{sensorData}C";
                            s3 = (sensorData);
                            break;




                            // Add more cases if you have additional sensors
                    }

                }

            });
        }
        private void HandleDataForSlave06(byte[] dataBuffer, int dataIndex, byte numBytes)
        {


            BeginInvoke((MethodInvoker)delegate
            {
                // Extract the first value (08FD)
                int firstValue = (dataBuffer[dataIndex + 3] << 8) | dataBuffer[dataIndex + 4];
                double voltage = firstValue / 10.0;
                textBoxVolt.Text = $"Voltage: {voltage} V";



                // Extract the last value (022B)
                int lastValue = (dataBuffer[dataIndex + 3 + numBytes - 4] << 8) | dataBuffer[dataIndex + 3 + numBytes - 3];
                double current = lastValue / 100.0;
                textBoxAmp.Text = $"Current: {current} A";
                Debug.WriteLine($" Volt {voltage}, Current {current}");
                double Power = current * voltage;
                textBoxPower1.Text = $"{Power} W";
                double EER = tBtu / Power;
                textBoxEER.Text = $"{EER} ";
            });
        }
        private void ErrorClearTimer_Tick(object sender, EventArgs e)
        {
            textBoxError.Text = "";
            errorClearTimer.Stop();
        }
        private void HandleDataForSlave1(int dataIndex, byte numBytes)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                // Extract data for each sensor and update sensorDataArray
                for (int i = dataIndex + 3; i < dataIndex + 3 + numBytes; i += 2)
                {
                    int sensorData = (dataBuffer[i] << 8) + dataBuffer[i + 1];
                    if ((sensorData & 0x8000) != 0)
                    {
                        sensorData -= 65536; // Convert FFFF to -1
                    }
                    int sensorIndex = (i - dataIndex - 3) / 2;
                    double celsiusData = (sensorData / 10.0); // Divide by 10 to get decimal representation

                    double fahrenheitData = CelsiusToFahrenheit(celsiusData);

                    // Update sensorDataArray
                    if (sensorIndex >= 0 && sensorIndex < sensorDataArray.Length)
                    {
                        sensorDataArray[sensorIndex] = celsiusData;
                    }

                    // Update corresponding TextBox based on sensorIndex
                    switch (sensorIndex)
                    {
                        case 0:
                            if (celsiusData < -55)
                            {
                                //textBoxS1.Text = "Short/Disconnected";
                            }
                            else
                            {
                                //textBoxS1.Text = $"{celsiusData + 0.9:F1}°C / {fahrenheitData +1.1:F1}°F";
                                //s1 = fahrenheitData+1.1;
                            }
                            break;
                        case 1:
                            if (celsiusData < -55)
                            {
                                textBoxS2.Text = $"Short/Disconnected";
                            }
                            else
                            {
                                textBoxS2.Text = $"{celsiusData:F1}°C / {fahrenheitData:F1}°F";
                                s2 = fahrenheitData;
                            }
                            break;
                        case 2:
                            if (celsiusData < -55)
                            {
                                //textBoxS3.Text = $"Short/Disconnected";
                            }
                            else
                            {
                                //textBoxS3.Text = $"{celsiusData :F1}°C / {fahrenheitData:F1}°F"; ;
                                //s3 = fahrenheitData ;
                            }
                            break;
                        case 3:
                            if (celsiusData < -55)
                            {
                                textBoxS4.Text = $"Short/Disconnected";
                            }
                            else
                            {
                                textBoxS4.Text = $"{celsiusData:F1} °C /  {fahrenheitData:F1}°F";
                                s4 = fahrenheitData - 1.5;
                            }
                            break;
                        case 4:
                            if (celsiusData < -55)
                            {
                                textBoxSH.Text = $"Short/Disconnected";
                            }
                            else
                            {
                                textBoxSH.Text = $"{celsiusData:F1}°C / {fahrenheitData:F1}°F";
                                sh = fahrenheitData;
                            }
                            break;
                        case 5:
                            if (celsiusData < -55)
                            {
                                textBoxSC.Text = $"Short/Disconnected";
                            }
                            else
                            {
                                textBoxSC.Text = $"{celsiusData:F1}°C / {fahrenheitData:F1}°F";
                                sc = fahrenheitData;
                            }
                            break;
                        case 6:
                            if (celsiusData < -55)
                            {
                                textBoxBS.Text = $"Short/Disconnected";
                            }
                            else
                            {
                                textBoxBS.Text = $"{celsiusData:F1} °C /  {fahrenheitData:F1}°F";
                                bs = fahrenheitData;
                            }
                            break;
                        case 7:
                            if (celsiusData < -55)
                            {
                                textBoxCIWB.Text = $"Short/Disconnected";
                            }
                            else
                            {
                                textBoxCIWB.Text = $"{celsiusData:F1} °C /  {fahrenheitData:F1}°F";
                                ciwb = fahrenheitData;
                            }
                            break;
                            // Add more cases if you have additional sensors
                    }

                    // Calculate BTU and latent capacity based on the updated sensorDataArray
                    if (s1 != double.NaN && a2 != double.NaN && s3 != double.NaN && s2 != double.NaN)
                    {
                        double Tw = CalculateWetBulbTemperature(s1, RH1);
                        double Tw1 = CalculateWetBulbTemperature(s3, RH2);
                        double TwFahrenheit = CelsiusToFahrenheit(Tw);
                        double Tw1Fahrenheit = CelsiusToFahrenheit(Tw1);

                        if (TwFahrenheit > 85.9)
                        {
                            TwFahrenheit = 85.9; //limit the temperature to max
                        }

                        if (Tw1Fahrenheit > 85.9)
                        {
                            Tw1Fahrenheit = 85.9; // Limit the temperature to the maximum (85.9°F)
                        }



                        double enthalpyS1 = GetEnthalpy(TwFahrenheit);
                        double enthalpyS3 = GetEnthalpy(s2);
                        textBoxS1.Text = $"{Tw:F1}°C / {TwFahrenheit:F1} °F";
                        textBoxS3.Text = $"{Tw1:F1}°C / {Tw1Fahrenheit:F1}°F";
                        textBox1.Text = textBoxS2.Text;
                        Debug.WriteLine($" s1: {s1}, s3: {s3}, enthalpyS1: {enthalpyS1}, enthalpyS3: {enthalpyS3}");

                        Debug.WriteLine($" Tw: {Tw}, Tw1: {Tw1}, TwFahrenheit: {TwFahrenheit}, Tw1Fahrenheit: {Tw1Fahrenheit}, enthalpyS1: {enthalpyS1}, enthalpyS3: {enthalpyS3}");

                        //textBoxS1.Text = $"{s1:F1}°C / {s1:F1}°F";
                        //textBoxS3.Text = $"{s1:F1}°C / {s3 :F1}°F";
                        if (heatingMode)  // Assuming heatingMode is a boolean variable indicating the mode
                        {
                            sBtu = (s5 * 1.08) * (a2 - s2);
                        }
                        else  // Cooling mode
                        {
                            sBtu = (s5 * 1.08) * (s2 - a2);
                        }


                        tBtu = s5 * 4.45 * (enthalpyS1 - enthalpyS3);
                        latentCapacity = tBtu - sBtu;

                        double dischargetemp = 0; // Default value for discharge temperature
                        if (!double.TryParse(textBoxLcap.Text, out dischargetemp))
                        {
                            dischargetemp = 0; // Fallback value if parsing fails
                        }

                        double ptDischarge = GetTemperatureFromPTChart(dischargetemp);
                        double Subcool = ptDischarge - sc;
                        double celSub = FahrenheitToCelsius(Subcool);
                        textBoxML.Text = $"{Subcool:F1} °F / {celSub:F1} °C";

                        double suctiontemp = 0; // Default value for suction temperature
                        if (!double.TryParse(textBoxUS1.Text, out suctiontemp))
                        {
                            suctiontemp = 0; // Fallback value if parsing fails
                        }

                        double ptSuction = GetTemperatureFromPTChart(suctiontemp);
                        double SuperHeat = bs - ptSuction;
                        double celsuper = FahrenheitToCelsius(SuperHeat);
                        textBoxUS2.Text = $"{SuperHeat:F1} °F / {celsuper:F1} °C";



                        UpdateResultsUI();
                    }

                    Debug.WriteLine($"SensorIndex: {sensorIndex}, CelsiusData: {celsiusData}, FahrenheitData: {fahrenheitData}");

                }
            });
        }
        private void InitializeErrorBlinkTimer()
        {
            errorBlinkTimer.Interval = 500; // Blink every 500ms (adjust as needed)
            errorBlinkTimer.Tick += (s, e) =>
            {
                pictureBox1.Visible = !pictureBox1.Visible; // Toggle visibility
            };
        }
        private void LogError(string errorMessage)
        {
            this.Invoke((MethodInvoker)delegate
            {
                // Add error to listBox1
                listBox1.Items.Add($"{DateTime.Now:HH:mm:ss} - {errorMessage}");
                listBox1.TopIndex = listBox1.Items.Count - 1; // Auto-scroll

                // Show blinking error indicator
                if (!isErrorBlinking)
                {
                    isErrorBlinking = true;
                    pictureBox1.Visible = true;
                    errorBlinkTimer.Start();
                }
            });
        }
        private double FahrenheitToCelsius(double fahrenheit)
        {
            return (fahrenheit - 32) * 5.0 / 9.0;
        }

        private double GetEnthalpy(double temperature)
        {
            // Round to the same precision as your enthalpyLookup keys
            temperature = Math.Round(temperature, 1);

            if (enthalpyLookup.ContainsKey(temperature))
            {
                return enthalpyLookup[temperature];
            }
            else
            {
                return 0.0;
            }
        }
        private double CalculateWetBulbTemperature(double temperature, double rh)
        {
            return temperature * Math.Atan(0.151977 * Math.Pow((rh + 8.313659), 0.5))
                + Math.Atan(temperature + rh)
                - Math.Atan(rh - 1.676331)
                + 0.00391838 * Math.Pow(rh, 1.5) * Math.Atan(0.023101 * rh)
                - 4.686035;
        }





        private void HandleDataForSlave2(int dataIndex, byte numBytes)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                if (dataBuffer.Length >= dataIndex + 3 + numBytes)
                {
                    double[] sensorData = new double[numBytes / 2];

                    for (int i = dataIndex + 3, sensorIndex = 0; i < dataIndex + 3 + numBytes; i += 2, sensorIndex++)
                    {
                        int sensorDataValue = (dataBuffer[i] << 8) + dataBuffer[i + 1];

                        // Additional code for handling sensorData as you provided
                        if ((sensorDataValue & 0x8000) != 0)
                        {
                            sensorDataValue -= 65536; // Convert FFFF to -1
                        }

                        double celsiusData = sensorDataValue / 10.0;
                        double fahrenheitData = CelsiusToFahrenheit(celsiusData);

                        sensorData[sensorIndex] = fahrenheitData;

                        // Update corresponding TextBox based on sensorIndex
                        switch (sensorIndex)
                        {
                            case 0:
                                if (celsiusData < -55)
                                {
                                    textBoxA1.Text = $"Short/Disconnected";
                                }
                                else
                                {
                                    textBoxA1.Text = $"{celsiusData:F1}°C / {fahrenheitData:F1}°F";
                                    a1 = fahrenheitData;
                                }
                                break;
                            case 1:
                                if (celsiusData < -55)
                                {
                                    textBoxA2.Text = $"Short/Disconnected";
                                }
                                else
                                {
                                    textBoxA2.Text = $"{celsiusData:F1}°C / {fahrenheitData:F1}°F";
                                    a2 = fahrenheitData;
                                }
                                break;
                            case 2:
                                if (celsiusData < -55)
                                {
                                    textBoxA3.Text = $"Short/Disconnected";
                                }
                                else
                                {
                                    textBoxA3.Text = $"{celsiusData:F1}°C / {fahrenheitData:F1}°F";
                                    a3 = fahrenheitData;
                                }
                                break;
                            case 3:
                                if (celsiusData < -55)
                                {
                                    textBoxA4.Text = $"Short/Disconnected";
                                }
                                else
                                {
                                    textBoxA4.Text = $"{celsiusData:F1}°C / {fahrenheitData:F1}°F";
                                    a4 = fahrenheitData;
                                }
                                break;
                            case 4:
                                if (celsiusData < -55)
                                {
                                    textBoxA5.Text = $"Short/Disconnected";
                                }
                                else
                                {
                                    textBoxA5.Text = $"{celsiusData:F1}°C / {fahrenheitData:F1}°F";
                                    a5 = fahrenheitData;
                                }
                                break;
                            case 5:
                                if (celsiusData < -55)
                                {
                                    textBoxA6.Text = $"Short/Disconnected";
                                }
                                else
                                {
                                    textBoxA6.Text = $"{celsiusData:F1}°C / {fahrenheitData:F1}°F";
                                    a6 = fahrenheitData;
                                }
                                break;
                            case 6:
                                if (celsiusData < -55)
                                {
                                    textBoxA7.Text = $"Short/Disconnected";
                                }
                                else
                                {
                                    textBoxA7.Text = $"{celsiusData:F1}°C / {fahrenheitData:F1}°F";
                                    a7 = fahrenheitData;
                                }
                                break;
                            case 7:
                                if (celsiusData < -55)
                                {
                                    textBoxA8.Text = $"Short/Disconnected";
                                }
                                else
                                {
                                    textBoxA8.Text = $"{celsiusData:F1}°C / {fahrenheitData:F1}°F";
                                    a8 = fahrenheitData;
                                }
                                break;


                                // Add more cases if needed
                        }
                    }
                    // Calculate averages after updating all sensors
                    // not being used currently
                    double avgCelsius1 = (a1 + a2 + a3) / 10.0;
                    double avgFahrenheit1 = CelsiusToFahrenheit(avgCelsius1);

                    double avgCelsius2 = (a4 + a5 + a6) / 10.0;
                    double avgFahrenheit2 = CelsiusToFahrenheit(avgCelsius2);

                    // Display averages


                }
            });
        }


        private void UpdateTextBoxML(int us2)
        {
            // Map sensor data to corresponding values
            int mlValue = -1;
            if (us2 == 43) mlValue = 300;
            else if (us2 == 42) mlValue = 400;
            else if (us2 == 41) mlValue = 500;
            else if (us2 == 40) mlValue = 600;
            else if (us2 == 39) mlValue = 700;
            else if (us2 == 38) mlValue = 800;
            else if (us2 == 36) mlValue = 900;
            else if (us2 == 35) mlValue = 1000;
            else if (us2 >= 34) mlValue = 1100;
            // Add more conditions as needed...

            // Update the textBoxML
            textBoxML.Text = $"{mlValue} ml";

            // Check if the tank is full (1000ml)
            if (mlValue >= 1000)
            {
                // Start blinking the red light
                isRedLightOn = true;
                blinkTimer.Start();

                // Show the red light picture box
                pictureBoxRedLight.Visible = true;
            }
            else
            {
                // Stop blinking and turn off the red light
                isRedLightOn = false;
                blinkTimer.Stop();

                // Hide the red light picture box
                pictureBoxRedLight.Visible = false;
            }
        }

        private void BlinkTimer_Tick(object sender, EventArgs e)
        {
            // Toggle the visibility of the red light
            pictureBoxRedLight.Visible = !pictureBoxRedLight.Visible;
        }

        // Assume this method is called when new data is received from the sensor




        private static double CelsiusToFahrenheit(double celsius)
        {
            return (celsius * 9 / 5) + 32;
        }




        private void UpdateResultsUI()
        {
            BeginInvoke((MethodInvoker)delegate
            {
                textBoxSbtu.Text = sBtu.ToString();
                textBoxTbtu.Text = tBtu.ToString();

            });
        }



        private void SendModbusCommand()
        {
            try
            {
                CheckIfPortIsOpen();

                // Replace 0x01 with your actual slave node ID
                byte slaveNodeID = 0x01;

                // Modbus command: Read Holding Registers
                byte[] modbusRequest = { slaveNodeID, 0x03, 0x00, 0x00, 0x00, 0x08 };

                // Calculate CRC
                ushort crc = CalculateCRC(modbusRequest);

                // Append CRC to the command
                byte[] modbusCommand = modbusRequest.Concat(BitConverter.GetBytes(crc)).ToArray();

                // Send the command to the serial port
                serialPort.Write(modbusCommand, 0, modbusCommand.Length);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private ushort CalculateCRC(byte[] data)
        {
            ushort crc = 0xFFFF;

            foreach (byte b in data)
            {
                crc = (ushort)((crc >> 8) ^ crcTable[(crc ^ b) & 0xFF]);
            }

            return crc;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            buttonOpenPort.Image = System.Drawing.Image.FromFile("C:\\Users\\Rayyan Tech\\Downloads\\On button bgr.png");
            buttonOpenPort.FlatStyle = FlatStyle.Flat;
            buttonOpenPort.FlatAppearance.BorderSize = 0;
        }


        public void button1_Click_1(object sender, EventArgs e)
        {
            if (!serialPort.IsOpen)
            {
                try
                {
                    Program.SetForm1(this);
                    // Configure the serial port settings
                    serialPort.PortName = comboBoxPort.SelectedItem.ToString(); // Set your COM port name
                    serialPort.BaudRate = 9600;   // Set your baud rate
                    serialPort.DataBits = 8;      // Set your data bits
                    serialPort.Parity = Parity.None;
                    serialPort.StopBits = StopBits.One;

                    // Open the serial port
                    serialPort.Open();
                    Thread dataThread = new Thread(ReadAndUpdateData);
                    dataThread.Start();



                    BeginInvoke((MethodInvoker)delegate
                    {
                        labelConnectionStatus.Text = "connected";
                        labelConnectionStatus.ForeColor = Color.Green;
                        buttonOpenPort.Enabled = false;
                        buttonClosePort.Enabled = true;

                    });
                }
                catch (Exception ex)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        textBoxError.ForeColor = Color.Red;
                        textBoxError.Text = $"Error: {ex.Message}";
                        errorClearTimer.Stop();   // Restart the timer in case it's already running
                        errorClearTimer.Start();
                    });
                }
            }
            else
            {
                this.Invoke((MethodInvoker)delegate
                {
                    textBoxError.ForeColor = Color.Red;
                    textBoxError.Text = $"Port Already Open";
                    errorClearTimer.Stop();   // Restart the timer in case it's already running
                    errorClearTimer.Start();
                });
            }
        }
        private void SendModbusCommandToFivethSlave()
        {
            try
            {
                CheckIfPortIsOpen();

                // Replace 0x01 with your actual slave node ID
                byte slaveNodeID = 0x03;

                // Modbus command: Read Holding Registers
                byte[] modbusRequest = { slaveNodeID, 0x03, 0x00, 0x64, 0x00, 0x6 };

                // Calculate CRC
                ushort crc = CalculateCRC(modbusRequest);

                // Append CRC to the command
                byte[] modbusCommand = modbusRequest.Concat(BitConverter.GetBytes(crc)).ToArray();

                // Send the command to the serial port
                serialPort.Write(modbusCommand, 0, modbusCommand.Length);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void SendModbusCommandToFourthSlave()
        {
            try
            {

                // Define the Modbus RTU request as a byte array
                byte[] modbusRequest = { 0x06, 0x04, 0x00, 0x00, 0x00, 0x07 };

                // Calculate CRC
                ushort crc = CalculateCRC(modbusRequest);

                // Append CRC to the command
                byte[] modbusCommand = modbusRequest.Concat(BitConverter.GetBytes(crc)).ToArray();

                // Send the command to the serial port
                serialPort.Write(modbusCommand, 0, modbusCommand.Length);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        private void SendModbusCommandToThirdSlave()
        {
            try
            {
                CheckIfPortIsOpen();

                // Replace 0x02 with your actual slave node ID for the Third slave
                byte slaveNodeID = 0x05;

                // Modbus command: Read Holding Registers
                byte[] modbusRequest = { slaveNodeID, 0x03, 0x00, 0x00, 0x00, 0x02 };

                // Calculate CRC
                ushort crc = CalculateCRC(modbusRequest);

                // Append CRC to the command
                byte[] modbusCommand = modbusRequest.Concat(BitConverter.GetBytes(crc)).ToArray();

                // Send the command to the serial port
                serialPort.Write(modbusCommand, 0, modbusCommand.Length);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }



        private void SendModbusCommandToSecondSlave()
        {
            try
            {
                CheckIfPortIsOpen();

                // Replace 0x02 with your actual slave node ID for the second slave
                byte slaveNodeID = 0x02;

                // Modbus command: Read Holding Registers
                byte[] modbusRequest = { slaveNodeID, 0x03, 0x00, 0x00, 0x00, 0x08 };

                // Calculate CRC
                ushort crc = CalculateCRC(modbusRequest);

                // Append CRC to the command
                byte[] modbusCommand = modbusRequest.Concat(BitConverter.GetBytes(crc)).ToArray();

                // Send the command to the serial port
                serialPort.Write(modbusCommand, 0, modbusCommand.Length);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void CheckIfPortIsOpen()
        {
            // Check if the port is open before writing to it
            if (!serialPort.IsOpen)
            {
                throw new InvalidOperationException("Open port before sending the command.");
            }
        }

        private void HandleException(Exception ex)
        {
            // Handle the exception (e.g., display a message to the user)
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // You can throw a specific exception or log the error as needed
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] availablePorts = SerialPort.GetPortNames();
            comboBoxPort.Items.AddRange(availablePorts);

            if (availablePorts.Length > 0)
            {
                comboBoxPort.SelectedItem = availablePorts[0];
                serialPort.PortName = availablePorts[0];
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }



        private void button2_Click(object sender, EventArgs e)
        {

        }




        private void SaveDataToPdf()
        {
            // Determine the mode (heat or cool)
            string mode = heatingMode ? "Cool" : "Heat";

            // Specify the folder path where you want to save the PDF
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Datapdf");

            // Create the folder if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Combine the folder path with the file name including the mode
            string fileName = $"LabTest_{mode}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string filePath = Path.Combine(folderPath, fileName);

            // Create a document
            Document doc = new Document(PageSize.LETTER.Rotate()); // Set landscape orientation

            // Create a FileStream to write the PDF
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                // Create a PdfWriter instance
                using (PdfWriter writer = PdfWriter.GetInstance(doc, fs))
                {
                    // Open the document for writing
                    doc.Open();

                    // Set font
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    // Set larger and bold font for the title
                    iTextSharp.text.Font titleFont = new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                    // Add title
                    Paragraph title = new Paragraph($"Lab Test Result - {mode} Mode", titleFont); // Using the new font here
                    title.Alignment = Element.ALIGN_CENTER;
                    title.SpacingAfter = 10f;
                    doc.Add(title);

                    // Add current date and time
                    // Add current date and time in the top right corner
                    ColumnText.ShowTextAligned(writer.DirectContent,
                        Element.ALIGN_RIGHT,
                        new Phrase($"Date and Time: {DateTime.Now}", font),
                        doc.PageSize.Width - doc.RightMargin,
                        doc.PageSize.Height - doc.TopMargin - 20,
                        0);

                    // Add engineer name, unit name, compressor, coil size, capacity, fan details, motor, and gas
                    AddTextInBox(doc, writer, $"Engr Name: {textBoxNamelab.Text}", 3, 500, font);
                    AddTextInBox(doc, writer, $"Unit Name: {textBoxUnitlab.Text}", 3, 480, font);
                    AddTextInBox(doc, writer, $"Comp: {textBoxComplab.Text}", 3, 460, font);
                    AddTextInBox(doc, writer, $"Coil: {textBoxCoillab.Text}", 3, 440, font);
                    AddTextInBox(doc, writer, $"Cap: {textBoxCaplab.Text} BTU", 3, 420, font);
                    AddTextInBox(doc, writer, $"Fan: {textBoxFanlab.Text}", 3, 400, font);
                    AddTextInBox(doc, writer, $"Motor: {textBoxMotor.Text}", 3, 380, font);
                    AddTextInBox(doc, writer, $"Gas 410A: {textBoxGas.Text}", 3, 360, font);
                    AddTextInBox(doc, writer, $"I.C Details: {textBoxInverter.Text}", 3, 340, font);

                    // Draw group boxes and labels
                    DrawGroupBox("Evaporator", 230, 350, 220, 150, BaseColor.BLACK, writer.DirectContent, font);
                    DrawGroupBox("Condenser", 450, 350, 220, 150, BaseColor.BLACK, writer.DirectContent, font);
                    DrawGroupBox("Electrical", 230, 175, 220, 150, BaseColor.BLACK, writer.DirectContent, font);
                    DrawGroupBox("BTU", 450, 175, 220, 150, BaseColor.BLACK, writer.DirectContent, font);




                    // Add sensor data inside the boxes
                    float boxMargin = 10; // Margin for data inside the boxes
                    float boxTextSpacing = 15; // Spacing between lines of text inside the boxes

                    // Add sensor data to Evaporator box
                    AddTextInBox(doc, writer, $"Intake WB: {textBoxS1.Text}", 235, 485 - boxMargin, font);
                    AddTextInBox(doc, writer, $"Intake DB: {textBoxA2.Text}", 235, 485 - boxMargin - boxTextSpacing, font);
                    AddTextInBox(doc, writer, $"Discharge WB: {textBoxS3.Text}", 235, 485 - boxMargin - 2 * boxTextSpacing, font);
                    AddTextInBox(doc, writer, $"Discharge DB: {textBoxS2.Text}", 235, 485 - boxMargin - 3 * boxTextSpacing, font);
                    AddTextInBox(doc, writer, $"Room Temp: {textBoxA3.Text}", 235, 485 - boxMargin - 4 * boxTextSpacing, font);
                    AddTextInBox(doc, writer, $"Humidity in: {textBoxRH1.Text}", 235, 485 - boxMargin - 5 * boxTextSpacing, font);
                    AddTextInBox(doc, writer, $"Humidity out: {textBoxRH2.Text}", 235, 485 - boxMargin - 6 * boxTextSpacing, font);

                    // Add sensor data to Condenser box
                    AddTextInBox(doc, writer, $"Room Temp: {textBoxSH.Text}", 455, 485 - boxMargin, font);
                    AddTextInBox(doc, writer, $"Condenser Air out: {textBoxSC.Text}", 455, 485 - boxMargin - boxTextSpacing, font);
                    AddTextInBox(doc, writer, $"CFM: {s5}", 455, 485 - boxMargin - 2 * boxTextSpacing, font);
                    AddTextInBox(doc, writer, $"Discharge Line: {textBoxA5.Text}", 455, 485 - boxMargin - 3 * boxTextSpacing, font);
                    AddTextInBox(doc, writer, $"Liquid Line: {textBoxA6.Text}", 455, 485 - boxMargin - 4 * boxTextSpacing, font);
                    AddTextInBox(doc, writer, $"IPM Temp: {textBoxS4.Text}", 455, 485 - boxMargin - 5 * boxTextSpacing, font);


                    // Add sensor data to Electrical box
                    AddTextInBox(doc, writer, $"Volt: {textBoxVolt.Text}", 235, 305 - boxMargin, font);
                    AddTextInBox(doc, writer, $"Current: {textBoxAmp.Text}", 235, 305 - boxMargin - boxTextSpacing, font);
                    AddTextInBox(doc, writer, $"Power: {textBoxPower1.Text}", 235, 305 - boxMargin - 2 * boxTextSpacing, font);
                    AddTextInBox(doc, writer, $"EER : {textBoxEER.Text}", 235, 305 - boxMargin - 3 * boxTextSpacing, font);
                    // Add sensor data to BTU box
                    AddTextInBox(doc, writer, $"T.btu: {tBtu}", 455, 305 - boxMargin, font);
                    AddTextInBox(doc, writer, $"S.btu: {sBtu}", 455, 305 - boxMargin - boxTextSpacing, font);
                    AddTextInBox(doc, writer, $"Actual BTU: {textBox2.Text}", 455, 305 - boxMargin - 2 * boxTextSpacing, font);
                    AddTextInBox(doc, writer, $"Discharge Pressure: {textBoxLcap.Text}", 455, 305 - boxMargin - 3 * boxTextSpacing, font);
                    AddTextInBox(doc, writer, $"Suction Pressure: {textBoxUS1.Text}", 455, 305 - boxMargin - 4 * boxTextSpacing, font);
                    AddTextInBox(doc, writer, $"SuperHeat: {textBoxUS2.Text}", 455, 305 - boxMargin - 5 * boxTextSpacing, font);
                    AddTextInBox(doc, writer, $"SubCool: {textBoxML.Text}", 455, 305 - boxMargin - 6 * boxTextSpacing, font);

                    // Close the document
                    doc.Close();
                }
            }

            MessageBox.Show($"PDF saved successfully to {filePath}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Define method to add text inside a box
        void AddTextInBox(Document doc, PdfWriter writer, string text, float x, float y, iTextSharp.text.Font font)
        {
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(text, font), x, y, 0);
        }



        // Define method to draw group boxes
        void DrawGroupBox(string label, float x, float y, float width, float height, BaseColor color, PdfContentByte contentByte, iTextSharp.text.Font font)
        {
            // Draw rectangle for group box
            contentByte.SetColorStroke(color);
            contentByte.Rectangle(x, y, width, height);
            contentByte.Stroke();


            // Add label inside the group box
            ColumnText.ShowTextAligned(contentByte, Element.ALIGN_LEFT, new Phrase(label, font), x + 5, y + height + 5, 0);
        }

        // Button click event to toggle heating mode
        private void button14_Click(object sender, EventArgs e)
        {
            // Toggle the heatingMode when the button is clicked
            heatingMode = !heatingMode;
        }









        private void textBoxS1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_Click(object sender, EventArgs e)
        {

        }

        private void textBoxS3_Click(object sender, EventArgs e)
        {

        }

        private void textBoxS4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            SendModbusCommand();


        }

        private void button1_Click_3(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click_4(object sender, EventArgs e)
        {
            SendModbusCommandToSecondSlave();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            SendModbusCommandToThirdSlave();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            // Toggle the auto request flag
            isAutoRequestEnabled = !isAutoRequestEnabled;

            // Enable or disable the timer based on the flag
            if (isAutoRequestEnabled)
            {
                autoRequestTimer.Start();
            }
            else
            {
                autoRequestTimer.Stop();
            }
        }

        static Dictionary<double, double> ptChartR410A = new Dictionary<double, double>
{
    {-60.0, 0.9},   {-55.0, 1.9},   {-50.0, 3.2},   {-45.0, 7.0},   {-40.0, 10.1},  {-35.0, 13.5},
    {-30.0, 17.2},  {-25.0, 21.4},  {-20.0, 25.9},  {-18.0, 27.8},  {-16.0, 29.7},  {-14.0, 31.8},
    {-12.0, 33.9},  {-10.0, 36.1},  {-8.0, 38.4},   {-6.0, 40.7},   {-4.0, 43.1},   {-2.0, 45.6},
    {0.0, 48.2},    {1.0, 49.5},    {2.0, 50.9},    {3.0, 52.2},    {4.0, 53.6},    {5.0, 55.0},
    {6.0, 56.4},    {7.0, 57.9},    {8.0, 59.3},    {9.0, 60.8},    {10.0, 62.3},   {11.0, 63.9},
    {12.0, 65.4},   {13.0, 67.0},   {14.0, 68.6},   {15.0, 70.2},   {16.0, 71.9},   {17.0, 73.5},
    {18.0, 75.2},   {19.0, 77.0},   {20.0, 78.7},   {21.0, 80.5},   {22.0, 82.3},   {23.0, 84.1},
    {24.0, 85.9},   {25.0, 87.8},   {26.0, 89.7},   {27.0, 91.6},   {28.0, 93.5},   {29.0, 95.5},
    {30.0, 97.7},   {31.0, 99.5},   {32.0, 101.4},  {33.0, 103.6},  {34.0, 105.7},  {35.0, 107.9},
    {36.0, 110.0},  {37.0, 112.2},  {38.0, 114.0},  {40.0, 119.9},  {45.0, 132.4},  {50.0, 145.7},
    {55.0, 159.9},  {60.0, 175.1},  {65.0, 191.3},  {70.0, 208.5},  {75.0, 226.8},  {80.0, 246.1},
    {85.0, 266.5},  {90.0, 288.0},  {95.0, 310.5},  {100.0, 334.1}, {105.0, 358.9}, {110.0, 384.8},
    {115.0, 411.8}, {120.0, 439.9}, {125.0, 469.2}, {130.0, 499.6}, {135.0, 531.2}, {140.0, 563.9},
    {145.0, 597.8}, {150.0, 632.9}, {155.0, 669.1}
};
        private double GetTemperatureFromPTChart(double pressure)
        {
            // Round the pressure to one decimal place to match your chart's precision
            pressure = Math.Round(pressure, 1);

            // Check if the exact pressure is available in the dictionary
            if (ptChartR410A.ContainsValue(pressure))
            {
                // If an exact match is found, return the corresponding temperature
                return ptChartR410A.First(kvp => kvp.Value == pressure).Key;
            }

            // Find the closest lower and upper pressure values for interpolation
            var lower = ptChartR410A.Where(kvp => kvp.Value <= pressure).OrderByDescending(kvp => kvp.Value).FirstOrDefault();
            var upper = ptChartR410A.Where(kvp => kvp.Value >= pressure).OrderBy(kvp => kvp.Value).FirstOrDefault();

            // If no lower or upper value is found, return NaN (or handle error accordingly)
            if (lower.Equals(default(KeyValuePair<double, double>)) || upper.Equals(default(KeyValuePair<double, double>)))
            {
                return double.NaN; // Out of range
            }

            // Interpolate between the lower and upper values to estimate temperature
            double slope = (upper.Key - lower.Key) / (upper.Value - lower.Value);
            double temperature = lower.Key + (pressure - lower.Value) * slope;

            return temperature;
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.BorderStyle = BorderStyle.None;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {

            if (!serialPort.IsOpen)
            {
                try
                {
                    Program.SetForm1(this);
                    // Configure the serial port settings
                    serialPort.PortName = comboBoxPort.SelectedItem.ToString(); // Set your COM port name 
                    serialPort.BaudRate = 9600;   // Set your baud rate
                    serialPort.DataBits = 8;      // Set your data bits
                    serialPort.Parity = Parity.None;
                    serialPort.StopBits = StopBits.One;

                    // Open the serial port
                    serialPort.Open();
                    Thread dataThread = new Thread(ReadAndUpdateData);
                    dataThread.Start();


                    BeginInvoke((MethodInvoker)delegate
                    {
                        labelConnectionStatus2.Text = "connected";
                        labelConnectionStatus2.ForeColor = Color.Green;
                        buttonOpenPort.Enabled = false;
                        buttonClosePort.Enabled = true;
                    });
                }
                catch (Exception ex)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        textBoxError.ForeColor = Color.Red;
                        textBoxError.Text = $"Port Error: {ex.Message}";
                        errorClearTimer.Stop();   // Restart the timer in case it's already running
                        errorClearTimer.Start();
                    });
                }
            }
            else
            {
                this.Invoke((MethodInvoker)delegate
                {
                    textBoxError.ForeColor = Color.Red;
                    textBoxError.Text = $"Error: ";
                    errorClearTimer.Stop();   // Restart the timer in case it's already running
                    errorClearTimer.Start();
                });
            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            // Toggle the auto request flag
            isAutoRequestEnabled = !isAutoRequestEnabled;

            // Enable or disable the timer based on the flag
            if (isAutoRequestEnabled)
            {
                autoRequestTimer.Start();
            }
            else
            {
                autoRequestTimer.Stop();
            }
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            SendModbusCommand();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            SendModbusCommandToThirdSlave();
            SendModbusCommandToFivethSlave();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            SendModbusCommandToSecondSlave();

        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (Program.Form2Instance == null)
            {
                Program.Form1Instance = new Form1();
                Program.Form1Instance.FormClosed += (s, args) => Program.Form1Instance = null;
            }

            // Open Form2
            Form2 form2Instance = new Form2();
            form2Instance.Show();
            this.Hide();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (form3Instance == null)
            {
                form3Instance = new Form3();
                form3Instance.FormClosed += (s, args) => form3Instance = null; // Handle Form2 closing
            }

            // Set data or perform any other initialization as needed

            form3Instance.Show();
            this.Hide();
        }



        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            serialPort.PortName = comboBoxPort.SelectedItem.ToString();
        }



        private void button15_Click(object sender, EventArgs e)
        {




            SaveDataToPdf();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort.PortName = comboBoxPort.SelectedItem.ToString();
        }
        private void RefreshComPorts()
        {
            // Get the current selection (if any)
            string selectedPort = comboBoxPort.SelectedItem?.ToString();

            // Clear the existing items
            comboBoxPort.Items.Clear();

            // Get the updated list of available COM ports
            string[] availablePorts = SerialPort.GetPortNames();

            // Add the new ports to the ComboBox
            comboBoxPort.Items.AddRange(availablePorts);

            // If the previously selected port is still available, select it
            if (availablePorts.Contains(selectedPort))
            {
                comboBoxPort.SelectedItem = selectedPort;
            }
            else if (availablePorts.Length > 0)
            {
                // Otherwise, select the first available port
                comboBoxPort.SelectedItem = availablePorts[0];
            }
        }
        private void button17_Click(object sender, EventArgs e)
        {
            RefreshComPorts();
        }
        private void CloseSerialPort()
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    serialPort.Close();
                    // Additional cleanup or notifications if needed
                }
                catch (Exception ex)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        textBoxError.ForeColor = Color.Red;
                        textBoxError.Text = $"Error: {ex.Message}";
                        errorClearTimer.Stop();   // Restart the timer in case it's already running
                        errorClearTimer.Start();
                    });
                }
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Close the serial port when the form is closing
            CloseSerialPort();
        }
        private void button18_Click(object sender, EventArgs e)
        {
            CloseSerialPort();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void absolute_conditions()
        {


            // Check the conditions for TextBox S1 and S2
            if (TwFahrenheit >= 67.0 && TwFahrenheit <= 67.9)
            {
                checkBox1.Checked = true; // Set CheckBox1 to true if condition is met
            }
            else
            {
                checkBox1.Checked = false; // Optionally reset CheckBox1
            }

            if (a2 >= 80.0 && a2 <= 80.9)
            {
                checkBox2.Checked = true; // Set CheckBox2 to true if condition is met
            }
            else
            {
                checkBox2.Checked = false; // Optionally reset CheckBox2
            }

            // Calculate enthalpy and update the TextBox
            double enthalpyActual = GetEnthalpy(67.0); // Get Enthalpy for 67.0
            double enthalpyS69 = GetEnthalpy(s2);   // Get Enthalpy for 69.0 (assuming s4 is 69)
            double tBtu = s5 * 4.45 * (enthalpyActual - enthalpyS69); // Calculate tBtu

            textBox2.Text = tBtu.ToString(); // Update TextBox2 with calculated tBtu
        }
        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label32_Click(object sender, EventArgs e)
        {

        }

        private void textBoxS4_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            SendModbusCommand();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            SendModbusCommandToSecondSlave();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            SendModbusCommandToThirdSlave();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            SendModbusCommandToFourthSlave();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            SendModbusCommandToFivethSlave();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button24_Click(object sender, EventArgs e)
        {

            errorBlinkTimer.Stop();
            isErrorBlinking = false;
            pictureBox1.Visible = false; // Hide the error indicator

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                stop = true;
                serialPort.Close();
                labelConnectionStatus.Text = "Disconnected";
                labelConnectionStatus.ForeColor = Color.Red;
                buttonOpenPort.Enabled = true;
                buttonClosePort.Enabled = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxPortList_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort.PortName = comboBoxPort.SelectedItem.ToString();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }
    }
}