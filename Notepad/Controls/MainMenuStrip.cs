using Notepad.Objects;
using Notepad.Services;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Notepad.Controls
{
    class MainMenuStrip : MenuStrip
    {
        private const string NAME = "MainMenuStrip";

        private FontDialog _fontDialog;
        private MainForm _form;
        private OpenFileDialog _openFileDialog;
        private SaveFileDialog _saveFileDialog;
        public MainMenuStrip()
        {

            Name = NAME; // sert a identifier le menu pour y faire reference dans d'autre classe.
            Dock = DockStyle.Top;   // Sert a ancrer le control. Au top dans ce cas la.

            _fontDialog = new FontDialog();
            _openFileDialog = new OpenFileDialog();
            _saveFileDialog = new SaveFileDialog();

            FileDropDownMenu();
            EditDropDownMenu();
            FormatDropDownMenu();
            ViewDropDownMenu();

            HandleCreated += (s, e) =>
            {
                _form = FindForm() as MainForm;
            };

        }
        public void FileDropDownMenu()
        {
            // instanciation de la classe systeme ToolStripMenuItem avec le nom FileDropDownMenu. Utilisation du kw var possible.

            var FileDropDownMenu = new ToolStripMenuItem("Fichier");

            var NewMenu = new ToolStripMenuItem("Nouveau", null, null, Keys.Control | Keys.N);
            var OpenMenu = new ToolStripMenuItem("Ouvrir...", null, null, Keys.Control | Keys.O);
            var SaveMenu = new ToolStripMenuItem("Enregistrer", null, null, Keys.Control | Keys.S);
            var SaveAsMenu = new ToolStripMenuItem("Enregistrer sous...", null, null, Keys.Control | Keys.Shift | Keys.S);
            var QuitMenu = new ToolStripMenuItem("Quitter", null, null, Keys.Alt | Keys.F4);

            NewMenu.Click += (s, e) =>
            {
                var tabControl = _form.MainTabControl;
                var tabCount = tabControl.TabCount;

                var FileName = $"Sans filtre {tabCount + 1}";
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

            OpenMenu.Click += async (s, e) =>
            {
                if(_openFileDialog.ShowDialog() == DialogResult.OK)
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
                            await writer.WriteAsync(currentFile.Content);
                        }
                        currentFile.Content = currentRtbText;
                        _form.Text = currentFile.FileName;
                        _form.MainTabControl.SelectedTab.Text = currentFile.SafeFileName;
                    }
                    else
                    {
                        SaveAsMenu.PerformClick();
                    }
                }
            };

            SaveAsMenu.Click += async (s, e) =>
            {
                if (_saveFileDialog.ShowDialog() == DialogResult.OK)
                {
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

            QuitMenu.Click += (s, e) =>
            {
                Application.Exit();
            };

            // Ajouter les items dans le ToolStripMenu sous forme de tableau grâce a AddRange.
            FileDropDownMenu.DropDownItems.AddRange(new ToolStripItem[] { NewMenu, OpenMenu, SaveMenu, SaveAsMenu, QuitMenu });

            Items.Add(FileDropDownMenu);
        }

        private void OpenMenu_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void EditDropDownMenu()
        {
            var EditDropDown = new ToolStripMenuItem("Edition");

            var Cancel = new ToolStripMenuItem("Annuler", null, null, Keys.Control | Keys.Z);
            var Restore = new ToolStripMenuItem("Restaurer", null, null, Keys.Control | Keys.Y);

            Cancel.Click += (s, e) => { if (_form.CurrentRtb.CanUndo) _form.CurrentRtb.Undo(); };
            Restore.Click += (s, e) => { if (_form.CurrentRtb.CanRedo) _form.CurrentRtb.Redo(); };

            EditDropDown.DropDownItems.AddRange(new ToolStripItem[] { Cancel, Restore });

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
