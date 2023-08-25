using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
using System.Windows.Shapes;

namespace FIFA22_INFO
{
    /// <summary>
    /// EMIRATES_FA_CUP.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public class FACUP
    {
        public string FLeague_Year;
        public string FChampions;
        public string FSecond_Place;
        public string FRemark;

        public FACUP(string fLeague_Year, string fChampions, string fSecond_Place, string fRemark)
        {
            FLeague_Year = fLeague_Year;
            FChampions = fChampions;
            FSecond_Place = fSecond_Place;
            FRemark = fRemark;
        }
    }

    public class FACUP_RANKING
    {
        public string RRanking;
        public string RTeam_Name;
        public string RChampions_CNT;
        public string RRunnerUp_CNT;

        public FACUP_RANKING(string rRanking, string rTeam_Name, string rChampions_CNT, string rRunnerUp_CNT)
        {
            RRanking = rRanking;
            RTeam_Name = rTeam_Name;
            RChampions_CNT = rChampions_CNT;
            RRunnerUp_CNT = rRunnerUp_CNT;
        }
    }

    public partial class EMIRATES_FA_CUP : Window
    {
        public static List<FACUP> mFACUPList = new List<FACUP>();
        public static List<FACUP_RANKING> mFACUPRANKINGList = new List<FACUP_RANKING>();

        public string m_sTeamName = string.Empty;
        public string m_sOption = string.Empty;

        public EMIRATES_FA_CUP()
        {
            InitializeComponent();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ToMiniButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            SearchFunc();
        }

        // Set DataGrid Champions, Runner_Up UI
        private void SetDataGridUsingQueryResultList()
        {
            List<Premier_League_Data> uiList = new List<Premier_League_Data>();

            for (int i = 0; i < mFACUPList.Count; i++)
            {
                FACUP pr = mFACUPList[i];

                uiList.Add(new Premier_League_Data()
                {
                    League_Year = mFACUPList[i].FLeague_Year.Trim(),
                    ChampionsLOGO = mFACUPList[i].FChampions.Trim(),
                    Champions = mFACUPList[i].FChampions.Trim(),
                    Second_PlaceLOGO = mFACUPList[i].FSecond_Place.Trim(),
                    Second_Place = mFACUPList[i].FSecond_Place.Trim(),
                    Remark = mFACUPList[i].FRemark.Trim()
                });
            }

            grdEmployee.ItemsSource = uiList;
            grdEmployee.ScrollIntoView(grdEmployee.Items[grdEmployee.Items.Count - 1]);
        }

        // Set DataGrid Ranking UI
        private void SetDataGridRankingUI()
        {
            List<EnglandTotalData> uList = new List<EnglandTotalData>();

            for(int i=0; i<mFACUPRANKINGList.Count; i++)
            {
                FACUP_RANKING far = mFACUPRANKINGList[i];

                uList.Add(new EnglandTotalData()
                {
                    Ranking = int.Parse(mFACUPRANKINGList[i].RRanking.Trim()),
                    Team_Logo = mFACUPRANKINGList[i].RTeam_Name.Trim(),
                    Team_Name = mFACUPRANKINGList[i].RTeam_Name.Trim(),
                    Champions_CNT = mFACUPRANKINGList[i].RChampions_CNT.Trim(),
                    Second_Place_CNT = mFACUPRANKINGList[i].RRunnerUp_CNT.Trim()
                });
            }

            Ranking_DataGrid.ItemsSource = uList;
        }

        private void Row_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int seelctedIndex = ((DataGrid)sender).SelectedIndex;

            FACUP fa = mFACUPList[seelctedIndex];

            League_Year_textBox.Text = fa.FLeague_Year.ToString();
            League_Year2_textBox.Text = fa.FLeague_Year.ToLower();
            Champion_Name_Textbox.Text = fa.FChampions.ToString();
            Runner_UP_Textbox.Text = fa.FSecond_Place.ToString();
            Remark_Textbox.Text = fa.FRemark.ToString();

            NpgsqlConnection conn = null;
            try
            {
                conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();

                string sql = "select Count(*) from emirates_fa_cup t where t.champions = '" + Champion_Name_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year_textBox.Text + "';";

                NpgsqlCommand RankCmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader reader = RankCmd.ExecuteReader();

                string strWInsCount = string.Empty;

                while (reader.Read())
                {
                    strWInsCount = reader[0].ToString();
                }

                NumOfWins_Textbox.Text = strWInsCount;

                reader.Close();

                string Secsql = "select COUNT(*) from emirates_fa_cup T where T.second_place = '" + Runner_UP_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year2_textBox.Text + "';";

                NpgsqlCommand SecCmd = new NpgsqlCommand(Secsql, conn);
                NpgsqlDataReader secreader = SecCmd.ExecuteReader();

                string strSecCount = string.Empty;

                while (secreader.Read())
                {
                    strSecCount = secreader[0].ToString();
                }

                NumOfRunner_Up_Textbox_Copy.Text = strSecCount;

                secreader.Close();

                ImageData image = new ImageData();

                image.SECOND_TEAMNAME = Runner_UP_Textbox.Text.Trim();
                image.COUNT = strWInsCount;
                image.TEAMNAME = Champion_Name_Textbox.Text.Trim();

                BitmapImage bitmap = new BitmapImage(new Uri("Image/England_Team/" + image.TEAMNAME .ToString() + ".png", UriKind.Relative));
                ImageBrush brush = new ImageBrush(bitmap);
                imageRec.Fill = brush;

                BitmapImage runbit = new BitmapImage(new Uri("Image/England_Team/" + image.SECOND_TEAMNAME.ToLower() + ".png", UriKind.Relative));
                ImageBrush runbrush = new ImageBrush(runbit);
                Run_imageRec.Fill = runbrush;

                int nCount = int.Parse(image.COUNT);

                if(nCount >= 10 && nCount % 10 == 0)
                {
                    BitmapImage bit = new BitmapImage(new Uri("Image/Star/" + image.COUNT.ToString() + ".png", UriKind.Relative));
                    ImageBrush br = new ImageBrush(bit);
                    CountStarRec.Fill = br;
                }
                else
                {
                    BitmapImage bit = new BitmapImage(new Uri("Image/Star/Empty.png", UriKind.Relative));
                    ImageBrush br = new ImageBrush(bit);
                    CountStarRec.Fill = br;
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }

           

           

            /*
            BitmapImage bit = new BitmapImage(new Uri("Image/" + image.COUNT.ToString() + ".png", UriKind.Relative));
            ImageBrush br = new ImageBrush(bit);
            CountStarRec.Fill = br;
             */


        }

        private void SearchFunc()
        {
            NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);
            conn.Open();
            List<string> list = new List<string>();

            string sql = "select * from EMIRATES_FA_CUP ORDER BY LEAGUE_YEAR;";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            mFACUPList.Clear();

            while (reader.Read())
            {
                FACUP pr = new FACUP(reader[0].ToString(),
                    reader[1].ToString(),
                    reader[2].ToString(),
                    reader[3].ToString());

                mFACUPList.Add(pr);
            }

            SetDataGridUsingQueryResultList();

            reader.Close();

            // DataGrid에 있는 최선 정보 txt파일에 저장
            string sOption = "EMIRATES_FA_CUP";

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\" + sOption + ".txt";

            FileStream stream = File.Create(path);
            stream.Close();

            if (File.Exists(path))
            {
                for (int i = 0; i < mFACUPList.Count; i++)
                {
                    File.AppendAllText(path, mFACUPList[i].FLeague_Year.Trim() + "\n");
                    File.AppendAllText(path, mFACUPList[i].FChampions.Trim() + "\n");
                    File.AppendAllText(path, mFACUPList[i].FSecond_Place.Trim() + "\n");
                    File.AppendAllText(path, mFACUPList[i].FRemark.Trim() + "\n");
                }
            }

            string Ranksql = "select rank() over( order by t.emirates_fa_cup_winner_cnt desc) Ranking, t.team_name, t.emirates_fa_cup_winner_cnt , t.emirates_fa_cup_run_cnt  " +
                "from england_total t " +
                "where t.emirates_fa_cup_run_cnt != 0 OR t.emirates_fa_cup_winner_cnt != 0 " +
                "order by t.emirates_fa_cup_winner_cnt desc, " +
                "t.emirates_fa_cup_run_cnt desc;";

            NpgsqlCommand RankCmd = new NpgsqlCommand(Ranksql, conn);
            NpgsqlDataReader Rankreader = RankCmd.ExecuteReader();

            mFACUPRANKINGList.Clear();

            while (Rankreader.Read())
            {
                FACUP_RANKING far = new FACUP_RANKING(Rankreader[0].ToString(),
                    Rankreader[1].ToString(),
                    Rankreader[2].ToString(),
                    Rankreader[3].ToString());

                mFACUPRANKINGList.Add(far);
            }

            SetDataGridRankingUI();

            Rankreader.Close();
        }

        private void keyDown_Event(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.Close();
            }
            else if(e.Key == Key.S)
            {
                SearchFunc();
            }
            else if (e.Key == Key.C)
            {
                this.Close();
                CARABAO_CUP cf = new CARABAO_CUP();
                cf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                cf.Show();
            }
            else if( e.Key == Key.X) 
            {
                this.Close();
                Premier_League pl = new Premier_League();
                pl.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                pl.Show();
            }
        }

        private void Ranking_DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            FACUP_RANKING ccr = mFACUPRANKINGList[selectedIndex];

            string teamName = ccr.RTeam_Name.Trim();
            string sOption = "EMIRATES_FA_CUP";

            //DataPassProdCd(teamName);
            TeamCareer tc = new TeamCareer();
            tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tc.Show();
            tc.GetTeam(teamName, sOption);
        }

        private void Ranking_DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            FACUP_RANKING ccr = mFACUPRANKINGList[selectedIndex];

            string teamName = ccr.RTeam_Name.Trim();
            string sOption = "EMIRATES_FA_CUP";

            if(e.Key == Key.Enter)
            {
                //DataPassProdCd(teamName);
                TeamCareer tc = new TeamCareer();
                tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                tc.Show();
                tc.GetTeam(teamName, sOption);
            }
        }
    }
}
