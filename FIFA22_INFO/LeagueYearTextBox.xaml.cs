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

namespace FIFA22_INFO
{
    /// <summary>
    /// LeagueYearTextBox.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LeagueYearTextBox : UserControl
    {
        public LeagueYearTextBox()
        {
            InitializeComponent();
        }

        private void YEAR_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void year_TextChanged(object sender, TextChangedEventArgs e)
        {
            int n = LeagueYear_Textbox.Text.Length;
            string str = string.Empty;

            if (n == 4)
            {
                str = LeagueYear_Textbox.Text.Substring(2, 2);
                int df = int.Parse(str) + 1;
                if (df == 100)
                {
                    df = 0;
                }

                LeagueYear_Textbox.Text += "/" + df.ToString().PadLeft(2, '0');
            }
        }
    }
}
