using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Replication;
using System;
using System.Collections.Generic;
using ReplicationBackupSolution.Backupers;
using System.IO;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace ReplicationBackupSolution
{
    class BackupRunner
    {
        SimpleLogger logger = new SimpleLogger();

        public void Run()
        {
            foreach (String instance in getInstancesName())
            {
                try
                {
                    logger.Info("Working on: " + instance);

                    ReplicationServer replicationServer = ServerLinkFactory.GetReplicationServerLink(instance);
                    ReplicationBackuper replicationBackuper = new ReplicationBackuper(replicationServer);
                    replicationBackuper.Backup();

                    Server server = ServerLinkFactory.GetServerLink(instance);
                    JobsBackuper jobsBackuper = new JobsBackuper(server);
                    jobsBackuper.Backup();

                    PermissionBackuper permissionBackuper = new PermissionBackuper(server);
                    permissionBackuper.Backup();

                    logger.Info("Server: " + server.Name + " done");
            }
                catch (Exception e)
            {
                logger.Error(e.StackTrace);
            }
        }
            logger.Info("End of execution");
        }

        private List<String> getInstancesName()
        {
            List<String> resultList = new List<String>();


            String[] lines = File.ReadAllText(@"servers.txt").Split(new char[] { ',' });
            foreach (String line in lines)
            {
                resultList.Add(line.Trim());
            }
            return resultList;
        }
    }
}