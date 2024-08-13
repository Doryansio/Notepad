using Notepad.Controls;
using Notepad.Objects;
using System.Linq;
using System.Windows.Forms;

namespace Notepad
{
    public partial class MainForm : Form
    {
        public RichTextBox CurrentRtb;
        public TabControl MainTabControl;
        public Session Session;
        public TextFile CurrentFile;

        public MainForm()
        {
            InitializeComponent();
            Session = new Session();


            var menuStrip = new MainMenuStrip();
            MainTabControl = new MainTabControl();

            CurrentRtb = new CustomRichTextBox();






            Controls.AddRange(new Control[] { MainTabControl, menuStrip });

            InitialiazeFile();
        }
        /// <summary>
        /// Permet de creer un onglet a l'ouverture de l'application
        /// S'il n'y en a pas.
        /// </summary>
        private void InitialiazeFile()
        {
            if (Session.TextFiles.Count == 0)
            {
                var file = new TextFile("Sans Titre 1");
                var rtb = new CustomRichTextBox();
                MainTabControl.TabPages.Add(file.SafeFileName);
                var tabPages = MainTabControl.TabPages[0];
                tabPages.Controls.Add(rtb);
                rtb.Select();

                Session.TextFiles.Add(file);
                CurrentFile = file;
                CurrentRtb = rtb;

            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Session.Save();

            foreach (var file in Session.TextFiles) 
            {
                var fileIndex = Session.TextFiles.IndexOf(file);
                var rtb = MainTabControl.TabPages[fileIndex].Controls.Find("RtbtextFilecontent", true).First();
                
                if(file.FileName.StartsWith("Sans Titre"))
                {
                    file.Content = rtb.Text;
                    Session.BackupFile(file);
                }
            }
        }
    }
}
