using rpatest.Abstractions.Steps;

using System;
using System.Collections.Generic;
using System.Threading;

namespace rpatest.Abstractions
{
    public class ProcessPlayer
    {
        public void Play(IEnumerable<Step> steps)
        {
            Console.WriteLine("Starting process...");
            var counter = 0;
            foreach (var step in steps)
            {
                Console.WriteLine($"{++counter}: {step.Name}");
                step.Execute();
                Thread.Sleep(300);
            }
            Console.WriteLine("The process completed!");
        }
    }
}
