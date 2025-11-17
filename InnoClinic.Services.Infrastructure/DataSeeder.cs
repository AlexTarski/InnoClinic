using System;
using System.Threading.Tasks;

using InnoClinic.Services.Domain.Entities;
using InnoClinic.Shared.DataSeeding;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Services.Infrastructure
{
    public class DataSeeder
    {
        private readonly ServicesContext _context;

        public DataSeeder(ServicesContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (!await _context.ServiceCategories.AnyAsync())
            {
                var serviceCategories = new ServiceCategory[]
                {
                    CreateServiceCategory(SampleData.Consultations),
                    CreateServiceCategory(SampleData.Diagnostics),
                    CreateServiceCategory(SampleData.Analyses)
                };

                await _context.AddRangeAsync(serviceCategories);
            }

            if (!await _context.Specializations.AnyAsync())
            {
                var specializations = new Specialization[]
                {
                    CreateSpecialization(SampleData.Cardiology),
                    CreateSpecialization(SampleData.Dermatology),
                    CreateSpecialization(SampleData.Neurology),
                    CreateSpecialization(SampleData.Radiology),
                    CreateSpecialization(SampleData.Hematology),
                    CreateSpecialization(SampleData.Urology),
                    CreateSpecialization(SampleData.Gastro),
                    CreateSpecialization(SampleData.Endo),
                    CreateSpecialization(SampleData.Infectious)
                };

                await _context.AddRangeAsync(specializations);
            }

            if (!await _context.Services.AnyAsync())
            {
                var services = new Service[]
                {
                    CreateService(SampleData.CardioConsultation),
                    CreateService(SampleData.DermaConsultation),
                    CreateService(SampleData.NeuroConsultation),
                    CreateService(SampleData.GastroConsultation),
                    CreateService(SampleData.EndoConsultation),
                    CreateService(SampleData.Ecg),
                    CreateService(SampleData.SkinBiopsy),
                    CreateService(SampleData.Eeg),
                    CreateService(SampleData.AbdominalUltrasound),
                    CreateService(SampleData.ThyroidUltrasound),
                    CreateService(SampleData.CompleteBloodCount),
                    CreateService(SampleData.CovidPcrTest),
                    CreateService(SampleData.Urinalysis),
                    CreateService(SampleData.BloodGlucoseTest),
                    CreateService(SampleData.LiverFuncTest)
                };

                await _context.AddRangeAsync(services);
            }

            await _context.SaveChangesAsync();
        }

        private static Service CreateService(Guid serviceId)
        {
            var service = SampleData.Services[serviceId];

            return new Service
            {
                Id = serviceId,
                CategoryId = service.CategoryId,
                SpecializationId = service.SpecializationId,
                Name = service.Name,
                Price = service.Price,
                IsActive = service.IsActive
            };
        }

        private static ServiceCategory CreateServiceCategory(Guid serviceCategoryId)
        {
            var serviceCategory = SampleData.ServiceCategories[serviceCategoryId];

            return new ServiceCategory
            {
                Id = serviceCategoryId,
                Name = serviceCategory.Name,
                TimeSlotSize = serviceCategory.TimeSlotSize,
            };
        }

        private static Specialization CreateSpecialization(Guid specializationId)
        {
            var specialization = SampleData.Specializations[specializationId];

            return new Specialization
            {
                Id= specializationId,
                Name = specialization.Name,
                IsActive= specialization.IsActive
            };
        }
    }
}