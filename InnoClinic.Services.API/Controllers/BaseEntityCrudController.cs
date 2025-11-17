using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using InnoClinic.Services.Business.Interfaces;
using InnoClinic.Services.Business.Models;
using InnoClinic.Services.Domain.Entities;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;
using InnoClinic.Shared.Pagination;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace InnoClinic.Services.API.Controllers
{
    public abstract class BaseEntityCrudController<T, TParams, K> : ControllerBase
    where T : Entity
    where TParams : QueryStringParameters
    where K : EntityModel
    {
        protected readonly ILogger<BaseEntityCrudController<T, TParams, K>> _logger;
        protected readonly IEntityService<T, TParams> _service;
        protected readonly IMapper _mapper;

        protected BaseEntityCrudController(ILogger<BaseEntityCrudController<T, TParams, K>> logger,
            IEntityService<T, TParams> service,
            IMapper mapper)
        {
            _logger = logger ?? throw new DiNullReferenceException(nameof(logger));
            _service = service ?? throw new DiNullReferenceException(nameof(service));
            _mapper = mapper ?? throw new DiNullReferenceException(nameof(mapper));
        }

        protected async Task<IActionResult> GetAllAsync()
        {
            var result = await _service.GetAllAsync();

            return Ok(_mapper.Map<IEnumerable<K>>(result));
        }

        protected async Task<IActionResult> GetAllFilteredAsync(TParams queryParams)
        {
            var result = await _service.GetAllFilteredAsync(queryParams);
            AddPaginationHeader(result.TotalCount, result.PageSize, result.CurrentPage, result.TotalPages,
                result.HasNext, result.HasPrevious);

            return Ok(result);
        }

        protected async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);

                return Ok(_mapper.Map<K>(result));
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warning(_logger, ex, $"Failed to get by ID: {id}");

                return NotFound($"{typeof(T).Name} with ID {id} was not found");
            }
        }

        private void AddPaginationHeader(int totalCount, int pageSize, int currentPage, int totalPages,
                bool hasNext, bool hasPrevious)
        {
            var metadata = new
            {
                totalCount,
                pageSize,
                currentPage,
                totalPages,
                hasNext,
                hasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        }

        protected bool IsReceptionist()
        {
            return User.Identity?.IsAuthenticated == true
               && User.IsInRole(UserRoles.Receptionist);
        }
    }
}