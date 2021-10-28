using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Services.Freelancer.Entities
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Application> Application { get; set; }
        public virtual DbSet<Competence> Competence { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectCompetence> ProjectCompetence { get; set; }
        public virtual DbSet<Type> Type { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserCompetence> UserCompetence { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasKey(e => e.IdApplication);

                entity.ToTable("APPLICATION");

                //entity.HasIndex(e => e.ProjectId, "PROJECT_FK");

                //entity.HasIndex(e => e.UserId, "USER_FK");

                entity.Property(e => e.IdApplication)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("ID_APPLICATION");

                entity.Property(e => e.ApplicationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("APPLICATION_DATE");
                
                entity.Property(e => e.IsAssignedTo)
                    .HasColumnType("int")
                    .HasMaxLength(2)
                    .HasColumnName("IS_ASSIGNED_TO");

                entity.Property(e => e.ProjectId)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("PROJECT_ID");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_APPLICAT_PROJECT_PROJECT");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_APPLICAT_USER_USER");
            });

            modelBuilder.Entity<Competence>(entity =>
            {
                entity.HasKey(e => e.IdCompetence);

                entity.ToTable("COMPETENCE");

                entity.Property(e => e.IdCompetence)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("ID_COMPETENCE");

                entity.Property(e => e.CompetenceName)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("COMPETENCE_NAME");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.IdMessage);

                entity.ToTable("MESSAGE");

                //entity.HasIndex(e => e.Sender, "FROM_FK");

                //entity.HasIndex(e => e.Receiver, "TO_FK");

                entity.Property(e => e.IdMessage)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.DateSeen)
                    .HasColumnType("datetime");

                entity.Property(e => e.IsSeen);

                entity.Property(e => e.MessageDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.Receiver)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("RECEIVER");

                entity.Property(e => e.Sender)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("SENDER");

                entity.HasOne(d => d.ReceiverNavigation)
                    .WithMany(p => p.MessageReceiverNavigations)
                    .HasForeignKey(d => d.Receiver)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MESSAGE_FROM_USER");

                entity.HasOne(d => d.SenderNavigation)
                    .WithMany(p => p.MessageSenderNavigations)
                    .HasForeignKey(d => d.Sender)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MESSAGE_TO_USER");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.IdProject);

                entity.ToTable("PROJECT");

                //entity.HasIndex(e => e.AddedBy, "ADDED_BY_FK");

                entity.Property(e => e.IdProject)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("ID_PROJECT");

                entity.Property(e => e.AddedBy)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("ADDED_BY");

                entity.Property(e => e.ApplyBefore)
                    .HasColumnType("datetime")
                    .HasColumnName("APPLY_BEFORE");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("DATE");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("PRICE");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");

                entity.HasOne(d => d.AddedByNavigation)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.AddedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROJECT_ADDED_BY_USER");
            });

            modelBuilder.Entity<ProjectCompetence>(entity =>
            {
                entity.HasKey(e => new { e.CompetenceId, e.ProjectId });

                entity.ToTable("PROJECT_COMPETENCE");

                //entity.HasIndex(e => e.ProjectId, "PROJECT_COMPETENCE2_FK");

                //entity.HasIndex(e => e.CompetenceId, "PROJECT_COMPETENCE_FK");

                entity.Property(e => e.CompetenceId)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("COMPETENCE_ID");

                entity.Property(e => e.ProjectId)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("PROJECT_ID");

                entity.HasOne(d => d.Competence)
                    .WithMany(p => p.ProjectCompetences)
                    .HasForeignKey(d => d.CompetenceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROJECT__PROJECT_C_COMPETEN");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectCompetences)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROJECT__PROJECT_C_PROJECT");
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.HasKey(e => e.IdType);

                entity.ToTable("TYPE");

                entity.Property(e => e.IdType)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("ID_TYPE");

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NAME_TYPE");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.ToTable("USER");

                //entity.HasIndex(e => e.TypeId, "TYPE_UTILISATEUR_FK");

                entity.Property(e => e.IdUser)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("ID_USER");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(256)
                    .HasColumnName("COMPANY_NAME");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("FIRST_NAME");

                entity.Property(e => e.Identifiant)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("IDENTIFIANT");

                entity.Property(e => e.LastConnected)
                    .HasColumnType("datetime")
                    .HasColumnName("LAST_CONNECTED");

                entity.Property(e => e.LastName)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("LAST_NAME");

                entity.Property(e => e.Mail)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MAIL");

                entity.Property(e => e.Password)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.TypeId)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("TYPE_ID");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USER_TYPE_UTIL_TYPE");
            });

            modelBuilder.Entity<UserCompetence>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.CompetenceId });

                entity.ToTable("USER_COMPETENCE");

                //entity.HasIndex(e => e.CompetenceId, "USER_COMPETENCE2_FK");

                //entity.HasIndex(e => e.UserId, "USER_COMPETENCE_FK");

                entity.Property(e => e.UserId)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");

                entity.Property(e => e.CompetenceId)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("COMPETENCE_ID");

                entity.HasOne(d => d.Competence)
                    .WithMany(p => p.UserCompetences)
                    .HasForeignKey(d => d.CompetenceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USER_COM_USER_COMP_COMPETEN");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserCompetences)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USER_COM_USER_COMP_USER");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
