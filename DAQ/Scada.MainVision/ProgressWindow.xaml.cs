using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Scada.Controls;

namespace Scada.MainVision
{
	/// <summary>
	/// Interaction logic for ProgressWindow.xaml
	/// </summary>
	public partial class ProgressWindow : Window
	{
		public ProgressWindow()
		{
			InitializeComponent();
		}


        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            
            this.BusyCtrl.IsBusy = true;
            this.BusyCtrl.Text = "";

            var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += (s, evt) =>
            {
                this.Close();
            };
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            dispatcherTimer.Start();

        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void WindowMoveHandler(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
	}
}
