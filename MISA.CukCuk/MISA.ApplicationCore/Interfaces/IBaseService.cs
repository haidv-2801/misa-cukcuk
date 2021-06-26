using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Interfaces
{
    public interface IBaseService<TEntity>
    {

        /// <summary>
        /// Lấy danh sách bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// DVHAI (25/06/2021)
        IEnumerable<TEntity> GetEntities();

        /// <summary>
        ///  Lấy bản ghi theo id
        /// </summary>
        /// <param name="entityId">Id của bản ghi</param>
        /// <returns>Bản ghi thông tin 1 bản ghi</returns>
        /// DVHAI (25/06/2021)
        TEntity GetEntityById(Guid entityId);

        /// <summary>
        ///  Lấy bản ghi viên theo mã
        /// </summary>
        /// <param name="entityCode">Mã bản ghi</param>
        /// <returns>Bản ghi thông tin 1 bản ghi</returns>
        /// DVHAI (25/06/2021)
        TEntity GetEntityByCode(string entityCode);

        /// <summary>
        /// Thêm bản ghi
        /// </summary>
        /// <param name="enitity">Thông tin bản ghi</param>
        /// <returns>Số bản ghi</returns>
        int Insert(TEntity enitity);

        /// <summary>
        /// Cập nhập thông tin bản ghi
        /// </summary>
        /// <param name="entityId">Id bản ghi</param>
        /// <param name="entity">Thông tin bản ghi</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// DVHAI (25/06/2021)
        int Update(Guid entityId, TEntity entity);

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        /// DVHAI (25/06/2021)
        int Delete(Guid entityId);
    }
}
