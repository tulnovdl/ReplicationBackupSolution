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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReplicationBackuperCli
{
    class Backuper
    {
        SimpleLogger logger = new SimpleLogger();

        public void Run()
        {
            String folderDateNow = DateTime.Now.ToShortDateString().Replace('\\', '_').Replace('/', '_');
            String baseDir = getBaseDir();

            foreach (String instance in getInstances())
            {
                try
                {
                    ReplicationServer replicationServer = ServerLinkFactory.GetReplicationServerLink(instance);
                    String pathToBackupPublications = baseDir + @"\SQLBackups" + @"\ReplicaitonBackups\" + replicationServer.Name + @"\" + folderDateNow + @"\Publications\";
                    createDirectory(pathToBackupPublications);
                    BackupPublications(pathToBackupPublications, folderDateNow, replicationServer);

                    Server server = ServerLinkFactory.GetServerLink(instance);
                    String pathToBackupJobs = baseDir + @"\SQLBackups" + @"\JobsBackups\" + server.Name + @"\" + folderDateNow + @"\";
                    createDirectory(pathToBackupJobs);
                    BackupJobs(server, pathToBackupJobs);
                    logger.Info("Server: " + server.Name + " done");
                }
                catch (Exception e)
                {
                    logger.Error(e.StackTrace);
                }
            }
            logger.Info("End of execution");
        }
        private void BackupPublications(string NetworkPath, string folderDate, ReplicationServer replicationServer)
        {
            foreach (ReplicationDatabase replicationDatabase in replicationServer.ReplicationDatabases)
            {
                if (replicationDatabase.HasPublications)
                {
                    foreach (TransPublication TP in replicationDatabase.TransPublications)
                    {
                        TextWriter tw = new StreamWriter(NetworkPath + "\\" + TP.DatabaseName + "-" + TP.Name + ".sql");
                        tw.Write(TP.Script(ScriptOptions.Creation | ScriptOptions.IncludeAll ^ ScriptOptions.IncludeReplicationJobs));
                        tw.Close();
                    }
                }
            }
        }

        private void BackupJobs(Server server, String pathToCreate)
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

                jobName = GetSafeFilename(jobName);
                String fullPath = pathToCreate + jobName + @".sql";
                TextWriter tw = new StreamWriter(fullPath);
                tw.Write(script);
                tw.Close();
            }
        }

        private List<String> getInstances()
        {
            List<String> resultList = new List<String>();

            String[] lines = File.ReadAllText(@"servers.txt").Split(new char[] { ',' });
            foreach (String line in lines)
            {
                resultList.Add(line.Trim());
            }
            return resultList;
        }
        
        private String getBaseDir()
        {
            //TODO error check && validate
            return File.ReadAllLines(@"basedir.txt")[0].Trim();
        }

        private void createDirectory(String path)
        {
            Directory.CreateDirectory(path);
        }

        public string GetSafeFilename(string filename)
        {

            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));

        }
    }
}

