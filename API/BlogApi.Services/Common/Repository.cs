using System;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using CityNavigator.Model.Base;
using CityNavigator.Services;
using CityNavigator.Services.Base;

namespace CityNavigator.Services.Base
{
    public class Repository : IMongoRepository
    {
        private readonly IMongoContext _context;

        public Repository(IMongoContext context)
        {
            _context = context;
        }

        private IMongoDatabase GetPrimaryDatabase()
        {
            var connectionString = ConfigurationManager.Configuration.GetSection("ConnectionStrings")["CityNavigatorConnection"];
            var databaseName = ConfigurationManager.Configuration["CityNavigatorDatabaseName"];
            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        }

        protected IMongoCollection<TEntity> Collection<TEntity>()
            where TEntity : class, ICollection, new()
        {
            var database = GetPrimaryDatabase();
            var collectionName = _context.Collection<TEntity>().Name;
            return database.GetCollection<TEntity>(collectionName);
        }

        public IMongoQueryable<TEntity> Query<TEntity>()
            where TEntity : class, ICollection, new()
        {
            return Collection<TEntity>().AsQueryable();
        }

        public async Task<TEntity> Get<TEntity>(ObjectId id)
            where TEntity : class, ICollection, new()
        {
            return await Get<TEntity>(x => x.Id == id);
        }

        public async Task<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> filterExpression)
            where TEntity : class, ICollection, new()
        {
            return await GetList(filterExpression).FirstOrDefaultAsync();
        }

        public IMongoQueryable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> filterExpression = null)
            where TEntity : class, ICollection, new()
        {
            var query = Query<TEntity>();
            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }
            return query;
        }

        public IFindFluent<TEntity, TEntity> GetList<TEntity>(FilterDefinition<TEntity> filterDefinition)
           where TEntity : class, ICollection, new()
        {
            return Collection<TEntity>().Find(filterDefinition);
        }

        public IFindFluent<TEntity, TEntity> In<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> propertySelector, List<TProperty> ids, Expression<Func<TEntity, bool>> predicate = null)
           where TEntity : class, ICollection, new()
        {
            var filter = Builders<TEntity>.Filter;
            var filterDefinition = filter.In(propertySelector, ids);

            if (predicate != null)
            {
                filterDefinition = filterDefinition & filter.Where(predicate);
            }

            return GetList(filterDefinition);
        }

        public async Task Add<TEntity>(TEntity entity)
            where TEntity : class, ICollection, new()
        {
            await Collection<TEntity>().InsertOneAsync(entity);
        }

        public async Task AddRange<TEntity>(List<TEntity> entities)
            where TEntity : class, ICollection, new()
        {
            await Collection<TEntity>().InsertManyAsync(entities);
        }

        public async Task Update<TEntity>(ObjectId id, UpdateDefinition<TEntity> updateDefinition)
            where TEntity : class, ICollection, new()
        {
            await Update(x => x.Id == id, updateDefinition);
        }

        public async Task Update<TCollection>(ObjectId id, TCollection entity)
            where TCollection : class, ICollection, new()
        {
            await Update(x => x.Id == id, entity);
        }

        public async Task Update<TEntity>(Expression<Func<TEntity, bool>> filterExpression, UpdateDefinition<TEntity> updateDefinition)
            where TEntity : class, ICollection, new()
        {
            await Collection<TEntity>().UpdateOneAsync(filterExpression, updateDefinition);
        }

        public async Task Update<TEntity>(Expression<Func<TEntity, bool>> filterExpression, TEntity entity)
            where TEntity : class, ICollection, new()
        {
            await Collection<TEntity>().ReplaceOneAsync(filterExpression, entity);
        }

        public async Task UpdateMany<TEntity>(Expression<Func<TEntity, bool>> filterExpression, UpdateDefinition<TEntity> updateDefinition)
            where TEntity : class, ICollection, new()
        {
            await Collection<TEntity>().UpdateManyAsync(filterExpression, updateDefinition);
        }

        public async Task Delete<TEntity>(Expression<Func<TEntity, bool>> filterExpression)
            where TEntity : class, ICollection, new()
        {
            await Collection<TEntity>().DeleteOneAsync(filterExpression);
        }

        public async Task DeleteMany<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> propertySelector, List<TProperty> ids)
            where TEntity : class, ICollection, new()
        {
            var deletes = Builders<TEntity>.Filter.In(propertySelector, ids);
            await Collection<TEntity>().DeleteManyAsync(deletes);
        }
    }
}