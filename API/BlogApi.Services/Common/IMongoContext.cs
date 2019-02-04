using CityNavigator.Model.Attributes;
using CityNavigator.Model.Base;
using CityNavigator.Services.Base;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace CityNavigator.Services.Base
{
    public interface IMongoContext
    {
        CollectionDescriptor Collection<TEntity>()
            where TEntity : ICollection;
    }

    public class MongoContext : IMongoContext
    {
        private readonly ConcurrentDictionary<Type, CollectionDescriptor> _collections =
            new ConcurrentDictionary<Type, CollectionDescriptor>();

        public CollectionDescriptor Collection<TEntity>()
            where TEntity : ICollection
        {
            return _collections.GetOrAdd(typeof(TEntity), t => GetCollectionDescriptor(typeof(TEntity)));
        }

        private CollectionDescriptor GetCollectionDescriptor(Type type)
        {
            var descriptor = type.GetCustomAttribute<CollectionAttribute>();
            var name = descriptor != null ? descriptor.Name : type.Name;
            return new CollectionDescriptor { Name = name };
        }
    }
}
