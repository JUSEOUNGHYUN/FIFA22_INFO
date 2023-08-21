using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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
    /// <summary>
    /// LIGUE1.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public class LIGUE1_UBER_EATS
    {
        public string LLeague_Year;
        public string LChampions;
        public string LSecond_Place;
        public string LThird_Place;
        public string LFourth_Place;
        public string LRemark;

        public LIGUE1_UBER_EATS(string lLeague_Year, string lChampions, string lSecond_Place, string lThird_Place, string lFourth_Place, string lRemark)
        {
            LLeague_Year = lLeague_Year;
            LChampions = lChampions;
            LSecond_Place = lSecond_Place;
            LThird_Place = lThird_Place;
            LFourth_Place = lFourth_Place;
            LRemark = lRemark;
        }
    }

    public class LIGUE1_UBER_EATS_RANKING
    {
        public string LRRanking;
        public string LRTeam_Name;
        public string LRChampions_CNT;
        public string LRRunnerUp_CNT;
        public string LRThird_CNT;
        public string LRFourth_CNT;

        public LIGUE1_UBER_EATS_RANKING(string lRRanking, string lRTeam_Name, string lRChampions_CNT, string lRRunnerUp_CNT, string lRThird_CNT, string lRFourth_CNT)
        {
            LRRanking = lRRanking;
            LRTeam_Name = lRTeam_Name;
            LRChampions_CNT = lRChampions_CNT;
            LRRunnerUp_CNT = lRRunnerUp_CNT;
            LRThird_CNT = lRThird_CNT;
            LRFourth_CNT = lRFourth_CNT;
        }
    }

    public partial class LIGUE1 : Window
    {
        public static List<LIGUE1_UBER_EATS> mLIGUE1List = new List<LIGUE1_UBER_EATS>();
        public static List<LIGUE1_UBER_EATS_RANKING> mLIGUE1RANKINGList = new List<LIGUE1_UBER_EATS_RANKING>();

        public LIGUE1()
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

            for (int i = 0; i < mLIGUE1List.Count; i++)
            {
                uiList.Add(new Other_League()
                { 
                    League_Year = mLIGUE1List[i].LLeague_Year.Trim(),
                    ChampionsLOGO = mLIGUE1List[i].LChampions.Trim(),
                    Champions= mLIGUE1List[i].LChampions.Trim(),
                    Second_PlaceLOGO = mLIGUE1List[i].LSecond_Place.Trim(),
                    Second_Place = mLIGUE1List[i].LSecond_Place.Trim(),
                    Third_PlaceLOGO = mLIGUE1List[i].LThird_Place.Trim(),
                    Third_Place= mLIGUE1List[i].LThird_Place.Trim(),
                    Fourth_PlaceLOGO= mLIGUE1List[i].LFourth_Place.Trim(),
                    Fourth_Place= mLIGUE1List[i].LFourth_Place.Trim(),
                    Remark= mLIGUE1List[i].LRemark.Trim(),
                });
            }
            grdEmployee.ItemsSource = uiList;
            grdEmployee.ScrollIntoView(grdEmployee.Items[grdEmployee.Items.Count - 1]);

        }

        private void SetDataGridRankingUI()
        {
            List<Other_League_Ranking_Data> uList = new List<Other_League_Ranking_Data>();

            for(int i=0; i<mLIGUE1RANKINGList.Count; i++)
            {
                LIGUE1_UBER_EATS_RANKING lur = mLIGUE1RANKINGList[i];

                uList.Add(new Other_League_Ranking_Data()
                {
                    Ranking = int.Parse(mLIGUE1RANKINGList[i].LRRanking.Trim()),
                    Team_Logo = mLIGUE1RANKINGList[i].LRTeam_Name.Trim(),
                    Team_Name = mLIGUE1RANKINGList[i].LRTeam_Name.Trim(),
                    Champions_CNT = mLIGUE1RANKINGList[i].LRChampions_CNT.Trim(),
                    Second_Place_CNT = mLIGUE1RANKINGList[i].LRRunnerUp_CNT.Trim(),
                    Third_Place_CNT = mLIGUE1RANKINGList[i].LRThird_CNT.Trim(),
                    Fourth_Place_CNT = mLIGUE1RANKINGList[i].LRFourth_CNT.Trim()
                });

            }
            Ranking_DataGrid.ItemsSource = uList;

        }

        private void Row_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int seelctedIndex = ((DataGrid)sender).SelectedIndex;

            LIGUE1_UBER_EATS cc = mLIGUE1List[seelctedIndex];
            League_Year_textBox.Text = cc.LLeague_Year.ToString();
            League_Year2_textBox.Text = cc.LLeague_Year.ToLower();
            Champion_Name_Textbox.Text = cc.LChampions.ToString();
            Runner_UP_Textbox.Text = cc.LSecond_Place.ToString();
            Remark_Textbox.Text = cc.LRemark.ToString();

            NpgsqlConnection conn = null;

            try
            {
                conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();

                string sql = "select Count(*) from LIGUE1_UBER_EATS t where t.champions = '" + Champion_Name_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year_textBox.Text + "';";

                NpgsqlCommand RankCmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader reader = RankCmd.ExecuteReader();

                string strWInsCount = string.Empty;

                while (reader.Read())
                {
                    strWInsCount = reader[0].ToString();
                }

                NumOfWins_Textbox.Text = strWInsCount;

                reader.Close();

                string Secsql = "select COUNT(*) from LIGUE1_UBER_EATS T where T.second_place = '" + Runner_UP_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year2_textBox.Text + "';";

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

                BitmapImage bitmap = new BitmapImage(new Uri("Image/LIGUE1_UBER_EATS_Team/" + image.TEAMNAME.ToString() + ".png", UriKind.Relative));
                ImageBrush brush = new ImageBrush(bitmap);
                imageRec.Fill = brush;

                BitmapImage runbit = new BitmapImage(new Uri("Image/LIGUE1_UBER_EATS_Team/" + image.SECOND_TEAMNAME.ToLower() + ".png", UriKind.Relative));
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

            string sql = "select * from LIGUE1_UBER_EATS lue order by league_year";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            mLIGUE1List.Clear();

            while (reader.Read())
            {
                LIGUE1_UBER_EATS lu = new LIGUE1_UBER_EATS(reader[0].ToString(),
                    reader[1].ToString(),
                    reader[2].ToString(),
                    reader[3].ToString(),
                    reader[4].ToString(),
                    reader[5].ToString());

                mLIGUE1List.Add(lu);
            }

            SetDataGridUsingQueryResultList();

            reader.Close();


            // DataGrid에 있는 최선 정보 txt파일에 저장
            string sOption = "LIGUE1_UBER_EATS";

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\" + sOption + ".txt";

            FileStream stream = File.Create(path);
            stream.Close();

            if (File.Exists(path))
            {
                for (int i = 0; i < mLIGUE1List.Count; i++)
                {
                    File.AppendAllText(path, mLIGUE1List[i].LLeague_Year.Trim() + "\n");
                    File.AppendAllText(path, mLIGUE1List[i].LChampions.Trim() + "\n");
                    File.AppendAllText(path, mLIGUE1List[i].LSecond_Place.Trim() + "\n");
                    File.AppendAllText(path, mLIGUE1List[i].LThird_Place.Trim() + "\n");
                    File.AppendAllText(path, mLIGUE1List[i].LFourth_Place.Trim() + "\n");
                    File.AppendAllText(path, mLIGUE1List[i].LRemark.Trim() + "\n");
                }
            }

            string Ranksql = "select rank() over( order by t.CHAMPIONS_CNT desc) Ranking, t.team_name, t.CHAMPIONS_CNT , t.SECOND_PLACE_CNT,  " +
                "t.THIRD_PLACE_CNT, t.FOURTH_PLACE_CNT from LIGUE1_UBER_EATS_RANKING t " +
                "where t.SECOND_PLACE_CNT != 0 OR t.CHAMPIONS_CNT != 0 or t.THIRD_PLACE_CNT != 0 or t.FOURTH_PLACE_CNT != 0 " +
                "order by t.CHAMPIONS_CNT desc, " +
                "t.SECOND_PLACE_CNT desc, t.THIRD_PLACE_CNT desc, t.FOURTH_PLACE_CNT  desc";

            NpgsqlCommand RankCmd = new NpgsqlCommand(Ranksql, conn);
            NpgsqlDataReader Rankreader = RankCmd.ExecuteReader();

            mLIGUE1RANKINGList.Clear();

            while (Rankreader.Read())
            {
                LIGUE1_UBER_EATS_RANKING ccr = new LIGUE1_UBER_EATS_RANKING(Rankreader[0].ToString(),
                    Rankreader[1].ToString(),
                    Rankreader[2].ToString(),
                    Rankreader[3].ToString(),
                    Rankreader[4].ToString(),
                    Rankreader[5].ToString());

                mLIGUE1RANKINGList.Add(ccr);
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
            else if(e.Key == Key.C)
            {
                this.Close();
                BUNDESLIGA b = new BUNDESLIGA();
                b.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                b.Show();
            }
            else if(e.Key == Key.X)
            {
                this.Close();
                LALIGA_SANTANDER ls = new LALIGA_SANTANDER();
                ls.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ls.Show();
            }
        }
    }
}
