#define WORKING_HOME

using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
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
    public class PREMIER
    {
        public string LLeague_Year;
        public string LChampions;
        public string LSecond_Place;
        public string LThird_Place;
        public string LFourth_Place;
        public string LRemark;

        public PREMIER(string lLeague_Year, string lChampions, string lSecond_Place, string lThird_Place, string lFourth_Place, string lRemark)
        {
            LLeague_Year = lLeague_Year;
            LChampions = lChampions;
            LSecond_Place = lSecond_Place;
            LThird_Place = lThird_Place;
            LFourth_Place = lFourth_Place;
            LRemark = lRemark;
        }
    }

    public class PREMIER_RANKING
    {
        public string LRRanking;
        public string LRTeam_Name;
        public string LRChampions_CNT;
        public string LRRunnerUp_CNT;
        public string LRThird_CNT;
        public string LRFourth_CNT;

        public PREMIER_RANKING(string lRRanking, string lRTeam_Name, string lRChampions_CNT, string lRRunnerUp_CNT, string lRThird_CNT, string lRFourth_CNT)
        {
            LRRanking = lRRanking;
            LRTeam_Name = lRTeam_Name;
            LRChampions_CNT = lRChampions_CNT;
            LRRunnerUp_CNT = lRRunnerUp_CNT;
            LRThird_CNT = lRThird_CNT;
            LRFourth_CNT = lRFourth_CNT;
        }
    }

    /// <summary>
    /// Premier_League.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Premier_League : Window
    {
        public static List<PREMIER> mPREMEIRList = new List<PREMIER>();
        public static List<PREMIER_RANKING> mPRERANKINGList = new List<PREMIER_RANKING>();

        public string m_sTeamName = string.Empty;
        public string m_sOption = string.Empty;

        public Premier_League()
        {
            InitializeComponent();
        }

        private void ToMiniButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void SetDataGridUsingQueryResultList()
        {
            List<Other_League> uiList = new List<Other_League>();

            for(int i=0; i< mPREMEIRList.Count; i++)
            {
                PREMIER pr = mPREMEIRList[i];

                uiList.Add(new Other_League()
                {
                    League_Year = mPREMEIRList[i].LLeague_Year.Trim(),
                    ChampionsLOGO = mPREMEIRList[i].LChampions.Trim(),
                    Champions = mPREMEIRList[i].LChampions.Trim(),
                    Second_PlaceLOGO = mPREMEIRList[i].LSecond_Place.Trim(),
                    Second_Place = mPREMEIRList[i].LSecond_Place.Trim(),
                    Third_PlaceLOGO = mPREMEIRList[i].LThird_Place.Trim(),
                    Third_Place = mPREMEIRList[i].LThird_Place.Trim(),
                    Fourth_PlaceLOGO = mPREMEIRList[i].LFourth_Place.Trim(),
                    Fourth_Place = mPREMEIRList[i].LFourth_Place.Trim(),
                    Remark = mPREMEIRList[i].LRemark.Trim()  
                });
            }

            grdEmployee.ItemsSource = uiList;
            grdEmployee.ScrollIntoView(grdEmployee.Items[grdEmployee.Items.Count - 1]);
        }


        // Set DataGrid Ranking UI
        private void SetDataGridRankingUI()
        {
            List<Other_League_Ranking_Data> uList = new List<Other_League_Ranking_Data>();

            for (int i = 0; i < mPRERANKINGList.Count; i++)
            {
                PREMIER_RANKING far = mPRERANKINGList[i];

                uList.Add(new Other_League_Ranking_Data()
                {
                    Ranking = int.Parse(mPRERANKINGList[i].LRRanking.Trim()),
                    Team_Logo = mPRERANKINGList[i].LRTeam_Name.Trim(),
                    Team_Name = mPRERANKINGList[i].LRTeam_Name.Trim(),
                    Champions_CNT = mPRERANKINGList[i].LRChampions_CNT.Trim(),
                    Second_Place_CNT = mPRERANKINGList[i].LRRunnerUp_CNT.Trim(),
                    Third_Place_CNT = mPRERANKINGList[i].LRThird_CNT.Trim(),
                    Fourth_Place_CNT = mPRERANKINGList[i].LRFourth_CNT.Trim()
                });
            }

            Ranking_DataGrid.ItemsSource = uList;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            SearchFunc();
        }

        private void Row_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int seelctedIndex = ((DataGrid)sender).SelectedIndex;

            PREMIER fa = mPREMEIRList[seelctedIndex];

            League_Year_textBox.Text = fa.LLeague_Year.ToString();
            League_Year2_textBox.Text = fa.LLeague_Year.ToLower();
            Champion_Name_Textbox.Text = fa.LChampions.ToString();
            Runner_UP_Textbox.Text = fa.LSecond_Place.ToString();
            Remark_Textbox.Text = fa.LRemark.ToString();

            NpgsqlConnection conn = null;
            try
            {
                conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();

                string sql = "select Count(*) from Premier_League t where t.champions = '" + Champion_Name_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year_textBox.Text + "';";

                NpgsqlCommand RankCmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader reader = RankCmd.ExecuteReader();

                string strWInsCount = string.Empty;

                while (reader.Read())
                {
                    strWInsCount = reader[0].ToString();
                }

                NumOfWins_Textbox.Text = strWInsCount;

                reader.Close();

                string Secsql = "select COUNT(*) from Premier_League T where T.second_place = '" + Runner_UP_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year2_textBox.Text + "';";

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

        private void SearchFunc()
        {
            List<string> list = new List<string>();

            string sql = "select * from premier_league ORDER BY LEAGUE_YEAR;";

            NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);

            conn.Open();

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            mPREMEIRList.Clear();

            while (reader.Read())
            {
                PREMIER pr = new PREMIER(reader[0].ToString().Trim(),
                    reader[1].ToString().Trim(),
                    reader[2].ToString().Trim(),
                    reader[3].ToString(),
                    reader[4].ToString(),
                    reader[5].ToString());

                mPREMEIRList.Add(pr);
            }

            SetDataGridUsingQueryResultList();

            reader.Close();
            // 보류 2023/08/14
            /*

            string Teamsql = "select distinct(champions) from "+ sOption + " union select distinct(SECOND_PLACE) from "+ sOption + " order by champions;";

            NpgsqlCommand TeamCmd = new NpgsqlCommand(Teamsql, conn);
            NpgsqlDataReader Teamreader = TeamCmd.ExecuteReader();

            List<string> TeamList = new List<string>();

            while (Teamreader.Read())
            {
                TeamList.Add(Teamreader[0].ToString().Trim());
            }

            for(int i=0; i<TeamList.Count; i++)
            {
                string str = "select (select count(*) from " + sOption + " where champions = '" + TeamList[i] + "')," +
                   "(select count(*) from " + sOption + " where second_place  = '" + TeamList[i] + "')";
            }
             */

            // DataGrid에 있는 최선 정보 txt파일에 저장
            string sOption = "PREMIER_LEAGUE";

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\" + sOption + ".txt";

            FileStream stream = File.Create(path);
            stream.Close();

            if (File.Exists(path))
            {
                for (int i = 0; i < mPREMEIRList.Count; i++)
                {
                    File.AppendAllText(path, mPREMEIRList[i].LLeague_Year.Trim() + "\n");
                    File.AppendAllText(path, mPREMEIRList[i].LChampions.Trim() + "\n");
                    File.AppendAllText(path, mPREMEIRList[i].LSecond_Place.Trim() + "\n");
                    File.AppendAllText(path, mPREMEIRList[i].LThird_Place.Trim() + "\n");
                    File.AppendAllText(path, mPREMEIRList[i].LFourth_Place.Trim() + "\n");
                    File.AppendAllText(path, mPREMEIRList[i].LRemark.Trim() + "\n");
                }
            }

            string Ranksql = "select rank() over( order by t.CHAMPIONS_CNT desc) Ranking, t.team_name, t.CHAMPIONS_CNT , t.SECOND_PLACE_CNT,  " +
                "t.THIRD_PLACE_CNT, t.FOURTH_PLACE_CNT from PREMIER_LEAGUE_RANKING t " +
                "where t.SECOND_PLACE_CNT != 0 OR t.CHAMPIONS_CNT != 0 or t.THIRD_PLACE_CNT != 0 or t.FOURTH_PLACE_CNT != 0 " +
                "order by t.CHAMPIONS_CNT desc, " +
                "t.SECOND_PLACE_CNT desc, t.THIRD_PLACE_CNT desc, t.FOURTH_PLACE_CNT  desc";

            NpgsqlCommand RankCmd = new NpgsqlCommand(Ranksql, conn);
            NpgsqlDataReader Rankreader = RankCmd.ExecuteReader();

            mPRERANKINGList.Clear();

            while (Rankreader.Read())
            {
                PREMIER_RANKING far = new PREMIER_RANKING(Rankreader[0].ToString(),
                    Rankreader[1].ToString(),
                    Rankreader[2].ToString(),
                    Rankreader[3].ToString(),
                    Rankreader[4].ToString(),
                    Rankreader[5].ToString());

                mPRERANKINGList.Add(far);
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
            if(e.Key == Key.S)
            {
                SearchFunc();
            }
            if(e.Key == Key.C)
            {
                this.Close();
                EMIRATES_FA_CUP fa = new EMIRATES_FA_CUP();
                fa.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                fa.Show();
            }
            if(e.Key == Key.X)
            {
                this.Close();
                EFL_CHAPIONSHIP cc = new EFL_CHAPIONSHIP();
                cc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                cc.Show();
            }
            if(e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                SaveFunc();
            }
        }

        private void Ranking_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            PREMIER_RANKING ccr = mPRERANKINGList[selectedIndex];

            string teamName = ccr.LRTeam_Name.Trim();
            string sOption = "PREMIER_LEAGUE";

            TeamCareer tc = new TeamCareer();
            tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tc.Show();
            tc.GetTeam(teamName, sOption);

        }

        private void Ranking_DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            PREMIER_RANKING ccr = mPRERANKINGList[selectedIndex];

            string teamName = ccr.LRTeam_Name.Trim();
            string sOption = "PREMIER_LEAGUE";

            if (e.Key == Key.Enter)
            {
                TeamCareer tc = new TeamCareer();
                tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                tc.Show();
                tc.GetTeam(teamName, sOption);
            }
        }

        public static string UpdateSqlFunc(string sTable, string sCoulmn, string udpateData, string LeagueYear)
        {
            string sql = string.Empty;
            sql = "update " + sTable + " set " + sCoulmn + " = '" + udpateData + "' where league_year = '" + LeagueYear + "';";
            return sql;
        }

        private void SaveFunc()
        {
            if (MessageBox.Show("데이터를 저장하시 겠습니까??", "데이터 저장", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                List<Other_League> list = grdEmployee.ItemsSource as List<Other_League>;
                int n = mPREMEIRList.Count;

                List<string> updateList = new List<string>();

                for (int i = 0; i < list.Count; i++)
                {

                    PREMIER pr = mPREMEIRList[i];
                    Other_League other = list[i];

                    if (pr.LChampions != other.Champions)
                    {
                        updateList.Add(UpdateSqlFunc("premier_league", "champions", other.Champions, other.League_Year));
                    }
                    if (pr.LSecond_Place != other.Second_Place)
                    {
                        updateList.Add(UpdateSqlFunc("premier_league", "Second_Place", other.Second_Place, other.League_Year));
                    }
                    if (pr.LThird_Place != other.Third_Place)
                    {
                        updateList.Add(UpdateSqlFunc("premier_league", "Third_Place", other.Third_Place, other.League_Year));
                    }
                    if (pr.LFourth_Place != other.Fourth_Place)
                    {
                        updateList.Add(UpdateSqlFunc("premier_league", "Fourth_Place", other.Fourth_Place, other.League_Year));
                    }
                    if (pr.LRemark != other.Remark)
                    {
                        updateList.Add(UpdateSqlFunc("premier_league", "Remark", other.Remark, other.League_Year));
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

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFunc();
        }

        private void Champion_Name_Textbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(Champion_Name_Textbox.Text != string.Empty)
            {
                string teamName = Champion_Name_Textbox.Text.Trim();
                string sOption = "PREMIER_LEAGUE";

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
