using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InnoClinic.Appointments.Business.Interfaces;
using InnoClinic.Appointments.Domain;
using InnoClinic.Appointments.Domain.Entities;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;
using InnoClinic.Shared.Pagination;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Appointments.Business.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ILogger<AppointmentService> _logger;
        private readonly IAppointmentsRepository _repository;

        public AppointmentService(IAppointmentsRepository repository, ILogger<AppointmentService> logger)
        {
            _logger = logger ?? throw new DiNullReferenceException(nameof(logger));
            _repository = repository ?? throw new DiNullReferenceException(nameof(repository));
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetAllAsync));
            var result = await _repository.GetAllAsync();
            Logger.DebugExitingMethod(_logger, nameof(GetAllAsync));

            return result;
        }

        public async Task<PagedList<Appointment>> GetAllFilteredAsync(QueryStringParameters queryParams)
        {
            try
            {
                Logger.DebugStartProcessingMethod(_logger, nameof(GetAllFilteredAsync));
                var query = _repository.GetEntityQuery();
                ApplyFilters(ref query, queryParams);
                query = query.OrderBy(user => user.Id);

                var result = await _repository.GetAllAsync(query, queryParams);
                Logger.DebugExitingMethod(_logger, nameof(GetAllFilteredAsync));

                return result;
            }
            catch (Exception ex) when (ex is OverflowException || ex is PageOutOfRangeException)
            {
                Logger.WarningFailedDoAction(_logger, nameof(GetAllFilteredAsync));

                throw new PaginationArgumentException($"Failed to get {nameof(Appointment)}: {ex.Message}", ex);
            }
        }

        public async Task<Appointment> GetByIdAsync(Guid id)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetByIdAsync));
            var result = await _repository.GetByIdAsync(id);
            Logger.DebugExitingMethod(_logger, nameof(GetByIdAsync));

            return result ?? throw new KeyNotFoundException($"{nameof(Appointment)} with ID {id} was not found");
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _repository.SaveAllAsync();
        }

        public void ApplyFilters(ref IQueryable<Appointment> query, QueryStringParameters queryParams)
        {
            
        }
    }
}