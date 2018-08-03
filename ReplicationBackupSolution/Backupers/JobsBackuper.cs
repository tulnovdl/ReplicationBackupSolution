using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using System;
using System.Collections.Specialized;
using System.IO;
using ReplicationBackupSolution.Helpers;

namespace ReplicationBackupSolution.Backupers
{
    class JobsBackuper
    {

        private Server server;
        private string dirPath;

        public JobsBackuper(Server server)
        {
            this.server = server ?? throw new ArgumentNullException(nameof(server));
            this.dirPath = PathBuilder.Build(server);
        }

        public void Backup()
        {
            StringCollection stringCollection = new StringCollection();
            ScriptingOptions scriptOptions = new ScriptingOptions
            {
                IncludeDatabaseContext = true
            };

            String script = "";
            String jobName = "";
            foreach (Job job in server.JobServer.Jobs)
            {
                script = "";
                jobName = job.Name.ToString();
                stringCollection = job.Script(scriptOptions);

                foreach (string s in stringCollection)
                {
                    script += s;
                }

                jobName = string.Join("_", jobName.Split(Path.GetInvalidFileNameChars()));
                TextWriter tw = new StreamWriter(dirPath + jobName + @".sql");
                tw.Write(script);
                tw.Close();
            }
        }
    }
}
