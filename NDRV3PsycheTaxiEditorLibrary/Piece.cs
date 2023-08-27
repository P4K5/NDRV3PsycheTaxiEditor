using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDRV3PsycheTaxiEditorLibrary
{
    internal class Piece
    {
        public String Text { get; set; }
        public String TextHidden { get; set; }
        public int Order { get; set; }
        public Piece(String Text, int Order)
        {
            this.Text = Text;
            TextHidden = "□";
            this.Order = Order;
        }

        public String GetCSV()
        {
            return "\"" + Text + "\",\"" + TextHidden + "\",\"" + Order + "\"";
        }
    }
}
