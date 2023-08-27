using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NDRV3PsycheTaxiEditorLibrary
{
    public class Question
    {
        private List<Layout> Layout { get; set; }
        private List<Piece> Piece { get; set; }
        private String FileNumber;


        public Question(String question, String FileNumber)
        {
            CultureInfo customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ",";
            Thread.CurrentThread.CurrentCulture = customCulture;

            this.FileNumber = FileNumber;
            NDRV3PsycheTaxiEditorLibrary.Layout.currentID = 0;
            Layout = new List<Layout>();
            Piece = new List<Piece>();
            String[] questionSplit = question.Split(@"\n");
            if (questionSplit.Length <= 0 || questionSplit.Length > 2) throw new NotSupportedException();
            if (questionSplit.Length == 1)
            {
                GenerateLayout(question, 0, true);
            }
            else
            {
                GenerateLayout(questionSplit[0], -.5f, true);
                GenerateLayout(questionSplit[1], .5f, false);

                if (questionSplit[0].Length % 2 != 0) questionSplit[0] += ' ';
                question = questionSplit[0] + questionSplit[1];
            }

            GeneratePiece(question);

            
        }

        private void GenerateLayout(String text, float Y, bool firstLine)
        {
            int textPieceCount = (int)Math.Ceiling((float)text.Length / 2);
            float addedValue = (textPieceCount % 2 == 0) ? .5f : 0;

            float X = -(textPieceCount / 2) + addedValue;

            for(int i = 0; i < textPieceCount; i++)
            {
                String[] FileNumberSplit = FileNumber.Split('_');
                int TextNumber = (firstLine && i == 0) ? Int16.Parse(FileNumberSplit[0]) * 100 + Int16.Parse(FileNumberSplit[1]) : 0;
                Layout.Add(new Layout(NDRV3PsycheTaxiEditorLibrary.Layout.currentID++, TextNumber, X++, Y));
            }
        }

        private void GeneratePiece(String text)
        {
            if (text.Length % 2 != 0) text += ' ';

            for(int i = 0; i < text.Length / 2; i++)
            {
                Piece.Add(new Piece(text[i * 2].ToString() + (String)text[i * 2 + 1].ToString(), i));
            }
        }

        public void Shuffle()
        {
            Piece.Shuffle();
        }


        public void Save(String HTPath)
        {
            if(!Directory.Exists(@"tmp\psycheTaxi"))
            {
                Directory.CreateDirectory(@"tmp\psycheTaxi");
            }

            using(var stream = File.Create(@"tmp\psycheTaxi\layout" + FileNumber + ".dat.csv"))
            {
                using (var writer = new StreamWriter(stream, new UnicodeEncoding(false, true)))
                {
                    writer.WriteLine("\"位置 (LABEL)\",\"テキスト番号 (u16)\",\"Ｘ座標 (f32)\",\"Ｙ座標 (f32)\"");
                    foreach (var item in Layout)
                    {
                        writer.WriteLine(item.GetCSV());
                    }
                }
            }

            using (var stream = File.Create(@"tmp\psycheTaxi\piece" + FileNumber + ".dat.csv"))
            {
                using (var writer = new StreamWriter(stream, new UnicodeEncoding(false, true)))
                {
                    writer.WriteLine("\"文字 (UTF16)\",\"表示前 (UTF16)\",\"位置 (ASCII)\"");
                    foreach (var item in Piece)
                    {
                        writer.WriteLine(item.GetCSV());
                    }
                }
            }

            var packToDat = Process.Start(HTPath, @"dat pack -b tmp\psycheTaxi");
            packToDat.WaitForExit();

            File.Delete(@"tmp\psycheTaxi\layout" + FileNumber + ".dat.csv");
            File.Delete(@"tmp\psycheTaxi\piece" + FileNumber + ".dat.csv");

            if (!Directory.Exists(@"output\psycheTaxi"))
            {
                Directory.CreateDirectory(@"output\psycheTaxi");
            }

            File.Move(@"tmp\psycheTaxi\layout" + FileNumber + ".dat", @"output\psycheTaxi\layout" + FileNumber + ".dat", true);
            File.Move(@"tmp\psycheTaxi\piece" + FileNumber + ".dat", @"output\psycheTaxi\piece" + FileNumber + ".dat", true);
            if (File.Exists(@"output\psycheTaxi\readme.txt")) File.Delete(@"output\psycheTaxi\readme.txt");
            File.AppendAllText(@"output\psycheTaxi\readme.txt", @"Put these files in: \minigame\brain_drive\brain_drive_US.spc");
        }
    }
}
