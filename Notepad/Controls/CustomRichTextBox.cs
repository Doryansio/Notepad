using System.Drawing;
using System.Windows.Forms;

namespace Notepad.Controls
{
    public class CustomRichTextBox : RichTextBox
    {
        private const string NAME = "RtbtextFilecontent";
        public CustomRichTextBox()
        {
            Name = NAME;
            AcceptsTab = true; //permet d'accepter la tabulation dans le rtbtxtfilecontent
            Font = new Font("Arial", 12.0F, FontStyle.Regular);
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.None;
            ContextMenuStrip = new RichTextBoxContextMenuStrip(this);
        }
    }
}
