using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using MISA.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.CukCuk.Web.Controllers
{
    [Route("/v1/[controller]")]
    [ApiController]
    public class BaseEntityController<TEntity> : ControllerBase
    {
        #region Declare
        IBaseService<TEntity> _baseService;
        #endregion

        public BaseEntityController(IBaseService<TEntity> baseService)
        {
            _baseService = baseService;
        }

        /// <summary>
        /// Lấy danh sách thực thể
        /// </summary>
        /// <returns>Danh sách thực thể</returns>
        /// CreatedBy: DVHAI 24/06/2021
        [HttpGet]
        public IActionResult Get()
        {
            var entities = _baseService.GetEntities();

            return Ok(entities);
        }

        /// <summary>
        /// Lấy thực thể theo id
        /// </summary>
        /// <param name="id">id của thực thể</param>
        /// <returns>Một thực thể tìm được theo id</returns>
        /// CreateedBy: DVHAI 24/06/2021
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var entity = _baseService.GetEntityById(Guid.Parse(id));

            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        /// <summary>
        /// Thêm một thực thể mới
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Sô bản ghi bị ảnh hưởng</returns>
        /// CreateedBy: DVHAI 24/06/2021
        [HttpPost]
        public IActionResult Post([FromBody] TEntity entity)
        {
            var serviceResult = _baseService.Insert(entity);

            //if (serviceResult.MISACode == MISACode.InValid)
            //    return Content(HttpStatusCode.BadRequest, serviceResult);

            return Created("Add", serviceResult);
        }

        /// <summary>
        /// Sửa một thực thể
        /// </summary>
        /// <param name="id">id của bản ghi</param>
        /// <param name="entity">thông tin của bản ghi</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// CreateedBy: DVHAI 24/06/2021
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] string id, [FromBody] TEntity entity)
        {
            var serviceResult = _baseService.Update(Guid.Parse(id), entity);

            return Ok(serviceResult);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="id">id của bản ghi</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var rowAffects = _baseService.Delete(Guid.Parse(id));
            return Ok(rowAffects);
        }

        [HttpPost("import")]
        public IActionResult Import(IFormFile formFile, CancellationToken cancellationToken)
        {
            //1. Đọc file excel
            var serviceResult = _baseService.readExcelFile(formFile, cancellationToken);

            //2. Validate dữ liệu


            //3. Cất dữ liệu
            return Ok(serviceResult);
        }

        [HttpPost("multiinsert")]
        public IActionResult MultiInsert([FromBody] IEnumerable<TEntity> ieEntities)
        {

            return Ok();
        }
    }
}
