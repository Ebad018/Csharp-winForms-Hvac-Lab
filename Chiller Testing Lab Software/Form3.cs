using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chiller_Testing_Lab_Software
{
    public partial class Form3 : Form
    {
        
        private int YourInitialFormWidth;
        private int YourInitialFormHeight;
        private Form1 form1Instance;

        public Form3()
        {
            InitializeComponent();

            // Store the initial dimensions of the form
            YourInitialFormWidth = Width;
            YourInitialFormHeight = Height;

            // Subscribe to the Resize event
            Resize += MainForm_Resize;
        }
        //private void SaveSettings()
        //{
        //    try
        //    {
        //        double s1Offset = Properties.Settings.Default.S1OffSet;
        //        double s2Offset = Properties.Settings.Default.S2OffSet;
        //        double s3Offset = Properties.Settings.Default.S3OffSet;
        //        double s4Offset = Properties.Settings.Default.S4OffSet;
        //        double scOffset = Properties.Settings.Default.SCOffSet;
        //        double shOffset = Properties.Settings.Default.SHOffSet;
        //        double dlOffset = Properties.Settings.Default.DLOffSet;
        //        double llOffset = Properties.Settings.Default.LLOffSet;
        //        double a1Offset = Properties.Settings.Default.A1OffSet;
        //        double a2Offset = Properties.Settings.Default.A2OffSet;
        //        double a3Offset = Properties.Settings.Default.A3OffSet;
        //        double a4Offset = Properties.Settings.Default.A4OffSet;
        //        double a5Offset = Properties.Settings.Default.A5OffSet;
        //        double a6Offset = Properties.Settings.Default.A6OffSet;
        //        double a7Offset = Properties.Settings.Default.A7OffSet;
        //        double a8Offset = Properties.Settings.Default.A8OffSet;

        //        // Read values from the TextBoxes or other controls in Form3
        //        double s1Offset = double.Parse(textBoxS1Offset.Text); 
        //        double s2Offset = double.Parse(textBoxS2Offset.Text); 
        //        double s3Offset = double.Parse(textBoxS3Offset.Text);
        //        double s4Offset = double.Parse(textBoxS4Offset.Text);
        //        double scOffset = double.Parse(textBoxSCOffset.Text);
        //        double shOffset = double.Parse(textBoxSHOffset.Text);
        //        double dlOffset = double.Parse(textBoxDLOffset.Text);
        //        double llOffset = double.Parse(textBoxLLOffset.Text);
        //        double a1Offset = double.Parse(textBoxA1Offset.Text);
        //        double a2Offset = double.Parse(textBoxA2Offset.Text);
        //        double a3Offset = double.Parse(textBoxA3Offset.Text);
        //        double a4Offset = double.Parse(textBoxA4Offset.Text);
        //        double a5Offset = double.Parse(textBoxA5Offset.Text);
        //        double a6Offset = double.Parse(textBoxA6Offset.Text);
        //        double a7Offset = double.Parse(textBoxA7Offset.Text);
        //        double a8Offset = double.Parse(textBoxA8Offset.Text);

        //        // Modify the settings
                

                
        //        Properties.Settings.Default.S2OffSet = s2Offset;
        //        Properties.Settings.Default.S3OffSet = s3Offset;
        //        Properties.Settings.Default.S4OffSet = s4Offset;
        //        Properties.Settings.Default.SCOffSet = scOffset;
        //        Properties.Settings.Default.SHOffSet = shOffset;
        //        Properties.Settings.Default.DLOffSet = dlOffset;
        //        Properties.Settings.Default.LLOffSet = llOffset;
        //        Properties.Settings.Default.A1OffSet = a1Offset;
        //        Properties.Settings.Default.A2OffSet = a2Offset;
        //        Properties.Settings.Default.A3OffSet = a3Offset;
        //        Properties.Settings.Default.A4OffSet = a4Offset;
        //        Properties.Settings.Default.A5OffSet = a5Offset;
        //        Properties.Settings.Default.A6OffSet = a6Offset;
        //        Properties.Settings.Default.A7OffSet = a7Offset;
        //        Properties.Settings.Default.A8OffSet = a8Offset;
        //        // Repeat for other sensors...

        //        // Save the changes
        //        Properties.Settings.Default.Save();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log or display the exception
        //        MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

       
    

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

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (form1Instance == null)
            {
                form1Instance = new Form1();
                form1Instance.FormClosed += (s, args) => form1Instance = null; // Handle Form2 closing
            }

            // Set data or perform any other initialization as needed

            form1Instance.Show();
            this.Hide();
        }

        private void textBoxS4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //SaveSettings();
        }

        // Other methods and event handlers for your form...
    }
}