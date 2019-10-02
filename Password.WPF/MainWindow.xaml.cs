using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Password.Core;

namespace Password.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PwndTester Tester;
        public MainWindow()
        {
            Tester = new PwndTester();
            InitializeComponent();
        }

        private void PasswordInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            PasswordHash.Text = Tester.Hash(PasswordInput.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fulloutput = CheckedFullOutput.IsChecked;
            var savetofile = CheckedSaveToFile.IsChecked;
            Output.Text = Tester.SendHTTP(Tester.FiveHash,fulloutput.Value,savetofile.Value);
        }
    }
}
