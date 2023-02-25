using System;
using System.Collections.Generic;

class Process
{
    public int id;
    public int burst;
    public int arrival;
    public int waiting;
}

class ShortestRemainingTime
{
    static void Main(string[] args)
    {
        List<Process> processes = new List<Process>();

        // Create some example processes
        processes.Add(new Process { id = 1, burst = 5, arrival = 0 });
        processes.Add(new Process { id = 2, burst = 3, arrival = 1 });
        processes.Add(new Process { id = 3, burst = 8, arrival = 2 });
        processes.Add(new Process { id = 4, burst = 6, arrival = 3 });
        processes.Add(new Process { id = 5, burst = 4, arrival = 4 });

        int currentTime = 0;
        int totalWaitingTime = 0;
        int totalTurnaroundTime = 0;
        int completedProcesses = 0;
        Process runningProcess = null;
        PriorityQueue<Process> readyQueue = new PriorityQueue<Process>();

        Console.WriteLine("Gantt Chart:");

        while (completedProcesses < processes.Count)
        {
            // Check for new arrivals
            foreach (Process process in processes)
            {
                if (process.arrival == currentTime)
                {
                    readyQueue.Enqueue(process, process.burst);
                }
            }

            // Check if the running process has completed
            if (runningProcess != null && runningProcess.burst == 0)
            {
                Console.Write("| P{0} ", runningProcess.id);
                totalTurnaroundTime += currentTime - runningProcess.arrival;
                completedProcesses++;
                runningProcess = null;
            }

            // If no process is running, start the shortest one in the queue
            if (runningProcess == null && readyQueue.Count > 0)
            {
                runningProcess = readyQueue.Dequeue();
                Console.Write("| P{0} ", runningProcess.id);
            }

            // If a process is running, decrement its remaining time and update waiting times
            if (runningProcess != null)
            {
                runningProcess.burst--;
                foreach (Process process in readyQueue.GetElements())
                {
                    process.waiting++;
                }
            }

            currentTime++;
        }

        double avgWaitingTime = 0;
        double avgTurnaroundTime = 0;

        Console.WriteLine("|");
        Console.WriteLine("Current time: {0}", currentTime);

        // Calculate waiting times and print them
        Console.WriteLine("Waiting times:");
        foreach (Process process in processes)
        {
            process.waiting -= process.arrival; // adjust for arrival time
            totalWaitingTime += process.waiting;
            avgWaitingTime += process.waiting;
            Console.WriteLine("P{0}: {1}", process.id, process.waiting);
        }

        avgWaitingTime /= processes.Count;
        avgTurnaroundTime = (double)totalTurnaroundTime / processes.Count;

        Console.WriteLine("Average waiting time: {0}", avgWaitingTime);
        Console.WriteLine("Average turnaround time: {0}", avgTurnaroundTime);
    }
}

class PriorityQueue<T>
{
    private List<Tuple<T, int>> elements = new List<Tuple<T, int>>();

    private Dictionary<T, int> waitingTimes = new Dictionary<T, int>();

    public int Count
    {
        get { return elements.Count; }
    }

    public List<T> GetElements()
    {
        List<T> result = new List<T>();
        foreach (var item in elements)
        {
            result.Add(item.Item1);
        }
        return result;
    }

    public void Enqueue(T item, int priority)
    {
        elements.Add(Tuple.Create(item, priority));
        if (!waitingTimes.ContainsKey(item))
        {
            waitingTimes[item] = 0;
        }
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item2 < elements[bestIndex].Item2)
            {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Item1;

        // Add the waiting time of the dequeued process to its waiting time
        foreach (var item in waitingTimes.Keys.ToList())
        {
            if (!item.Equals(bestItem))
            {
                waitingTimes[item]++;
            }
            else
            {
                waitingTimes[item] = 0;
            }
        }

        elements.RemoveAt(bestIndex);
        return bestItem;
    }

    public int GetWaitingTime(T item)
    {
        return waitingTimes[item];
    }
}

