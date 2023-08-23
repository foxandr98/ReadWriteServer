namespace ReadWriteServer
{
    public class Program
    {
        private const int numThreads = 20;
        private static bool running = true;
        public static void Main()
        {
            Console.WriteLine("Запуск...");

            // Серия потоков, которые рандомно читают и пишут
            // данные из общего разделяемого ресурса
            Thread[] t = new Thread[numThreads];
            for (int i = 0; i < numThreads; i++)
            {
                t[i] = new Thread(new ThreadStart(SomeAction));
                // Именование потоков, начиная с "А"
                t[i].Name = new String(new char[] { (char)(i + 65) });
                Console.WriteLine(t[i].Name + " created");
                t[i].Start();
            }

            // Ждем 10 секунд
            Thread.Sleep(TimeSpan.FromSeconds(10));
            // Завершаем потоки
            running = false;
            for (int i = 0; i < numThreads; i++)
                t[i].Join();

            //Печатаем статистику по действиям и таймаутам
            Server.PrintStatistics();
        }

        public static void SomeAction()
        {
            Random rnd = new Random();
            while (running)
            {
                int action = rnd.Next(0, 2);
                switch (action)
                {
                    case 0:
                        Server.GetCount();
                        break;
                    case 1:
                        Server.AddToCount(rnd.Next(10));
                        break;
                }
                Thread.Sleep(500);
            }
        }
    }
}