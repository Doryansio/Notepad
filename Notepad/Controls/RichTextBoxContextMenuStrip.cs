using System.Windows.Forms;

namespace Notepad.Controls
{

    public class RichTextBoxContextMenuStrip : ContextMenuStrip
    {
        private RichTextBox _RichTextBox;
        private const string NAME = "RtbContextMenuStrip";
        public RichTextBoxContextMenuStrip(RichTextBox richTextBox)
        {
            _RichTextBox = richTextBox;
            Name = NAME;

            var Cut = new ToolStripMenuItem("Couper");
            var Copy = new ToolStripMenuItem("Copier");
            var Paste = new ToolStripMenuItem("Coller");
            var SelectAll = new ToolStripMenuItem("Selectionner tout");

            Cut.Click += (s, e) => _RichTextBox.Cut();
            Copy.Click += (s, e) => _RichTextBox.Copy();
            Paste.Click += (s, e) => _RichTextBox.Paste();
            SelectAll.Click += (s, e) => _RichTextBox.SelectAll();

            Items.AddRange(new ToolStripMenuItem[] { Cut, Copy, Paste, SelectAll });
        }
    }
}
