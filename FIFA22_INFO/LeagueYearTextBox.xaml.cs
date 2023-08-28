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
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            string str = string.Empty;

            int N = LeagueYear_Textbox.Text.Length;
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
            if (n == 7)
            {
                string str1 = LeagueYear_Textbox.Text;

                int nCurrentIndex = LeagueYear_Textbox.CaretIndex;

                List<string> list = str1.Split('/').ToList();

                int nFirst = 0;
                int nLast = 0;

                string sFirst = "";
                string sLast = "";

                if (nCurrentIndex == 4 || nCurrentIndex == 3)
                {
                    //nFirst = int.Parse(list[0]);
                    nFirst = int.Parse(list[0].Substring(2, 2));
                    nLast = nFirst + 1;
                    if (nLast == 100)
                    {
                        nLast = 0;
                    }

                    LeagueYear_Textbox.Text = list[0] + "/" + nLast.ToString().PadLeft(2, '0');
                }
                else if (nCurrentIndex == 6 || nCurrentIndex == 7)
                {
                    int nAllFirst = int.Parse(list[0]);
                    string sYear = list[0].Substring(0, 2);
                    nLast = int.Parse(list[1]);
                    nFirst = nLast - 1;
                    if (nFirst == -1)
                    {
                        nFirst = 99;
                        int ndf = int.Parse(sYear) - 1;
                        sYear = ndf.ToString();
                    }

                    LeagueYear_Textbox.Text = sYear + nFirst.ToString().PadLeft(2, '0') + "/" + nLast.ToString().PadLeft(2, '0');
                }
            }
        }

        private void year_KeyEvent(object sender, KeyEventArgs e)
        {

        }

        private void year_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            List<string> list = new List<string>();

            string str = LeagueYear_Textbox.Text;

            if (e.Key == Key.Up)
            {
                if (LeagueYear_Textbox.Text == string.Empty)
                {
                    LeagueYear_Textbox.Text = "2020/21";
                }
                else
                {
                    list = LeagueYear_Textbox.Text.Split('/').ToList();

                    int nFirst = int.Parse(list[0]) + 1;
                    int nLast = int.Parse(list[1]) + 1;

                    if (nLast == 100)
                    {
                        nLast = 0;
                    }

                    LeagueYear_Textbox.Text = nFirst.ToString() + "/" + nLast.ToString().PadLeft(2, '0');
                }
            }
            else if (e.Key == Key.Down)
            {
                if (LeagueYear_Textbox.Text == string.Empty)
                {
                    LeagueYear_Textbox.Text = "2020/21";
                }
                else
                {
                    list = LeagueYear_Textbox.Text.Split('/').ToList();

                    int nFirst = int.Parse(list[0]) - 1;
                    int nLast = int.Parse(list[1]) - 1;

                    if (nLast == -1)
                    {
                        nLast = 99;
                    }

                    LeagueYear_Textbox.Text = nFirst.ToString() + "/" + nLast.ToString().PadLeft(2, '0');
                }
            }
        }

        public void GetLeagueYear(string str)
        {
            LeagueYear_Textbox.Text = str;
        }

        public string SetLeagueYear()
        {
            return LeagueYear_Textbox.Text;
        }

    }
}
