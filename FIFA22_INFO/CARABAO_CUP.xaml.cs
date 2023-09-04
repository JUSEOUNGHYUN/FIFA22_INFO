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
    /// CARABAO_CUP.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public class CARABAOCUP
    {
        public string CLeague_Year;
        public string CChampions;
        public string CSecond_Place;
        public string CRemark;

        public CARABAOCUP(string cLeague_Year, string cChampions, string cSecond_Place, string cRemark)
        {
            CLeague_Year = cLeague_Year;
            CChampions = cChampions;
            CSecond_Place = cSecond_Place;
            CRemark = cRemark;
        }
    }

    public class CARABAOCUP_RANKING
    {
        public string CRRanking;
        public string CRTeam_Name;
        public string CRChampions_CNT;
        public string CRRunnerUp_CNT;

        public CARABAOCUP_RANKING(string cRRanking, string cRTeam_Name, string cRChampions_CNT, string cRRunnerUp_CNT)
        {
            CRRanking = cRRanking;
            CRTeam_Name = cRTeam_Name;
            CRChampions_CNT = cRChampions_CNT;
            CRRunnerUp_CNT = cRRunnerUp_CNT;
        }
    }

    public partial class CARABAO_CUP : Window
    {

        public static List<CARABAOCUP> mCARABAOList = new List<CARABAOCUP>();
        public static List<CARABAOCUP_RANKING> mCARARANKINGList = new List<CARABAOCUP_RANKING>();

        public string m_sTeamName = string.Empty;
        public string m_sOption = string.Empty;

        public CARABAO_CUP()
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

            for(int i=0; i<mCARABAOList.Count; i++)
            {
                CARABAOCUP cc = mCARABAOList[i];

                uiList.Add(new Premier_League_Data()
                {
                    League_Year = mCARABAOList[i].CLeague_Year.Trim(),
                    ChampionsLOGO = mCARABAOList[i].CChampions.Trim(),
                    Champions = mCARABAOList[i].CChampions.Trim(),
                    Second_PlaceLOGO = mCARABAOList[i].CSecond_Place.Trim(),
                    Second_Place = mCARABAOList[i].CSecond_Place.Trim(),
                    Remark = mCARABAOList[i].CRemark.Trim()
                });
            }
            grdEmployee.ItemsSource = uiList;
            grdEmployee.ScrollIntoView(grdEmployee.Items[grdEmployee.Items.Count - 1]);
        }

        private void SetDataGridRankingUI()
        {
            List<EnglandTotalData> uList = new List<EnglandTotalData>();

            for(int i=0; i<mCARARANKINGList.Count; i++)
            {
                CARABAOCUP_RANKING ccr = mCARARANKINGList[i];

                uList.Add(new EnglandTotalData()
                {
                    Ranking = int.Parse(mCARARANKINGList[i].CRRanking.Trim()),
                    Team_Logo = mCARARANKINGList[i].CRTeam_Name.Trim(),
                    Team_Name = mCARARANKINGList[i].CRTeam_Name.Trim(),
                    Champions_CNT = mCARARANKINGList[i].CRChampions_CNT.Trim(),
                    Second_Place_CNT = mCARARANKINGList[i].CRRunnerUp_CNT.Trim()
                });
            }

            Ranking_DataGrid.ItemsSource = uList;
        }

        private void Row_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int seelctedIndex = ((DataGrid)sender).SelectedIndex;

            CARABAOCUP cc = mCARABAOList[seelctedIndex];
            League_Year_textBox.Text = cc.CLeague_Year.ToString();
            League_Year2_textBox.Text = cc.CLeague_Year.ToLower();
            Champion_Name_Textbox.Text = cc.CChampions.ToString();
            Runner_UP_Textbox.Text = cc.CSecond_Place.ToString();
            Remark_Textbox.Text = cc.CRemark.ToString();

            NpgsqlConnection conn = null;

            try
            {
                conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();

                string sql = "select Count(*) from CARABAO_CUP t where t.champions = '" + Champion_Name_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year_textBox.Text + "';";

                NpgsqlCommand RankCmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader reader = RankCmd.ExecuteReader();

                string strWInsCount = string.Empty;

                while (reader.Read())
                {
                    strWInsCount = reader[0].ToString();
                }

                NumOfWins_Textbox.Text = strWInsCount;

                reader.Close();

                string Secsql = "select COUNT(*) from CARABAO_CUP T where T.second_place = '" + Runner_UP_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year2_textBox.Text + "';";

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
            NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);
            conn.Open();
            List<string> list = new List<string>();

            string sql = "select * from CARABAO_CUP t order by league_year";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            mCARABAOList.Clear();

            while (reader.Read())
            {
                CARABAOCUP cc = new CARABAOCUP(reader[0].ToString(),
                    reader[1].ToString(),
                    reader[2].ToString(),
                    reader[3].ToString());

                mCARABAOList.Add(cc);
            }

            SetDataGridUsingQueryResultList();

            reader.Close();

            // DataGrid에 있는 최선 정보 txt파일에 저장
            string sOption = "CARABAO_CUP";

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\" + sOption + ".txt";

            FileStream stream = File.Create(path);
            stream.Close();

            if (File.Exists(path))
            {
                for (int i = 0; i < mCARABAOList.Count; i++)
                {
                    File.AppendAllText(path, mCARABAOList[i].CLeague_Year.Trim() + "\n");
                    File.AppendAllText(path, mCARABAOList[i].CChampions.Trim() + "\n");
                    File.AppendAllText(path, mCARABAOList[i].CSecond_Place.Trim() + "\n");
                    File.AppendAllText(path, mCARABAOList[i].CRemark.Trim() + "\n");
                }
            }

            string Ranksql = "select rank() over( order by t.CARABAO_CUP_WINNER_CNT desc) Ranking, t.team_name, t.CARABAO_CUP_WINNER_CNT , t.CARABAO_CUP_RUN_CNT  " +
                "from england_total t " +
                "where t.CARABAO_CUP_RUN_CNT != 0 OR t.CARABAO_CUP_WINNER_CNT != 0 " +
                "order by t.CARABAO_CUP_WINNER_CNT desc, " +
                "t.CARABAO_CUP_RUN_CNT desc;";

            NpgsqlCommand RankCmd = new NpgsqlCommand(Ranksql, conn);
            NpgsqlDataReader Rankreader = RankCmd.ExecuteReader();

            mCARARANKINGList.Clear();

            while (Rankreader.Read())
            {
                CARABAOCUP_RANKING ccr = new CARABAOCUP_RANKING(Rankreader[0].ToString(),
                    Rankreader[1].ToString(),
                    Rankreader[2].ToString(),
                    Rankreader[3].ToString());

                mCARARANKINGList.Add(ccr);
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
            else if(e.Key == Key.S)
            {
                SearchFunc();
            }
            else if(e.Key == Key.C)
            {
                this.Close();
                EFL_CHAPIONSHIP pl = new EFL_CHAPIONSHIP();
                pl.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                pl.Show();
            }
            else if(e.Key == Key.X)
            {
                this.Close();
                EMIRATES_FA_CUP fa = new EMIRATES_FA_CUP();
                fa.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                fa.Show();
            }
        }

        private void Ranking_DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            CARABAOCUP_RANKING ccr = mCARARANKINGList[selectedIndex];

            string teamName = ccr.CRTeam_Name.Trim();
            string sOption = "CARABAO_CUP";

            TeamCareer tc = new TeamCareer();
            tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tc.Show();
            tc.GetTeam(teamName, sOption);
        }

        private void Ranking_DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            CARABAOCUP_RANKING ccr = mCARARANKINGList[selectedIndex];

            string teamName = ccr.CRTeam_Name.Trim();
            string sOption = "CARABAO_CUP";

            if(e.Key == Key.Enter)
            {
                TeamCareer tc = new TeamCareer();
                tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                tc.Show();
                tc.GetTeam(teamName, sOption);
            }
        }
    }
}
