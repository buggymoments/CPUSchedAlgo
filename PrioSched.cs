using System;
using System.Collections.Generic;

public class Process
{
    public int id;
    public int arrivalTime;
    public int burstTime;
    public int priority;
    public int waitingTime;
    public int turnaroundTime;
    public bool completed;

    public Process(int id, int arrivalTime, int burstTime, int priority)
    {
        this.id = id;
        this.arrivalTime = arrivalTime;
        this.burstTime = burstTime;
        this.priority = priority;
        this.waitingTime = 0;
        this.turnaroundTime = 0;
        this.completed = false;
    }
}

public class Program
{
    static void Main(string[] args)
    {
        List<Process> processes = new List<Process>();

        processes.Add(new Process(1, 0, 5, 2));
        processes.Add(new Process(2, 1, 3, 1));
        processes.Add(new Process(3, 2, 8, 4));
        processes.Add(new Process(4, 3, 6, 3));
        processes.Add(new Process(5, 4, 4, 5));

        processes.Sort((p1, p2) => p1.priority.CompareTo(p2.priority));

        int time = 0;
        int totalWaitingTime = 0;
        int totalTurnaroundTime = 0;
        int completedProcesses = 0;

        Console.WriteLine("Gantt Chart:");

        while (completedProcesses < processes.Count)
        {
            Process currentProcess = null;
            int highestPriority = int.MaxValue;

            foreach (Process p in processes)
            {
                if (!p.completed && p.arrivalTime <= time && p.priority < highestPriority)
                {
                    currentProcess = p;
                    highestPriority = p.priority;
                }
            }

            if (currentProcess != null)
            {

                Console.Write("|P" + currentProcess.id + "|");
                currentProcess.burstTime--;
                time++;

                if (currentProcess.burstTime == 0)
                {
                    currentProcess.completed = true;
                    completedProcesses++;

                    currentProcess.turnaroundTime = time - currentProcess.arrivalTime;
                    currentProcess.waitingTime = currentProcess.turnaroundTime - currentProcess.burstTime;
                    totalWaitingTime += currentProcess.waitingTime;
                    totalTurnaroundTime += currentProcess.turnaroundTime;
                }
            }
            else
            {
                // if no process is available to run, increment the time
                Console.Write("| idle |");
                time++;
            }
        }

        Console.WriteLine();
        Console.WriteLine("Current time: " + time);
        Console.WriteLine("Average waiting time: " + (double)totalWaitingTime / processes.Count);
        Console.WriteLine("Average turnaround time: " + (double)totalTurnaroundTime / processes.Count);

        foreach (Process p in processes)
        {
            Console.WriteLine("Waiting time for P" + p.id + ": " + p.waitingTime);
        }

        Console.ReadLine();
    }
}
