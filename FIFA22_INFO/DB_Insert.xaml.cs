//#define WORKING_HOME

using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
        //string mConnString = "HOST=localhost;PORT=5432;USERNAME=postgres;PASSWORD=1234;DATABASE=FIFA22;";
#endif

        List<string> mNonTeamList = new List<string>();
        List<string> mTeamList = new List<string>();

        public DB_Insert()
        {
            InitializeComponent();
            InputMethod.SetIsInputMethodEnabled(this.TeamName_textBox, false);

            LeagueOption_comboBox.Items.Add("PREMIER_LEAGUE");          // 0
            LeagueOption_comboBox.Items.Add("EMIRATES_FA_CUP");         // 1
            LeagueOption_comboBox.Items.Add("CARABAO_CUP");             // 2

            LeagueOption_comboBox.Items.Add("CHAMPIONS_LEAGUE");        // 3
            LeagueOption_comboBox.Items.Add("EUROPA_LEAGUE");           // 4
            LeagueOption_comboBox.Items.Add("CONFERENCE_LEAGUE");       // 5
            LeagueOption_comboBox.Items.Add("SUPER_CUP");               // 6

            LeagueOption_comboBox.Items.Add("JUPILER_PRO_LEAGUE");        // 7
            LeagueOption_comboBox.Items.Add("EFL_CHAMPIONSHIP");        // 8
            LeagueOption_comboBox.Items.Add("LIGUE1_UBER_EATS");        // 9
            LeagueOption_comboBox.Items.Add("BUNDESLIGA");              // 10
            LeagueOption_comboBox.Items.Add("SERIE_A");                 // 11
            LeagueOption_comboBox.Items.Add("EREDIVISIE");              // 12
            LeagueOption_comboBox.Items.Add("LIGA_PORTUGAL");         // 13
            LeagueOption_comboBox.Items.Add("LALIGA_SANTANDER");        // 14
            LeagueOption_comboBox.Items.Add("LALIGA_SMARTBANK");        // 15

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
            Delete_LeagueOption_comboBox.Items.Add("LALIGA_SMARTBANK");        // 13

            Delete_LeagueOption_comboBox.SelectedIndex = 0;

            Update_LeagueOption_comboBox.Items.Add("PREMIER_LEAGUE");          // 0
            Update_LeagueOption_comboBox.Items.Add("EMIRATES_FA_CUP");         // 1
            Update_LeagueOption_comboBox.Items.Add("CARABAO_CUP");             // 2

            Update_LeagueOption_comboBox.Items.Add("CHAMPIONS_LEAGUE");        // 3
            Update_LeagueOption_comboBox.Items.Add("EUROPA_LEAGUE");           // 4
            Update_LeagueOption_comboBox.Items.Add("CONFERENCE_LEAGUE");       // 5
            Update_LeagueOption_comboBox.Items.Add("SUPER_CUP");               // 6

            Update_LeagueOption_comboBox.Items.Add("JUPILER_PRO_LEAGUE");        // 7
            Update_LeagueOption_comboBox.Items.Add("EFL_CHAMPIONSHIP");        // 8
            Update_LeagueOption_comboBox.Items.Add("LIGUE1_UBER_EATS");        // 7
            Update_LeagueOption_comboBox.Items.Add("BUNDESLIGA");              // 8
            Update_LeagueOption_comboBox.Items.Add("SERIE_A");                 // 9
            Update_LeagueOption_comboBox.Items.Add("EREDIVISIE");              // 10
            Update_LeagueOption_comboBox.Items.Add("LIGA_PORTUGAL");         // 11
            Update_LeagueOption_comboBox.Items.Add("LALIGA_SANTANDER");        // 12
            Update_LeagueOption_comboBox.Items.Add("LALIGA_SMARTBANK");        // 13

            Update_LeagueOption_comboBox.SelectedIndex = 0;
        }

        public void GetSelectedData(string sOption, string sLeagueYear, string sRemark)
        {
            Update_LeagueOption_comboBox.SelectedItem = sOption;
            Update_Vice_LeagueOption_comboBox.SelectedItem = "remark";
            UpdateContent_textBox.Text = sRemark;
            Condition_textBox.Text = sLeagueYear;
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

            try
            {
                conn.Open();
                mTeamList.Clear();

                string strOption = LeagueOption_comboBox.SelectedItem.ToString();
                string NextYear = GetNestLeaguYear(strOption);

                // table에서 입력한 팀이름이 있는 확인
                List<string> TeamList = new List<string>();
                string TeamSelectsql = string.Empty;

                int selectedIndex = LeagueOption_comboBox.SelectedIndex;

                if (selectedIndex >= 1 && selectedIndex <= 6)
                {
                    if (selectedIndex >= 1 && selectedIndex <= 2)
                    {
                        /*
                        TeamSelectsql = "select distinct(champions) from premier_league union select distinct(second_place) from premier_league union select distinct(champions) from emirates_fa_cup union select distinct(second_place) from emirates_fa_cup union select distinct(champions) from carabao_cup union " +
                            "select distinct(second_place) from carabao_cup order by champions;";
                         */
                        TeamSelectsql = "select distinct(champions) from emirates_fa_cup union select distinct(second_place) from emirates_fa_cup union select distinct(champions) from carabao_cup union " +
                            "select distinct(second_place) from carabao_cup order by champions;";

                        NpgsqlCommand cmd1 = new NpgsqlCommand(TeamSelectsql, conn);
                        NpgsqlDataReader reader1 = cmd1.ExecuteReader();

                        while (reader1.Read())
                        {
                            mTeamList.Add(reader1[0].ToString().Trim());
                        }

                        reader1.Close();
                    }
                    else if (selectedIndex >= 3 && selectedIndex <= 6)
                    {
                        TeamSelectsql = "select distinct(champions) from champions_league union select distinct(second_place) from champions_league union select distinct(champions) from europa_league union select distinct(second_place) from europa_league union select distinct(champions) from conference_league union " +
        "select distinct(second_place) from conference_league union select distinct(champions) from super_cup union select distinct(second_place) from super_cup order by champions;";

                        NpgsqlCommand cmd1 = new NpgsqlCommand(TeamSelectsql, conn);
                        NpgsqlDataReader reader1 = cmd1.ExecuteReader();

                        while (reader1.Read())
                        {
                            mTeamList.Add(reader1[0].ToString().Trim());
                        }

                        reader1.Close();
                    }

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
                            if (selectedIndex == 15)
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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

                int ns1 = mTeamList.IndexOf(s1);
                int ns2 = mTeamList.IndexOf(s2);

                string sInsertRankingSql = string.Empty;
                mNonTeamList.Clear();

                if (selectedIndex >= 0 && selectedIndex <= 2)
                {
                    sTableName = "england_total";
                    sInsertRankingSql = "insert into " + sTableName + " (TEAM_NAME, PREMIER_LEAGUE_WINNER_CNT, PREMIER_LEAGUE_RUN_CNT, EMIRATES_FA_CUP_WINNER_CNT, EMIRATES_FA_CUP_RUN_CNT,CARABAO_CUP_WINNER_CNT, CARABAO_CUP_RUN_CNT) values ('" + s1 + "',0,0,0,0,0,0);";
                }
                else if (selectedIndex >= 3 && selectedIndex <= 6)
                {
                    sTableName = "TOTAL";
                }

                // insert into TOTAL (TEAM_NAME, CHAMPIONS_LEAGUE_WINNER_CNT, CHAMPIONS_LEAGUE_RUN_CNT, EUROPA_LEAGUE_WINNER_CNT, EUROPA_LEAGUE_RUN_CNT, CONFERENCE_LEAGUE_WINNER_CNT,CONFERENCE_LEAGUE_RUN_CNT,  SUPER_CUP_WINNER_CNT, SUPER_CUP_RUN_CNT) values ('SAMSUNG FC',89,19,14,2,3,2,69,29); 

                if (ns1 == -1)
                {
                    if(sTableName == "england_total")
                    {
                        sInsertRankingSql = "insert into "+ sTableName + " (TEAM_NAME, PREMIER_LEAGUE_WINNER_CNT, PREMIER_LEAGUE_RUN_CNT, EMIRATES_FA_CUP_WINNER_CNT, EMIRATES_FA_CUP_RUN_CNT,CARABAO_CUP_WINNER_CNT, CARABAO_CUP_RUN_CNT) values ('" + s1 + "',0,0,0,0,0,0);";
                    }
                    else
                    {
                        sInsertRankingSql = "insert into " + sTableName + " (TEAM_NAME, CHAMPIONS_LEAGUE_WINNER_CNT, CHAMPIONS_LEAGUE_RUN_CNT, EUROPA_LEAGUE_WINNER_CNT, EUROPA_LEAGUE_RUN_CNT, CONFERENCE_LEAGUE_WINNER_CNT,CONFERENCE_LEAGUE_RUN_CNT,  SUPER_CUP_WINNER_CNT, SUPER_CUP_RUN_CNT) values ('" + s1 + "', 0,0,0,0,0,0,0,0);";
                    }
                    mNonTeamList.Add(sInsertRankingSql);
                }
                if(ns2 == -1)
                {

                    if (sTableName == "england_total")
                    {
                        sInsertRankingSql = "insert into " + sTableName + " (TEAM_NAME, PREMIER_LEAGUE_WINNER_CNT, PREMIER_LEAGUE_RUN_CNT, EMIRATES_FA_CUP_WINNER_CNT, EMIRATES_FA_CUP_RUN_CNT,CARABAO_CUP_WINNER_CNT, CARABAO_CUP_RUN_CNT) values ('" + s2 + "',0,0,0,0,0,0);";
                    }
                    else
                    {
                        sInsertRankingSql = "insert into " + sTableName + " (TEAM_NAME, CHAMPIONS_LEAGUE_WINNER_CNT, CHAMPIONS_LEAGUE_RUN_CNT, EUROPA_LEAGUE_WINNER_CNT, EUROPA_LEAGUE_RUN_CNT, CONFERENCE_LEAGUE_WINNER_CNT,CONFERENCE_LEAGUE_RUN_CNT,  SUPER_CUP_WINNER_CNT, SUPER_CUP_RUN_CNT) values ('" + s2 + "', 0,0,0,0,0,0,0,0);";
                    }
                    mNonTeamList.Add(sInsertRankingSql);
                }

                for (int i = 0; i < mNonTeamList.Count; i++)
                {
                    NpgsqlCommand NonTeamInsertcmd = new NpgsqlCommand();
                    NonTeamInsertcmd.Connection = conn;
                    NonTeamInsertcmd.CommandText = mNonTeamList[i];
                    NonTeamInsertcmd.ExecuteNonQuery();
                }
            }
            else if(n== -1)
            {
                sOption = Delete_LeagueOption_comboBox.SelectedItem.ToString();
                selectedIndex = Delete_LeagueOption_comboBox.SelectedIndex;
            }

            string sql = "";
            string UpdateSql = "";
            string UpdateSql1 = "";

            int nChampionsCnt = 0;
            int nRunnerUpCnt = 0;

            sql = "select (select t."+ sOption + "_winner_cnt from "+ sTableName + " t where t.team_name = '" + s1 + "') , " +
                "(select t."+ sOption + "_run_cnt from "+ sTableName + " t where t.team_name =  '" + s2 + "')";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
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
            }

            reader.Close();

            nChampionsCnt += n;
            nRunnerUpCnt += n;

            UpdateSql = "update "+ sTableName + " set " + sOption +"_winner_cnt = " + nChampionsCnt.ToString() + " where team_name = '" + s1 + "';";
            UpdateSql1 = "update "+ sTableName + " set " + sOption + "_run_cnt = " + nRunnerUpCnt.ToString() + " where team_name = '" + s2 + "';";

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
            try
            {
                conn.Open();

                int selectedIndex = 0;
                string sOption = string.Empty;

                if (n == 1)
                {
                    selectedIndex = LeagueOption_comboBox.SelectedIndex;
                    sOption = LeagueOption_comboBox.SelectedItem.ToString();

                    int ns1 = mTeamList.IndexOf(s1);
                    int ns2 = mTeamList.IndexOf(s2);
                    int ns3 = mTeamList.IndexOf(s3);
                    int ns4 = mTeamList.IndexOf(s4);

                    string sInsertRankingSql = string.Empty;
                    mNonTeamList.Clear();

                    if (ns1 == -1)
                    {
                        sInsertRankingSql = "insert into " + sOption + "_RANKING  (TEAM_NAME, CHAMPIONS_CNT, SECOND_PLACE_CNT, THIRD_PLACE_CNT, FOURTH_PLACE_CNT) values ('" + s1 + "', 0,0,0,0);";
                        mNonTeamList.Add(sInsertRankingSql);
                    }
                    if (ns2 == -1)
                    {
                        sInsertRankingSql = "insert into " + sOption + "_RANKING  (TEAM_NAME, CHAMPIONS_CNT, SECOND_PLACE_CNT, THIRD_PLACE_CNT, FOURTH_PLACE_CNT) values ('" + s2 + "', 0,0,0,0);";
                        mNonTeamList.Add(sInsertRankingSql);
                    }
                    if (ns3 == -1)
                    {
                        sInsertRankingSql = "insert into " + sOption + "_RANKING  (TEAM_NAME, CHAMPIONS_CNT, SECOND_PLACE_CNT, THIRD_PLACE_CNT, FOURTH_PLACE_CNT) values ('" + s3 + "', 0,0,0,0);";
                        mNonTeamList.Add(sInsertRankingSql);
                    }
                    if (ns4 == -1)
                    {
                        sInsertRankingSql = "insert into " + sOption + "_RANKING  (TEAM_NAME, CHAMPIONS_CNT, SECOND_PLACE_CNT, THIRD_PLACE_CNT, FOURTH_PLACE_CNT) values ('" + s4 + "', 0,0,0,0);";
                        mNonTeamList.Add(sInsertRankingSql);
                    }

                    for (int i = 0; i < mNonTeamList.Count; i++)
                    {
                        NpgsqlCommand NonTeamInsertcmd = new NpgsqlCommand();
                        NonTeamInsertcmd.Connection = conn;
                        NonTeamInsertcmd.CommandText = mNonTeamList[i];
                        NonTeamInsertcmd.ExecuteNonQuery();
                    }
                }
                if (n == -1)
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
                string UpdateSql1 = "UPDATE " + sOption + "_RANKING set SECOND_PLACE_CNT = " + nRunnerUpCnt.ToString() + " where team_name = '" + s2 + "';";
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Postgresql Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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

            if(lastYear == string.Empty || lastYear == "")
            {
                sql = "select pl.league_year from premier_league pl order by pl.league_year desc limit 1;";

                NpgsqlCommand cmd1 = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader reader1 = cmd1.ExecuteReader();

                while(reader1.Read())
                {
                    lastYear = reader1[0].ToString().Trim();
                }

                reader1.Close();
            }

            List<string> yearList = lastYear.Split('/').ToList();

            int nFront = int.Parse(yearList[0]);
            int nBack = int.Parse(yearList[1]);

            nFront += 1;
            nBack += 1;
            if(nFront == 100)
            {
                nFront = 0;
            }
            if(nBack == 100)
            {
                nBack = 0;
            }

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

        private void UpdateTeamNameReceive(string sTeamName)
        {
            TeamName_textBox.Text = sTeamName;
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

        private void UPDATE_button_Click(object sender, RoutedEventArgs e)
        {
            UpdateFunc();
        }

        private void Update_LeagueOption_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = Update_LeagueOption_comboBox.SelectedIndex;

            Update_Vice_LeagueOption_comboBox.Items.Clear();

            if (selectedIndex >= 0 && selectedIndex <= 6)
            {
                Update_Vice_LeagueOption_comboBox.Items.Add("league_year");
                Update_Vice_LeagueOption_comboBox.Items.Add("champions");
                Update_Vice_LeagueOption_comboBox.Items.Add("second_place");
                Update_Vice_LeagueOption_comboBox.Items.Add("remark");

                Update_Vice_LeagueOption_comboBox.SelectedIndex = 3;
            }
            else
            {
                Update_Vice_LeagueOption_comboBox.Items.Add("league_year");
                Update_Vice_LeagueOption_comboBox.Items.Add("champions");
                Update_Vice_LeagueOption_comboBox.Items.Add("second_place");
                Update_Vice_LeagueOption_comboBox.Items.Add("third_place");
                Update_Vice_LeagueOption_comboBox.Items.Add("fourth_place");
                Update_Vice_LeagueOption_comboBox.Items.Add("remark");

                Update_Vice_LeagueOption_comboBox.SelectedIndex = 5;
            }
        }

        private void UpdateFunc()
        {
            NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);
            conn.Open();

            string Updatesql = string.Empty;

            if (Update_Vice_LeagueOption_comboBox.Text == "remark")
            {
                Updatesql = "Update " + Update_LeagueOption_comboBox.Text + " set " + Update_Vice_LeagueOption_comboBox.Text + " = '" + UpdateContent_textBox.Text + " " + TeamName_textBox.Text + "' where league_year = '" + Condition_textBox.Text + "';";
            }
            else if(Update_Vice_LeagueOption_comboBox.Text == "champions" || Update_Vice_LeagueOption_comboBox.Text == "second_place" || Update_Vice_LeagueOption_comboBox.Text == "third_place" || Update_Vice_LeagueOption_comboBox.Text == "fourth_place")
            {
                Updatesql = "Update " + Update_LeagueOption_comboBox.Text + " set " + Update_Vice_LeagueOption_comboBox.Text + " = '" + TeamName_textBox.Text + "' where league_year = '" + Condition_textBox.Text + "';";
            }

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(Updatesql, conn);
                cmd.ExecuteNonQuery();

                MessageBox.Show("업데이트가 완료되었습니다.", "Update" , MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Content_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Condition_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            string str = string.Empty;

            int N = UpdateContent_textBox.Text.Length;
        }

        private void Condition_TextChanged(object sender, TextChangedEventArgs e)
        {
            int n = Condition_textBox.Text.Length;
            string str = string.Empty;


            if (n == 4)
            {
                str = Condition_textBox.Text.Substring(2, 2);
                int df = int.Parse(str) + 1;
                if (df == 100)
                {
                    df = 0;
                }
                Condition_textBox.Text += "/" + df.ToString().PadLeft(2, '0');
            }
            if(n==7)
            {
                string str1 = Condition_textBox.Text;

                int nCurrentIndex = Condition_textBox.CaretIndex;

                List<string> list = str1.Split('/').ToList();

                int nFirst = 0;
                int nLast = 0;

                if(nCurrentIndex == 4 || nCurrentIndex == 3)
                {
                    //nFirst = int.Parse(list[0]);
                    nFirst = int.Parse(list[0].Substring(2, 2));
                    nLast = nFirst + 1;
                    if(nLast == 100)
                    {
                        nLast = 0;
                    }

                    Condition_textBox.Text = list[0] + "/" + nLast.ToString().PadLeft(2, '0');
                }
                else if(nCurrentIndex == 6 || nCurrentIndex == 7)
                {
                    int nAllFirst = int.Parse(list[0]);
                    string sYear = list[0].Substring(0, 2);
                    nLast = int.Parse(list[1]);
                    nFirst = nLast - 1;
                    if(nFirst == -1)
                    {
                        nFirst = 99;
                        int ndf = int.Parse(sYear) - 1;
                        sYear = ndf.ToString();
                    }

                    Condition_textBox.Text = sYear + nFirst.ToString().PadLeft(2,'0') + "/" + nLast.ToString().PadLeft(2,'0');
                }
            }
        }

        private void Condition_KeyEvent(object sender, KeyEventArgs e)
        {
            string str = Condition_textBox.Text;

            if (e.Key == Key.Enter)
            {
                UpdateFunc();
            }

        }

        private void TeamName_Textbox_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            AllTeam at = new AllTeam();
            at.DataPassProdCd += new AllTeam.DataPassProdCdEventHandler(UpdateTeamNameReceive);
            at.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            at.ShowDialog();
        }

        private void TeamName_Textbox_KeyEvent(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                AllTeam at = new AllTeam();
                at.DataPassProdCd += new AllTeam.DataPassProdCdEventHandler(UpdateTeamNameReceive);
                at.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                at.ShowDialog();
            }
        }

        private void TeamName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z0-9\\s]+");
            if (regex.IsMatch(e.Text))
            {
                //e.Handled = !((e.Text[0] >= 'a' && e.Text[0] <= 'z') || (e.Text[0] >= 'A' && e.Text[0] <= 'Z'));
                e.Handled = true;
            }
        }

        private void Upper_TeamNameTextbox(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Text = textBox.Text.ToUpper();
            textBox.CaretIndex = textBox.Text.Length;
        }

        private void Condition_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            List<string> list = new List<string>();

            string str = Condition_textBox.Text;

            if (e.Key == Key.Up)
            {
                if(Condition_textBox.Text == string.Empty)
                {
                    Condition_textBox.Text = "2020/21";
                }
                else
                {
                    list = Condition_textBox.Text.Split('/').ToList();

                    int nFirst = int.Parse(list[0]) + 1;
                    int nLast = int.Parse(list[1]) + 1;

                    if(nLast == 100)
                    {
                        nLast = 0;
                    }

                    Condition_textBox.Text = nFirst.ToString() + "/" + nLast.ToString().PadLeft(2, '0');
                }
            }
            else if (e.Key == Key.Down)
            {
                if (Condition_textBox.Text == string.Empty)
                {
                    Condition_textBox.Text = "2020/21";
                }
                else
                {
                    list = Condition_textBox.Text.Split('/').ToList();

                    int nFirst = int.Parse(list[0]) - 1;
                    int nLast = int.Parse(list[1]) - 1;

                    if(nLast == -1)
                    {
                        nLast = 99;
                    }

                    Condition_textBox.Text = nFirst.ToString() + "/" + nLast.ToString().PadLeft(2, '0');
                }
            }
        }

        private void Champions_KeyEvent(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                AllTeam at = new AllTeam();
                at.DataPassProdCd += new AllTeam.DataPassProdCdEventHandler(TeamNameReceive);
                at.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                at.ShowDialog();
            }
        }

        private void Second_KeyEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AllTeam at = new AllTeam();
                at.DataPassProdCd += new AllTeam.DataPassProdCdEventHandler(TeamNameReceive2);
                at.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                at.ShowDialog();
            }
        }

        private void Third_KeyEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AllTeam at = new AllTeam();
                at.DataPassProdCd += new AllTeam.DataPassProdCdEventHandler(TeamNameReceive3);
                at.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                at.ShowDialog();
            }
        }

        private void Fourth_KeyEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AllTeam at = new AllTeam();
                at.DataPassProdCd += new AllTeam.DataPassProdCdEventHandler(TeamNameReceive4);
                at.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                at.ShowDialog();
            }
        }
    }
}
