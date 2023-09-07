using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// SERIE_A.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public class SERIE_A_LEAGUE
    {
        public string SELeague_Year;
        public string SEChampions;
        public string SESecond_Place;
        public string SEThird_Place;
        public string SEFourth_Place;
        public string SERemark;

        public SERIE_A_LEAGUE(string bLeague_Year, string bChampions, string bSecond_Place, string bThird_Place, string bFourth_Place, string bRemark)
        {
            SELeague_Year = bLeague_Year;
            SEChampions = bChampions;
            SESecond_Place = bSecond_Place;
            SEThird_Place = bThird_Place;
            SEFourth_Place = bFourth_Place;
            SERemark = bRemark;
        }
    }

    public class SERIE_A_LEAGUE_RANKING
    {
        public string SERRanking;
        public string SERTeam_Name;
        public string SERChampions_CNT;
        public string SERRunnerUp_CNT;
        public string SERThird_CNT;
        public string SERFourth_CNT;

        public SERIE_A_LEAGUE_RANKING(string sERRanking, string sERTeam_Name, string sERChampions_CNT, string sERRunnerUp_CNT, string sERThird_CNT, string sERFourth_CNT)
        {
            SERRanking = sERRanking;
            SERTeam_Name = sERTeam_Name;
            SERChampions_CNT = sERChampions_CNT;
            SERRunnerUp_CNT = sERRunnerUp_CNT;
            SERThird_CNT = sERThird_CNT;
            SERFourth_CNT = sERFourth_CNT;
        }
    }

    public partial class SERIE_A : Window
    {
        public static List<SERIE_A_LEAGUE> mSELEAGUEList = new List<SERIE_A_LEAGUE>();
        public static List<SERIE_A_LEAGUE_RANKING> mSERANKINGList = new List<SERIE_A_LEAGUE_RANKING>();

        public string m_sTeamName = string.Empty;
        public string m_sOption = string.Empty;

        public SERIE_A()
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

        private void SetDataGridUsingQueryResultList()
        {
            List<Other_League> uiList = new List<Other_League>();

            for (int i = 0; i < mSELEAGUEList.Count; i++)
            {
                uiList.Add(new Other_League()
                {
                    League_Year = mSELEAGUEList[i].SELeague_Year.Trim(),
                    ChampionsLOGO = mSELEAGUEList[i].SEChampions.Trim(),
                    Champions = mSELEAGUEList[i].SEChampions.Trim(),
                    Second_PlaceLOGO = mSELEAGUEList[i].SESecond_Place.Trim(),
                    Second_Place = mSELEAGUEList[i].SESecond_Place.Trim(),
                    Third_PlaceLOGO = mSELEAGUEList[i].SEThird_Place.Trim(),
                    Third_Place = mSELEAGUEList[i].SEThird_Place.Trim(),
                    Fourth_PlaceLOGO = mSELEAGUEList[i].SEFourth_Place.Trim(),
                    Fourth_Place = mSELEAGUEList[i].SEFourth_Place.Trim(),
                    Remark = mSELEAGUEList[i].SERemark.Trim(),
                });
            }
            grdEmployee.ItemsSource = uiList;
            grdEmployee.ScrollIntoView(grdEmployee.Items[grdEmployee.Items.Count - 1]);
        }

        private void SetDataGridRankingUI()
        {
            List<Other_League_Ranking_Data> uList = new List<Other_League_Ranking_Data>();

            for (int i = 0; i < mSERANKINGList.Count; i++)
            {
                SERIE_A_LEAGUE_RANKING lur = mSERANKINGList[i];

                uList.Add(new Other_League_Ranking_Data()
                {
                    Ranking = int.Parse(lur.SERRanking.Trim()),
                    Team_Logo = lur.SERTeam_Name.Trim(),
                    Team_Name = lur.SERTeam_Name.Trim(),
                    Champions_CNT = lur.SERChampions_CNT.Trim(),
                    Second_Place_CNT = lur.SERRunnerUp_CNT.Trim(),
                    Third_Place_CNT = lur.SERThird_CNT.Trim(),
                    Fourth_Place_CNT = lur.SERFourth_CNT.Trim()
                });
            }
            Ranking_DataGrid.ItemsSource = uList;
        }

        private void Row_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int seelctedIndex = ((DataGrid)sender).SelectedIndex;

            SERIE_A_LEAGUE cc = mSELEAGUEList[seelctedIndex];
            League_Year_textBox.Text = cc.SELeague_Year.ToString();
            League_Year2_textBox.Text = cc.SELeague_Year.ToLower();
            Champion_Name_Textbox.Text = cc.SEChampions.ToString();
            Runner_UP_Textbox.Text = cc.SESecond_Place.ToString();
            Remark_Textbox.Text = cc.SERemark.ToString();

            NpgsqlConnection conn = null;

            try
            {
                conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();

                string sql = "select Count(*) from SERIE_A t where t.champions = '" + Champion_Name_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year_textBox.Text + "';";

                NpgsqlCommand RankCmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader reader = RankCmd.ExecuteReader();

                string strWInsCount = string.Empty;

                while (reader.Read())
                {
                    strWInsCount = reader[0].ToString();
                }

                NumOfWins_Textbox.Text = strWInsCount;

                reader.Close();

                string Secsql = "select COUNT(*) from SERIE_A T where T.second_place = '" + Runner_UP_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year2_textBox.Text + "';";

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

                BitmapImage bitmap = new BitmapImage(new Uri("Image/SERIE_A_Team/" + image.TEAMNAME.ToString() + ".png", UriKind.Relative));
                ImageBrush brush = new ImageBrush(bitmap);
                imageRec.Fill = brush;

                BitmapImage runbit = new BitmapImage(new Uri("Image/SERIE_A_Team/" + image.SECOND_TEAMNAME.ToLower() + ".png", UriKind.Relative));
                ImageBrush runbrush = new ImageBrush(runbit);
                Run_imageRec.Fill = runbrush;

                int nCount = int.Parse(image.COUNT);

                if (nCount >= 10 && nCount % 10 == 0)
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
            catch (Exception ex)
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
        }

        private void SearchFunc()
        {
            NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);
            conn.Open();

            string sql = "select * from SERIE_A lue order by league_year";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            mSELEAGUEList.Clear();

            while (reader.Read())
            {
                SERIE_A_LEAGUE lu = new SERIE_A_LEAGUE(reader[0].ToString(),
                    reader[1].ToString(),
                    reader[2].ToString(),
                    reader[3].ToString(),
                    reader[4].ToString(),
                    reader[5].ToString());

                mSELEAGUEList.Add(lu);
            }

            SetDataGridUsingQueryResultList();

            reader.Close();


            // DataGrid에 있는 최선 정보 txt파일에 저장
            string sOption = "SERIE_A";

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\" + sOption + ".txt";

            FileStream stream = File.Create(path);
            stream.Close();

            if (File.Exists(path))
            {
                for (int i = 0; i < mSELEAGUEList.Count; i++)
                {
                    File.AppendAllText(path, mSELEAGUEList[i].SELeague_Year.Trim() + "\n");
                    File.AppendAllText(path, mSELEAGUEList[i].SEChampions.Trim() + "\n");
                    File.AppendAllText(path, mSELEAGUEList[i].SESecond_Place.Trim() + "\n");
                    File.AppendAllText(path, mSELEAGUEList[i].SEThird_Place.Trim() + "\n");
                    File.AppendAllText(path, mSELEAGUEList[i].SEFourth_Place.Trim() + "\n");
                    File.AppendAllText(path, mSELEAGUEList[i].SERemark.Trim() + "\n");
                }
            }

            string Ranksql = "select rank() over( order by t.CHAMPIONS_CNT desc) Ranking, t.team_name, t.CHAMPIONS_CNT , t.SECOND_PLACE_CNT,  " +
                "t.THIRD_PLACE_CNT, t.FOURTH_PLACE_CNT from SERIE_A_RANKING t " +
                "where t.THIRD_PLACE_CNT != 0 or t.FOURTH_PLACE_CNT != 0 or t.SECOND_PLACE_CNT != 0 OR t.CHAMPIONS_CNT != 0 " +
                "order by t.CHAMPIONS_CNT desc, " +
                "t.SECOND_PLACE_CNT desc, t.THIRD_PLACE_CNT desc, t.FOURTH_PLACE_CNT desc";

            NpgsqlCommand RankCmd = new NpgsqlCommand(Ranksql, conn);
            NpgsqlDataReader Rankreader = RankCmd.ExecuteReader();

            mSERANKINGList.Clear();

            while (Rankreader.Read())
            {
                SERIE_A_LEAGUE_RANKING ccr = new SERIE_A_LEAGUE_RANKING(Rankreader[0].ToString(),
                    Rankreader[1].ToString(),
                    Rankreader[2].ToString(),
                    Rankreader[3].ToString(),
                    Rankreader[4].ToString(),
                    Rankreader[5].ToString());

                mSERANKINGList.Add(ccr);
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
                EREDIVISIE b = new EREDIVISIE();
                b.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                b.Show();
            }
            else if (e.Key == Key.X)
            {
                this.Close();
                BUNDESLIGA ls = new BUNDESLIGA();
                ls.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ls.Show();
            }
        }

        private void Ranking_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            SERIE_A_LEAGUE_RANKING ccr = mSERANKINGList[selectedIndex];

            m_sTeamName = ccr.SERTeam_Name.Trim();
            m_sOption = "SERIE_A";

            //DataPassProdCd(teamName);
            TeamCareer tc = new TeamCareer();
            tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tc.Show();
            tc.GetTeam(m_sTeamName, m_sOption);
        }

        private void Ranking_DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            SERIE_A_LEAGUE_RANKING ccr = mSERANKINGList[selectedIndex];

            m_sTeamName = ccr.SERTeam_Name.Trim();
            m_sOption = "SERIE_A";

            if(e.Key == Key.Enter)
            {
                //DataPassProdCd(teamName);
                TeamCareer tc = new TeamCareer();
                tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                tc.Show();
                tc.GetTeam(m_sTeamName, m_sOption);
            }
        }

        private void Champion_Name_Textbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Champion_Name_Textbox.Text != string.Empty)
            {
                string teamName = Champion_Name_Textbox.Text.Trim();
                string sOption = "SERIE_A";

                TeamCareer tc = new TeamCareer();
                tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                tc.Show();
                tc.GetTeam(teamName, sOption);
            }
            else
            {
                MessageBox.Show("우승팀을 표에서 선택해주세요.", "우승팀 선택", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
