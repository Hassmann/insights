using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.IO;

namespace SLD.Insights
{
	using Configuration;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;

	public partial class InsightsSource
	{
		private static InsightsSettings DefaultSettings => new InsightsSettings
		{
			Sources = new[]
			{
				new SourceSettings
				{
					Name = "Insights",
					Level = TraceLevel.Verbose
				}
			}
		};

		private static InsightsSettings FindAnySettings()
		{
			var found = FindCustomSettings() ?? FindAppConfigSettings();

			if (found is null)
			{
				TraceSelf("No configuration found, using defaults", TraceLevel.Warning);

				found = DefaultSettings;
			}

			return found;
		}

		#region Custom: Insights.Levels

		private const string CustomFileName = "Insights.Levels";
		private const string SettingsPattern = @"^(-?)\s*(.*)\s*:\s*(Off|Warning|Verbose|Error|Info)$";

		private static InsightsSettings FindCustomSettings()
		{
			var fileName = Path.Combine(BasePath, CustomFileName);

			if (File.Exists(fileName))
			{
				var lines = ValidLines(fileName);

				var sources = lines
					.Select(line => Regex.Match(line, SettingsPattern))
					.Select(ResolveMatch)
					.Where(part => !part.IsSpecial);

				return new InsightsSettings
				{
					DumpExceptions = true,

					Sources = sources
						.Select(source => new SourceSettings
						{
							Name = source.Name,
							Level = source.Level
						})
						.ToArray()
				};
			}

			return null;
		}

		static (bool IsSpecial, string Name, TraceLevel Level) ResolveMatch(Match match)
			=> (
				match.Groups[1].Value.Any(),
				match.Groups[2].Value.Trim(),
				(TraceLevel)Enum.Parse(typeof(TraceLevel), match.Groups[3].Value.Trim())
			);

		static IEnumerable<string> ValidLines(string fileName)
			=> File
				.ReadAllLines(fileName)
				.Select(line => line.Trim())
				.Where(line => Regex.IsMatch(line, SettingsPattern))
				;

		#endregion Custom: Insights.Levels

		#region AppSettings (JSON)

		private const string AppConfigFileName = "appsettings.json";

		private static IConfigurationRoot GetConfiguration()
		{
			if (File.Exists(Path.Combine(BasePath, AppConfigFileName)))
			{
				return new ConfigurationBuilder()
					.SetBasePath(BasePath)
					.AddJsonFile(AppConfigFileName)
					.Build();
			}

			return null;
		}

		private static InsightsSettings FindAppConfigSettings()
		{
			// Configuration
			IConfigurationRoot configuration = GetConfiguration();

			if (configuration != null)
			{
				IConfigurationSection section = configuration.GetSection("Insights");

				if (section != null)
				{
					return section.Get<InsightsSettings>();
				}
				else
				{
					TraceSelf("No 'Insights' section in configuration", TraceLevel.Warning);
				}
			}
			else
			{
				TraceSelf("No configuration file found", TraceLevel.Warning);
			}

			return null;
		}

		#endregion AppSettings (JSON)
	}
}