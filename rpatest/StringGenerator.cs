using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rpatest
{
    static class StringGenerator
    {
        public static async Task<string> Method1(string fileName)
        {
            Console.WriteLine("Write to file from multiple threads - method 1: ReaderWriterLockSlim");

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var readerWriterLockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            var tasks = Enumerable.Range(10, 10).Select(n => Task.Run(() => WriteNumberToFile(n))).ToArray();

            await Task.WhenAll(tasks);

            var text = File.ReadAllText(fileName);
            return text;

            async Task WriteNumberToFile(int number)
            {
                readerWriterLockSlim.EnterWriteLock();

                using (var fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                {
                    using (var writer = new StreamWriter(fs))
                    {
                        await writer.WriteAsync($"{number} ");
                    }
                }

                readerWriterLockSlim.ExitWriteLock();
            }
        }

        public static async Task<string> Method2(string fileName)
        {
            Console.WriteLine("Write to file from multiple threads - method 2: BlockingCollection");

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using var bc = new BlockingCollection<int>();

            var task = Task.Run(() => WriteToFile(bc));
            var tasks = Enumerable.Range(10, 10).Select(n => Task.Run(() => WriteNumber(n, bc))).ToArray();

            await Task.WhenAll(tasks);
            bc.CompleteAdding();

            await task;

            var text = File.ReadAllText(fileName);
            return text;

            void WriteNumber(int number, BlockingCollection<int> bc)
            {
                bc.Add(number);
            }

            async Task WriteToFile(BlockingCollection<int> bc)
            {
                using (var fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                {
                    using (var writer = new StreamWriter(fs))
                    {
                        foreach (var number in bc.GetConsumingEnumerable())
                        {
                            await writer.WriteAsync($"{number} ");
                        }
                    }
                }
            }
        }
    }
}
