using System;

namespace InnoClinic.Documents.Domain.Entities
{
    public class Document : File
    {
        public required Guid ResultId { get; set; }
    }
}