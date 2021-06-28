using MISA.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore
{
    public class BaseService<TEntity> : IBaseService<TEntity>
    {
        #region Declare
        IBaseRepository<TEntity> _baseRepository;
        #endregion

        #region Constructer
        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        #endregion

        #region Methods
        public IEnumerable<TEntity> GetEntities()
        {
            var entities = _baseRepository.GetEntities();
            return entities;
        }

        public TEntity GetEntityById(Guid entityId)
        {
            var entity = _baseRepository.GetEntityById(entityId);
            return entity;
        }

        //public TEntity GetEntityByCode(string entityCode)
        //{
        //    var entity = _baseRepository.GetEntityByCode(entityCode);
        //    return entity;
        //}

        public int Insert(TEntity entity)
        {
            int rowAffects = _baseRepository.Insert(entity);
            return rowAffects;
        }

        public int Update(Guid entityId, TEntity entity)
        {
            int rowAffects = _baseRepository.Update(entityId, entity);
            return rowAffects;
        }

        public int Delete(Guid entityId)
        {
            int rowAffects = _baseRepository.Delete(entityId);
            return rowAffects;
        }
        #endregion
    }
}
