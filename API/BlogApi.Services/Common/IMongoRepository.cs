using CityNavigator.Services.Base;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CityNavigator.Services.Base
{
    public interface IMongoRepository
    {
        IMongoQueryable<TEntity> Query<TEntity>()
            where TEntity : class, ICollection, new();

        Task<TEntity> Get<TEntity>(ObjectId id)
            where TEntity : class, ICollection, new();

        Task<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> filterExpression)
            where TEntity : class, ICollection, new();

        IMongoQueryable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> filterExpression = null)
            where TEntity : class, ICollection, new();

        IFindFluent<TEntity, TEntity> GetList<TEntity>(FilterDefinition<TEntity> filterDefinition)
           where TEntity : class, ICollection, new();

        IFindFluent<TEntity, TEntity> In<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> propertySelector, List<TProperty> ids, Expression<Func<TEntity, bool>> predicate = null)
           where TEntity : class, ICollection, new();

        Task Add<TEntity>(TEntity entity)
            where TEntity : class, ICollection, new();

        Task AddRange<TEntity>(List<TEntity> entities)
            where TEntity : class, ICollection, new();

        Task Update<TEntity>(ObjectId id, UpdateDefinition<TEntity> updateDefinition)
            where TEntity : class, ICollection, new();

        Task Update<TCollection>(ObjectId id, TCollection entity)
            where TCollection : class, ICollection, new();

        Task Update<TEntity>(Expression<Func<TEntity, bool>> filterExpression, UpdateDefinition<TEntity> updateDefinition)
            where TEntity : class, ICollection, new();

        Task Update<TEntity>(Expression<Func<TEntity, bool>> filterExpression, TEntity entity)
            where TEntity : class, ICollection, new();

        Task UpdateMany<TEntity>(Expression<Func<TEntity, bool>> filterExpression, UpdateDefinition<TEntity> updateDefinition)
            where TEntity : class, ICollection, new();

        Task Delete<TEntity>(Expression<Func<TEntity, bool>> filterExpression)
            where TEntity : class, ICollection, new();

        Task DeleteMany<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> propertySelector, List<TProperty> ids)
            where TEntity : class, ICollection, new();
    }
}
