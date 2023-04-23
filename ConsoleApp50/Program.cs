using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace ConsoleApp50
{
    class Program
    {
        public static void MyThread()
        {
            List<object> collection = new List<object>() { 1, "two", 3.0 };

            collection.ForEach(item =>
            {
                Console.WriteLine(item.ToString());
            });
        }
        public static void ThreadParam(object obj)
        {
            int delay = (int)obj;
            Thread t = Thread.CurrentThread;

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Работает " + t.Name + " поток!");
                Thread.Sleep(delay);
            }
            Console.WriteLine("Завершает работу " + t.Name + " поток!");

            Console.WriteLine("Имя потока: {0}", t.Name);
            Console.WriteLine("Запущен ли поток: {0}", t.IsAlive);
            Console.WriteLine("Приоритет потока: {0}", t.Priority);
            Console.WriteLine("Статус потока: {0}", t.ThreadState);
        }
        static void Main(string[] args)
        {
            Thread t = Thread.CurrentThread;

            t.Name = "Метод Main";

            Thread th = new Thread(new ThreadStart(MyThread));
            th.IsBackground = true;
            Console.WriteLine("Имя потока: {0}", th.Name);
            Console.WriteLine("Запущен ли поток: {0}", th.IsAlive);
            Console.WriteLine("Статус потока: {0}", th.ThreadState);
            th.Start();
        }
        class Bank
        {
            private int money;
            private string name;
            private int percent;

            public int Money
            {
                get => money;
                set => SetValueAndLog(ref money, value, nameof(Money));
            }

            public string Name
            {
                get => name;
                set => SetValueAndLog(ref name, value, nameof(Name));
            }

            public int Percent
            {
                get => percent;
                set => SetValueAndLog(ref percent, value, nameof(Percent));
            }

            private static readonly object fileLock = new object();
            private static readonly string logFileName = "bank.log";

            private void SetValueAndLog<T>(ref T field, T newValue, string propertyName)
            {
                var oldValue = field;
                field = newValue;
                var thread = new Thread(() =>
                {
                    lock (fileLock)
                    {
                        using (var writer = new StreamWriter(logFileName, true))
                        {
                            writer.WriteLine($"{DateTime.Now}: {propertyName} changed from {oldValue} to {newValue}");
                        }
                    }
                });
                thread.Start();
            }
        }
    }
}