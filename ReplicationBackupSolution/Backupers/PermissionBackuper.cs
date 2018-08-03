using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using System;
using System.Collections.Specialized;
using System.IO;
using ReplicationBackupSolution.Helpers;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using System.Threading;

namespace ReplicationBackupSolution.Backupers
{
    class PermissionBackuper
    {

        private Server server;
        private string dirPath;

        public PermissionBackuper(Server server)
        {
            this.server = server ?? throw new ArgumentNullException(nameof(server));
            this.dirPath = PathBuilder.Build(server, true);
        }

        public void Backup()
        {

            foreach (Database database in server.Databases)
            {
                Scripter scripter = new Scripter(server);
                scripter.Options.ScriptDrops = false;
                //scripter.Options.WithDependencies = true;
                scripter.Options.Indexes = true;
                scripter.Options.DriAllConstraints = true;
                scripter.Options.IncludeDatabaseRoleMemberships = true;
                scripter.Options.Indexes = true;
                scripter.Options.Permissions = true;

                String script = "";
                StringCollection stringCollection;
                foreach (Table table in database.Tables)
                {
                    if (table.IsSystemObject == false)
                    {
                        stringCollection = scripter.Script(new Urn[] { table.Urn });
                        foreach (string s in stringCollection)
                        {
                            script += s + Environment.NewLine;
                        }
                    }
                    TextWriter tw = new StreamWriter(dirPath + database.Name + @".sql");
                    tw.Write(script);
                    tw.Close();
                }
            }
        }
    }
}

//new Thread(() =>
//                {
//    Thread.CurrentThread.IsBackground = true;
//}).Start();