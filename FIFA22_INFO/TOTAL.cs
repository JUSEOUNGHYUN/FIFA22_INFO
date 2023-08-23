using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIFA22_INFO
{
    public class TOTAL
    {
        public string LTEAM_NAME;
        public string LChampions;
        public string LChampions_run;
        public string LEuropa;
        public string LEuropa_run;
        public string LConference;
        public string LConference_run;
        public string LSuperCup;
        public string LSuperCup_run;

        public TOTAL(string lTEAM_NAME, string lChampions, string lChampions_run, string lEuropa, string lEuropa_run, string lConference, string lConference_run, string lSuperCup, string lSuperCup_run)
        {
            LTEAM_NAME = lTEAM_NAME;
            LChampions = lChampions;
            LChampions_run = lChampions_run;
            LEuropa = lEuropa;
            LEuropa_run = lEuropa_run;
            LConference = lConference;
            LConference_run = lConference_run;
            LSuperCup = lSuperCup;
            LSuperCup_run = lSuperCup_run;
        }
    }
}
