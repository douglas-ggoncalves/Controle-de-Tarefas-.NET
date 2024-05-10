using ControleDeTarefas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleDeTarefas.Data.Map
{
    public class TaskMap : IEntityTypeConfiguration<TaskModel>
    {
        public void Configure(EntityTypeBuilder<TaskModel> builder)
        {
            builder.HasKey(x => x.IDTarefa);
            builder.Property(x => x.Titulo).HasMaxLength(100);
            builder.Property(x => x.Descricao).HasMaxLength(1000);
        }
    }
}
