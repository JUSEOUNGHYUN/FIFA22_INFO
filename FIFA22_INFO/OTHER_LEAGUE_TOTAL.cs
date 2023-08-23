using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIFA22_INFO
{
    public class OTHER_LEAGUE_TOTAL
    {
        public string LRTeam_Name { get; set; }
        public string LRChampions_CNT { get; set; }
        public string LRSecond_CNT { get; set; }
        public string LRThird_CNT { get; set; }
        public string LRFourth_CNT { get; set; }

        public OTHER_LEAGUE_TOTAL(string lRTeam_Name, string lRChampions_CNT, string lRSecond_CNT, string lRThird_CNT, string lRFourth_CNT)
        {
            LRTeam_Name = lRTeam_Name;
            LRChampions_CNT = lRChampions_CNT;
            LRSecond_CNT = lRSecond_CNT;
            LRThird_CNT = lRThird_CNT;
            LRFourth_CNT = lRFourth_CNT;
        }
    }
}
