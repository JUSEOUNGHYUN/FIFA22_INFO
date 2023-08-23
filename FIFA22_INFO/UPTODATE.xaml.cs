using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// UPTODATE.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UPTODATE : Window
    {
        public static List<ENGLAND_TOTAL> et = new List<ENGLAND_TOTAL>();
        public static List<TOTAL> mTotal = new List<TOTAL>();
        public static List<OTHER_LEAGUE_TOTAL> mOtherList = new List<OTHER_LEAGUE_TOTAL>();


        public UPTODATE()
        {
            InitializeComponent();

            LeagueOption_comboBox.Items.Add("ENGLAND");
            LeagueOption_comboBox.Items.Add("UEFA");
            LeagueOption_comboBox.Items.Add("LIGUE1_UBER_EATS");        // 
            LeagueOption_comboBox.Items.Add("BUNDESLIGA");              // 
            LeagueOption_comboBox.Items.Add("SERIE_A");                 // 
            LeagueOption_comboBox.Items.Add("EREDIVISIE");              // 
            LeagueOption_comboBox.Items.Add("LIGA_PORTUGAL");           // 
            LeagueOption_comboBox.Items.Add("LALIGA_SANTANDER");        // 
        }

        public bool TableExists(string tableName)
        {
            string sql = "SELECT * FROM information_schema.tables WHERE table_name = '" + tableName + "'";
            using (var con = new NpgsqlConnection(MainWindow.mConnString))
            {
                using (var cmd = new NpgsqlCommand(sql))
                {
                    if (cmd.Connection == null)
                        cmd.Connection = con;
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                    {
                        try
                        {
                            if (rdr != null && rdr.HasRows)
                                return true;
                            return false;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                }
            }
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

        private void keyDown_Event(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = LeagueOption_comboBox.SelectedIndex;
            string sTableName = string.Empty;
            List<string> TeamList = new List<string>();

            if(selectedIndex == 0)
            {
                sTableName = "england_total";

                string Teamsql = "select distinct (champions) from PREMIER_LEAGUE union select distinct (second_place) from PREMIER_LEAGUE union select distinct (champions) from EMIRATES_FA_CUP union select distinct (second_place) from EMIRATES_FA_CUP union select distinct (champions) from CARABAO_CUP union select distinct (second_place) from CARABAO_CUP order by champions;";

                NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(Teamsql, conn);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    TeamList.Add(reader[0].ToString().Trim());
                }

                reader.Close();

                for (int i = 0; i < TeamList.Count; i++)
                {
                    string Countsql = "select (select count(*) from PREMIER_LEAGUE where champions = '" + TeamList[i] + "'),"
                        + "(select count(*) from PREMIER_LEAGUE where second_place = '" + TeamList[i] + "'),"
                        + "(select count(*) from EMIRATES_FA_CUP where champions = '" + TeamList[i] + "'),"
                        + "(select count(*) from EMIRATES_FA_CUP where second_place = '" + TeamList[i] + "'),"
                        + "(select count(*) from CARABAO_CUP where champions = '" + TeamList[i] + "'),"
                        + "(select count(*) from CARABAO_CUP where second_place = '" + TeamList[i] + "');";

                    NpgsqlCommand cmd1 = new NpgsqlCommand(Countsql, conn);
                    NpgsqlDataReader reader1 = cmd1.ExecuteReader();

                    while (reader1.Read())
                    {
                        ENGLAND_TOTAL to = new ENGLAND_TOTAL(TeamList[i], reader1[0].ToString().Trim(),
                            reader1[1].ToString().Trim(),
                            reader1[2].ToString().Trim(),
                            reader1[3].ToString().Trim(),
                            reader1[4].ToString().Trim(),
                            reader1[5].ToString().Trim()
                            );
                        et.Add(to);
                    }
                    reader1.Close();
                }

                bool isExtis = TableExists(sTableName);

                if (isExtis)
                {
                    string dropsql = "drop table " + sTableName + ";";

                    NpgsqlCommand dropcmd = new NpgsqlCommand(dropsql, conn);
                    dropcmd.ExecuteNonQuery();
                }

                string createsql = "create table " + sTableName + " ( TEAM_NAME CHAR(100) primary key, PREMIER_LEAGUE_WINNER_CNT INTEGER, PREMIER_LEAGUE_RUN_CNT INTEGER, EMIRATES_FA_CUP_WINNER_CNT INTEGER, EMIRATES_FA_CUP_RUN_CNT INTEGER, CARABAO_CUP_WINNER_CNT INTEGER, CARABAO_CUP_RUN_CNT INTEGER);";
                NpgsqlCommand createcmd = new NpgsqlCommand(createsql, conn);
                createcmd.ExecuteNonQuery();

                for (int i = 0; i < et.Count; i++)
                {
                    ENGLAND_TOTAL to = et[i];
                    string insertsql = "insert into " + sTableName + " (TEAM_NAME, PREMIER_LEAGUE_WINNER_CNT, PREMIER_LEAGUE_RUN_CNT, EMIRATES_FA_CUP_WINNER_CNT, EMIRATES_FA_CUP_RUN_CNT, CARABAO_CUP_WINNER_CNT,CARABAO_CUP_RUN_CNT) values ('"
                        + to.LTEAM_NAME.ToString() + "'," + to.LPremier_League_Champions + "," + to.LPremier_League_Run + "," + to.LFACUP_Champions + "," + to.LFACUP_Run
                        + "," + to.LCARABAOCUP_Champions + "," + to.LCARABAOCUP_Run + ");";

                    NpgsqlCommand cmd2 = new NpgsqlCommand(insertsql, conn);
                    cmd2.ExecuteNonQuery();
                }
            }
            else if(selectedIndex == 1)
            {
                sTableName = "total";

                string Teamsql = "select distinct (champions) from champions_league union select distinct (second_place) from champions_league union select distinct (champions) from europa_league union select distinct (second_place) from europa_league union select distinct (champions) from conference_league union select distinct (second_place) from conference_league union select distinct (champions) from super_cup union select distinct (second_place) from super_cup order by champions;";

                NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(Teamsql, conn);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    TeamList.Add(reader[0].ToString().Trim());
                }

                reader.Close();

                for (int i = 0; i < TeamList.Count; i++)
                {
                    string Countsql = "select (select count(*) from champions_league where champions = '" + TeamList[i] + "'),"
                        + "(select count(*) from champions_league where second_place = '" + TeamList[i] + "'),"
                         + "(select count(*) from europa_league where champions = '" + TeamList[i] + "'),"
                          + "(select count(*) from europa_league where second_place = '" + TeamList[i] + "'),"
                           + "(select count(*) from conference_league where champions = '" + TeamList[i] + "'),"
                            + "(select count(*) from conference_league where second_place = '" + TeamList[i] + "'),"
                             + "(select count(*) from super_cup where champions = '" + TeamList[i] + "'),"
                              + "(select count(*) from super_cup where second_place = '" + TeamList[i] + "');"
                        ;

                    NpgsqlCommand cmd1 = new NpgsqlCommand(Countsql, conn);
                    NpgsqlDataReader reader1 = cmd1.ExecuteReader();

                    while (reader1.Read())
                    {
                        TOTAL to = new TOTAL(TeamList[i], reader1[0].ToString().Trim(), reader1[1].ToString().Trim(), reader1[2].ToString().Trim(),
                            reader1[3].ToString().Trim(), reader1[4].ToString().Trim(), reader1[5].ToString().Trim(), reader1[6].ToString().Trim(),
                            reader1[7].ToString().Trim());

                        mTotal.Add(to);
                    }

                    reader1.Close();
                }

                bool isExtis = TableExists("total");

                if (isExtis)
                {
                    string dropsql = "drop table total;";

                    NpgsqlCommand dropcmd = new NpgsqlCommand(dropsql, conn);
                    dropcmd.ExecuteNonQuery();
                }

                string createsql = "create table TOTAL ( TEAM_NAME CHAR(30) primary key, CHAMPIONS_LEAGUE_WINNER_CNT INTEGER, CHAMPIONS_LEAGUE_RUN_CNT INTEGER, EUROPA_LEAGUE_WINNER_CNT INTEGER, EUROPA_LEAGUE_RUN_CNT INTEGER, CONFERENCE_LEAGUE_WINNER_CNT INTEGER, CONFERENCE_LEAGUE_RUN_CNT INTEGER, SUPER_CUP_WINNER_CNT INTEGER, SUPER_CUP_RUN_CNT INTEGER);";
                NpgsqlCommand createcmd = new NpgsqlCommand(createsql, conn);
                createcmd.ExecuteNonQuery();

                for (int i = 0; i < mTotal.Count; i++)
                {
                    TOTAL to = mTotal[i];
                    string insertsql = "insert into TOTAL (TEAM_NAME, CHAMPIONS_LEAGUE_WINNER_CNT, CHAMPIONS_LEAGUE_RUN_CNT, EUROPA_LEAGUE_WINNER_CNT, EUROPA_LEAGUE_RUN_CNT, CONFERENCE_LEAGUE_WINNER_CNT,CONFERENCE_LEAGUE_RUN_CNT,  SUPER_CUP_WINNER_CNT, SUPER_CUP_RUN_CNT) values ('"
                        + to.LTEAM_NAME.ToString() + "'," + to.LChampions + "," + to.LChampions_run + "," + to.LEuropa + "," + to.LEuropa_run
                        + "," + to.LConference + "," + to.LConference_run + "," + to.LSuperCup + "," + to.LSuperCup_run + ");";

                    NpgsqlCommand cmd2 = new NpgsqlCommand(insertsql, conn);
                    cmd2.ExecuteNonQuery();
                }
            }
            else
            {
                sTableName = LeagueOption_comboBox.Text.ToLower();

                string Teamsql = "select distinct(champions) from " + sTableName +
                " union select distinct (second_place) from " + sTableName + " union " +
                "select distinct (third_place) from " + sTableName +
                " union select distinct(fourth_place) from " + sTableName + " order by champions;";

                NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(Teamsql, conn);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    TeamList.Add(reader[0].ToString().Trim());
                }

                reader.Close();

                for (int i = 0; i < TeamList.Count; i++)
                {
                    string Countsql = "select (select count(*) from " + sTableName + " where champions = '" + TeamList[i] + "'),"
                        + "(select count(*) from " + sTableName + " where second_place = '" + TeamList[i] + "'),"
                         + "(select count(*) from " + sTableName + " where third_place = '" + TeamList[i] + "'),"
                              + "(select count(*) from " + sTableName + " where fourth_place = '" + TeamList[i] + "');";

                    NpgsqlCommand cmd1 = new NpgsqlCommand(Countsql, conn);
                    NpgsqlDataReader reader1 = cmd1.ExecuteReader();

                    while (reader1.Read())
                    {
                        OTHER_LEAGUE_TOTAL to = new OTHER_LEAGUE_TOTAL(TeamList[i], reader1[0].ToString().Trim(), reader1[1].ToString().Trim(), reader1[2].ToString().Trim(),
                            reader1[3].ToString().Trim());

                        mOtherList.Add(to);
                    }

                    reader1.Close();
                }

                string sTableRankingName = sTableName + "_ranking";

                bool isExtis = TableExists(sTableRankingName);

                if (isExtis)
                {
                    string dropsql = "drop table " + sTableRankingName + ";";

                    NpgsqlCommand dropcmd = new NpgsqlCommand(dropsql, conn);
                    dropcmd.ExecuteNonQuery();
                }

                string createsql = "create table " + sTableRankingName + " ( TEAM_NAME CHAR(30) primary key, CHAMPIONS_CNT INTEGER, SECOND_PLACE_CNT INTEGER, THIRD_PLACE_CNT INTEGER, FOURTH_PLACE_CNT INTEGER);";
                NpgsqlCommand createcmd = new NpgsqlCommand(createsql, conn);
                createcmd.ExecuteNonQuery();

                for (int i = 0; i < mOtherList.Count; i++)
                {
                    OTHER_LEAGUE_TOTAL to = mOtherList[i];
                    string insertsql = "insert into " + sTableRankingName + " (TEAM_NAME, CHAMPIONS_CNT, SECOND_PLACE_CNT, THIRD_PLACE_CNT, FOURTH_PLACE_CNT) values ('"
                        + to.LRTeam_Name.ToString() + "'," + to.LRChampions_CNT + "," + to.LRSecond_CNT + "," + to.LRThird_CNT + "," + to.LRFourth_CNT + ");";

                    NpgsqlCommand cmd2 = new NpgsqlCommand(insertsql, conn);
                    cmd2.ExecuteNonQuery();
                }

            }
        }
    }
}
