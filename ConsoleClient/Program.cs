using Engines;
using EnginesTestStands;

namespace EngineTest
{
	internal class Program
	{
		public static void Main()
		{
			var task = Run();
			task.Wait();
		}

		private static async Task Run()
		{
			double ambientTemperature = 0;

			while (true)
			{
				try
				{
					Console.Write("Enter ambient temperature: ");
					ambientTemperature = Convert.ToDouble(Console.ReadLine());
				}
				catch
				{
					continue;
				}
				break;
			}

			EngineTestOverheat test = new EngineTestOverheat(new Engine());
			var result = (EngineTestOverheatResult)await test.Run(ambientTemperature);
			if (result.IsEngineOverheated)
			{
				Console.WriteLine($"Engine is overheated: ellapsed time {result.EllapsedTimeBeforeOverheat}");
			}
			else
			{
				Console.WriteLine($"Engine is not overheated: ellapsed time {result.EllapsedTimeBeforeOverheat}");
			}

			var test2 = new EngineMaxPowerTest(new Engine());
			var result2 = (EngineMaxPowerTestResult)await test2.Run(ambientTemperature);
			Console.WriteLine($"Max Power {result2.MaxPower}");
		}
	}
}