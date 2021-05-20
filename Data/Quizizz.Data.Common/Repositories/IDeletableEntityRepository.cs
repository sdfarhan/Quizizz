﻿namespace Quizizz.Data.Common.Repositories
{
    using System.Linq;

    using Quizizz.Data.Common.Models;

    public interface IDeletableEntityRepository<TEntity> : IDeletableEntityRepository<TEntity>
        where TEntity : class, IDeletableEntity
    {
        IQueryable<TEntity> AllWithDeleted();

        IQueryable<TEntity> AllAsNoTrackingWithDeleted();

        void HardDelete(TEntity entity);

        void Undelete(TEntity entity);
    }
}
