using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.Db
{
    public class TransitBinding
    {
        public string birthStr;
        public TransitBinding()
        {

        }
        public TransitBinding(UserData data)
        {
            birthStr = String.Format("{0}/{1:D2}/{2:D2} {3:D2}:{4:D2}:{5:D2}", data.birth_year,
                data.birth_month, data.birth_day, data.birth_hour, data.birth_minute, data.birth_second);

        }

    }
}
