using Notepad.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Notepad.Controls
{
    public class TabControlContextMenuStrip : ContextMenuStrip
    {
        private const string NAME = "TabControlContextMenuStrip";
        private MainForm _form;
        public TabControlContextMenuStrip()
        {
            Name = NAME;

            var closeTab = new ToolStripMenuItem("fermer");
            var closeAllTabsExceptThis = new ToolStripMenuItem("fermer tout sauf ce fichier");
            var OpenFileInExplorer = new ToolStripMenuItem("Ouvrir le repertoire du fichier en cours dans l'explorateur");


            Items.AddRange(new ToolStripMenuItem[] { closeTab, closeAllTabsExceptThis, OpenFileInExplorer });

            HandleCreated += (s, e) =>
            {
                _form = SourceControl.FindForm() as MainForm;
            };

            closeTab.Click += (s, e) =>
            {
                var selectedTab = _form.MainTabControl.SelectedTab;

                _form.Session.TextFiles.Remove(_form.CurrentFile);

                if (_form.MainTabControl.TabCount > 1)
                {
                    _form.MainTabControl.TabPages.Remove(selectedTab);
                    var newIndex = _form.MainTabControl.TabCount - 1;
                    _form.MainTabControl.SelectedIndex = newIndex;
                    _form.CurrentFile = _form.Session.TextFiles[newIndex];
                }
                else
                {
                    var fileName = "Sans Titre 1";
                    var file = new TextFile(fileName);

                    _form.CurrentFile = file;
                    _form.CurrentRtb.Clear();

                    _form.MainTabControl.SelectedTab.Text= file.FileName;
                    _form.Session.TextFiles.Add(file);
                    _form.Text = "Sans Titre 1 - Notepad.NET";
                }
            };
            closeAllTabsExceptThis.Click += (s, e) =>
            {
                var fileToDelete = new List<TextFile>();
                if (_form.MainTabControl.TabCount > 1)
                {
                    TabPage selectedTab = _form.MainTabControl.SelectedTab;

                    // suppression des onglets qui ne correspondent pas a l'onglet selectionné
                    foreach (TabPage tabPages in _form.MainTabControl.TabPages) 
                    {
                        if(tabPages != selectedTab)
                        {
                            _form.MainTabControl.TabPages.Remove(tabPages);
                        }
                    }
                    foreach (var file in _form.Session.TextFiles) 
                    {
                        if(file != _form.CurrentFile)
                        {
                            fileToDelete.Add(file);
                        }
                    }
                    _form.Session.TextFiles = _form.Session.TextFiles.Except(fileToDelete).ToList();
                }
            };
        }
    }
}
