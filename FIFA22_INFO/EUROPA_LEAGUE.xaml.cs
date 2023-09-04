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
using static System.Collections.Specialized.BitVector32;

namespace FIFA22_INFO
{
    /// <summary>
    /// EUROPA_LEAGUE.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public class EUROPA
    {
        public string ELeague_Year;
        public string EChampions;
        public string ESecond_Place;
        public string ERemark;

        public EUROPA(string eLeague_Year, string eChampions, string eSecond_Place, string eRemark)
        {
            this.ELeague_Year = eLeague_Year;
            this.EChampions = eChampions;
            this.ESecond_Place = eSecond_Place;
            this.ERemark = eRemark;
        }
    }

    public class EUROPA_RANKING
    {
        public string ERRanking;
        public string ERTeam_Name;
        public string ERChampions_CNT;
        public string ERRunnerUp_CNT;

        public EUROPA_RANKING(string eRRanking, string eRTeam_Name, string eRChampions_CNT, string eRRunnerUp_CNT)
        {
            ERRanking = eRRanking;
            ERTeam_Name = eRTeam_Name;
            ERChampions_CNT = eRChampions_CNT;
            ERRunnerUp_CNT = eRRunnerUp_CNT;
        }
    }

    public partial class EUROPA_LEAGUE : Window
    {
        public static List<EUROPA> mEUROPAList = new List<EUROPA>();
        public static List<EUROPA_RANKING> mEUROPARANKINGList = new List<EUROPA_RANKING>();

        public string m_sTeamName = string.Empty;
        public string m_sOption = string.Empty;

        public EUROPA_LEAGUE()
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

        private void Row_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int seelctedIndex = ((DataGrid)sender).SelectedIndex;

            EUROPA cc = mEUROPAList[seelctedIndex];
            League_Year_textBox.Text = cc.ELeague_Year.ToString();
            League_Year2_textBox.Text = cc.ELeague_Year.ToLower();
            Champion_Name_Textbox.Text = cc.EChampions.ToString();
            Runner_UP_Textbox.Text = cc.ESecond_Place.ToString();
            Remark_Textbox.Text = cc.ERemark.ToString();

            NpgsqlConnection conn = null;

            try
            {
                conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();

                string sql = "select Count(*) from EUROPA_LEAGUE t where t.champions = '" + Champion_Name_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year_textBox.Text + "';";

                NpgsqlCommand RankCmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader reader = RankCmd.ExecuteReader();

                string strWInsCount = string.Empty;

                while (reader.Read())
                {
                    strWInsCount = reader[0].ToString();
                }

                NumOfWins_Textbox.Text = strWInsCount;

                reader.Close();

                string Secsql = "select COUNT(*) from EUROPA_LEAGUE T where T.second_place = '" + Runner_UP_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year2_textBox.Text + "';";

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

                BitmapImage bitmap = new BitmapImage(new Uri("Resources/" + image.TEAMNAME.ToString() + ".png", UriKind.Relative));
                ImageBrush brush = new ImageBrush(bitmap);
                imageRec.Fill = brush;

                BitmapImage runbit = new BitmapImage(new Uri("Resources/" + image.SECOND_TEAMNAME.ToLower() + ".png", UriKind.Relative));
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

        private void SetDataGridUsingQueryResultList()
        {
            List<Premier_League_Data> uiList = new List<Premier_League_Data>();

            for(int i=0; i<mEUROPAList.Count; i++)
            {
                EUROPA ep = mEUROPAList[i];

                uiList.Add(new Premier_League_Data()
                {
                    League_Year = mEUROPAList[i].ELeague_Year.Trim(),
                    ChampionsLOGO = mEUROPAList[i].EChampions.Trim(),
                    Champions = mEUROPAList[i].EChampions.Trim(),
                    Second_PlaceLOGO = mEUROPAList[i].ESecond_Place.Trim(),
                    Second_Place = mEUROPAList[i].ESecond_Place.Trim(),
                    Remark = mEUROPAList[i].ERemark.Trim()
                });
            }
            grdEmployee.ItemsSource = uiList;
            grdEmployee.ScrollIntoView(grdEmployee.Items[grdEmployee.Items.Count - 1]);
        }

        private void SetDataGridRankingUI()
        {
            List<EnglandTotalData> uList = new List<EnglandTotalData>();

            for (int i = 0; i < mEUROPARANKINGList.Count; i++)
            {
                EUROPA_RANKING clr = mEUROPARANKINGList[i];

                uList.Add(new EnglandTotalData()
                {
                    Ranking = int.Parse(mEUROPARANKINGList[i].ERRanking.Trim()),
                    Team_Logo = mEUROPARANKINGList[i].ERTeam_Name.Trim(),
                    Team_Name = mEUROPARANKINGList[i].ERTeam_Name.Trim(),
                    Champions_CNT = mEUROPARANKINGList[i].ERChampions_CNT.Trim(),
                    Second_Place_CNT = mEUROPARANKINGList[i].ERRunnerUp_CNT.Trim()
                });
            }
            Ranking_DataGrid.ItemsSource = uList;
        }

        private void SearchFunc()
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();
                List<string> list = new List<string>();

                string sql = "select * from EUROPA_LEAGUE t order by league_year";

                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                mEUROPAList.Clear();

                while (reader.Read())
                {
                    EUROPA cc = new EUROPA(reader[0].ToString(),
                        reader[1].ToString(),
                        reader[2].ToString(),
                        reader[3].ToString());

                    mEUROPAList.Add(cc);
                }

                SetDataGridUsingQueryResultList();

                reader.Close();

                // DataGrid에 있는 최선 정보 txt파일에 저장
                string sOption = "EUROPA_LEAGUE";

                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\" + sOption + ".txt";

                try
                {
                    FileStream stream = File.Create(path);
                    stream.Close();

                    if (File.Exists(path))
                    {
                        for (int i = 0; i < mEUROPAList.Count; i++)
                        {
                            File.AppendAllText(path, mEUROPAList[i].ELeague_Year.Trim() + "\n");
                            File.AppendAllText(path, mEUROPAList[i].EChampions.Trim() + "\n");
                            File.AppendAllText(path, mEUROPAList[i].ESecond_Place.Trim() + "\n");
                            File.AppendAllText(path, mEUROPAList[i].ERemark.Trim() + "\n");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }

                string Ranksql = "select rank() over( order by t.EUROPA_LEAGUE_WINNER_CNT desc) Ranking, t.team_name, t.EUROPA_LEAGUE_WINNER_CNT , t.EUROPA_LEAGUE_RUN_CNT  " +
                   "from TOTAL t " +
                   "where t.EUROPA_LEAGUE_RUN_CNT != 0 OR t.EUROPA_LEAGUE_WINNER_CNT != 0 " +
                   "order by t.EUROPA_LEAGUE_WINNER_CNT desc, " +
                   "t.EUROPA_LEAGUE_RUN_CNT desc;";

                NpgsqlCommand RankCmd = new NpgsqlCommand(Ranksql, conn);
                NpgsqlDataReader Rankreader = RankCmd.ExecuteReader();

                mEUROPARANKINGList.Clear();

                while (Rankreader.Read())
                {
                    EUROPA_RANKING ccr = new EUROPA_RANKING(Rankreader[0].ToString(),
                        Rankreader[1].ToString(),
                        Rankreader[2].ToString(),
                        Rankreader[3].ToString());

                    mEUROPARANKINGList.Add(ccr);
                }

                SetDataGridRankingUI();

                Rankreader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                CONFERENCE_LEAGUE cl = new CONFERENCE_LEAGUE();
                cl.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                cl.Show();
            }
            else if (e.Key == Key.X)
            {
                this.Close();
                CHAMPIONS_LEAGUE el = new CHAMPIONS_LEAGUE();
                el.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                el.Show();
            }
            else if(e.Key == Key.Decimal || e.Key == Key.OemPeriod)
            {
                SaveFunc();
            }
        }

        private void Ranking_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            EUROPA_RANKING ccr = mEUROPARANKINGList[selectedIndex];

            m_sTeamName = ccr.ERTeam_Name.Trim();
            m_sOption = "EUROPA_LEAGUE";

            //DataPassProdCd(teamName);
            TeamCareer tc = new TeamCareer();
            tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tc.Show();
            tc.GetTeam(m_sTeamName, m_sOption);
        }


        private void Ranking_DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            EUROPA_RANKING ccr = mEUROPARANKINGList[selectedIndex];

            m_sTeamName = ccr.ERTeam_Name.Trim();
            m_sOption = "EUROPA_LEAGUE";

            if (e.Key == Key.Enter)
            {
                TeamCareer tc = new TeamCareer();
                tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                tc.Show();
                tc.GetTeam(m_sTeamName, m_sOption);
            }
        }

        private void SaveFunc()
        {
            if (MessageBox.Show("데이터를 저장하시 겠습니까??", "데이터 저장", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                List<Premier_League_Data> list = grdEmployee.ItemsSource as List<Premier_League_Data>;
                int n = mEUROPAList.Count;

                string sOption = "europa_league";

                List<string> updateList = new List<string>();

                for (int i = 0; i < list.Count; i++)
                {
                    EUROPA pr = mEUROPAList[i];
                    Premier_League_Data other = list[i];

                    if (pr.EChampions != other.Champions)
                    {
                        updateList.Add(Premier_League.UpdateSqlFunc(sOption, "champions", other.Champions, other.League_Year));
                    }
                    if (pr.ESecond_Place != other.Second_Place)
                    {
                        updateList.Add(Premier_League.UpdateSqlFunc(sOption, "Second_Place", other.Second_Place, other.League_Year));
                    }
                    if (pr.ERemark != other.Remark)
                    {
                        updateList.Add(Premier_League.UpdateSqlFunc(sOption, "Remark", other.Remark, other.League_Year));
                    }
                }

                NpgsqlConnection conn = null;
                try
                {
                    conn = new NpgsqlConnection(MainWindow.mConnString);
                    conn.Open();

                    for (int i = 0; i < updateList.Count; i++)
                    {
                        NpgsqlCommand UpdateCommand = new NpgsqlCommand(updateList[i], conn);
                        UpdateCommand.ExecuteNonQuery();
                    }

                    MessageBox.Show("저장을 완료했습니다.", "데이터 업데이트", MessageBoxButton.OK, MessageBoxImage.Information);
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
        }


        private void SAVE_Click(object sender, RoutedEventArgs e)
        {
            SaveFunc();
        }
    }
}
