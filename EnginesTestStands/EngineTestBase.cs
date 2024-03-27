using Engines;

namespace EnginesTestStands
{
	public abstract class EngineTestBase
	{
		public AbstractEngine TestEngine { get; set; }
		protected CancellationTokenSource _cancelTokenSource;
		protected CancellationToken _token;
		protected int _maxRunTime;
		protected bool _isCancelled;

		public EngineTestBase(AbstractEngine engine, int maxRunTime)
		{
			TestEngine = engine;
			_maxRunTime = maxRunTime;
			_cancelTokenSource = new CancellationTokenSource();
			_token = _cancelTokenSource.Token;
			_cancelTokenSource.CancelAfter(TimeSpan.FromSeconds(_maxRunTime));
		}

		public abstract Task<object> Run(double ambientTemperature);
	}
}
