using Notepad.Controls;
using Notepad.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad
{
    public partial class MainForm : Form
    {
        public RichTextBox CurrentRtb;
        public TabControl MainTabControl;
        public Session Session;
        public TextFile CurentFile;

        public MainForm()
        {
            InitializeComponent();
            
            
            var menuStrip = new MainMenuStrip();
            MainTabControl = new MainTabControl();
            Session = new Session();
            CurrentRtb = new CustomRichTextBox();
            
            

            TextFile file = new TextFile("D:/test.txt");
            

            Controls.AddRange(new Control[] {MainTabControl, menuStrip });
        }
    }
}
