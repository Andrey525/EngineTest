using Engines;

namespace EnginesTestStands
{
	public abstract class EngineTestBase
	{
		public AbstractEngine TestEngine { get; set; } = null!;
		protected CancellationTokenSource _cancelTokenSource;
		protected CancellationToken _token;
		protected int _maxRunTime = 10; // 10 seconds
		protected bool _isCancelled = false;

		public EngineTestBase(AbstractEngine engine)
		{
			TestEngine = engine;
			_cancelTokenSource = new CancellationTokenSource();
			_token = _cancelTokenSource.Token;
			_cancelTokenSource.CancelAfter(TimeSpan.FromSeconds(_maxRunTime));
		}

		public abstract Task<object> Run(double ambientTemperature);
	}
}
