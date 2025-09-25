using System;
using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Documents.Domain.Entities
{
    public abstract class File
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Url { get; set; }
    }
}