using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplicationBackuperCli
{
    class Program
    {
        static void Main(string[] args)
        {
            Backuper backuper = new Backuper();
            backuper.Run();
        }
    }
}
