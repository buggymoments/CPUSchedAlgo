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
            RunSJF(processes);

            Console.ReadKey();
        }
        
        static void RunSJF(List<Process> processes)
        {
            // Sort processes by their burst time
            List<Process> sortedProcesses = processes.OrderBy(p => p.BurstTime).ToList();

            int currentTime = 0;
            double totalWaitTime = 0;
            double totalTurnaroundTime = 0;

            Console.WriteLine("Shortest Job First (SJF)");

            // Print the Gantt chart header
            Console.Write("| {0,-10} ", "Time");

            foreach (Process process in sortedProcesses)
            {
                Console.Write("| {0,-10} ", process.Name);
            }

            Console.WriteLine("|");

            // Print the Gantt chart content
            Console.Write("| {0,-10} ", currentTime);

            for (int i = 0; i < sortedProcesses.Count; i++)
            {
                Process current = sortedProcesses[i];

                Console.Write("| {0,-10} ", " ");

                if (i == sortedProcesses.Count - 1)
                {
                    Console.Write("|");
                }
            }

            Console.WriteLine();

            // Execute the processes
            while (sortedProcesses.Count > 0)
            {
                Process nextProcess = sortedProcesses[0];

                // Print the Gantt chart content
                Console.Write("| {0,-10} ", currentTime);

                for (int i = 0; i < sortedProcesses.Count; i++)
                {
                    Process process = sortedProcesses[i];

                    if (process.ArrivalTime <= currentTime)
                    {
                        Console.Write("| {0,-10} ", process.Name);

                        if (process.BurstTime < nextProcess.BurstTime)
                        {
                            nextProcess = process;
                        }
                    }
                    else
                    {
                        Console.Write("| {0,-10} ", " ");

                        if (i == sortedProcesses.Count - 1)
                        {
                            Console.Write("|");
                        }
                    }
                }

                Console.WriteLine();

                // Execute the next process
                sortedProcesses.Remove(nextProcess);
                totalWaitTime += currentTime - nextProcess.ArrivalTime;
                totalTurnaroundTime += currentTime + nextProcess.BurstTime - nextProcess.ArrivalTime;
                currentTime += nextProcess.BurstTime;
            }

            Console.WriteLine("Execution Order: {0}", string.Join(" -> ", processes.OrderBy(p => p.FinishTime).Select(p => p.Name)));
            Console.WriteLine("Average Waiting Time: {0:F2}", totalWaitTime / processes.Count);
            Console.WriteLine("Average Turnaround Time: {0:F2}", totalTurnaroundTime / processes.Count);
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

