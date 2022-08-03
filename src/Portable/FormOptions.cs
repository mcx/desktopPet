﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopPet
{
        /// <summary>
        /// Application options. Need a redesign, so it is not documented.
        /// </summary>
        /// <preliminary/>
    public partial class FormOptions : Form
    {
            /// <summary>
            /// Constructor
            /// </summary>
        public FormOptions()
        {
            InitializeComponent();
        }

            /// <summary>
            /// Restore default animation. Will restore the animation delivered with the app.
            /// </summary>
            /// <param name="sender">Caller object.</param>
            /// <param name="e">Click event values.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Retry;
            Close();
        }
        
            /// <summary>
            /// New page was loaded. Check if page starts with the -XML- key. If so, the page will be converted to an xml.
            /// </summary>
            /// <param name="sender">Caller as object.</param>
            /// <param name="e">Webpage event values.</param>
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser web = (WebBrowser)sender;
            string s = web.DocumentText;
            if(s.Substring(0, 5) == "-XML-")
            {
                Program.MyData.SetXml(s.Substring(5), "");
                Program.Mainthread.LoadNewXMLFromString(s.Substring(5));
                Close();
            }
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush _textBrush;
            
            // Get the item from the collection.
            TabPage _tabPage = tabControl1.TabPages[e.Index];

            // Use our own font.
            Font _tabFont;


            if (e.State == DrawItemState.Selected)
            {
                // Draw a different background colour, and don't paint a focus rectangle.
                _textBrush = new SolidBrush(Color.Black);
                g.FillRectangle(Brushes.White, e.Bounds);
                _tabFont = new Font(tabControl1.TabPages[e.Index].Font.FontFamily.ToString(), (float)11.0, FontStyle.Bold, GraphicsUnit.Pixel);
            }
            else
            {
                _textBrush = new SolidBrush(Color.Black);
                g.FillRectangle(Brushes.LightGray, e.Bounds);
                _tabFont = new Font(tabControl1.TabPages[e.Index].Font.FontFamily.ToString(), (float)10.0, FontStyle.Regular, GraphicsUnit.Pixel);
            }
            
            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_tabPage.Text, _tabFont, _textBrush, tabControl1.GetTabRect(e.Index), _stringFlags);
        }

        private void FormOptions_Load(object sender, EventArgs e)
        {
                // Set up audio values
            checkBox1.Checked = (Program.MyData.GetVolume() > 0.0);
			trackBar1.Value = (int)(Program.MyData.GetVolume() * 10);
            trackBar1.Enabled = checkBox1.Checked;
			label2.Text = Program.Mainthread.ErrorMessages.AudioErrorMessage;
            if (label2.Text.Length > 1)
            {
                trackBar1.Enabled = false;
                checkBox1.Enabled = false;
            }
			checkBox2.Checked = Program.MyData.GetWindowForeground();
            checkBox4.Checked = Program.MyData.GetStealTaskbarFocus();
            trackBar2.Value = Program.MyData.GetAutoStartPets();
            trackBar3.Value = (int)(Program.MyData.GetScale());
            label5.Text = trackBar2.Value.ToString();
            label2.Text = trackBar1.Value.ToString();
            label9.Text = Math.Pow(2, (trackBar3.Value - 1)).ToString() + "x";
            checkBox3.Checked = Program.MyData.GetMultiscreen();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            trackBar1.Enabled = checkBox1.Checked;
            if(!trackBar1.Enabled)
            {
                trackBar1.Value = 0;
                trackBar1_Scroll(sender, e);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Program.MyData.SetVolume((float)(trackBar1.Value / 10.0));
            if(Program.MyData.GetVolume() < 0.1f)
            {
                trackBar1.Enabled = false;
                checkBox1.Checked = false;
            }
            label2.Text = trackBar1.Value.ToString();
        }

		private void checkBox2_Click(object sender, EventArgs e)
		{
            Program.MyData.SetWindowForeground(checkBox2.Checked);
		}

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Program.MyData.SetStealTaskbarFocus(checkBox4.Checked);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
		{
            Program.MyData.SetAutoStartPets(trackBar2.Value);
            label5.Text = trackBar2.Value.ToString();
		}

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Program.MyData.SetMultiscreen(checkBox3.Checked);
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            Program.MyData.SetScale(trackBar3.Value);
            label9.Text = Math.Pow(2, (trackBar3.Value - 1)).ToString() + "x";

            using (var petProcess = new Process())
            {
                petProcess.StartInfo.FileName = Application.ExecutablePath;
                petProcess.Start();
            }

            Hide();
            Application.Exit();
        }
    }
}
