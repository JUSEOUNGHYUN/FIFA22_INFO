﻿using Npgsql;
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
    /// EFL_CHAPIONSHIP.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public class EFL_CHAPIONSHIP_LEAGUE
    {
        public string BLeague_Year;
        public string BChampions;
        public string BSecond_Place;
        public string BThird_Place;
        public string BFourth_Place;
        public string BRemark;

        public EFL_CHAPIONSHIP_LEAGUE(string bLeague_Year, string bChampions, string bSecond_Place, string bThird_Place, string bFourth_Place, string bRemark)
        {
            BLeague_Year = bLeague_Year;
            BChampions = bChampions;
            BSecond_Place = bSecond_Place;
            BThird_Place = bThird_Place;
            BFourth_Place = bFourth_Place;
            BRemark = bRemark;
        }
    }

    public class EFL_CHAPIONSHIP_RANKING
    {
        public string BRRanking;
        public string BRTeam_Name;
        public string BRChampions_CNT;
        public string BRRunnerUp_CNT;
        public string BRThird_CNT;
        public string BRFourth_CNT;

        public EFL_CHAPIONSHIP_RANKING(string bRRanking, string bRTeam_Name, string bRChampions_CNT, string bRRunnerUp_CNT, string bRThird_CNT, string bRFourth_CNT)
        {
            BRRanking = bRRanking;
            BRTeam_Name = bRTeam_Name;
            BRChampions_CNT = bRChampions_CNT;
            BRRunnerUp_CNT = bRRunnerUp_CNT;
            BRThird_CNT = bRThird_CNT;
            BRFourth_CNT = bRFourth_CNT;
        }
    }
    public partial class EFL_CHAPIONSHIP : Window
    {
        public static List<EFL_CHAPIONSHIP_LEAGUE> mEFLList = new List<EFL_CHAPIONSHIP_LEAGUE>();
        public static List<EFL_CHAPIONSHIP_RANKING> mEFLRANKINGList = new List<EFL_CHAPIONSHIP_RANKING>();


        public string m_sTeamName = string.Empty;
        public string m_sOption = string.Empty;

        public EFL_CHAPIONSHIP()
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

        private void SearchFunc()
        {
            NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);
            conn.Open();
            List<string> list = new List<string>();

            string sql = "select * from EFL_CHAMPIONSHIP lue order by league_year";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            mEFLList.Clear();

            while (reader.Read())
            {
                EFL_CHAPIONSHIP_LEAGUE lu = new EFL_CHAPIONSHIP_LEAGUE(reader[0].ToString(),
                    reader[1].ToString(),
                    reader[2].ToString(),
                    reader[3].ToString(),
                    reader[4].ToString(),
                    reader[5].ToString());

                mEFLList.Add(lu);
            }

            SetDataGridUsingQueryResultList();

            reader.Close();

            // DataGrid에 있는 최선 정보 txt파일에 저장
            string sOption = "EFL_CHAMPIONSHIP";

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\" + sOption + ".txt";

            FileStream stream = File.Create(path);
            stream.Close();

            if (File.Exists(path))
            {
                for (int i = 0; i < mEFLList.Count; i++)
                {
                    File.AppendAllText(path, mEFLList[i].BLeague_Year.Trim() + "\n");
                    File.AppendAllText(path, mEFLList[i].BChampions.Trim() + "\n");
                    File.AppendAllText(path, mEFLList[i].BSecond_Place.Trim() + "\n");
                    File.AppendAllText(path, mEFLList[i].BThird_Place.Trim() + "\n");
                    File.AppendAllText(path, mEFLList[i].BFourth_Place.Trim() + "\n");
                    File.AppendAllText(path, mEFLList[i].BRemark.Trim() + "\n");
                }
            }

            string Ranksql = "select rank() over( order by t.CHAMPIONS_CNT desc) Ranking, t.team_name, t.CHAMPIONS_CNT , t.SECOND_PLACE_CNT,  " +
                "t.THIRD_PLACE_CNT, t.FOURTH_PLACE_CNT from EFL_CHAMPIONSHIP_RANKING t " +
                "where t.THIRD_PLACE_CNT != 0 or t.FOURTH_PLACE_CNT != 0 or t.SECOND_PLACE_CNT != 0 OR t.CHAMPIONS_CNT != 0 " +
                "order by t.CHAMPIONS_CNT desc, " +
                "t.SECOND_PLACE_CNT desc, t.THIRD_PLACE_CNT desc, t.FOURTH_PLACE_CNT  desc";

            NpgsqlCommand RankCmd = new NpgsqlCommand(Ranksql, conn);
            NpgsqlDataReader Rankreader = RankCmd.ExecuteReader();

            mEFLRANKINGList.Clear();

            while (Rankreader.Read())
            {
                EFL_CHAPIONSHIP_RANKING ccr = new EFL_CHAPIONSHIP_RANKING(Rankreader[0].ToString(),
                    Rankreader[1].ToString(),
                    Rankreader[2].ToString(),
                    Rankreader[3].ToString(),
                    Rankreader[4].ToString(),
                    Rankreader[5].ToString());

                mEFLRANKINGList.Add(ccr);
            }

            SetDataGridRankingUI();

            Rankreader.Close();
        }

        private void SetDataGridUsingQueryResultList()
        {
            List<Other_League> uiList = new List<Other_League>();

            for (int i = 0; i < mEFLList.Count; i++)
            {
                uiList.Add(new Other_League()
                {
                    League_Year = mEFLList[i].BLeague_Year.Trim(),
                    ChampionsLOGO = mEFLList[i].BChampions.Trim(),
                    Champions = mEFLList[i].BChampions.Trim(),
                    Second_PlaceLOGO = mEFLList[i].BSecond_Place.Trim(),
                    Second_Place = mEFLList[i].BSecond_Place.Trim(),
                    Third_PlaceLOGO = mEFLList[i].BThird_Place.Trim(),
                    Third_Place = mEFLList[i].BThird_Place.Trim(),
                    Fourth_PlaceLOGO = mEFLList[i].BFourth_Place.Trim(),
                    Fourth_Place = mEFLList[i].BFourth_Place.Trim(),
                    Remark = mEFLList[i].BRemark.Trim(),
                });
            }
            grdEmployee.ItemsSource = uiList;
            grdEmployee.ScrollIntoView(grdEmployee.Items[grdEmployee.Items.Count - 1]);
        }

        private void SetDataGridRankingUI()
        {
            List<Other_League_Ranking_Data> uList = new List<Other_League_Ranking_Data>();

            for (int i = 0; i < mEFLRANKINGList.Count; i++)
            {
                EFL_CHAPIONSHIP_RANKING lur = mEFLRANKINGList[i];

                uList.Add(new Other_League_Ranking_Data()
                {
                    Ranking = int.Parse(mEFLRANKINGList[i].BRRanking.Trim()),
                    Team_Logo = mEFLRANKINGList[i].BRTeam_Name.Trim(),
                    Team_Name = mEFLRANKINGList[i].BRTeam_Name.Trim(),
                    Champions_CNT = mEFLRANKINGList[i].BRChampions_CNT.Trim(),
                    Second_Place_CNT = mEFLRANKINGList[i].BRRunnerUp_CNT.Trim(),
                    Third_Place_CNT = mEFLRANKINGList[i].BRThird_CNT.Trim(),
                    Fourth_Place_CNT = mEFLRANKINGList[i].BRFourth_CNT.Trim()
                });
            }
            Ranking_DataGrid.ItemsSource = uList;

        }

        private void Row_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int seelctedIndex = ((DataGrid)sender).SelectedIndex;

            EFL_CHAPIONSHIP_LEAGUE cc = mEFLList[seelctedIndex];
            League_Year_textBox.Text = cc.BLeague_Year.ToString();
            League_Year2_textBox.Text = cc.BLeague_Year.ToLower();
            Champion_Name_Textbox.Text = cc.BChampions.ToString();
            Runner_UP_Textbox.Text = cc.BSecond_Place.ToString();
            Remark_Textbox.Text = cc.BRemark.ToString();

            NpgsqlConnection conn = null;

            try
            {
                conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();

                string sql = "select Count(*) from EFL_CHAMPIONSHIP t where t.champions = '" + Champion_Name_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year_textBox.Text + "';";

                NpgsqlCommand RankCmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader reader = RankCmd.ExecuteReader();

                string strWInsCount = string.Empty;

                while (reader.Read())
                {
                    strWInsCount = reader[0].ToString();
                }

                NumOfWins_Textbox.Text = strWInsCount;

                reader.Close();

                string Secsql = "select COUNT(*) from EFL_CHAMPIONSHIP T where T.second_place = '" + Runner_UP_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year2_textBox.Text + "';";

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

        private void Ranking_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            EFL_CHAPIONSHIP_RANKING ccr = mEFLRANKINGList[selectedIndex];

            m_sTeamName = ccr.BRTeam_Name.Trim();
            m_sOption = "EFL_CHAMPIONSHIP";

            //DataPassProdCd(teamName);
            TeamCareer tc = new TeamCareer();
            tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tc.Show();
            tc.GetTeam(m_sTeamName, m_sOption);
        }

        private void Ranking_DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            EFL_CHAPIONSHIP_RANKING ccr = mEFLRANKINGList[selectedIndex];

            m_sTeamName = ccr.BRTeam_Name.Trim();
            m_sOption = "EFL_CHAMPIONSHIP";
            
            if(e.Key == Key.Enter)
            {
                //DataPassProdCd(teamName);
                TeamCareer tc = new TeamCareer();
                tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                tc.Show();
                tc.GetTeam(m_sTeamName, m_sOption);
            }
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
                Premier_League b = new Premier_League();
                b.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                b.Show();
            }
            else if (e.Key == Key.X)
            {
                this.Close();
                CARABAO_CUP ls = new CARABAO_CUP();
                ls.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ls.Show();
            }
        }

        private void Champion_Name_Textbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Champion_Name_Textbox.Text != string.Empty)
            {
                string teamName = Champion_Name_Textbox.Text.Trim();
                string sOption = "EFL_CHAMPIONSHIP";

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
