using ControleDeTarefas.Data.Map;
using ControleDeTarefas.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleDeTarefas.Data
{
    public class SystemTaskDBContext : DbContext
    {
        public SystemTaskDBContext(DbContextOptions<SystemTaskDBContext> options) : base(options)
        {
            
        }

        public DbSet<TaskModel> Tarefas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TaskMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}