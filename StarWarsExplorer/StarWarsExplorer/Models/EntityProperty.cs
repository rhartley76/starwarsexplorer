namespace StarWarsExplorer.Models
{
	public class EntityProperty
	{
		public EntityProperty(string name, string value)
		{
			Name = name;
			Value = value;
		}
		public string Name { get; }

		public string Value { get; }
	}
}