﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad.Controls
{
    class MainMenuStrip : MenuStrip
    {
        private const string NAME = "MainMenuStrip";
        private FontDialog _fontDialog;
        public MainMenuStrip()
        {
            Name = NAME; // sert a identifier le menu pour y faire reference dans d'autre classe.
            Dock = DockStyle.Top;   // Sert a ancrer le control. Au top dans ce cas la.

            _fontDialog = new FontDialog();

            FileDropDownMenu();
            EditDropDownMenu();
            FormatDropDownMenu();
            ViewDropDownMenu();

        }
        public void FileDropDownMenu()
        {
            // instanciation de la classe systeme ToolStripMenuItem avec le nom FileDropDownMenu. Utilisation du kw var possible.

            var FileDropDownMenu = new ToolStripMenuItem("Fichier");

            var NewMenu = new ToolStripMenuItem("Nouveau", null, null, Keys.Control | Keys.N);
            var OpenMenu = new ToolStripMenuItem("Ouvrir...", null, null, Keys.Control | Keys.O);
            var SaveMenu = new ToolStripMenuItem("Enregistrer", null, null, Keys.Control | Keys.S);
            var SaveAsMenu = new ToolStripMenuItem("Enregistrer sous...", null, null, Keys.Control | Keys.Shift | Keys.N);
            var QuitMenu = new ToolStripMenuItem("Quitter", null, null, Keys.Alt | Keys.F4);

            // Ajouter les items dans le ToolStripMenu sous forme de tableau grâce a AddRange.
            FileDropDownMenu.DropDownItems.AddRange(new ToolStripItem[] { NewMenu, OpenMenu, SaveMenu, SaveAsMenu, QuitMenu });

            Items.Add(FileDropDownMenu);
        }

        public void EditDropDownMenu()
        {
            var EditDropDown = new ToolStripMenuItem("Edition");

            var Cancel = new ToolStripMenuItem("Annuler", null, null, Keys.Control | Keys.Z);
            var Restore = new ToolStripMenuItem("Restaurer", null, null, Keys.Control | Keys.Y);

            Cancel.Click += (s, e) => { if (MainForm.RichTextBox.CanUndo) MainForm.RichTextBox.Undo(); };
            Restore.Click += (s, e) => { if (MainForm.RichTextBox.CanRedo) MainForm.RichTextBox.Redo(); };

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
                _fontDialog.Font = MainForm.RichTextBox.Font;
                _fontDialog.ShowDialog();

                MainForm.RichTextBox.Font = _fontDialog.Font;
            };

            FormatDropDown.DropDownItems.AddRange(new ToolStripItem[] { Font});

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
                if (MainForm.RichTextBox.ZoomFactor < 3F) 
                {
                    MainForm.RichTextBox.ZoomFactor += 0.3F;
                }
            };

            ZoomOut.Click += (s, e) =>
            {
                if (MainForm.RichTextBox.ZoomFactor > 0.9F)
                {
                    MainForm.RichTextBox.ZoomFactor -= 0.3F;
                }
            };

            ZoomReset.Click += (s, e) => { MainForm.RichTextBox.ZoomFactor = 1F;  };

            ZoomDropDown.DropDownItems.AddRange(new ToolStripItem[] {ZoomIn, ZoomOut, ZoomReset});
            ViewDropDown.DropDownItems.AddRange(new ToolStripItem[] { AlwaysOnTop, ZoomDropDown});

            Items.Add(ViewDropDown);
        }
    }
}
