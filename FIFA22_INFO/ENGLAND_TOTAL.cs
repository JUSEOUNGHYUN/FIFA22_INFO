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
        public string LFACUP_Champions;
        public string LFACUP_Run;
        public string LCARABAOCUP_Champions;
        public string LCARABAOCUP_Run;

        public ENGLAND_TOTAL(string lTEAM_NAME, string lFACUP_Champions, string lFACUP_Run, string lCARABAOCUP_Champions, string lCARABAOCUP_Run)
        {
            LTEAM_NAME = lTEAM_NAME;
            LFACUP_Champions = lFACUP_Champions;
            LFACUP_Run = lFACUP_Run;
            LCARABAOCUP_Champions = lCARABAOCUP_Champions;
            LCARABAOCUP_Run = lCARABAOCUP_Run;
        }
    }
}
