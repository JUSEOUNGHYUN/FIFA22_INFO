//#define WORKING_HOME

using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FIFA22_INFO
{
    /// <summary>
    /// DB_Insert.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DB_Insert : Window
    {

#if WORKING_HOME

        string mConnString = "HOST=localhost;PORT=5432;USERNAME=postgres;PASSWORD=1234;DATABASE=postgres;";
#else
        string mConnString = "HOST=localhost;PORT=5432;USERNAME=postgres;PASSWORD=1234;DATABASE=FIFA22;";
#endif

        List<string> mNonTeamList = new List<string>();
        List<string> mTeamList = new List<string>();

        public DB_Insert()
        {
            InitializeComponent();

            LeagueOption_comboBox.Items.Add("PREMIER_LEAGUE");          // 0
            LeagueOption_comboBox.Items.Add("EMIRATES_FA_CUP");         // 1
            LeagueOption_comboBox.Items.Add("CARABAO_CUP");             // 2

            LeagueOption_comboBox.Items.Add("CHAMPIONS_LEAGUE");        // 3
            LeagueOption_comboBox.Items.Add("EUROPA_LEAGUE");           // 4
            LeagueOption_comboBox.Items.Add("CONFERENCE_LEAGUE");       // 5
            LeagueOption_comboBox.Items.Add("SUPER_CUP");               // 6

            LeagueOption_comboBox.Items.Add("LIGUE1_UBER_EATS");        // 7
            LeagueOption_comboBox.Items.Add("BUNDESLIGA");              // 8
            LeagueOption_comboBox.Items.Add("SERIE_A");                 // 9
            LeagueOption_comboBox.Items.Add("EREDIVISIE");              // 10
            LeagueOption_comboBox.Items.Add("LIGA_PORTUGAL");         // 11
            LeagueOption_comboBox.Items.Add("LALIGA_SANTANDER");        // 12

            LeagueOption_comboBox.SelectedIndex = 0;

            Delete_LeagueOption_comboBox.Items.Add("PREMIER_LEAGUE");          // 0
            Delete_LeagueOption_comboBox.Items.Add("EMIRATES_FA_CUP");         // 1
            Delete_LeagueOption_comboBox.Items.Add("CARABAO_CUP");             // 2

            Delete_LeagueOption_comboBox.Items.Add("CHAMPIONS_LEAGUE");        // 3
            Delete_LeagueOption_comboBox.Items.Add("EUROPA_LEAGUE");           // 4
            Delete_LeagueOption_comboBox.Items.Add("CONFERENCE_LEAGUE");       // 5
            Delete_LeagueOption_comboBox.Items.Add("SUPER_CUP");               // 6

            Delete_LeagueOption_comboBox.Items.Add("LIGUE1_UBER_EATS");        // 7
            Delete_LeagueOption_comboBox.Items.Add("BUNDESLIGA");              // 8
            Delete_LeagueOption_comboBox.Items.Add("SERIE_A");                 // 9
            Delete_LeagueOption_comboBox.Items.Add("EREDIVISIE");              // 10
            Delete_LeagueOption_comboBox.Items.Add("LIGA_PORTUGAL");         // 11
            Delete_LeagueOption_comboBox.Items.Add("LALIGA_SANTANDER");        // 12

            Delete_LeagueOption_comboBox.SelectedIndex = 0;
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

        // Sql Insert 
        private void INSERT_button_Click(object sender, RoutedEventArgs e)
        {
            InsertFunc();
        }

        private void InsertFunc()
        {
            NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);
            conn.Open();

            string strOption = LeagueOption_comboBox.SelectedItem.ToString();
            string NextYear = GetNestLeaguYear(strOption);

            // table에서 입력한 팀이름이 있는 확인
            List<string> TeamList = new List<string>();
            string TeamSelectsql = string.Empty;

            int selectedIndex = LeagueOption_comboBox.SelectedIndex;

            if (selectedIndex >= 0 && selectedIndex <= 6)
            {
                if (selectedIndex < 0)
                {
                    MessageBox.Show("리그 옵션을 선택하세요", "리그 옵션", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (CHAMPION_textBox.Text == string.Empty)
                {
                    MessageBox.Show("우승팀을 입력하세요", "우승팀 입력", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (RUNNERUP_textBox.Text == string.Empty)
                {
                    MessageBox.Show("준 우승팀을 입력하세요", "준 우승팀 입력", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (selectedIndex >= 0 && CHAMPION_textBox.Text != string.Empty && RUNNERUP_textBox.Text != string.Empty)
                {
                    // table에서 입력한 팀이름이 있는 확인
                    TeamSelectsql = "";

                    try
                    {
                        //string sql = "insert into " + strOption + " (LEAGUE_YEAR, CHAMPIONS, SECOND_PLACE, REMARK) values ('" + NextYear + "' , 'SAMSUNG FC' , 'LIVERPOOL' , '');";
                        NpgsqlCommand cmd = new NpgsqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = "insert into " + strOption + " values (@LEAGUE_YEAR, @CHAMPIONS, @SECOND_PLACE, @REMARK)";
                        cmd.Parameters.AddWithValue("LEAGUE_YEAR", NextYear);
                        cmd.Parameters.AddWithValue("CHAMPIONS", CHAMPION_textBox.Text);
                        cmd.Parameters.AddWithValue("SECOND_PLACE", RUNNERUP_textBox.Text);
                        cmd.Parameters.AddWithValue("REMARK", REMARK_textBox.Text);
                        cmd.ExecuteNonQuery();

                        UpdateTotal(1, CHAMPION_textBox.Text, RUNNERUP_textBox.Text);

                        MessageBox.Show("입력이 완료되었습니다.", "DB Insert", MessageBoxButton.OK, MessageBoxImage.Information);
                        CHAMPION_textBox.Clear();
                        RUNNERUP_textBox.Clear();
                        THIRD_textBox.Clear();
                        FOUTH_textBox.Clear();
                        REMARK_textBox.Clear();
                        CHAMPION_textBox.Focus();

                        LeagueOption_comboBox.SelectedIndex = selectedIndex + 1;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Postgresql Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        //conn.Close();
                    }
                }
            }
            else
            {
                if (CHAMPION_textBox.Text == string.Empty)
                {
                    MessageBox.Show("우승팀을 입력하세요", "우승팀 입력", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (RUNNERUP_textBox.Text == string.Empty)
                {
                    MessageBox.Show("준 우승팀을 입력하세요", "준 우승팀 입력", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (THIRD_textBox.Text == string.Empty)
                {
                    MessageBox.Show("3등 팀을 입력하세요", "3등 팀 입력", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (FOUTH_textBox.Text == string.Empty)
                {
                    MessageBox.Show("4등 팀을 입력하세요", "4등 팀 입력", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else // 4팀을 다 입력했다면...
                {
                    mTeamList.Clear();
                    // table에서 입력한 팀 이름이 있는 확인 === 보류
                    TeamSelectsql = "select distinct (champions) from " + strOption + " union " +
                        "select distinct (SECOND_PLACE) from " + strOption + " union " +
                        "select distinct (THIRD_PLACE) from " + strOption + " union " +
                        "select distinct (FOURTH_PLACE) from " + strOption + " order by champions";

                    NpgsqlCommand cmd1 = new NpgsqlCommand(TeamSelectsql, conn);
                    NpgsqlDataReader reader1 = cmd1.ExecuteReader();

                    while (reader1.Read())
                    {
                        mTeamList.Add(reader1[0].ToString().Trim());
                    }

                    reader1.Close();

                    try
                    {
                        NpgsqlCommand cmd = new NpgsqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = "insert into " + strOption + " values (@LEAGUE_YEAR, @CHAMPIONS, @SECOND_PLACE, @THIRD_PLACE, @FOURTH_PLACE ,@REMARK)";
                        cmd.Parameters.AddWithValue("LEAGUE_YEAR", NextYear);
                        cmd.Parameters.AddWithValue("CHAMPIONS", CHAMPION_textBox.Text);
                        cmd.Parameters.AddWithValue("SECOND_PLACE", RUNNERUP_textBox.Text);
                        cmd.Parameters.AddWithValue("THIRD_PLACE", THIRD_textBox.Text);
                        cmd.Parameters.AddWithValue("FOURTH_PLACE", FOUTH_textBox.Text);
                        cmd.Parameters.AddWithValue("REMARK", REMARK_textBox.Text);
                        cmd.ExecuteNonQuery();

                        ManagedOtherLeagueTotal(1, CHAMPION_textBox.Text, RUNNERUP_textBox.Text, THIRD_textBox.Text, FOUTH_textBox.Text);
                        MessageBox.Show("입력이 완료되었습니다.", "DB Insert", MessageBoxButton.OK, MessageBoxImage.Information);
                        CHAMPION_textBox.Clear();
                        RUNNERUP_textBox.Clear();
                        THIRD_textBox.Clear();
                        FOUTH_textBox.Clear();
                        REMARK_textBox.Clear();
                        CHAMPION_textBox.Focus();

                        // 2023_08_15 테스트 진행 예정
                        if (selectedIndex == 12)
                        {
                            LeagueOption_comboBox.SelectedIndex = 0;
                        }
                        else
                        {
                            LeagueOption_comboBox.SelectedIndex = selectedIndex + 1;
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Postgresql Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {

                    }
                }
            }
        }

        // Sql Delete
        private void DELETE_button_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);

            conn.Open();

            int selectedIndex = Delete_LeagueOption_comboBox.SelectedIndex;
            string sOption = Delete_LeagueOption_comboBox.SelectedItem.ToString();
            string deletesql = "delete from " + sOption + " where league_year = '" + League_Year_textBox.Text + "';";

            // Premier_League , facup, carabaocup  == England_Total || champions_league, europa, conference == Total
            if (selectedIndex >= 0 && selectedIndex <= 6)
            {
                string Selectsql = "select * from " + Delete_LeagueOption_comboBox.SelectedItem.ToString() + " where league_year = '" + League_Year_textBox.Text + "';";

                NpgsqlCommand Selectcmd = new NpgsqlCommand(Selectsql, conn);
                NpgsqlDataReader Selectreader = Selectcmd.ExecuteReader();

                string strChampions = string.Empty;
                string strSecond = string.Empty;

                while (Selectreader.Read())
                {
                    strChampions = Selectreader[1].ToString().Trim();
                    strSecond = Selectreader[2].ToString().Trim();
                }

                Selectreader.Close();

                NpgsqlCommand DeleteCmd = new NpgsqlCommand(deletesql, conn);
                DeleteCmd.ExecuteNonQuery();

                UpdateTotal(-1, strChampions, strSecond);
            }
            else
            {
                string deleteOtherLeagueSql = "select * from " + sOption + " lue where league_year = '" + League_Year_textBox.Text + "';";

                NpgsqlCommand cmd = new NpgsqlCommand(deleteOtherLeagueSql, conn);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                string s1 = "";
                string s2 = "";
                string s3 = "";
                string s4 = "";

                while (reader.Read())
                {
                    s1 = reader[1].ToString();
                    s2 = reader[2].ToString();
                    s3 = reader[3].ToString();
                    s4 = reader[4].ToString();
                }

                reader.Close();

                NpgsqlCommand DeleteCmd = new NpgsqlCommand(deletesql, conn);
                DeleteCmd.ExecuteNonQuery();

                ManagedOtherLeagueTotal(-1, s1, s2, s3, s4);
                MessageBox.Show("입력이 완료되었습니다.", "DB Insert", MessageBoxButton.OK, MessageBoxImage.Information);
                League_Year_textBox.Clear();
            }
        }

        // Premier_League, EMIRATES_FA_CUP, CARABAO_CUP, CHAMPIONS_LEAGUE, EUROPA_LEAGUE, CONFERENCE_CUP, SUPER CUP
        private void UpdateTotal(int n, string s1, string s2)
        {
            NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);

            conn.Open();

            string sOption = string.Empty;
            string sTableName = string.Empty;
            int selectedIndex = 0;
            if (n==1)
            {
                sOption = LeagueOption_comboBox.SelectedItem.ToString();
                selectedIndex = LeagueOption_comboBox.SelectedIndex;
            }
            if(n== -1)
            {
                sOption = Delete_LeagueOption_comboBox.SelectedItem.ToString();
                selectedIndex = Delete_LeagueOption_comboBox.SelectedIndex;
            }

            if(selectedIndex >= 0 && selectedIndex <=2)
            {
                sTableName = "england_total";
            }
            else if(selectedIndex >=3 && selectedIndex <=6)
            {
                sTableName = "TOTAL";
            }

            string sql = "";
            string sql1 = "";
            string UpdateSql = "";
            string UpdateSql1 = "";

            int nChampionsCnt = 0;
            int nRunnerUpCnt = 0;

            /*
            select t.premier_league_run_cnt  from england_total t
            where t.team_name ='LIVERPOOL';
             */

            sql = "select (select t."+ sOption + "_winner_cnt from "+ sTableName + " t where t.team_name = '" + s1 + "') , " +
                "(select t."+ sOption + "_run_cnt from "+ sTableName + " t where t.team_name =  '" + s2 + "')";
            /*
            switch (selectedIndex)
            {
                case 0:
                    sql = "select t.premier_league_winner_cnt from england_total t where t.team_name = '"
                        + CHAMPION_textBox.Text + "';";
                    sql1 = "select t.premier_league_run_cnt from england_total t where t.team_name =  '"
                        + RUNNERUP_textBox.Text + "';";
                    break;

                case 1:
                    sql = "select t.EMIRATES_FA_CUP_WINNER_CNT from england_total t where t.team_name = '"
                        + CHAMPION_textBox.Text + "';";
                    sql1 = "select t.EMIRATES_FA_CUP_RUN_CNT from england_total t where t.team_name =  '"
                        + RUNNERUP_textBox.Text + "';";
                    break;

                case 2:
                    sql = "select t.CARABAO_CUP_WINNER_CNT from england_total t where t.team_name = '"
                        + CHAMPION_textBox.Text + "';";
                    sql1 = "select t.CARABAO_CUP_RUN_CNT from england_total t where t.team_name =  '"
                        + RUNNERUP_textBox.Text + "';";
                    break;
            }
             */

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                nChampionsCnt = int.Parse(reader[0].ToString());
                nRunnerUpCnt = int.Parse(reader[1].ToString());
            }

            reader.Close();

            nChampionsCnt += n;
            nRunnerUpCnt += n;

            UpdateSql = "update "+ sTableName + " set " + sOption +"_winner_cnt = " + nChampionsCnt.ToString() + " where team_name = '" + s1 + "';";
            UpdateSql1 = "update "+ sTableName + " set " + sOption + "_run_cnt = " + nRunnerUpCnt.ToString() + " where team_name = '" + s2 + "';";

            /*
            switch (selectedIndex)
            {
                case 0:
                    UpdateSql = "update england_total set premier_league_winner_cnt = " + nChampionsCnt.ToString() + " where team_name = '" + CHAMPION_textBox.Text + "';";
                    UpdateSql1 = "update england_total set premier_league_run_cnt = " + nRunnerUpCnt.ToString() + " where team_name = '" + RUNNERUP_textBox.Text + "';";
                    break;

                case 1:
                    UpdateSql = "update england_total set EMIRATES_FA_CUP_WINNER_CNT = " + nChampionsCnt.ToString() + " where team_name = '" + CHAMPION_textBox.Text + "';";
                    UpdateSql1 = "update england_total set EMIRATES_FA_CUP_RUN_CNT = " + nRunnerUpCnt.ToString() + " where team_name = '" + RUNNERUP_textBox.Text + "';";
                    break;

                case 2:
                    UpdateSql = "update england_total set CARABAO_CUP_WINNER_CNT = " + nChampionsCnt.ToString() + " where team_name = '" + CHAMPION_textBox.Text + "';";
                    UpdateSql1 = "update england_total set CARABAO_CUP_RUN_CNT = " + nRunnerUpCnt.ToString() + " where team_name = '" + RUNNERUP_textBox.Text + "';";
                    break;

                default:
                    break;
            }
             */
            
            try
            {
                NpgsqlCommand UpdateCommand = new NpgsqlCommand(UpdateSql, conn);
                UpdateCommand.ExecuteNonQuery();

                NpgsqlCommand UpdateCommand1 = new NpgsqlCommand(UpdateSql1, conn);
                UpdateCommand1.ExecuteNonQuery();

                //MessageBox.Show("업데이트가 완료되었습니다.", "DB Update", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Postgresql Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        // LIGUE1_UBER_EATS, BUNDESLIGA, SERIE_A, EREDIVISIE, LIGA_PORTUGAL, LALIGA_SANTANDER
        private void ManagedOtherLeagueTotal(int n, string s1, string s2, string s3, string s4)
        {
            NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);
            conn.Open();

            int selectedIndex = 0;
            string sOption = string.Empty;

            if(n==1)
            {
                selectedIndex = LeagueOption_comboBox.SelectedIndex;
                sOption = LeagueOption_comboBox.SelectedItem.ToString();

                int ns1 = mTeamList.IndexOf(s1);
                int ns2 = mTeamList.IndexOf(s2);
                int ns3 = mTeamList.IndexOf(s3);
                int ns4 = mTeamList.IndexOf(s4);

                string sInsertRankingSql = string.Empty;


                if (ns1 == -1)
                {
                    sInsertRankingSql = "insert into " + sOption + "_RANKING  (TEAM_NAME, CHAMPIONS_CNT, SECOND_PLACE_CNT, THIRD_PLACE_CNT, FOURTH_PLACE_CNT) values ('" + s1 + "', 0,0,0,0);";
                    mNonTeamList.Add(sInsertRankingSql);
                }
                else if(ns2 == -1)
                {
                    sInsertRankingSql = "insert into " + sOption + "_RANKING  (TEAM_NAME, CHAMPIONS_CNT, SECOND_PLACE_CNT, THIRD_PLACE_CNT, FOURTH_PLACE_CNT) values ('" + s2 + "', 0,0,0,0);";
                    mNonTeamList.Add(sInsertRankingSql);
                }
                else if(ns3 == -1)
                {
                    sInsertRankingSql = "insert into " + sOption + "_RANKING  (TEAM_NAME, CHAMPIONS_CNT, SECOND_PLACE_CNT, THIRD_PLACE_CNT, FOURTH_PLACE_CNT) values ('" + s3 + "', 0,0,0,0);";
                    mNonTeamList.Add(sInsertRankingSql);
                }
                else if(ns4 == -1)
                {
                    // insert into LIGA_PORTUGAL_RANKING (TEAM_NAME, CHAMPIONS_CNT, SECOND_PLACE_CNT, THIRD_PLACE_CNT, FOURTH_PLACE_CNT) values ('VITORIA SC',1,1,4,2); 
                    sInsertRankingSql = "insert into " + sOption + "_RANKING  (TEAM_NAME, CHAMPIONS_CNT, SECOND_PLACE_CNT, THIRD_PLACE_CNT, FOURTH_PLACE_CNT) values ('" + s4 + "', 0,0,0,0);";
                    mNonTeamList.Add(sInsertRankingSql);
                }

                for(int i=0; i<mNonTeamList.Count; i++)
                {
                    NpgsqlCommand NonTeamInsertcmd = new NpgsqlCommand();
                    NonTeamInsertcmd.Connection = conn;
                    NonTeamInsertcmd.CommandText = mNonTeamList[i];
                    NonTeamInsertcmd.ExecuteNonQuery();
                }
            }
            if(n== -1)
            {
                selectedIndex = Delete_LeagueOption_comboBox.SelectedIndex;
                sOption = Delete_LeagueOption_comboBox.SelectedItem.ToString();
            }

            string sql = "select (select CHAMPIONS_CNT from " + sOption + "_RANKING t where team_name = '" + s1 + "')," +
                "(select SECOND_PLACE_CNT from " + sOption + "_RANKING t where team_name = '" + s2 + "')," +
                "(select THIRD_PLACE_CNT from " + sOption + "_RANKING t where team_name = '" + s3 + "')," +
                "(select FOURTH_PLACE_CNT from " + sOption + "_RANKING t where team_name = '" + s4 + "');";

            int nChampionsCnt = 0;
            int nRunnerUpCnt = 0;
            int nThirdCnt = 0;
            int nFourthCnt = 0;

            // Champions Count
            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            string str1 = string.Empty;
            string str2 = string.Empty;
            string str3 = string.Empty;
            string str4 = string.Empty;

            while (reader.Read())
            {
                if (reader[0] != DBNull.Value)
                {
                    nChampionsCnt = int.Parse(reader[0].ToString());
                }
                else
                {
                    nChampionsCnt = 0;
                }
                if (reader[1] != DBNull.Value)
                {
                    nRunnerUpCnt = int.Parse(reader[1].ToString());
                }
                else
                {
                    nRunnerUpCnt = 0;
                }
                if (reader[2] != DBNull.Value)
                {
                    nThirdCnt = int.Parse(reader[2].ToString());
                }
                else
                {
                    nThirdCnt = 0;
                }
                if (reader[3] != DBNull.Value)
                {
                    nFourthCnt = int.Parse(reader[3].ToString());
                }
                else
                {
                    nFourthCnt = 0;
                }
            }

            reader.Close();

            nChampionsCnt += n;
            nRunnerUpCnt += n;
            nThirdCnt += n;
            nFourthCnt += n;

            string UpdateSql = "UPDATE " + sOption + "_RANKING set champions_cnt = " + nChampionsCnt.ToString() + " where team_name = '" + s1 + "';";
            string UpdateSql1 = "UPDATE " + sOption + "_RANKING set SECOND_PLACE_CNT = " + nRunnerUpCnt.ToString() + " where team_name = '" + s2+ "';";
            string UpdateSql2 = "UPDATE " + sOption + "_RANKING set THIRD_PLACE_CNT = " + nThirdCnt.ToString() + " where team_name = '" + s3 + "';";
            string UpdateSql3 = "UPDATE " + sOption + "_RANKING set FOURTH_PLACE_CNT = " + nFourthCnt.ToString() + " where team_name = '" + s4 + "';";

            try
            {
                NpgsqlCommand UpdateCommand = new NpgsqlCommand(UpdateSql, conn);
                UpdateCommand.ExecuteNonQuery();

                NpgsqlCommand UpdateCommand1 = new NpgsqlCommand(UpdateSql1, conn);
                UpdateCommand1.ExecuteNonQuery();

                NpgsqlCommand UpdateCommand2 = new NpgsqlCommand(UpdateSql2, conn);
                UpdateCommand2.ExecuteNonQuery();

                NpgsqlCommand UpdateCommand3 = new NpgsqlCommand(UpdateSql3, conn);
                UpdateCommand3.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Postgresql Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetNestLeaguYear(string strOption)
        {
            string lastYear = string.Empty;

            string sql = "select pl.league_year from " + strOption + " pl order by pl.league_year desc limit 1;";

            NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);

            conn.Open();

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read()) 
            {
                lastYear = reader[0].ToString().Trim();
            }

            string strNestYear = string.Empty;
            reader.Close();

            List<string> yearList = lastYear.Split('/').ToList();

            int nFront = int.Parse(yearList[0]);
            int nBack = int.Parse(yearList[1]);

            nFront += 1;
            nBack += 1;

            lastYear = nFront.ToString().PadLeft(2,'0') + "/" + nBack.ToString().PadLeft(2, '0');

            return lastYear;
        }

        #region -- textBox_PreviewTextInput
        private void CHAMPION_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void RUNNERUP_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void THIRD_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void FOUTH_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void REMARK_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        #endregion

        #region -- Delete Input Year TextBox 왼쪽에 년도 입력했으면 자동으로 다음 년도 써지게...

        private void YEAR_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            string str = string.Empty;

            int N = League_Year_textBox.Text.Length;
        }

        private void year_TextChanged(object sender, TextChangedEventArgs e)
        {
            int n = League_Year_textBox.Text.Length;
            string str = string.Empty;

            if(n == 4)
            {
                str = League_Year_textBox.Text.Substring(2, 2);
                int df = int.Parse(str) + 1;
                if(df == 100)
                {
                    df = 0;
                }

                League_Year_textBox.Text += "/" + df.ToString().PadLeft(2,'0');
            }
            System.Diagnostics.Trace.WriteLine("Row_SelectionChanged m_nSeletedRow=[" + str + "] m_nSelectedCol=[" + str + "]");
        }

        #endregion

        private void Champion_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            AllTeam at = new AllTeam();
            at.DataPassProdCd += new AllTeam.DataPassProdCdEventHandler(TeamNameReceive);
            at.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            at.ShowDialog();

        }

        private void TeamNameReceive(string sTeamName)
        {
            CHAMPION_textBox.Text = sTeamName;
        }

        private void TeamNameReceive2(string sTeamName)
        {
            RUNNERUP_textBox.Text = sTeamName;
        }

        private void TeamNameReceive3(string sTeamName)
        {
            THIRD_textBox.Text = sTeamName;
        }

        private void TeamNameReceive4(string sTeamName)
        {
            FOUTH_textBox.Text = sTeamName;
        }

        private void Second_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            AllTeam at = new AllTeam();
            at.DataPassProdCd += new AllTeam.DataPassProdCdEventHandler(TeamNameReceive2);
            at.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            at.ShowDialog();
        }

        private void Third_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            AllTeam at = new AllTeam();
            at.DataPassProdCd += new AllTeam.DataPassProdCdEventHandler(TeamNameReceive3);
            at.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            at.ShowDialog();
        }

        private void Fourth_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            AllTeam at = new AllTeam();
            at.DataPassProdCd += new AllTeam.DataPassProdCdEventHandler(TeamNameReceive4);
            at.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            at.ShowDialog();
        }

        private void keyDown_Event(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void Remark_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                InsertFunc();
            }
        }
    }
}
