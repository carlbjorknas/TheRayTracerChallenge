using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace TheRayTracerChallenge.Utils
{
    class Timer : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private readonly string _memberName;

        public Timer([CallerMemberName] string memberName = "")
        {
            _stopwatch = new Stopwatch();
            Console.Out.WriteLine("--> " + memberName);
            _stopwatch.Start();
            _memberName = memberName;
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            Console.Out.WriteLine($"<-- {_memberName} {_stopwatch.ElapsedMilliseconds:N} ms");
        }
    }
}
