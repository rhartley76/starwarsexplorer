using System.Windows;
using StarWarsExplorer.Models;

namespace StarWarsExplorer
{
	/// <summary>
	/// Interaction logic for EntityDetails.xaml
	/// </summary>
	public partial class EntityDetails : Window
	{
		public EntityDetails(Entity entity)
		{
			InitializeComponent();
			DataContext = entity;
		}
	}
}
