using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatUI
{
    public static class TextWriter
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            //test of ths: http://stackoverflow.com/questions/1926264/color-different-parts-of-a-richtextbox-string
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 4;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
