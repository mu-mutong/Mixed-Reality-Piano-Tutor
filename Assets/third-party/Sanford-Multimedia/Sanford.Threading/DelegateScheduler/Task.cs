#region License

/* Copyright (c) 2007 Leslie Sanford
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated documentation files (the "Software"), to 
 * deal in the Software without restriction, including without limitation the 
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or 
 * sell copies of the Software, and to permit persons to whom the Software is 
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software. 
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 * THE SOFTWARE.
 */

#endregion

#region Contact

/*
 * Leslie Sanford
 * Email: jabberdabber@hotmail.com
 */

#endregion

using System;
using System.Diagnostics;

namespace Sanford.Threading
{
    public class Task : IComparable
    {
        #region IComparable Members

        public int CompareTo(object obj)
        {
            var t = obj as Task;

            if (t == null) throw new ArgumentException("obj is not the same type as this instance.");

            return -NextTimeout.CompareTo(t.NextTimeout);
        }

        #endregion

        #region Task Members

        #region Fields

        // The number of times left to invoke the delegate associated with this Task.

        // The interval between delegate invocation.

        // The delegate to invoke.

        // The arguments to pass to the delegate when it is invoked.
        private readonly object[] args;

        // The time for the next timeout;

        // For locking.
        private readonly object lockObject = new object();

        #endregion

        #region Construction

        internal Task(
            int count,
            int millisecondsTimeout,
            Delegate method,
            object[] args)
        {
            Count = count;
            MillisecondsTimeout = millisecondsTimeout;
            Method = method;
            this.args = args;

            ResetNextTimeout();
        }

        #endregion

        #region Methods

        internal void ResetNextTimeout()
        {
            NextTimeout = DateTime.Now.AddMilliseconds(MillisecondsTimeout);
        }

        internal object Invoke(DateTime signalTime)
        {
            Debug.Assert(Count == DelegateScheduler.Infinite || Count > 0);

            var returnValue = Method.DynamicInvoke(args);

            if (Count == DelegateScheduler.Infinite)
            {
                NextTimeout = NextTimeout.AddMilliseconds(MillisecondsTimeout);
            }
            else
            {
                Count--;

                if (Count > 0) NextTimeout = NextTimeout.AddMilliseconds(MillisecondsTimeout);
            }

            return returnValue;
        }

        public object[] GetArgs()
        {
            return args;
        }

        #endregion

        #region Properties

        public DateTime NextTimeout { get; private set; }

        public int Count { get; private set; }

        public Delegate Method { get; }

        public int MillisecondsTimeout { get; }

        #endregion

        #endregion
    }
}