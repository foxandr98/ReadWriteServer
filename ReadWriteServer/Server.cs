using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadWriteServer
{
    public static class Server
    {
        static ReaderWriterLock rwl = new ReaderWriterLock();

        //Shared resource
        private static int count = 0;

        public static int GetCount()
        {
            return count;
        }

        public static void AddToCount(int add)
        {

        }
    }
}
