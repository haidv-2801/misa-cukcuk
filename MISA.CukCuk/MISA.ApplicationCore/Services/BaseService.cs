using MISA.ApplicationCore.Entities;
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

        public virtual int Insert(TEntity entity)
        {
            var isValid = Validate(entity);

            if (isValid)
                _baseRepository.Insert(entity);
            
            return 0;
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

        protected bool Validate(TEntity entity)
        {
            var isValid = true;

            //1. Đọc các property
            var properties = entity.GetType().GetProperties();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity);

                //1.1 Kiểm tra xem  có attribute cần phải validate không
                if(property.IsDefined(typeof(IRequired), false))
                {
                    //1.1.1 Check bắt buộc nhập
                    if (propertyValue == null)
                        isValid = false;
                }

                if(property.IsDefined(typeof(CheckDuplicate), false))
                {
                    //1.1.2 Check trùng
                    var entityDuplicate = _baseRepository.GetEntityByProperty(propertyName, propertyValue);
                    if(entityDuplicate != null)
                        isValid = false;
                }
            }

            return isValid;
        }
        #endregion
    }
}
