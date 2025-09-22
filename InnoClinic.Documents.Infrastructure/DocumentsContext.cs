using InnoClinic.Documents.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Documents.Infrastructure;

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