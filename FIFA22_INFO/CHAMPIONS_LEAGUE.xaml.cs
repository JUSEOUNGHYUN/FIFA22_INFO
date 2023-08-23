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
    /// CHAMPIONS_LEAGUE.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public class CHAMPIONSLEAGUE
    {
        public string CLLeague_Year;
        public string CLChampions;
        public string CLSecond_Place;
        public string CLRemark;

        public CHAMPIONSLEAGUE(string cLLeague_Year, string cLChampions, string cLSecond_Place, string cLRemark)
        {
            CLLeague_Year = cLLeague_Year;
            CLChampions = cLChampions;
            CLSecond_Place = cLSecond_Place;
            CLRemark = cLRemark;
        }
    }

    public class CHAMPIONSLEAGUE_RANKING
    {
        public string CLRRanking;
        public string CLRTeam_Name;
        public string CLRChampions_CNT;
        public string CLRRunnerUp_CNT;

        public CHAMPIONSLEAGUE_RANKING(string cLRRanking, string cLRTeam_Name, string cLRChampions_CNT, string cLRRunnerUp_CNT)
        {
            CLRRanking = cLRRanking;
            CLRTeam_Name = cLRTeam_Name;
            CLRChampions_CNT = cLRChampions_CNT;
            CLRRunnerUp_CNT = cLRRunnerUp_CNT;
        }
    }


    public partial class CHAMPIONS_LEAGUE : Window
    {
        public static List<CHAMPIONSLEAGUE> mCHAMPIONSList = new List<CHAMPIONSLEAGUE>();
        public static List<CHAMPIONSLEAGUE_RANKING> mCHAMPIONSRANKINGList = new List<CHAMPIONSLEAGUE_RANKING>();

        public delegate void DataPassProdCdEventHandler(string strTeamName);
        public event DataPassProdCdEventHandler DataPassProdCd;

        public CHAMPIONS_LEAGUE()
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
            List<Premier_League_Data> uiList = new List<Premier_League_Data>();

            for(int i=0; i<mCHAMPIONSList.Count; i++)
            {
                CHAMPIONSLEAGUE cl = mCHAMPIONSList[i];

                uiList.Add(new Premier_League_Data()
                {
                    League_Year = mCHAMPIONSList[i].CLLeague_Year.Trim(),
                    ChampionsLOGO = mCHAMPIONSList[i].CLChampions.Trim(),
                    Champions = mCHAMPIONSList[i].CLChampions.Trim(),
                    Second_PlaceLOGO = mCHAMPIONSList[i].CLSecond_Place.Trim(),
                    Second_Place = mCHAMPIONSList[i].CLSecond_Place.Trim(),
                    Remark = mCHAMPIONSList[i].CLRemark.Trim()
                });
            }
            grdEmployee.ItemsSource = uiList;
            grdEmployee.ScrollIntoView(grdEmployee.Items[grdEmployee.Items.Count - 1]);
        }

        private void SetDataGridRankingUI()
        {
            List<EnglandTotalData> uList = new List<EnglandTotalData>();

            for(int i=0; i<mCHAMPIONSRANKINGList.Count; i++)
            {
                CHAMPIONSLEAGUE_RANKING clr = mCHAMPIONSRANKINGList[i];

                uList.Add(new EnglandTotalData()
                {
                    Ranking = int.Parse(mCHAMPIONSRANKINGList[i].CLRRanking.Trim()),
                    Team_Logo = mCHAMPIONSRANKINGList[i].CLRTeam_Name.Trim(),
                    Team_Name = mCHAMPIONSRANKINGList[i].CLRTeam_Name.Trim(),
                    Champions_CNT = mCHAMPIONSRANKINGList[i].CLRChampions_CNT.Trim(),
                    Second_Place_CNT = mCHAMPIONSRANKINGList[i].CLRRunnerUp_CNT.Trim()
                });
            }
            Ranking_DataGrid.ItemsSource = uList;

        }

        private void Row_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int seelctedIndex = ((DataGrid)sender).SelectedIndex;

            CHAMPIONSLEAGUE cc = mCHAMPIONSList[seelctedIndex];
            League_Year_textBox.Text = cc.CLLeague_Year.ToString();
            League_Year2_textBox.Text = cc.CLLeague_Year.ToLower();
            Champion_Name_Textbox.Text = cc.CLChampions.ToString();
            Runner_UP_Textbox.Text = cc.CLSecond_Place.ToString();
            Remark_Textbox.Text = cc.CLRemark.ToString();

            NpgsqlConnection conn = null;

            try
            {
                conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();

                string sql = "select Count(*) from CHAMPIONS_LEAGUE t where t.champions = '" + Champion_Name_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year_textBox.Text + "';";

                NpgsqlCommand RankCmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader reader = RankCmd.ExecuteReader();

                string strWInsCount = string.Empty;

                while (reader.Read())
                {
                    strWInsCount = reader[0].ToString();
                }

                NumOfWins_Textbox.Text = strWInsCount;

                reader.Close();

                string Secsql = "select COUNT(*) from CHAMPIONS_LEAGUE T where T.second_place = '" + Runner_UP_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year2_textBox.Text + "';";

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

                BitmapImage bitmap = new BitmapImage(new Uri("Image/Champions_League_Team/" + image.TEAMNAME.ToString() + ".png", UriKind.Relative));
                ImageBrush brush = new ImageBrush(bitmap);
                imageRec.Fill = brush;

                BitmapImage runbit = new BitmapImage(new Uri("Image/Champions_League_Team/" + image.SECOND_TEAMNAME.ToLower() + ".png", UriKind.Relative));
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
            List<string> list = new List<string>();

            string sql = "select * from CHAMPIONS_LEAGUE t order by league_year";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            mCHAMPIONSList.Clear();

            while (reader.Read())
            {
                CHAMPIONSLEAGUE cc = new CHAMPIONSLEAGUE(reader[0].ToString(),
                    reader[1].ToString(),
                    reader[2].ToString(),
                    reader[3].ToString());

                mCHAMPIONSList.Add(cc);
            }

            SetDataGridUsingQueryResultList();

            reader.Close();

            // DataGrid에 있는 최선 정보 txt파일에 저장
            string sOption = "CHAMPIONS_LEAGUE";

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\" + sOption + ".txt";

            try
            {
                FileStream stream = File.Create(path);
                stream.Close();

                if (File.Exists(path))
                {
                    for (int i = 0; i < mCHAMPIONSList.Count; i++)
                    {
                        File.AppendAllText(path, mCHAMPIONSList[i].CLLeague_Year.Trim() + "\n");
                        File.AppendAllText(path, mCHAMPIONSList[i].CLChampions.Trim() + "\n");
                        File.AppendAllText(path, mCHAMPIONSList[i].CLSecond_Place.Trim() + "\n");
                        File.AppendAllText(path, mCHAMPIONSList[i].CLRemark.Trim() + "\n");
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            string Ranksql = "select rank() over( order by t.CHAMPIONS_LEAGUE_WINNER_CNT desc) Ranking, t.team_name, t.CHAMPIONS_LEAGUE_WINNER_CNT , t.CHAMPIONS_LEAGUE_RUN_CNT  " +
               "from TOTAL t " +
               "where t.CHAMPIONS_LEAGUE_RUN_CNT != 0 OR t.CHAMPIONS_LEAGUE_WINNER_CNT != 0 " +
               "order by t.CHAMPIONS_LEAGUE_WINNER_CNT desc, " +
               "t.CHAMPIONS_LEAGUE_RUN_CNT desc;";

            NpgsqlCommand RankCmd = new NpgsqlCommand(Ranksql, conn);
            NpgsqlDataReader Rankreader = RankCmd.ExecuteReader();

            mCHAMPIONSRANKINGList.Clear();

            while (Rankreader.Read())
            {
                CHAMPIONSLEAGUE_RANKING ccr = new CHAMPIONSLEAGUE_RANKING(Rankreader[0].ToString(),
                    Rankreader[1].ToString(),
                    Rankreader[2].ToString(),
                    Rankreader[3].ToString());

                mCHAMPIONSRANKINGList.Add(ccr);
            }

            SetDataGridRankingUI();

            Rankreader.Close();
        }

        private void keyDown_Event(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
            else if (e.Key == Key.S)
            {
                SearchFunc();
            }
            else if (e.Key == Key.C)
            {
                this.Close();
                EUROPA_LEAGUE el = new EUROPA_LEAGUE();
                el.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                el.Show();
            }
            else if (e.Key == Key.X) 
            {
                this.Close();
                SUPER_CUP sc = new SUPER_CUP();
                sc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                sc.Show();
            }
        }

        private void Ranking_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            CHAMPIONSLEAGUE_RANKING ccr = mCHAMPIONSRANKINGList[selectedIndex];

            string teamName = ccr.CLRTeam_Name.Trim();
            string sOption = "CHAMPIONS_LEAGUE";

            //DataPassProdCd(teamName);
            TeamCareer tc = new TeamCareer();
            tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tc.Show();
            tc.GetTeam(teamName, sOption);
        }
    }
}
