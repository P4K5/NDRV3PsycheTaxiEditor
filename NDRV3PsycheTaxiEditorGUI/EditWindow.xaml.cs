using NDRV3PsycheTaxiEditorLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NDRV3PsycheTaxiEditorGUI
{
    /// <summary>
    /// Logika interakcji dla klasy EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private String FileName;
        private String HTPath;
        public EditWindow(String FileName)
        {
            InitializeComponent();

            this.FileName = FileName;
            HTPath = File.ReadAllText("HTPath.txt");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            String json = File.ReadAllText("questions_US.json");
            var jsonDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            questionText.Text = jsonDictionary[FileName];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var question = new Question(questionText.Text, FileName);
                if (FileName != "06_4") question.Shuffle();
                question.Save(HTPath);
                Process.Start("explorer.exe", @"output\psycheTaxi");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Close();
            }
        }
    }
}
