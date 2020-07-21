using System;
using System.Diagnostics;

namespace FilteringChatLogger
{
    public class SectionTimer
    {
        private readonly string _name;
        private readonly Stopwatch _stopwatch;
        private ulong _executions;
        private ulong _ticks;

        public SectionTimer(in string name)
        {
            _executions = 0;
            _ticks = 0;
            _name = name;
            _stopwatch = new Stopwatch();
        }

        public void Start()
        {
            _stopwatch.Start();
        }

        public void Stop()
        {
            _stopwatch.Stop();
            _executions++;
            _ticks += (ulong) _stopwatch.ElapsedTicks;
            _stopwatch.Reset();
        }

        private ulong GetMeanNanoseconds()
        {
            if (_stopwatch.IsRunning) throw new InvalidOperationException("The SectionTimer should not be running.");
            return _ticks * 1000 * 1000 * 1000 / (ulong) Stopwatch.Frequency;
        }

        public string GetSummary()
        {
            if (_stopwatch.IsRunning) throw new InvalidOperationException("The SectionTimer should not be running.");
            if (_executions == 0) return $"The section {_name} was never executed.";

            var durationString = TimespanFormatter.FormatTimespan(GetMeanNanoseconds() / _executions);
            if (_executions == 1) return $"The section {_name} took {durationString}.";
            return $"After {_executions} executions, the section {_name} mean execution time was {durationString}.";
        }
    }
}