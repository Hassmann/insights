namespace SLD.Insights
{
	[DebuggerDisplay("\"{Name}\" ({DisplayLevel}),nq")]
	public partial class InsightsSource : DiagnosticListener
	{
		private const string InsightName = null;

		public InsightsSource(string area) : base(area.Trim())
		{
		}

		public InsightsSource(object source) : this(source.ToString())
		{
		}

		public TraceLevel DisplayLevel
		{
			get => GetDisplayLevel(this);
			set => SetDisplayLevel(this, value);
		}

		#region Direct

		public void Trace(string text, params object[] payload)
			=> Log(text, TraceLevel.Verbose, null, payload);

		public void Info(string text, params object[] payload)
			=> Log(text, TraceLevel.Info, null, payload);

		public void Warning(string text, params object[] payload)
			=> Log(text, TraceLevel.Warning, null, payload);

		public void Error(string text, Exception e = null, params object[] payload)
			=> Log(text, TraceLevel.Error, exception: e, payload);

		public void Log(string text, TraceLevel level = Insight.DefaultLevel, Exception exception = null, params object[] payload)
			=> Log(() => Create(level, text, exception, payload), level);

		#endregion Direct

		#region Deferred

		public void Log(Func<string> text, TraceLevel level = Insight.DefaultLevel)
			=> Log(() => Create(level, text()), level);

		public void Log(Func<(string text, object payload)> create, TraceLevel level = Insight.DefaultLevel)
			=> Log(() =>
			{
				var values = create();
				return Create(level, values.text, null, new object[] { values.payload });
			}, level);

		public void Log(Func<(string text, IEnumerable<object> payload)> create, TraceLevel level = Insight.DefaultLevel)
			=> Log(() =>
			{
				var values = create();
				return Create(level, values.text, null, values.payload.ToArray());
			}, level);

		public void Log(Func<(string text, Exception exception)> create, TraceLevel level = Insight.DefaultLevel)
			=> Log(() =>
			{
				var values = create();
				return Create(level, values.text, values.exception);
			}, level);

		public void Log(Func<(string text, Exception exception, object payload)> create, TraceLevel level = Insight.DefaultLevel)
			=> Log(() =>
			{
				var values = create();
				return Create(level, values.text, values.exception, new object[] { values.payload });
			}, level);

		public void Log(Func<(string text, Exception exception, IEnumerable<object> payload)> create, TraceLevel level = Insight.DefaultLevel)
			=> Log(() =>
			{
				var values = create();
				return Create(level, values.text, values.exception, values.payload.ToArray());
			}, level);

		#endregion Deferred

		public void Log(Func<Insight> createInsight, TraceLevel level = Insight.DefaultLevel)
		{
			if (IsEnabled(null, level))
			{
				try
				{
					var insight = createInsight();

					insight.Source = Name;

					Write(InsightName, insight);
				}
				catch (Exception e)
				{
					Error("Could not create Insight", e);
				}
			}
		}

		private Insight Create(TraceLevel level, string text, Exception exception = null, object[] payload = null)
					=> new Insight(level)
					{
						Text = text,
						Payload = payload,
						Exception = exception
					};
	}
}