using System;
using System.Collections.Generic;
using System.Threading;
using Godot;
using NokLib;

namespace NokLib_Godot;
public enum TaskType : byte { OneTime, Repeating, RepeatingInfinite }
public static class TaskScheduler
{
    public readonly struct TaskInfo
    {
        public readonly ulong ExectutionTime;
        public readonly Action Task;

        public TaskInfo(Action task, ulong executionTime) {
            Task = task;
            ExectutionTime = executionTime;
        }
    }

    public struct RepeatingTaskInfo
    {
        public readonly ulong ExectutionTime;
        public readonly ulong ExectutionDelay;
        public readonly Action Task;
        public uint RepetitionCounter;
        public readonly uint RepetitionCount;
    }

    public struct TaskHandle
    {
        private LinkedListNode<TaskInfo>? node;
        internal LinkedListNode<TaskInfo>? Node => node;
        public TaskInfo? TaskInfo => node is null ? null : node.Value;
        public bool IsValid => node is not null && !WasCancelled;
        public bool WasCancelled { get; private set; }

        public TaskHandle(LinkedListNode<TaskInfo> node) {
            this.node = node;
            WasCancelled = false;
        }

        public void CancelTask() {
            if (IsValid) {
                TaskScheduler.CancelTask(this);
                Invalidate();
                WasCancelled = true;
            }
        }

        public void Invalidate() => node = null;
    }

    private const int ConcurrentWriteTimeout = 100;
    private static ReaderWriterLockSlim threadLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
    private static readonly LinkedList<TaskInfo> tasks = new();
    private static readonly LinkedList<RepeatingTaskInfo> repeatingTasks = new LinkedList<RepeatingTaskInfo>();

    static TaskScheduler() {
        GodotProcessingHook.ProcessCallback += Tick;
    }

    private static void Tick(double delta) {
        if (tasks.Count == 0)
            return;
        ulong currentTime = Time.GetTicksMsec();
        for (var node = tasks.First; node is not null; node = node.Next) {
            var info = node.Value;
            if (currentTime >= info.ExectutionTime) {
                info.Task.Invoke();
                var temp = node.Previous ?? tasks.First!;
                tasks.Remove(node);
                node = temp;
            }
        }
    }

    public static TaskHandle ScheduleTask(Action task, ulong MsDelay) {
        var taskInfo = new TaskInfo(task, Time.GetTicksMsec() + MsDelay);
        var node = tasks.AddLast(taskInfo);
        return new TaskHandle(node);
    }

    /// <summary>
    /// For multithreaded task scheduling
    /// </summary>
    /// <param name="task"></param>
    /// <param name="MsDelay"></param>
    /// <exception cref="ConcurrencyException">Thrown if the method fails to obtain a lock within specified time</exception>
    public static TaskHandle ConcurrentScheduleTask(Action task, ulong MsDelay) {
        if (!threadLock.TryEnterWriteLock(ConcurrentWriteTimeout))
            throw new ConcurrencyException($"Failed to obtain task queue lock in time (timeout = {ConcurrentWriteTimeout})");
        TaskHandle handle;
        try {
            handle = ScheduleTask(task, MsDelay);
        }
        finally {
            threadLock.ExitWriteLock();
        }
        return handle;
    }

    /// <summary>
    /// Cancel a scheduled task
    /// </summary>
    /// <param name="handle"></param>
    /// <exception cref="InvalidHandleException">Thrown if the handle is invalid, or the task was already cancelled</exception>
    public static void CancelTask(TaskHandle handle) {
        if (handle.IsValid) {
            tasks.Remove(handle.Node!);
            handle.Invalidate();
        }
        else
            throw new InvalidHandleException("Attempted to cancel a task with an invalid handle");
    }
}
