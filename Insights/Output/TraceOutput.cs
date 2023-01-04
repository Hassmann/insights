using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Threading;

namespace SLD.Insights.Output
{
	public static class TraceOutput
	{
		//const string Elements = "│├┝╞┼╪┿ ═━─";
		private const int SourceIndentation = 30;

		private const int DefaultIndent = 3;
		private const int DisplayWidth = 100;

		private const char divider = '|';

		private static ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

		public static void Write(Insight insight)
			=> Write(ToTrace(insight));

		public static string ToTrace(this Insight insight)
		{
			string time = insight.Time.ToString(@"mm\:ss\.fff");

			char prefix;
			switch (insight.Level)
			{
				case TraceLevel.Error:
					prefix = 'E';
					break;

				case TraceLevel.Warning:
					prefix = 'W';
					break;

				default:
					prefix = insight.IsHighlight ? 'I' : ' ';
					break;
			};

			var line = $"{prefix} {time} {insight.Source,SourceIndentation} {divider} {insight.Text}";

			return line;
		}

		public static void Dump(Exception exception)
			=> Trace.WriteLine(exception.ToTrace());

		public static string ToTrace(this Exception ex)
		{
			using (var output = new StringWriter(CultureInfo.InvariantCulture))
			{
				var current = new ExceptionHelper(ex);
				int indent = 2;

				while (current != null)
				{
					output.WriteLine();

					output.WriteLine(Indent(
						indent,
						"{0} {1}",
						current.ExceptionName,
						new string('_', DisplayWidth - indent - current.ExceptionName.Length - 1)));

					output.WriteLine();

					output.WriteLine(Indent(
						indent,
						current.ExceptionMessage));

					output.WriteLine();

					output.WriteLine(Indent(
						indent,
						"Assembly: {0}",
						current.AssemblyName));

					output.WriteLine(Indent(
						indent,
						"File    : {0} (Line {1})",
						current.FileName,
						current.LineNumber));

					output.WriteLine(Indent(
						indent,
						"Method  : {0}.{1}",
						current.TypeName,
						current.MethodName));

					output.WriteLine();

					current = current.Inner;
					indent += DefaultIndent;

					if (current != null)
					{
						output.WriteLine(Indent(
							indent,
							new string('-', DisplayWidth - indent)));
					}
				}

				output.WriteLine(new string('_', DisplayWidth));

				return output.ToString();
			}
		}

		internal static void Write(string line)
		{
			_lock.EnterWriteLock();
			Trace.WriteLine(line);
			_lock.ExitWriteLock();
		}

		private static string Indent(int indent, [Localizable(false)] string text, params object[] parameters)
		{
			return
				new string(' ', indent) +
				string.Format(CultureInfo.InvariantCulture, text, parameters)
					.Replace(Environment.NewLine, Environment.NewLine + new string(' ', indent));
		}
	}
}