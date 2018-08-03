using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Replication;
using System;
using System.IO;

namespace ReplicationBackupSolution.Helpers
{
    static class PathBuilder
    {
        private static String dateNow = DateTime.Now.ToShortDateString().Replace('\\', '_').Replace('/', '_');
        
        public static string Build(ReplicationServer ReplicationServer)
        {
            String dirPath = GetBaseDirToBackup() + @"\ReplicationBackupSolution" + @"\ReplicaitonBackups\" + ReplicationServer.Name + @"\" + dateNow + @"\";
            Directory.CreateDirectory(dirPath);
            return dirPath;
        }

        public static string Build(Server Server)
        {
            string dirPath = GetBaseDirToBackup() + @"\ReplicationBackupSolution" + @"\JobsBackups\" + Server.Name + @"\" + dateNow + @"\";
            Directory.CreateDirectory(dirPath);
            return dirPath;
        }

        public static string Build(Server Server, Boolean mark)
        {
            string dirPath = GetBaseDirToBackup() + @"\ReplicationBackupSolution" + @"\PermissionsBackups\" + Server.Name + @"\" + dateNow + @"\";
            Directory.CreateDirectory(dirPath);
            return dirPath;
        }

        private static String GetBaseDirToBackup()
        {
            //TODO: error check && validate
            return File.ReadAllLines(@"basedirtobackup.txt")[0].Trim();
            //return @"\\qnap\SBE\!ReplicationBackups";
        }
    }
}
