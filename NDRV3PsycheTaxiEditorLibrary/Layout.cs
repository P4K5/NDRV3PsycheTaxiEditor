using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDRV3PsycheTaxiEditorLibrary
{
    internal class Layout
    {
        public static int currentID { get; set; }
        public int ID { get; set; }
        public int TextNumber { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public Layout(int ID, int TextNumber, float X, float Y)
        {
            this.ID = ID;
            this.TextNumber = TextNumber;
            this.X = X;
            this.Y = Y;
        }

        public String GetCSV()
        {
            return "\"" + ID + "\",\"" + TextNumber + "\",\"" + X + "\",\"" + Y + "\"";
        }
    }
}
