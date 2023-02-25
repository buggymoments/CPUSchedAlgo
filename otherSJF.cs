using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CPUSCHED
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Process> processes = new List<Process>
            {
                new Process { Name = "P1", ArrivalTime = 0, BurstTime = 13, Priority = 2 },
                new Process { Name = "P2", ArrivalTime = 3, BurstTime = 3, Priority = 1 },
                new Process { Name = "P3", ArrivalTime = 2, BurstTime = 8, Priority = 3 },
                new Process { Name = "P4", ArrivalTime = 1, BurstTime = 6, Priority = 2 },
                new Process { Name = "P5", ArrivalTime = 4, BurstTime = 4, Priority = 1 }
            };

            //Pang run nung algo
            RunSJF(processes);
            Console.ReadKey();
        }

        static void RunSJF(List<Process> processes)
        {
            List<Process> sortedProcesses = processes.OrderBy(p => p.ArrivalTime).ThenBy(p => p.BurstTime).ToList();

            int currentTime = 0;
            double totalWaitTime = 0;
            double totalTurnaroundTime = 0;

            Console.WriteLine("Shortest Job First (SJF)");

            Console.WriteLine();

            while (sortedProcesses.Count > 0)
            {
                Process nextProcess = sortedProcesses[0];

                // Execute the next process
                sortedProcesses.Remove(nextProcess);
                totalTurnaroundTime += currentTime + nextProcess.BurstTime - nextProcess.ArrivalTime;
                currentTime += nextProcess.BurstTime;
                nextProcess.FinishTime = currentTime;
                nextProcess.WaitingTime = currentTime - nextProcess.ArrivalTime - nextProcess.BurstTime;
                totalWaitTime += nextProcess.WaitingTime;
            }

            Console.WriteLine("Execution Order: {0}", string.Join(" -> ", processes.OrderBy(p => p.FinishTime).Select(p => p.Name)));
            Console.WriteLine("Average Waiting Time: {0:F2}", totalWaitTime / processes.Count);
            Console.WriteLine("Average Turnaround Time: {0:F2}", totalTurnaroundTime / processes.Count);

            Console.WriteLine("Waiting times:");
            foreach (Process process in processes)
            {
                Console.WriteLine("{0}: {1}", process.Name, process.WaitingTime);
            }
        }



        public class Process
        {
            public string Name { get; set; }
            public int ArrivalTime { get; set; }
            public int BurstTime { get; set; }
            public int Priority { get; set; }
            public int WaitingTime { get; set; }
            public int TurnaroundTime { get; set; }
            public int FinishTime { get; set; } // new property
        }
    }
}
