using Notepad.Controls;
using Notepad.Objects;
using System.IO;
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
            //Sert a initialiser le form doit se placer en premier dans la classe main form
            InitializeComponent();
            Session = new Session();
            var menuStrip = new MainMenuStrip();
            MainTabControl = new MainTabControl();
            CurrentRtb = new CustomRichTextBox();
            


            Controls.AddRange(new Control[] { MainTabControl, menuStrip, });

            InitialiazeFile();
        }
        /// <summary>
        /// Permet de creer un onglet a l'ouverture de l'application
        /// S'il n'y en a pas.
        /// </summary>
        private async void InitialiazeFile()
        {
            Session = await Session.Load();


            if (Session.TextFiles.Count == 0)
            {
                var file = new TextFile("Sans Titre 1");


                MainTabControl.TabPages.Add(file.SafeFileName);
                var tabPages = MainTabControl.TabPages[0];
                var rtb = new CustomRichTextBox();
                tabPages.Controls.Add(rtb);
                rtb.Select();

                Session.TextFiles.Add(file);
                CurrentFile = file;
                CurrentRtb = rtb;

            }
            else
            {
                var activeIndex = Session.ActiveIndex;


                foreach (var file in Session.TextFiles)
                {
                    if (File.Exists(file.FileName) || File.Exists(file.BackUpFileName))
                    {
                        var rtb = new CustomRichTextBox();
                        var tabCount = MainTabControl.TabCount;

                        MainTabControl.TabPages.Add(file.SafeFileName);
                        MainTabControl.TabPages[tabCount].Controls.Add(rtb);

                        rtb.Text = file.Content;

                    }
                }
                CurrentFile = Session.TextFiles[activeIndex];
                MainTabControl.SelectedIndex = activeIndex;
                CurrentRtb = (CustomRichTextBox)MainTabControl.TabPages[activeIndex].Controls.Find("RtbtextFilecontent", true).First();

                CurrentRtb.Select();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            Session.ActiveIndex = MainTabControl.SelectedIndex;
            Session.Save();
            

            foreach (var file in Session.TextFiles)
            {
                var fileIndex = Session.TextFiles.IndexOf(file);
                var rtb = MainTabControl.TabPages[fileIndex].Controls.Find("RtbtextFilecontent", true).First();

                if (file.FileName.StartsWith("Sans Titre"))
                {
                    file.Content = rtb.Text;
                    Session.BackupFile(file);
                }
            }

        }
    }
}
