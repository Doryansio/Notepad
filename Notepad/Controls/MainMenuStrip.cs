using System;
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
        public MainMenuStrip()
        {
            Name = NAME; // sert a identifier le menu pour y faire reference dans d'autre classe.
            Dock = DockStyle.Top;   // Sert a ancrer le control. Au top dans ce cas la.

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
            var EditDropDownMenu = new ToolStripMenuItem("Edition");

            var CancelMenu = new ToolStripMenuItem("Annuler", null, null, Keys.Control | Keys.Z);
            var RestoreMenu = new ToolStripMenuItem("Restaurer", null, null, Keys.Control | Keys.Y);

            EditDropDownMenu.DropDownItems.AddRange(new ToolStripItem[] { CancelMenu, RestoreMenu });

            Items.Add(EditDropDownMenu);
        }

        public void FormatDropDownMenu()
        {
            var FormatDropDownMenu = new ToolStripMenuItem("Format");

            var FontMenu = new ToolStripMenuItem("Police...");
            

            FormatDropDownMenu.DropDownItems.AddRange(new ToolStripItem[] { FontMenu});

            Items.Add(FormatDropDownMenu);
        }

        public void ViewDropDownMenu()
        {
            var ViewDropDownMenu = new ToolStripMenuItem("Affichage");

            var AlwaysOnTop = new ToolStripMenuItem("Toujours devant");

            var ZoomDropDownMenu = new ToolStripMenuItem("Zoom");

            var ZoomInMenu = new ToolStripMenuItem("Zoom avant", null, null, Keys.Control | Keys.Add);
            var ZoomOutMenu = new ToolStripMenuItem("Zoom arriere", null, null, Keys.Control | Keys.Subtract);
            var ZoomReset = new ToolStripMenuItem("Restaurer le zoom par defaut", null, null, Keys.Control | Keys.Divide);

            //ShortcutKeyDisplayString permet de changer le string du racourci
            ZoomInMenu.ShortcutKeyDisplayString = "Ctrl+Num + ";
            ZoomOutMenu.ShortcutKeyDisplayString = "Ctrl+Num -";
            ZoomReset.ShortcutKeyDisplayString = "Ctrl+Num /";

            ZoomDropDownMenu.DropDownItems.AddRange(new ToolStripItem[] {ZoomInMenu, ZoomOutMenu, ZoomReset});
            ViewDropDownMenu.DropDownItems.AddRange(new ToolStripItem[] { AlwaysOnTop, ZoomDropDownMenu});

            Items.Add(ViewDropDownMenu);
        }
    }
}
