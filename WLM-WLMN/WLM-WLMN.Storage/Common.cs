using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cassandra;

namespace WLM_WLMN.CassandraStorage
{
    internal class Common
    {
        private static ISession _session;

        private Common()
        {
        }

        public static ISession Session
        {
            get
            {
                if (_session == null)
                {
                    var cluster = Cluster.Builder().AddContactPoint("localhost").Build();
                    _session = cluster.Connect("wlm");
                }
                return _session;
            }
        }
    }
}
