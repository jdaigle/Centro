using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Globalization;

namespace OpenEntity.Logging
{
    public class LogFileTraceListener : TraceListener
    {
        #region Fields
        private TextWriter textWriter;
        private bool disposed;
        private bool writingIndent;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LogFileTraceListener"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public LogFileTraceListener(string fileName)
        {
            this.textWriter = new StreamWriter(fileName, false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogFileTraceListener"/> class.
        /// </summary>
        /// <param name="name">The name of the <see cref="T:System.Diagnostics.TraceListener"/>.</param>
        public LogFileTraceListener(string fileName, string name)
            :base(name)
        {
            this.textWriter = new StreamWriter(fileName, false);
        }

        #region Member Overrides
        /// <summary>
        /// Gets a value indicating whether the trace listener is thread safe.
        /// </summary>
        /// <value></value>
        /// <returns>true if the trace listener is thread safe; otherwise, false. The default is false.
        /// </returns>
        public override bool IsThreadSafe
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// When overridden in a derived class, flushes the output buffer.
        /// </summary>
        public override void Flush()
        {
            if (this.disposed)
                throw new ObjectDisposedException("LogFileTraceListener");
            this.textWriter.Flush();
        }
        /// <summary>
        /// When overridden in a derived class, closes the output stream so it no longer receives tracing or debugging output.
        /// </summary>
        public override void Close()
        {
            this.textWriter.Close();
        }
        #endregion

        #region Write Methods
        /// <summary>
        /// Writes the indent to the listener you create when you implement this class, and resets the <see cref="P:System.Diagnostics.TraceListener.NeedIndent"/> property to false.
        /// </summary>
        protected override void WriteIndent()
        {
            this.writingIndent = true;
            base.WriteIndent();
            this.writingIndent = false;
        }
        /// <summary>
        /// When overridden in a derived class, writes the specified message to the listener you create in the derived class.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void Write(string message)
        {
            if (this.disposed)
                throw new ObjectDisposedException("LogFileTraceListener");
            if (this.IndentLevel > 0 && !this.writingIndent)
                this.WriteIndent();
            this.textWriter.Write(message);
        }
        /// <summary>
        /// When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void WriteLine(string message)
        {
            this.WriteLineInfo();
            if (this.disposed)
                throw new ObjectDisposedException("LogFileTraceListener");
            if (this.IndentLevel > 0 && !this.writingIndent)
                this.WriteIndent();
            this.textWriter.WriteLine(message);
        }
        private void WriteLineInfo()
        {
            if (this.HasProcessId)
            {
                this.textWriter.Write("Pid" + Process.GetCurrentProcess().Id + "| ");
            }
            if (this.HasThreadId)
            {
                this.textWriter.Write("Thread" + Thread.CurrentThread.ManagedThreadId + "| ");
            }
            if (this.HasDateTime)
            {
                this.textWriter.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.CurrentCulture) + "| ");
            }
            else if (this.HasTimestamp)
            {
                this.textWriter.Write(DateTime.Now.ToString("HH:mm:ss", CultureInfo.CurrentCulture) + "| ");
            }
        }
        #endregion

        #region Option Properties
        /// <summary>
        /// Gets or sets a value indicating whether this trace listener will write a DateTime stamp on a WriteLine.
        /// </summary>
        public bool HasDateTime
        {
            get
            {
                return (this.TraceOutputOptions & TraceOptions.DateTime) == TraceOptions.DateTime;
            }
            set
            {
                if (value)
                    this.TraceOutputOptions |= TraceOptions.DateTime;
                else
                    this.TraceOutputOptions ^= TraceOptions.DateTime;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this trace listener will write a Timestamp on a WriteLine.
        /// </summary>
        public bool HasTimestamp
        {
            get
            {
                return (this.TraceOutputOptions & TraceOptions.Timestamp) == TraceOptions.Timestamp;
            }
            set
            {
                if (value)
                    this.TraceOutputOptions |= TraceOptions.Timestamp;
                else
                    this.TraceOutputOptions ^= TraceOptions.Timestamp;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this trace listener will write a ProcessId on a WriteLine.
        /// </summary>
        public bool HasProcessId
        {
            get
            {
                return (this.TraceOutputOptions & TraceOptions.ProcessId) == TraceOptions.ProcessId;
            }
            set
            {
                if (value)
                    this.TraceOutputOptions |= TraceOptions.ProcessId;
                else
                    this.TraceOutputOptions ^= TraceOptions.ProcessId;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this trace listener will write a ThreadId on a WriteLine.
        /// </summary>
        public bool HasThreadId
        {
            get
            {
                return (this.TraceOutputOptions & TraceOptions.ThreadId) == TraceOptions.ThreadId;
            }
            set
            {
                if (value)
                    this.TraceOutputOptions |= TraceOptions.ThreadId;
                else
                    this.TraceOutputOptions ^= TraceOptions.ThreadId;
            }
        }
        #endregion

        #region Dispose Methods
        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.Diagnostics.TraceListener"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && !this.disposed)
                {
                    if (this.textWriter != null)
                        this.textWriter.Dispose();
                    this.disposed = true;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        #endregion
    }
}
