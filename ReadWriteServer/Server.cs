namespace ReadWriteServer
{
    public static class Server
    {
        // Блокировка
        static ReaderWriterLock rwl = new ReaderWriterLock();

        // Общий ресурс
        private static int count = 0;

        // Статистика.
        public static int readerTimeouts = 0;
        public static int writerTimeouts = 0;
        public static int reads = 0;
        public static int writes = 0;

        // Константа времени таймаута
        private const int timeOut = 50;

        public static int GetCount()
        {
            try
            {
                rwl.AcquireReaderLock(timeOut);
                try
                {
                    Display(String.Format("Чтение ресурса count: {0}", count));
                    Interlocked.Increment(ref reads);
                }
                finally
                {
                    rwl.ReleaseReaderLock();
                }
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref readerTimeouts);
            }

            return count;
        }

        public static void AddToCount(int add)
        {
            try
            {
                rwl.AcquireWriterLock(timeOut);
                try
                {
                    string msg = String.Format("Добавление числа к ресурсу count. {0} + {1} = ", add, count);
                    count += add;
                    msg += count;
                    Display(msg);
                    Interlocked.Increment(ref writes);
                }
                finally
                {
                    rwl.ReleaseWriterLock();
                }
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref writerTimeouts);
            }
        }

        private static void Display(string message)
        {
            Console.WriteLine(String.Format("Thread {0}: {1}", Thread.CurrentThread.Name, message));
        }

        public static void PrintStatistics()
        {
            // Отобразить общую статистику
            string stats = @"Статистика:
    {0} чтений 
    {1} записей 
    {2} таймаутов при чтении 
    {3} таймаутов при записи";
            Console.WriteLine(stats, reads, writes, readerTimeouts, writerTimeouts);
        }
    }
}
