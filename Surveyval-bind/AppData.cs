using System;
using System.Collections.Generic;

namespace Surveyval_bind
{
    [Serializable]
    class AppData
    {
        public List<Fragebogen> appFrageboegen { get; set; }
        public List<Frage> appFragen { get; set; }

        public AppData()
        {
            appFrageboegen = new List<Fragebogen>();
            appFragen = new List<Frage>();
        }

        internal Boolean isContaining(Fragebogen tmp)
        {
            foreach (Fragebogen item in appFrageboegen)
                if (tmp.strName.Equals(item.strName))
                    return true;

            return false;
        }

        internal Boolean isContaining(Frage tmp)
        {
            foreach (Frage item in appFragen)
                if (tmp.strFragetext.Equals(item.strFragetext))
                    return true;

            return false;
        }

        internal void removeFragebogen(Fragebogen tmp)
        {
            foreach (Fragebogen item in appFrageboegen)
            {
                if (item.strName.Equals(tmp.strName))
                    appFrageboegen.Remove(item);
            }
        }
    }
}
