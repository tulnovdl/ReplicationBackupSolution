namespace ReplicationBackupSolution
{
    class Program
    {
        static void Main(string[] args)
        {
            BackupRunner backupRunner = new BackupRunner();
            backupRunner.Run();
        }
    }
}