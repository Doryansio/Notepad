using Microsoft.VisualBasic;
using Notepad.Objects;
using Notepad.Services;
using System;
using System.ComponentModel.Design;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static Notepad.Controls.CustomRichTextBox;


namespace Notepad.Controls
{

    /// <summary>
    /// Classe herité de MenuStrip de Windows Forms
    /// </summary>
    class MainMenuStrip : MenuStrip
    {
        private const string NAME = "MainMenuStrip";
        public MainTabControl TabControl;
        private FontDialog _fontDialog;
        private MainForm _form;
        private OpenFileDialog _openFileDialog;
        private SaveFileDialog _saveFileDialog;
        private SaveFileDialog _renameFileDialog;
        





        /// <summary>
        /// Constructeur de la Classe MainMenuStrip
        /// </summary>
        public MainMenuStrip()
        {
            
            Name = NAME; // sert a identifier le menu pour y faire reference dans d'autre classe.
            Dock = DockStyle.Top;   // Sert a ancrer le control. Au top dans ce cas la.

            _fontDialog = new FontDialog();
            _openFileDialog = new OpenFileDialog()
            ;
            _saveFileDialog = new SaveFileDialog();

            //instanciation de la classe SFD avec le titre renommer.
            // + faire en sorte que lors de renommage du fichier l'ancien nom soit dans le dialog.
            _renameFileDialog = new SaveFileDialog();
            _renameFileDialog.Title = "Renommer";
            






            FileDropDownMenu();
            EditDropDownMenu();
            FormatDropDownMenu();
            ViewDropDownMenu();

            HandleCreated += (s, e) =>
            {
                _form = FindForm() as MainForm;
            };

        }

        /// <summary>
        /// Menu deroulant de la partie "Fichier" du ToolStripMenuItem
        /// </summary>
        public void FileDropDownMenu()
        {

            //Instanciation de la classe FileDropMenu
            var FileDropDownMenu = new ToolStripMenuItem("Fichier");

            var NewMenu = new ToolStripMenuItem("Nouveau", null, null, Keys.Control | Keys.N);
            var OpenMenu = new ToolStripMenuItem("Ouvrir...", null, null, Keys.Control | Keys.O);
            var SaveMenu = new ToolStripMenuItem("Enregistrer", null, null, Keys.Control | Keys.S);
            var SaveAsMenu = new ToolStripMenuItem("Enregistrer sous...", null, null, Keys.Control | Keys.Shift | Keys.S);
            var RenameMenu = new ToolStripMenuItem("Renommer..."); /// Renommer un fichier ?
            var QuitMenu = new ToolStripMenuItem("Quitter", null, null, Keys.Alt | Keys.F4);


            // evenement permettant de creer un nouveau fichier texte.
            NewMenu.Click += (s, e) =>
            {
                var tabControl = _form.MainTabControl;
                var tabCount = tabControl.TabCount;

                var FileName = $"Sans Titre {tabCount + 1}";
                var File = new TextFile(FileName);
                var rtb = new CustomRichTextBox();


                tabControl.TabPages.Add(File.SafeFileName);
                var newTabPages = tabControl.TabPages[tabCount];
                newTabPages.Controls.Add(rtb);

                _form.Session.TextFiles.Add(File);
                tabControl.SelectedTab = newTabPages;


                _form.CurrentFile = File;
                _form.CurrentRtb = rtb;
            };


            // event permettant d'ouvrir un fichier deja existant. Utilise la Classe privée OpenFileDialog pour acceder au fichier sur le disque.
            OpenMenu.Click += async (s, e) =>
            {
                if (_openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var tabControl = _form.MainTabControl;
                    var tabCount = tabControl.TabCount;

                    var file = new TextFile(_openFileDialog.FileName);
                    var rtb = new CustomRichTextBox();

                    _form.Text = $"{file.FileName} - Notepad.NET";

                    using (StreamReader reader = new StreamReader(file.FileName))
                    {
                        file.Content = await reader.ReadToEndAsync();
                    }
                    rtb.Text = file.Content;

                    tabControl.TabPages.Add(file.SafeFileName);
                    tabControl.TabPages[tabCount].Controls.Add(rtb);

                    _form.Session.TextFiles.Add(file);
                    _form.CurrentFile = file;
                    _form.CurrentRtb = rtb;
                    tabControl.SelectedTab = tabControl.TabPages[tabCount];
                }
            };

            SaveMenu.Click += async (s, e) =>
            {
                var currentFile = _form.CurrentFile;
                var currentRtbText = _form.CurrentRtb.Text;

                if (currentFile.Content != currentRtbText)
                {
                    if (File.Exists(currentFile.FileName))
                    {
                        using (StreamWriter writer = File.CreateText(currentFile.FileName))
                        {
                            await writer.WriteAsync(currentRtbText);
                        }
                        currentFile.Content = currentRtbText;
                        _form.Text = currentFile.FileName;
                        _form.MainTabControl.SelectedTab.Text = currentFile.SafeFileName;
                        MessageBox.Show("sauvegarde faite");
                        MessageBox.Show(currentRtbText);
                    }
                    else
                    {
                        SaveAsMenu.PerformClick();
                    }
                }
            };

            // event permettant d'ouvrir un fichier deja existant. Utilise la Classe privée SaveFileDialog pour acceder au fichier sur le disque.
            SaveAsMenu.Click += async (s, e) =>
            {
                if (_saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    
                    var currentFile = _form.CurrentFile.FileName;
                    var newFileName = _saveFileDialog.FileName;
                    var alreadyExists = false;

                    foreach (var file in _form.Session.TextFiles)
                    {
                        if (file.FileName == newFileName)
                        {
                            MessageBox.Show("ce fichier est deja ouvert dans Notepad.NET", "ERREUR", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            alreadyExists = true;
                            break;
                        }
                    }
                    //si le fichier n'existe pas deja
                    if (!alreadyExists)
                    {
                        var file = new TextFile(newFileName) { Content = _form.CurrentRtb.Text };
                        var oldFile = _form.Session.TextFiles.Where(x => x.FileName == _form.CurrentFile.FileName).First();

                        _form.Session.TextFiles.Replace(oldFile, file);

                        using (StreamWriter writer = File.CreateText(file.FileName))
                        {
                            await writer.WriteAsync(file.Content);
                        }

                        _form.MainTabControl.SelectedTab.Text = file.SafeFileName;
                        _form.Text = file.FileName;
                        _form.CurrentFile = file;
                    }


                }
            };

            /* Methode a refaire 
             la modification du fichier enleve le Path dans le fihier session de %app data% et ne renome
            pas le fichier dans dir NotePad 
            + Voir pour que le boutton renommerne soit pas enable si le fichier n'est pas sur le disque*/

            RenameMenu.Click +=  (s, e) =>
            {
                if(_renameFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var OldFileName = _form.CurrentFile;
                    var RenamedFileName = _renameFileDialog.FileName;
                    var alreadyExist = false;

                    foreach (var file in _form.Session.TextFiles)
                    {
                        if(OldFileName.Equals(RenamedFileName))
                        {
                            MessageBox.Show("Veuillez renommer le fichier avec un nom different.", "ERROR", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            alreadyExist = true;
                            break;
                        }
                    }
                    {

                    }

                    
                }
            };

            /* Methode a revoir
             enregister le travail si le fichier existe deja 
            et enregistrer sous si non 
            actuellement dans le deux cas la savefilediaog s'ouvre*/

            QuitMenu.Click += (s, e) =>
            {
               var response =  MessageBox.Show("Enregistrer votre travail ? ", "NotePad.Net", MessageBoxButtons.OKCancel);

                switch (response)
                {
                    case DialogResult.OK:
                        if (!File.Exists(_form.CurrentFile.ToString()))
                        {
                            SaveAsMenu.PerformClick();
                        }
                        else
                        {
                            SaveMenu.PerformClick();
                        }
                        break;
                    case DialogResult.Cancel:
                        Application.Exit();
                        break;
                    

                }
                
            };

            // Ajouter les items dans le ToolStripMenu sous forme de tableau grâce a AddRange.
            FileDropDownMenu.DropDownItems.AddRange(new ToolStripItem[] { NewMenu, OpenMenu, SaveMenu, SaveAsMenu, RenameMenu, QuitMenu });

            Items.Add(FileDropDownMenu);
        }

        private void OpenMenu_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void EditDropDownMenu()
        {
            var rtb = new CustomRichTextBox();
            var EditDropDown = new ToolStripMenuItem("Edition");

            var Cancel = new ToolStripMenuItem("Annuler", null, null, Keys.Control | Keys.Z);
            var Restore = new ToolStripMenuItem("Restaurer", null, null, Keys.Control | Keys.Y);
            var Goto = new ToolStripMenuItem("Go to ...", null, null, Keys.Control | Keys.G);

            Cancel.Click += (s, e) => { if (_form.CurrentRtb.CanUndo) _form.CurrentRtb.Undo(); };
            Restore.Click += (s, e) => { if (_form.CurrentRtb.CanRedo) _form.CurrentRtb.Redo(); };
            Goto.Click += (s, e) => 
            {
                
                string input = Interaction.InputBox("Ligne n°", "Go to", "1");
                try
                {
                    int line = Convert.ToInt32(input);
                    if (line > _form.CurrentRtb.Lines.Length)
                    {
                        MessageBox.Show("le nombre total de ligne dans le fichier est de " + _form.CurrentRtb.Lines.Length, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string[] lines = _form.CurrentRtb.Lines;
                        int len = 0;
                        for (int i = 0; i < line - 1; i++)
                        {
                            len = len + lines[i].Length+1;
                        }
                        _form.CurrentRtb.Focus();
                        _form.CurrentRtb.Select(len, 0);
                    }
                    
                        
                }catch (Exception ex)
                {
                    MessageBox.Show("Saisissez un nombre", "Entrée invalide", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };


            EditDropDown.DropDownItems.AddRange(new ToolStripItem[] { Cancel, Restore, Goto });

            Items.Add(EditDropDown);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void FormatDropDownMenu()
        {
            var FormatDropDown = new ToolStripMenuItem("Format");

            var Font = new ToolStripMenuItem("Police...");

            Font.Click += (s, e) =>
            {
                _fontDialog.Font = _form.CurrentRtb.Font;
                _fontDialog.ShowDialog();

                _form.CurrentRtb.Font = _fontDialog.Font;
            };

            FormatDropDown.DropDownItems.AddRange(new ToolStripItem[] { Font });

            Items.Add(FormatDropDown);
        }

        public void ViewDropDownMenu()
        {
            var ViewDropDown = new ToolStripMenuItem("Affichage");
            var AlwaysOnTop = new ToolStripMenuItem("Toujours devant");

            var ZoomDropDown = new ToolStripMenuItem("Zoom");
            var ZoomIn = new ToolStripMenuItem("Zoom avant", null, null, Keys.Control | Keys.Add);
            var ZoomOut = new ToolStripMenuItem("Zoom arriere", null, null, Keys.Control | Keys.Subtract);
            var ZoomReset = new ToolStripMenuItem("Restaurer le zoom par defaut", null, null, Keys.Control | Keys.Divide);

            //ShortcutKeyDisplayString permet de changer le string du racourci
            ZoomIn.ShortcutKeyDisplayString = "Ctrl+Num + ";
            ZoomOut.ShortcutKeyDisplayString = "Ctrl+Num -";
            ZoomReset.ShortcutKeyDisplayString = "Ctrl+Num /";

            AlwaysOnTop.Click += (s, e) =>
            {

                if (AlwaysOnTop.Checked)
                {
                    AlwaysOnTop.Checked = false;
                    Program.MainForm.TopMost = false;
                }
                else
                {
                    AlwaysOnTop.Checked = true;
                    Program.MainForm.TopMost = true;
                }
            };

            ZoomIn.Click += (s, e) =>
            {
                if (_form.CurrentRtb.ZoomFactor < 3F)
                {
                    _form.CurrentRtb.ZoomFactor += 0.3F;
                }
            };

            ZoomOut.Click += (s, e) =>
            {
                if (_form.CurrentRtb.ZoomFactor > 0.9F)
                {
                    _form.CurrentRtb.ZoomFactor -= 0.3F;
                }
            };

            ZoomReset.Click += (s, e) => { _form.CurrentRtb.ZoomFactor = 1F; };

            ZoomDropDown.DropDownItems.AddRange(new ToolStripItem[] { ZoomIn, ZoomOut, ZoomReset });
            ViewDropDown.DropDownItems.AddRange(new ToolStripItem[] { AlwaysOnTop, ZoomDropDown });

            Items.Add(ViewDropDown);
        }
    }
}
