using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
    Common command line application routines

        This class extends the Program class (created by default
        for command line apps) and provides common behaviour,
        such as command-line parsing, Ctrl-C / Ctrl-Break
        handling, embedded references etc.

    Keith Fletcher
    Mar 2016

    This file is Unlicensed.
    See the foot of the file, or refer to <http://unlicense.org>
*/

namespace fletcher.org
{
    public static class CommandLine
    {
        static CommandLine()
        {
            _cancelTokenSource = new CancellationTokenSource();
            _taskFactory = new TaskFactory(_cancelTokenSource.Token);
            _resetEvent = new ManualResetEventSlim();
            _mainTask = _taskFactory.StartNew(() => _resetEvent.Wait());
            _tasks = new List<Task>();
        }

        #region Shutdown event handling
        public delegate void ShutdownEventHandler(object sender, ShutdownEventArgs e);
        public class ShutdownEventArgs : EventArgs
        {
            internal ShutdownEventArgs(ShutdownType shutdownType)
                : base()
            {
                ShutdownType = shutdownType;
            }

            public ShutdownType ShutdownType { get; private set; }

            //public bool Cancel { get; set; } = false;
        }

        static ShutdownEventHandler _shutdownHandler;
        static object _eventLock = new object();

        public static event ShutdownEventHandler Shutdown
        {
            add
            {
                lock (_eventLock)
                {
                    // We need to hook up the CtrlHandler to capture the close events, but don't
                    // really want to do it unless someone is going to subscribe to the
                    // Shutdown event
                    if (_handler == null)
                    {
                        _handler += new NativeMethods.EventHandler(OnShutdown);
                        NativeMethods.SetConsoleCtrlHandler(_handler, true);
                    }
                    _shutdownHandler += value;
                }
            }
            remove
            {
                lock (_eventLock)
                {
                    _shutdownHandler -= value;
                }
            }
        }


        static bool OnShutdown(ShutdownType type)
        {
            if (_shutdownHandler != null)
            {
                var args = new ShutdownEventArgs(type);
                _shutdownHandler(null, args);
                //if (!args.Cancel)
                {
                    Cancel();
                    Task.Delay(TimeSpan.FromMilliseconds(100)).ContinueWith(_ => _resetEvent.Set());
                }
            }
            return true;
        }

        public enum ShutdownType
        {
            ControlC = 0,
            ControlBreak = 1,
            Close = 2,
            Logoff = 5,
            Shutdown = 6,

            // This is an artificial type that I've added, so I can signal
            // that an application has run it's course and ended naturally
            // using the shutdown infrastructure.
            AppCompleted = int.MaxValue
        }

        #region NativeMethods
        static class NativeMethods
        {
            internal delegate bool EventHandler(ShutdownType sig);

            [DllImport("Kernel32")]
            internal static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);


        }
        #endregion NativeMethods

        static NativeMethods.EventHandler _handler = null;
        #endregion Shutdown event handling


        //static void Main(string[] args)
        //{
        //    // Some biolerplate to react to close window event, CTRL-C, kill, etc
        //    _handler += new NativeMethods.EventHandler(Handler);
        //    if (!NativeMethods.SetConsoleCtrlHandler(_handler, true))
        //        throw new ApplicationException("Could not set attach to the SetConsoleCtrlHandler event.");

        //    // Expects there to be an OnStart method with this signature:
        //    // Task OnStart(CancellationToken)
        //    AddTask(Task.Run(() => OnStart(_cancelTokenSource.Token), _cancelTokenSource.Token));


        //    _resetEvent.Wait();
        //    Console.WriteLine("Exit");
        //}

        public static void AddTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            if (task.IsCompleted)
                return;
            lock(_tasks)
            {
                task.ContinueWith(t => RemoveTask(t));
                _tasks.Add(task);
            }
        }

        static void RemoveTask(Task task)
        {
            lock (_tasks)
            {
                if (task != null)
                    _tasks.Remove(task);
                for (var i = _tasks.Count - 1; i >= 0; --i)
                {
                    if (_tasks[i].IsCompleted)
                        _tasks.RemoveAt(i);
                }
                if (_tasks.Count == 0)
                    _resetEvent.Set();
            }
        }

        static void Cancel()
        {
            if (!_cancelTokenSource.IsCancellationRequested)
                _cancelTokenSource.Cancel();
        }

        public static void Wait()
        {
            RemoveTask(null);
            _resetEvent.Wait();
            //_mainTask.Wait();
        }

        static readonly CancellationTokenSource _cancelTokenSource;
        static readonly TaskFactory _taskFactory;
        static readonly Task _mainTask;
        static readonly ManualResetEventSlim _resetEvent;
        static readonly List<Task> _tasks;

    }
}

/*
This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org>
*/
