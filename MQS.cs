using System;
using System.Collections.Generic;

class Process
{
    public int ID { get; set; }
    public int ArrivalTime { get; set; }
    public int BurstTime { get; set; }
    public int RemainingTime { get; set; }
    public int WaitingTime { get; set; }
    public int TurnaroundTime { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        // Define the time slice for each queue
        const int q1TimeSlice = 8;
        const int q2TimeSlice = 16;
        const int q3TimeSlice = 32;
        var processes = new List<Process>
        {
            new Process { ID = 1, ArrivalTime = 0, BurstTime = 6 },
            new Process { ID = 2, ArrivalTime = 5, BurstTime = 13 },
            new Process { ID = 3, ArrivalTime = 13, BurstTime = 19 },
            new Process { ID = 4, ArrivalTime = 17, BurstTime = 5 },
            new Process { ID = 5, ArrivalTime = 23, BurstTime = 9 },
            new Process { ID = 6, ArrivalTime = 31, BurstTime = 18 },
            new Process { ID = 7, ArrivalTime = 34, BurstTime = 3 },
            new Process { ID = 8, ArrivalTime = 39, BurstTime = 2 }
        };

        // Sort the processes by arrival time
        processes.Sort((a, b) => a.ArrivalTime.CompareTo(b.ArrivalTime));

        // Create the queues
        var q1 = new Queue<Process>();
        var q2 = new Queue<Process>();
        var q3 = new Queue<Process>();

        // Add the processes to the first queue
        foreach (var process in processes)
        {
            q1.Enqueue(process);
        }

        // Simulate the execution of the processes
        int currentTime = 0;
        int totalWaitingTime = 0;
        int totalTurnaroundTime = 0;
        int totalProcesses = processes.Count;

        while (q1.Count > 0 || q2.Count > 0 || q3.Count > 0)
        {
            // Check if there are any processes in the first queue
            if (q1.Count > 0)
            {
                var process = q1.Dequeue();
                if (process.BurstTime <= q1TimeSlice)
                {
                    process.WaitingTime = currentTime - process.ArrivalTime;
                    process.TurnaroundTime = process.WaitingTime + process.BurstTime;
                    totalWaitingTime += process.WaitingTime;
                    totalTurnaroundTime += process.TurnaroundTime;
                    Console.WriteLine($"Process {process.ID} completed in queue 1 at time {currentTime}. Waiting time: {process.WaitingTime}. Turnaround time: {process.TurnaroundTime}.");
                    currentTime += process.BurstTime;
                }
                else
                {
                    process.RemainingTime = process.BurstTime - q1TimeSlice;
                    process.WaitingTime = currentTime - process.ArrivalTime;
                    q2.Enqueue(process);
                    currentTime += q1TimeSlice;
                }
            }
            // Check if there are any processes in the second queue
            else if (q2.Count > 0)
            {
                var process = q2.Dequeue();
                if (process.RemainingTime <= q2TimeSlice)
                {
                    process.WaitingTime += currentTime - process.TurnaroundTime;
                    process.TurnaroundTime = process.WaitingTime + process.BurstTime;
                    totalWaitingTime += process.WaitingTime;
                    totalTurnaroundTime += process.TurnaroundTime;
                    Console.WriteLine($"Process {process.ID} completed in queue 2 at time {currentTime}. Waiting time: {process.WaitingTime}. Turnaround time: {process.TurnaroundTime}.");
                    currentTime += process.RemainingTime;
                }
                else
                {
                    process.RemainingTime -= q2TimeSlice;
                    process.WaitingTime += currentTime - process.TurnaroundTime;
                    q3.Enqueue(process);
                    currentTime += q2TimeSlice;
                }
            }
            // Check if there are any processes in the third queue
            else if (q3.Count > 0)
            {
                var process = q3.Dequeue();
                process.WaitingTime += currentTime - process.TurnaroundTime;
                process.TurnaroundTime = process.WaitingTime + process.BurstTime;
                totalWaitingTime += process.WaitingTime;
                totalTurnaroundTime += process.TurnaroundTime;
                Console.WriteLine($"Process {process.ID} completed in queue 3 at time {currentTime}. Waiting time: {process.WaitingTime}. Turnaround time: {process.TurnaroundTime}.");
                currentTime += process.RemainingTime;
            }
        }
        // Calculate and display the average waiting time and turnaround time
        double avgWaitingTime = (double)totalWaitingTime / totalProcesses;
        double avgTurnaroundTime = (double)totalTurnaroundTime / totalProcesses;
        Console.WriteLine($"Average waiting time: {avgWaitingTime}");
        Console.WriteLine($"Average turnaround time: {avgTurnaroundTime}");
    }
}
