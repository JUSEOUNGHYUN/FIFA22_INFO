using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIFA22_INFO
{
    public class ENGLAND_TOTAL
    {
        public string LTEAM_NAME;
        public string LPremier_League_Champions;
        public string LPremier_League_Run;
        public string LFACUP_Champions;
        public string LFACUP_Run;
        public string LCARABAOCUP_Champions;
        public string LCARABAOCUP_Run;

        public ENGLAND_TOTAL(string lTEAM_NAME, string lPremier_League_Champions, string lPremier_League_Run, string lFACUP_Champions, string lFACUP_Run, string lCARABAOCUP_Champions, string lCARABAOCUP_Run)
        {
            LTEAM_NAME = lTEAM_NAME;
            LPremier_League_Champions = lPremier_League_Champions;
            LPremier_League_Run = lPremier_League_Run;
            LFACUP_Champions = lFACUP_Champions;
            LFACUP_Run = lFACUP_Run;
            LCARABAOCUP_Champions = lCARABAOCUP_Champions;
            LCARABAOCUP_Run = lCARABAOCUP_Run;
        }
    }
}
