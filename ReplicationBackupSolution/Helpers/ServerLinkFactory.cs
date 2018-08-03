using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Replication;
using System;


namespace ReplicationBackupSolution
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
            conn.MultipleActiveResultSets = true;
            return new Server(conn);
        }

        public static ReplicationServer GetReplicationServerLink(String instance)
        {
            ServerConnection conn = new ServerConnection(instance);
            conn.MultipleActiveResultSets = true;
            return new ReplicationServer(conn);
        }
    }
}
