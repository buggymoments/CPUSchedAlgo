// See https://aka.ms/new-console-template for more information
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
            RunFCFS(processes);
        
            Console.ReadKey();
        }

        static void RunFCFS(List<Process> processes)
        {
            Console.WriteLine("First Come First Serve (FCFS)");
            List<Process> sortedProcesses = new List<Process>(processes);
            sortedProcesses.Sort((a, b) => a.ArrivalTime.CompareTo(b.ArrivalTime));

            int currentTime = sortedProcesses[0].ArrivalTime;
            int totalWaitingTime = 0;
            int totalTurnaroundTime = 0;

            Console.Write("Gantt Chart: ");

            foreach (Process process in sortedProcesses)
            {
                while (currentTime < process.ArrivalTime)
                {
                    Console.Write("- ");
                    currentTime++;
                }

                Console.Write("{0} ", process.Name);

                totalWaitingTime += currentTime - process.ArrivalTime;
                totalTurnaroundTime += currentTime - process.ArrivalTime + process.BurstTime;

                currentTime += process.BurstTime;
            }

            double averageWaitingTime = (double)totalWaitingTime / sortedProcesses.Count;
            double averageTurnaroundTime = (double)totalTurnaroundTime / sortedProcesses.Count;

            Console.WriteLine("\n\nAverage waiting time: {0:F2}", averageWaitingTime);
            Console.WriteLine("Average turnaround time: {0:F2}", averageTurnaroundTime);
            Console.WriteLine();
        }

        static void ShowAverageTimes(List<Process> processes)
        {
            int totalWaitingTime = 0;
            int totalTurnaroundTime = 0;

            foreach (Process process in processes)
            {
                totalWaitingTime += process.WaitingTime;
                totalTurnaroundTime += process.TurnaroundTime;
            }

            double averageWaitingTime = (double)totalWaitingTime / processes.Count;
            double averageTurnaroundTime = (double)totalTurnaroundTime / processes.Count;

            Console.WriteLine("Average Waiting Time: {0}", averageWaitingTime);
            Console.WriteLine("Average Turnaround Time: {0}", averageTurnaroundTime);
        }
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

