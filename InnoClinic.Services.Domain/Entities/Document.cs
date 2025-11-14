using System;

namespace InnoClinic.Services.Domain.Entities
{
    public class Document : File
    {
        public required Guid ResultId { get; set; }
    }
}