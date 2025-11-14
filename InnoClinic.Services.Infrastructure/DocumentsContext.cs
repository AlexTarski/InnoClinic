using InnoClinic.Services.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Services.Infrastructure
{
    public class DocumentsContext : DbContext
    {
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Document> Documents { get; set; }

        public DocumentsContext()
        {
        }

        public DocumentsContext(DbContextOptions<DocumentsContext> options)
            : base(options) { }
    }
}