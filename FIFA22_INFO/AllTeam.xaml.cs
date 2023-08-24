using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
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
using System.Collections.ObjectModel;

namespace FIFA22_INFO
{
    /// <summary>
    /// AllTeam.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public class TeamItem
    {
        public TeamItem()
        {
            this.Team_Item = new ObservableCollection<TeamItem>();
        }

        public string Team_Name { get; set; }
        //public string ImageSource { get; set; }

        public ObservableCollection<TeamItem> Team_Item { get; set; }
    }

    public class Head_DepthTeam : List<string>
    {
        public TeamItem[] mMenuItem = new TeamItem[9];

        public Head_DepthTeam()
        {
            this.Add("PREMIER_LEAGUE");
            this.Add("LIGUE1_UBER_EATS");
            this.Add("BUNDESLIGA");
            this.Add("SERIE_A");
            this.Add("EREDIVISIE");
            this.Add("LIGA_PORTUGAL");
            this.Add("LALIGA_SANTANDER");
            this.Add("JUPILER_PRO_LEAGUE");
            this.Add("SPOR_TOTO_SUPERLIG");
        }
    }

    // PREMIER_LEAGUE
    public class PREMIER_LEAGUE_TEAM : List<string>
    {
        public TeamItem[] mMenuItem = new TeamItem[40];

        public PREMIER_LEAGUE_TEAM()
        {
            this.Add("AFC BOURNEMOUTH");
            this.Add("ARSENAL");
            this.Add("ASTON VILLA");
            this.Add("BARNSLEY");
            this.Add("BLACKBURN ROVERS");
            this.Add("BRENTFORD");
            this.Add("BRIGHTON");
            this.Add("BURNLEY");
            this.Add("CARDIFF CITY");
            this.Add("CHELSEA");
            this.Add("COVENTRY CITY");
            this.Add("CRYSTAL PALACE");
            this.Add("EVERTON");
            this.Add("FULHAM");
            this.Add("IPSWICH");
            this.Add("LEEDS UNITED");
            this.Add("LEICESTER CITY");
            this.Add("LIVERPOOL");
            this.Add("MANCHESTER CITY");
            this.Add("MANCHESTER UTD");
            this.Add("MIDDLESBROUGH");
            this.Add("MK DONS");
            this.Add("NEWCASTLE UTD");
            this.Add("NORWICH");
            this.Add("NOTTINGHAM FOREST");
            this.Add("PETERBOROUGH");
            this.Add("POHANG");
            this.Add("PRESTON");
            this.Add("QPR");
            this.Add("READING");
            this.Add("SAMSUNG FC");
            this.Add("SHEFFIELD UTD");
            this.Add("SOUTHAMPTON");
            this.Add("SPURS");
            this.Add("SWANSEA CITY");
            this.Add("SWINDON TOWN");
            this.Add("WATFORD");
            this.Add("WEST BROM");
            this.Add("WEST HAM");
            this.Add("WOLVES");
        }
    }

    // LIGUE1_UBER_EATS
    public class LIGUE1_UBER_EATS_TEAM : List<string>
    {
        public TeamItem[] mMenuItem = new TeamItem[14];

        public LIGUE1_UBER_EATS_TEAM()
        {
            this.Add("AS MONACO");
            this.Add("AS SAINT-ETIENNE");
            this.Add("FC NANTES");
            this.Add("GIRONDINS DE BX");
            this.Add("LOSC LILLE");
            this.Add("MONTPELLIER HSC");
            this.Add("OGC NICE");
            this.Add("OL");
            this.Add("OM");
            this.Add("PARIS SG");
            this.Add("RC STRASBOURG");
            this.Add("STADE DE REIMS");
            this.Add("STADE RENNAIS FC");
        }
    }

    public class BUNDESLIGA_TEAM : List<string> 
    {
        public TeamItem[] mMenuItem = new TeamItem[14];

        public BUNDESLIGA_TEAM()
        {
            this.Add("BAYERN MUNCHEN");
            this.Add("BORUSSIA DORTMUND");
            this.Add("FC KOLN");
            this.Add("FC SCHALKE 04");
            this.Add("FRANKFURT");
            this.Add("FSV MAINZ 05");
            this.Add("HERTHA BERLIN");
            this.Add("LEVERKUSEN");
            this.Add("MONCHENGLADBACH");
            this.Add("RB LEIPZIG");
            this.Add("SC FREIBURG");
            this.Add("TSG HOFFENHEIM");
            this.Add("VFB STUTTGART");
            this.Add("VFL WOLFSBURG");
        }
    }

    public class SERIE_A_TEAM : List<string>
    {
        public TeamItem[] mMenuItem = new TeamItem[12];

        public SERIE_A_TEAM()
        {
            this.Add("AC MILAN");
            this.Add("AS ROMA");
            this.Add("ATALANTA");
            this.Add("BOLOGNA");
            this.Add("EMPOLI");
            this.Add("FIORENTINA");
            this.Add("HELLAS VERONA");
            this.Add("INTER MILAN");
            this.Add("JUVENTUS");
            this.Add("LAZIO");
            this.Add("NAPOLI");
            this.Add("TORINO");
        }
    }

    public class EREDIVISIE_TEAM : List<string>
    {
        public TeamItem[] mMenuItem = new TeamItem[13];

        public EREDIVISIE_TEAM()
        {
            this.Add("AJAX");
            this.Add("AZ");
            this.Add("FC GRONINGEN");
            this.Add("FC TWENTE");
            this.Add("FC UTRECHT");
            this.Add("FEYENOORD");
            this.Add("FORTUNA DUSSELDORF");
            this.Add("FORTUNA SITTARD");
            this.Add("HERACLES ALMELO");
            this.Add("NEC NIJMEGEN");
            this.Add("PSV");
            this.Add("SC HEERENVEEN");
            this.Add("VITESSE");
        }
    }

    public class LIGA_PORTUGAL_TEAM : List<string>
    {
        public TeamItem[] mMenuItem = new TeamItem[11];
        
        public LIGA_PORTUGAL_TEAM()
        {
            this.Add("BELENENSES SAD");
            this.Add("BOAVISTA FC");
            this.Add("FC FAMALICAO");
            this.Add("FC PORTO");
            this.Add("MARITIMO");
            this.Add("PACOS FERREIRA");
            this.Add("PORTIMONENSE SC");
            this.Add("SC BRAGA");
            this.Add("SL BENFICA");
            this.Add("SPORTING CP");
            this.Add("VITORIA SC");
        }
    }

    public class LALIGA_SANTANDER_TEAM : List<string>
    {
        public TeamItem[] mMenuItem = new TeamItem[16];

        public LALIGA_SANTANDER_TEAM()
        {
            this.Add("AT MADRID");
            this.Add("ATHLETIC CLUB");
            this.Add("CA OSASUNA");
            this.Add("FC BARCELONA");
            this.Add("GETAFE CF");
            this.Add("GRANADA CF");
            this.Add("LEVANTE UD");
            this.Add("RC CELTA");
            this.Add("RCD ESPANYOL");
            this.Add("RCD MALLORCA");
            this.Add("REAL BETIS");
            this.Add("REAL MADRID");
            this.Add("REAL SOCIEDAD");
            this.Add("SEVILLA FC");
            this.Add("VALENCIA CF");
            this.Add("VILLARREAL CF");
        }
    }

    public class JUPILER_PRO_LEAGUE : List<string>
    {
        public TeamItem[] mMenuItem = new TeamItem[9];

        public JUPILER_PRO_LEAGUE()
        {
            this.Add("CLUB BRUGGE");
            this.Add("RSC ANDERLECHT");
            this.Add("ROYAL ANTWERP FC");
            this.Add("SP.CHARLEROI");
            this.Add("KRC GENK");
            this.Add("KAA GENT");
            this.Add("STANDARD LIEGE");
            this.Add("KV OOSTENDE");
            this.Add("KV MECHELEN");
        }
    }

    public class SPOR_TOTO_SUPERLIG : List<string>
    {
        public TeamItem[] mMenuItem = new TeamItem[4];

        public SPOR_TOTO_SUPERLIG()
        {
            this.Add("BESIKTAS");
            this.Add("GARATASARAY");
            this.Add("TRABZONSPOR");
            this.Add("FENERBAHCE");
        }
    }


    public partial class AllTeam : Window
    {
        public delegate void DataPassProdCdEventHandler(string strProdCd);

        public event DataPassProdCdEventHandler DataPassProdCd;

        public List<TeamItem> Team_Items { get; private set; }


        public AllTeam()
        {
            InitializeComponent();
            MakeInserAllItem();

            DataContext = this;
            Team_Items = new List<TeamItem>
            {
                //new TeamItem {Team_Name = "AFC BOURNEMOUTH", ImageSource = "Resources/AFC BOURNEMOUTH.png"}
            };
        }

        private void MakeInserAllItem()
        {
            TeamItem allItem = new TeamItem() { Team_Name = "All Team" };

            Head_DepthTeam hs = new Head_DepthTeam();
            PREMIER_LEAGUE_TEAM epl = new PREMIER_LEAGUE_TEAM();
            LIGUE1_UBER_EATS_TEAM ligue1 = new LIGUE1_UBER_EATS_TEAM();
            BUNDESLIGA_TEAM bun = new BUNDESLIGA_TEAM();
            SERIE_A_TEAM ser = new SERIE_A_TEAM();
            EREDIVISIE_TEAM ere = new EREDIVISIE_TEAM();
            LIGA_PORTUGAL_TEAM por = new LIGA_PORTUGAL_TEAM();
            LALIGA_SANTANDER_TEAM laliga = new LALIGA_SANTANDER_TEAM();
            JUPILER_PRO_LEAGUE jp = new JUPILER_PRO_LEAGUE();
            SPOR_TOTO_SUPERLIG sts = new SPOR_TOTO_SUPERLIG();

            for (int i=0; i< hs.Count; i++)
            {
                string str = hs[i];
                hs.mMenuItem[i] = new TeamItem() { Team_Name = str };
                allItem.Team_Item.Add(hs.mMenuItem[i]);

            }
            AllTeam_treeView.Items.Add(allItem);

            for(int i=0; i<epl.Count;i++)
            {
                string str = epl[i];
                epl.mMenuItem[i] = new TeamItem() {  Team_Name=str };
                hs.mMenuItem[0].Team_Item.Add(epl.mMenuItem[i]);
            }
            for(int i=0; i<ligue1.Count; i++)
            {
                string str = ligue1[i];
                ligue1.mMenuItem[i] = new TeamItem() { Team_Name = str };
                hs.mMenuItem[1].Team_Item.Add(ligue1.mMenuItem[i]);
            }
            for (int i = 0; i < bun.Count; i++)
            {
                string str = bun[i];
                bun.mMenuItem[i] = new TeamItem() { Team_Name = str };
                hs.mMenuItem[2].Team_Item.Add(bun.mMenuItem[i]);
            }
            for (int i = 0; i < ser.Count; i++)
            {
                string str = ser[i];
                ser.mMenuItem[i] = new TeamItem() { Team_Name = str };
                hs.mMenuItem[3].Team_Item.Add(ser.mMenuItem[i]);
            }
            for (int i = 0; i < ere.Count; i++)
            {
                string str = ere[i];
                ere.mMenuItem[i] = new TeamItem() { Team_Name = str };
                hs.mMenuItem[4].Team_Item.Add(ere.mMenuItem[i]);
            }
            for (int i = 0; i < por.Count; i++)
            {
                string str = por[i];
                por.mMenuItem[i] = new TeamItem() { Team_Name = str };
                hs.mMenuItem[5].Team_Item.Add(por.mMenuItem[i]);
            }
            for (int i = 0; i < laliga.Count; i++)
            {
                string str = laliga[i];
                laliga.mMenuItem[i] = new TeamItem() { Team_Name = str };
                hs.mMenuItem[6].Team_Item.Add(laliga.mMenuItem[i]);
            }
            for(int i=0; i<jp.Count; i++)
            {
                string str = jp[i];
                jp.mMenuItem[i] = new TeamItem() { Team_Name = str };
                hs.mMenuItem[7].Team_Item.Add(jp.mMenuItem[i]);
            }
            for(int i=0; i<sts.Count; i++)
            {
                string str = sts[i];
                sts.mMenuItem[i] = new TeamItem() { Team_Name = str};
                hs.mMenuItem[8].Team_Item.Add(sts.mMenuItem[i]);
            }

        }


        private void Exit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Upper_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Text = textBox.Text.ToUpper();
            textBox.CaretIndex = textBox.Text.Length;
        }

        private void Enter_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Treeview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine(" ComeIn MouseDoubleClick Event");

            TreeViewItem view = sender as TreeViewItem;

            TeamItem item = AllTeam_treeView.SelectedItem as TeamItem;


            try
            {
                if(item != null)
                {
                    int n = item.Team_Item.Count;

                    if (n == 0)
                    {
                        string sTeamName = item.Team_Name.ToString();
                        DataPassProdCd(sTeamName);

                        this.Close();

                    }
                }
            }
            catch(Exception ex)
            {

            }

            /*
            if(sender is TreeViewItem)
            {
                if(!((TreeViewItem)sender).IsSelected)
                {
                    return;
                }
            }

            //var clickItem = TryGetClickedItem(AllTeam_treeView, e);
            var clickItem = TryGetClickedItem(AllTeam_treeView, e);
            if(clickItem == null)
            {
                return;
            }

            string str = clickItem.

            e.Handled = true;
            

            try
            {
                if(item != null)
                {
                    MessageBox.Show("ITEM NOT NULL", "ITEM", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    MessageBox.Show("ITEM NULL", "ITEM", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
             */

        }


        private void HandleItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((TreeViewItem)sender).DataContext is TeamItem item)
            {
                string str = item.Team_Name;
                Debug.WriteLine(item.Team_Name);
            }
            else
            {
                System.Diagnostics.Trace.WriteLine(" No MouseDoubleClick Event");
            }
        }

        private void TextBlock_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var textBlock = (TextBlock)sender;
            var text = textBlock.Text;

            int i = 0;
        }

    }
}
