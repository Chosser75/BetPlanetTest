using System;
using Microsoft.EntityFrameworkCore;

namespace BetPlanetTest
{
    /// <summary>
    /// Класс-контекст базы данных test
    /// </summary>
    public partial class testContext : DbContext
    {

        public testContext()
        {
        }
                
        public testContext(DbContextOptions<testContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(GetPostgresConnectionString());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comments>(entity =>
            {
                entity.ToTable("comments", "test");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('test.seq_comments'::regclass)");

                entity.Property(e => e.IdUser).HasColumnName("id_user");

                entity.Property(e => e.Txt)
                    .IsRequired()
                    .HasColumnName("txt")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users", "test");

                entity.HasIndex(e => e.Email)
                    .HasName("UQ_users_email")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('test.seq_users'::regclass)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("character varying");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.HasSequence("seq_comments", "test");

            modelBuilder.HasSequence("seq_users", "test");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        /// <summary>
        /// Генерирует из переменных среды и возвращает строку параметров соединения с сервером Postgres
        /// </summary>
        /// <returns></returns>
        private string GetPostgresConnectionString()
        {
            // Параметры соединения с сервером Postgres, получаемые из ENV среды
            // согласно техзаданию: "Настройки соединения с сервером Postgres читать из ENV среды"
            int port = int.Parse(Environment.GetEnvironmentVariable("port"));            
            string host = Environment.GetEnvironmentVariable("host");
            string user = Environment.GetEnvironmentVariable("user");
            string password = Environment.GetEnvironmentVariable("password");            
            string schema = Environment.GetEnvironmentVariable("schema");
            // или все же database, а не schema?
            //string database = Environment.GetEnvironmentVariable("database");

            // По техзаданию неясно, где же все-таки хранить эту переменную, в ENV или config-файле:
            //string endpoint = Environment.GetEnvironmentVariable("endpoint");

            return String.Format("Host={0};Port={1};Database={2};Username={3};Password={4}", host, port, schema, user, password);
        }
    }
}
