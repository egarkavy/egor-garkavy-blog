using System;

namespace CityNavigator.Model.Attributes
{
    public class CollectionAttribute : Attribute
    {
        public string Name { get; private set; }

        public CollectionAttribute(string name)
        {
            Name = name;
        }
    }
}
