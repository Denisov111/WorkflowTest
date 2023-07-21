namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int x = 0;  // общий ресурс

            AutoResetEvent waitHandler = new AutoResetEvent(false);  // объект-событие

            // запускаем пять потоков
            for (int i = 1; i < 6; i++)
            {
                Thread myThread = new(Print);
                myThread.Name = $"Поток {i}";
                myThread.Start();
            }


            void Print()
            {
                waitHandler.WaitOne();  // ожидаем сигнала
                x = 1;
                for (int i = 1; i < 6; i++)
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name}: {x}");
                    x++;
                    Thread.Sleep(100);
                }
                waitHandler.Set();  //  сигнализируем, что waitHandler в сигнальном состоянии
            }
        }
    }
}