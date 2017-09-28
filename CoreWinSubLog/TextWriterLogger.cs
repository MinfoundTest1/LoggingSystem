using System;
using System.IO;

namespace CoreWinSubLog
{
    /// <summary>
    /// Logger for output to a <see cref="TextWriter"/> implmentation.
    /// </summary>
    public class TextWriterLogger : Logger
    {
        private TextWriter @out;
        private readonly TextFileReadWrite _textWriter;

        public TextWriterLogger(TextWriter @out)
        {
            this.@out = @out;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="TextWriterLogger"/>.
        /// </summary>
        protected internal TextWriterLogger(TextFileReadWrite textWriter)
        {
            _textWriter = textWriter;
        }

        /// <summary>
        /// Log a message to the logger.
        /// </summary>
        /// <param name="level">Log level.</param>
        /// <param name="msg">Log message (format string).</param>
        /// <param name="args">Log message arguments.</param>
        public override void Log(LogRecord recoder, params object[] args)
        {
            _textWriter.Write(recoder,args);
        }
    }

    /// <summary>
    /// Manager for logging to a <see cref="TextWriter"/> implementation.
    /// </summary>
    public class TextWriterLogManager : LogManager
    {
        private readonly Logger _loggerImpl;

        /// <summary>
        /// Initializes an instance of the <see cref="TextWriterLogManager"/>.
        /// </summary>
        public TextWriterLogManager(TextFileReadWrite textWriter)
        {
            _loggerImpl = new TextWriterLogger(textWriter);
        }

        /// <summary>
        /// Get logger from the current log manager implementation.
        /// </summary>
        /// <param name="name">Classifier name, typically namespace or type name.</param>
        /// <returns>Logger from the current log manager implementation.</returns>
        protected override Logger GetLoggerImpl(string name)
        {
            return _loggerImpl;
        }
    }
}
