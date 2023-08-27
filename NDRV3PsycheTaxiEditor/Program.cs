using NDRV3PsycheTaxiEditorLibrary;
using System.Globalization;

namespace NDRV3PsycheTaxiEditor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            

            var test = new Question(@"How many Monopads did Rantaro\nhave in the library?", "06_0");
            test.Shuffle();
            test.Save(@"C:\Program Files\HarmonyTools\HarmonyTools.exe");
        }
    }
}