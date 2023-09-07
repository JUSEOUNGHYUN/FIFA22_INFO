using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace FIFA22_INFO
{
    /// <summary>
    /// TeamNameTextBox.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TeamNameTextBox : UserControl
    {
        public TeamNameTextBox()
        {
            InitializeComponent();
        }

        private void TeamName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Text = textBox.Text.ToUpper();
            textBox.CaretIndex = textBox.Text.Length;
        }

        private void TeamName_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void TeamName_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z0-9\\s]+");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}
