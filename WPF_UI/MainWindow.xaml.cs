using Engines;
using EnginesTestStands;
using System.Windows;
using System.Windows.Controls;

namespace WPF_UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		double _ambientTemperature;
		public MainWindow()
		{
			InitializeComponent();
			restartButton.Visibility = Visibility.Collapsed;
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				_ambientTemperature = Convert.ToDouble(textbox.Text);

				button.Visibility = Visibility.Collapsed;
				textbox.Visibility = Visibility.Collapsed;

				var task = Run();
			}
			catch { }
		}

		private void Restart_Button_Click(object sender, RoutedEventArgs e)
		{
			restartButton.Visibility = Visibility.Collapsed;
			button.Visibility = Visibility.Visible;
			textbox.Visibility = Visibility.Visible;

			textbox.Clear();
			textbox.Text = "Enter ambient temperature";
			label.Content = string.Empty;
		}

		private async Task Run()
		{
			EngineTestOverheat test = new EngineTestOverheat(new Engine());
			var result = (EngineTestOverheatResult)await test.Run(_ambientTemperature);
			if (result.IsEngineOverheated)
			{
				label.Content = $"Engine is overheated: ellapsed time {result.EllapsedTimeBeforeOverheat}\n";
			}
			else
			{
				label.Content = $"Engine is not overheated: ellapsed time {result.EllapsedTimeBeforeOverheat}\n";
			}

			var test2 = new EngineMaxPowerTest(new Engine());
			var result2 = (EngineMaxPowerTestResult)await test2.Run(_ambientTemperature);
			label.Content += $"Max Power {result2.MaxPower}";

			restartButton.Visibility = Visibility.Visible;
		}
	}
}