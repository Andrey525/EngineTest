namespace Engines
{
	public abstract class AbstractEngine
	{
		public double CriticalTemperature { get; protected set; }
		public double CurrentTemperature { get; protected set; }
		public double AmbientTemperature { get; set; }
		public double CurrentMoment { get; protected set; }
		public double CurrentСrankshaftSpeed { get; protected set; }

		public bool IsOverheated
		{
			get
			{
				return CurrentTemperature >= CriticalTemperature;
			}
		}

		public abstract double Vh { get; }
		public abstract double Vc { get; }

		public abstract event Action CriticalTemperatureCallback;
		public abstract event Action MaxCrankshaftRotationSpeedCallback;

		public abstract Task Run(CancellationToken token);
	}
}
