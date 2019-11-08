using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StarWarsExplorer.Models;
using StarWarsExplorer.ViewModels;

namespace StarWarsExplorer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new ExplorerViewModel();
		}

		private void ItemDoubleClick_Handler(object sender, MouseButtonEventArgs e)
		{
			var detailsWindow = new EntityDetails(((ListViewItem)sender).Content as Entity);
			detailsWindow.ShowDialog();
		}
	}
}
