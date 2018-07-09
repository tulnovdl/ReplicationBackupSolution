using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using Microsoft.SqlServer.Replication;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplicationBackuperCli
{
    class ServerLinkFactory
    {
        String instance;
        String userName;
        String password;
        Server server;

        public static Server GetServerLink(String instance, String userName, String password)
        {
            ServerConnection conn = new ServerConnection(instance, userName, password);
            return new Server(conn);
        }

        public static ReplicationServer GetReplicationServerLink(String instance, String userName, String password)
        {
            ServerConnection conn = new ServerConnection(instance, userName, password);
            return new ReplicationServer(conn);
        }
    }
}
