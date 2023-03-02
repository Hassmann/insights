
namespace SLD.Insights.Output
{
	/// <summary>
	/// Extracts info for tracing
	/// </summary>
	public class ExceptionHelper
	{
		public ExceptionHelper(Exception ex)
		{
			if (ex == null)
			{
				return;
			}

			if (ex.TargetSite is not null)
			{
				AssemblyName = ex.TargetSite.Module?.Assembly?.GetName().Name;
				TypeName = ex.TargetSite.DeclaringType?.Name;
				MethodName = ex.TargetSite.Name;
			}

			if (ex.StackTrace is not null)
			{
				FileName = ParseFileName(ex);
				LineNumber = ParseLineNumber(ex);
			}

			ExceptionName = ex.GetType().Name;

			ExceptionMessage = ex.Message;

			if (ex.InnerException is not null)
			{
				Inner = new ExceptionHelper(ex.InnerException);
			}
		}

		public ExceptionHelper Inner { get; }

		public int LineNumber { get; }

		public string AssemblyName { get; }

		public string FileName { get; }

		public string TypeName { get; }

		public string MethodName { get; }

		public string ExceptionName { get; }

		public string ExceptionMessage { get; }

		private static string ParseFileName(Exception ex)
		{
			int originalLineIndex = ex.StackTrace.IndexOf(":line", StringComparison.Ordinal);

			if (originalLineIndex == -1)
			{
				return "Unavailable";
			}

			string originalLine = ex.StackTrace.Substring(0, originalLineIndex);
			string[] sections = originalLine.Split('\\');
			return sections.LastOrDefault();
		}

		private static int ParseLineNumber(Exception ex)
		{
			string[] sections = ex.StackTrace.Split(' ');

			int index = 0;
			foreach (string section in sections)
			{
				if (section.EndsWith(":line", StringComparison.Ordinal))
				{
					break;
				}

				index++;
			}

			if (index == sections.Length)
			{
				return 0;
			}

			string lineNumber = sections[index + 1];

			if (!int.TryParse(lineNumber, out int number))
			{
				number = -1;
			}

			return number;
		}
	}
}