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
        // Define the time slice for each round-robin iteration
        const int timeSlice = 4;
        // Define the list of processes
        var processes = new List<Process>
        {
            new Process { ID = 1, ArrivalTime = 0, BurstTime = 15, RemainingTime = 1 },
            new Process { ID = 2, ArrivalTime = 1, BurstTime = 5, RemainingTime = 5},
            new Process { ID = 3, ArrivalTime = 2, BurstTime = 1, RemainingTime = 31},
            new Process { ID = 4, ArrivalTime = 3, BurstTime = 16, RemainingTime = 56},
            new Process { ID = 5, ArrivalTime = 4, BurstTime = 19, RemainingTime =12 }
        };

        // Sort the processes by arrival time
        processes.Sort((a, b) => a.ArrivalTime.CompareTo(b.ArrivalTime));

        // Initialize the waiting time and turnaround time variables
        int totalWaitingTime = 0;
        int totalTurnaroundTime = 0;

        // Initialize the list of completed processes
        var completedProcesses = new List<Process>();

        // Initialize the current time and the index of the current process
        int currentTime = 0;
        int currentProcessIndex = 0;

        // Initialize the list of time slots for the Gantt chart
        var ganttChart = new List<int>();

        // Loop until all processes are completed
        while (completedProcesses.Count < processes.Count)
        {
            // Get the current process
            var currentProcess = processes[currentProcessIndex];

            // Update the waiting time for all waiting processes
            for (int i = 0; i < processes.Count; i++)
            {
                if (i != currentProcessIndex && processes[i].ArrivalTime <= currentTime && !completedProcesses.Contains(processes[i]))
                {
                    processes[i].WaitingTime += timeSlice;
                }
            }

            // Add the current process to the Gantt chart
            ganttChart.Add(currentProcess.ID);

            // Update the remaining time for the current process
            currentProcess.RemainingTime -= timeSlice;

            // Check if the current process is completed
            if (currentProcess.RemainingTime <= 0)
            {
                // Calculate the turnaround time for the current process
                currentProcess.TurnaroundTime = currentTime - currentProcess.ArrivalTime;

                // Add the current process to the list of completed processes
                completedProcesses.Add(currentProcess);

                // Add the waiting time and turnaround time for the current process to the total waiting time and total turnaround time variables
                totalWaitingTime += currentProcess.WaitingTime;
                totalTurnaroundTime += currentProcess.TurnaroundTime;

                // Move to the next process
                currentProcessIndex++;
            }

            // Check if all processes have been checked
            if (currentProcessIndex >= processes.Count)
            {
                // Reset the index and sort the remaining processes by remaining time
                currentProcessIndex = 0;
                var remainingProcesses = processes.FindAll(p => !completedProcesses.Contains(p));
                remainingProcesses.Sort((a, b) => a.RemainingTime.CompareTo(b.RemainingTime));
                if (remainingProcesses.Count > 0)
                {
                    // Set the current process to the first process in the remaining processes list
                    currentProcessIndex = processes.IndexOf(remainingProcesses[0]);
                }
            }

            // Update the current time
            currentTime += timeSlice;
        }

        // Calculate the average waiting time and average turnaround time
        double avgWaitingTime = (double)totalWaitingTime / processes.Count;
        double avgTurnaroundTime = (double)totalTurnaroundTime / processes.Count;

        // Print the Gantt chart and the average waiting time and average turnaround time
        Console.WriteLine("Gantt Chart:");
        foreach (int processID in ganttChart)
        {
            Console.Write($"P{processID} ");
        }
        Console.WriteLine();
        Console.WriteLine($"Average Waiting Time: {avgWaitingTime:F2}");
        Console.WriteLine($"Average Turnaround Time: {avgTurnaroundTime:F2}");
    }
}
