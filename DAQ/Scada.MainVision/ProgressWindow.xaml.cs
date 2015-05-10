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
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void SetValue(int value)
        {
            this.ProgressBar.Value = value;
        }

        private void WindowMoveHandler(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
	}
}
