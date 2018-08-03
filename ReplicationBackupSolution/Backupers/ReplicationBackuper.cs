using Microsoft.SqlServer.Replication;
using System;
using System.IO;
using ReplicationBackupSolution.Helpers;

namespace ReplicationBackupSolution.Backupers
{
    class ReplicationBackuper
    {
        private ReplicationServer replicationServer;
        private string dirPath;

        public ReplicationBackuper(ReplicationServer replicationServer)
        {
            this.replicationServer = replicationServer ?? throw new ArgumentNullException(nameof(replicationServer));
            this.dirPath = PathBuilder.Build(this.replicationServer);
        }

        public void Backup()
        {
            foreach (ReplicationDatabase replicationDatabase in replicationServer.ReplicationDatabases)
            {
                if (replicationDatabase.HasPublications)
                {
                    foreach (TransPublication TP in replicationDatabase.TransPublications)
                    {
                        TextWriter tw = new StreamWriter(dirPath + TP.DatabaseName + @"-" + TP.Name + @".sql");
                        tw.Write(TP.Script(ScriptOptions.Creation | ScriptOptions.IncludeAll ^ ScriptOptions.IncludeReplicationJobs));
                        tw.Close();
                    }
                }
            }
        }
    }
}
