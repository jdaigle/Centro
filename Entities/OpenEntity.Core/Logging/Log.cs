using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace OpenEntity.Logging
{
    public static class Log
    {
        private static int indentLevel;
        private static int indentSize = 4;
        private static LogTraceListenerCollection listeners;
        /// <summary>
        /// Gets or sets a value indicating whether System.Diagnostics.Debug.Flush()
        /// should be called on the System.Diagnostics.Debug.Listeners after every write.
        /// </summary>
        /// <value>
        /// true if System.Diagnostics.Debug.Flush() is called on the System.Diagnostics.Debug.Listeners
        /// after every write; otherwise, false.
        /// </value>
        public static bool AutoFlush { get; set; }
        /// <summary>
        /// Gets or sets the indent level.
        /// </summary>
        /// <value>The indent level. The default is 0.</value>
        public static int IndentLevel
        {
            get { return indentLevel; }
            set
            {
                indentLevel = value;
                foreach (var listener in Listeners)
                    listener.IndentLevel = value;
            }
        }
        /// <summary>
        /// Gets or sets the number of spaces in an indent.
        /// </summary>
        /// <value>The number of spaces in an indent. The default is four.</value>
        public static int IndentSize
        {
            get { return indentSize; }
            set
            {
                indentSize = value;
                foreach (var listener in Listeners)
                    listener.IndentSize = value;
            }
        }
        /// <summary>
        /// Gets the collection of listeners that is monitoring the debug output.
        /// </summary>
        /// <value>
        /// A System.Diagnostics.TraceListenerCollection representing a collection of
        /// type System.Diagnostics.TraceListener that monitors the debug output.
        /// </value>
        public static LogTraceListenerCollection Listeners
        {
            get
            {
                if (listeners == null)
                {
                    listeners = new LogTraceListenerCollection();
                    listeners.Add(new DefaultTraceListener());
                }
                return listeners;
            }
        }
        /// <summary>
        /// Flushes the output buffer and then calls the Close method on each of the
        /// System.Diagnostics.Debug.Listeners.
        /// </summary>
        public static void Close()
        {
            foreach (var listener in Listeners)
            {
                listener.Flush();
                listener.Close();
            }
        }
        /// <summary>
        /// Flushes the output buffer and causes buffered data to write to the System.Diagnostics.Debug.Listeners
        /// collection.
        /// </summary>
        public static void Flush()
        {
            foreach (var listener in Listeners)
            {
                listener.Flush();
            }
        }
        /// <summary>
        /// Increases the current System.Diagnostics.Debug.IndentLevel by one.
        /// </summary>
        public static void Indent()
        {
            IndentLevel++;
        }
        /// <summary>
        /// Decreases the current System.Diagnostics.Debug.IndentLevel by one.
        /// </summary>
        public static void Unindent()
        {
            if (IndentLevel > 0)
                IndentLevel--;
        }
        /// <summary>
        /// Writes the value of the object's System.Object.ToString() method to the trace
        /// listeners in the System.Diagnostics.Debug.Listeners collection.
        /// </summary>
        /// <param name="value">An object whose name is sent to the System.Diagnostics.Debug.Listeners.</param>
        public static void Write(object value)
        {
            foreach (var listener in Listeners)
            {
                listener.Write(value);
                if (AutoFlush)
                    listener.Flush();
            }
        }
        /// <summary>
        /// Writes a message to the trace listeners in the System.Diagnostics.Debug.Listeners
        /// collection.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public static void Write(string message)
        {
            foreach (var listener in Listeners)
            {
                listener.Write(message);
                if (AutoFlush)
                    listener.Flush();
            }
        }
        /// <summary>
        /// Writes a category name and the value of the object's System.Object.ToString()
        /// method to the trace listeners in the System.Diagnostics.Debug.Listeners collection.
        /// </summary>
        /// <param name="value">An object whose name is sent to the System.Diagnostics.Debug.Listeners.</param>
        /// <param name="category">A category name used to organize the output.</param>
        public static void Write(object value, string category)
        {
            foreach (var listener in Listeners)
            {
                listener.Write(value, category);
                if (AutoFlush)
                    listener.Flush();
            }
        }
        /// <summary>
        /// Writes a category name and message to the trace listeners in the System.Diagnostics.Debug.Listeners collection.
        /// </summary>
        /// <param name="message">A message to write.</param>
        /// <param name="category">A category name used to organize the output.</param>
        public static void Write(string message, string category)
        {
            foreach (var listener in Listeners)
            {
                listener.Write(message, category);
                if (AutoFlush)
                    listener.Flush();
            }
        }
        /// <summary>
        /// Writes the value of the object's System.Object.ToString() method to the trace
        /// listeners in the System.Diagnostics.Debug.Listeners collection.
        /// </summary>
        /// <param name="value">An object whose name is sent to the System.Diagnostics.Debug.Listeners.</param>
        public static void WriteLine(object value)
        {
            foreach (var listener in Listeners)
            {
                listener.WriteLine(value);
                if (AutoFlush)
                    listener.Flush();
            }
        }
        /// <summary>
        /// Writes a message followed by a line terminator to the trace listeners in
        /// the System.Diagnostics.Debug.Listeners collection.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public static void WriteLine(string message)
        {
            foreach (var listener in Listeners)
            {
                listener.WriteLine(message);
                if (AutoFlush)
                    listener.Flush();
            }
        }
        /// <summary>
        /// Writes a category name and the value of the object's System.Object.ToString()
        /// method to the trace listeners in the System.Diagnostics.Debug.Listeners collection.
        /// </summary>
        /// <param name="value">An object whose name is sent to the System.Diagnostics.Debug.Listeners.</param>
        /// <param name="category">A category name used to organize the output.</param>
        public static void WriteLine(object value, string category)
        {
            foreach (var listener in Listeners)
            {
                listener.WriteLine(value, category);
                if (AutoFlush)
                    listener.Flush();
            }
        }
        /// <summary>
        /// Writes a category name and message to the trace listeners in the System.Diagnostics.Debug.Listeners
        /// collection.
        /// </summary>
        /// <param name="message">A message to write.</param>
        /// <param name="category">A category name used to organize the output.</param>
        public static void WriteLine(string message, string category)
        {
            foreach (var listener in Listeners)
            {
                listener.WriteLine(message, category);
                if (AutoFlush)
                    listener.Flush();
            }
        }
        /// <summary>
        /// Writes a message in the "Database" activity category to the trace listeners in the System.Diagnostics.Debug.Listeners
        /// collection.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public static void DatabaseActivity(string message)
        {
            WriteLine(message, "Database");
        }
        /// <summary>
        /// Writes a message in the "User" activity category to the trace listeners in the System.Diagnostics.Debug.Listeners
        /// collection.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public static void UserActivity(string message)
        {
            WriteLine(message, "User");
        }
    }
}
