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
        for command line applications) and provides common behaviour,
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
            _allTasksCompletedRE = new ManualResetEventSlim();
            _onAppClosingRE = new ManualResetEventSlim(true);
            _tasks = new List<Task>();
        }

        #region ConsoleClose handler
        public static void HookConsoleClose()
        {
            if (_consoleCloseHandler == null)
            {
                _consoleCloseHandler += new NativeMethods.EventHandler(type =>
                {
                    _onAppClosingRE.Reset();
                    try
                    {
                        _shutdownType = type;
                        OnAppClosing();
                    }
                    finally
                    {
                        _onAppClosingRE.Set();
                    }
                    return true;
                });
                NativeMethods.SetConsoleCtrlHandler(_consoleCloseHandler, true);
            }

        }

        #region NativeMethods
        static class NativeMethods
        {
            internal delegate bool EventHandler(ShutdownType sig);

            [DllImport("Kernel32")]
            internal static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);


        }
        #endregion NativeMethods

        static NativeMethods.EventHandler _consoleCloseHandler = null;
        #endregion ConsoleClose handler

        #region AppClosing/AppClosed event handling

        #region AppClosing
        public delegate void AppClosingEventHandler(object sender, AppClosingEventArgs e);

        public class AppClosingEventArgs : AppClosedEventArgs
        {
            internal AppClosingEventArgs(ShutdownType shutdownType)
                : base(shutdownType)
            { }

            public bool Cancel { get; set; } = false;
        }

        public static event AppClosingEventHandler AppClosing
        {
            add
            {
                lock (_eventLock)
                {
                    // Make sure we've hooked into the console close event,
                    HookConsoleClose();
                    _appClosingHandler += value;
                }
            }
            remove
            {
                lock (_eventLock)
                {
                    _appClosingHandler -= value;
                }
            }
        }

        static void OnAppClosing()
        {
            if (Interlocked.CompareExchange(ref _appClosingFlag, 1, 0) == 0)
            {
                if (_appClosingHandler != null)
                {
                    var args = new AppClosingEventArgs(_shutdownType);
                    _appClosingHandler(null, args);
                    if (!args.Cancel)
                        Cancel();
                    else
                    {
                        _shutdownType = ShutdownType.AppCompleted;
                        Interlocked.Exchange(ref _appClosingFlag, 0);
                    }
                }
            }
        }
        // Control how OnAppClosing can be called
        // 0 - Can be called anywhere, any time
        // 1 - Is busy in a call, or has been called and the app is cancelled. Don't call again until it's back to 0
        static int _appClosingFlag = 0;

        static AppClosingEventHandler _appClosingHandler;
        #endregion AppClosing

        #region AppClosed
        public delegate void AppClosedEventHandler(object sender, AppClosedEventArgs e);

        public class AppClosedEventArgs : EventArgs
        {
            internal AppClosedEventArgs(ShutdownType shutdownType)
                : base()
            {
                ShutdownType = shutdownType;
            }

            public ShutdownType ShutdownType { get; private set; }
        }


        public static event AppClosedEventHandler AppClosed
        {
            add
            {
                lock (_eventLock)
                {
                    // Make sure we've hooked into the console close event.
                    HookConsoleClose();
                    _appClosedHandler += value;
                }
            }
            remove
            {
                lock (_eventLock)
                {
                    _appClosedHandler -= value;
                }
            }
        }

        static void OnAppClosed()
        {
            if (Interlocked.CompareExchange(ref _appClosedFlag, 1, 0) == 0)
            {
                if (_appClosedHandler != null)
                {
                    var args = new AppClosedEventArgs(_shutdownType);
                    _appClosedHandler(null, args);
                }
            }
        }
        static int _appClosedFlag = 0; // Only allow OnAppClosed to be called once

        static AppClosedEventHandler _appClosedHandler;
        #endregion AppClosed

        public enum ShutdownType
        {
            ControlC = 0,
            ControlBreak = 1,
            Close = 2,
            Logoff = 5,
            Shutdown = 6,

            // This is an artificial type that I've added, so I can signal
            // that an application has run it's course and ended naturally
            AppCompleted = int.MaxValue
        }

        static object _eventLock = new object(); // Lock event attach and detach
        #endregion AppClosing/AppClosed event handling

        public static void WriteLine(string msg)
        {
            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}]: {msg}");
        }

        public static void WriteLine(string format, params object[] args)
            => WriteLine(string.Format(format, args));

        public static T Monitor<T>(string functionName, Func<T> func)
        {
            WriteLine($"Enter {functionName}");
            T ret;
            try
            {
                ret = func();
            }
            catch(Exception ex)
            {
                WriteLine($"Unhandled exception in {functionName}: {ex.Message}");
                throw;
            }
            
            WriteLine($"Exit {functionName}");
            return ret;
        }

        public static void Monitor(string functionName, Action action)
        {
            WriteLine($"Enter {functionName}");
            try
            {
                action();
            }
            catch (Exception ex)
            {
                WriteLine($"Unhandled exception in {functionName}: {ex.Message}");
                throw;
            }
            WriteLine($"Exit {functionName}");
        }

        #region AddTask / RemoveTask
        public static Task AddTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            if (!task.IsCompleted)
            {
                lock (_tasks)
                {
                    task.ContinueWith(t => RemoveTask(t));
                    _tasks.Add(task);
                }
            }
            return task;
        }

        public static Task AddTask(Action<CancellationToken> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            return AddTask(_taskFactory.StartNew(() => action(_cancelTokenSource.Token)));
        }

        public static Task AddTask(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            return AddTask(_taskFactory.StartNew(action));
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
                    _allTasksCompletedRE.Set();
            }
        }
        #endregion AddTask / RemoveTask

        #region Close application
        public static void CloseApp() => Cancel();

        static void Cancel()
        {
            // Make sure Cancel is only called once
            if (Interlocked.CompareExchange(ref _cancelled, 1, 0) == 0)
                _cancelTokenSource.Cancel();
        }
        static int _cancelled = 0; // 0 - Not cancelled, 1 = cancelled
        #endregion Close application

        public static void Wait()
        {
            // Force a check for completed tasks
            RemoveTask(null);

            // ..  and then wait for them all to complete
            _allTasksCompletedRE.Wait();

            // Make sure AppClosing is given time to complete
            _onAppClosingRE.Wait();

            // Then finally signal that the app is just about to close
            OnAppClosed();
        }

        static ShutdownType _shutdownType = ShutdownType.AppCompleted;
        static readonly CancellationTokenSource _cancelTokenSource;
        static readonly TaskFactory _taskFactory;
        static readonly ManualResetEventSlim _allTasksCompletedRE;
        static readonly ManualResetEventSlim _onAppClosingRE;
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
