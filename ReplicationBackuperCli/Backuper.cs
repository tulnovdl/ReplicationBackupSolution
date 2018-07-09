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
    class Backuper
    {
        public static void test()
        {
            string FolderDate;
            string NetworkPath;

            String instance = @"server\instance";
            String userName = @"username";
            String password = @"password";

            Server srv = ServerLinkFactory.GetServerLink(instance, userName, password);
            ReplicationServer RS = ServerLinkFactory.GetReplicationServerLink(instance, userName, password);

            FolderDate = DateTime.Now.ToShortDateString().Replace('\\', '_').Replace('/', '_');
            NetworkPath = @"\\confluence\D$" + @"\TestBackups" + @"\ReplicaitonBackups\" + FolderDate + "\\";
            Directory.CreateDirectory(NetworkPath);

            try
            {
                foreach (ReplicationDatabase RD in RS.ReplicationDatabases)
                {
                    if (RD.HasPublications)
                    {
                        foreach (TransPublication TP in RD.TransPublications)
                        {
                            TextWriter tw = new StreamWriter(NetworkPath + "\\" + TP.Name.ToString() + ".sql");
                            tw.Write(TP.Script(ScriptOptions.Creation | ScriptOptions.IncludeAll ^ ScriptOptions.IncludeReplicationJobs));
                            tw.Close();
                        }
                    }
                }
            }

            catch (Exception eh)
            {
                //MessageBox.Show(eh.ToString());
            }

            StringCollection strCol = new StringCollection();
            ScriptingOptions scriptOpt = new ScriptingOptions();
            scriptOpt.IncludeDatabaseContext = true;

            try
            {
                string script = "";
                string JobName;
                //Looping through the job
                foreach (Job J in srv.JobServer.Jobs)
                {
                    script = "";
                    JobName = J.Name.ToString();
                    strCol = J.Script(scriptOpt);

                    //concate the text of job
                    foreach (string s in strCol)
                    {
                        script += s;
                    }
                    //save the job file
                    string dir = @"\\confluence\D$" + @"\TestBackups" + @"\JobBackups\" + FolderDate + "\\";
                    System.IO.Directory.CreateDirectory(dir);
                    string fullPath = dir + JobName.Replace(':', '_').ToString() + @".sql";
                    TextWriter tw = new StreamWriter(fullPath);
                    tw.Write(script);
                    tw.Close();
                }
            }
            catch
            {
            }

        }

    }
}

