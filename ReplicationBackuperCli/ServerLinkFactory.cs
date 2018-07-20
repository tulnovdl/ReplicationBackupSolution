using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using Microsoft.SqlServer.Replication;
using System;


namespace ReplicationBackuperCli
{
    class ServerLinkFactory
    {
        /// <summary>
        /// We are using domain authentication. Hence, program will connect with permissions of user.
        /// </summary>
        /// <param name="instance">For example "servername.com\instancename"</param>
        /// <returns></returns>
        public static Server GetServerLink(String instance)
        {
            ServerConnection conn = new ServerConnection(instance);
            return new Server(conn);
        }

        public static ReplicationServer GetReplicationServerLink(String instance)
        {
            ServerConnection conn = new ServerConnection(instance);
            return new ReplicationServer(conn);
        }
    }
}
