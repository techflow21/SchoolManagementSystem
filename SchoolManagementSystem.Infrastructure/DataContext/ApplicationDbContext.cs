using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Core.Contracts;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Interfaces;


namespace SchoolManagementSystem.Infrastructure.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        private readonly Tenant _tenant;

        public ApplicationDbContext(DbContextOptions options, ITenantResolver tenantResolver)
            : base(options)
        {
            _tenant = tenantResolver.GetCurrentTenant();

            if (_tenant.ConnectionString is { } connectionString)
                Database.SetConnectionString(connectionString);
        }

        public DbSet<SchoolSetting> SchoolSettings { get; set; }
        public DbSet<SchoolSession> SchoolSessions { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<NonTeacher> NonTeachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<SchoolFee> SchoolFees { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<SMSMessage> SMSMessages { get; set; }
        public DbSet<ClientPayment> ClientPayments { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SchoolSetting>().HasKey(e => e.Id);
            modelBuilder.Entity<SchoolSetting>().Property(e => e.TenantId).IsRequired();
            modelBuilder.Entity<SchoolSetting>().HasQueryFilter(e => e.TenantId == _tenant.Name);

            modelBuilder.Entity<SchoolSession>().HasKey(e => e.Id);
            modelBuilder.Entity<SchoolSession>().Property(e => e.TenantId).IsRequired();
            modelBuilder.Entity<SchoolSession>().HasQueryFilter(e => e.TenantId == _tenant.Name);

            modelBuilder.Entity<Class>().HasKey(e => e.Id);
            modelBuilder.Entity<Class>().Property(e => e.TenantId).IsRequired();
            modelBuilder.Entity<Class>().HasQueryFilter(e => e.TenantId == _tenant.Name);

            modelBuilder.Entity<Teacher>().HasKey(e => e.Id);
            modelBuilder.Entity<Teacher>().Property(e => e.TenantId).IsRequired();
            modelBuilder.Entity<Teacher>().HasQueryFilter(e => e.TenantId == _tenant.Name);

            modelBuilder.Entity<Subject>().HasKey(e => e.Id);
            modelBuilder.Entity<Subject>().Property(e => e.TenantId).IsRequired();
            modelBuilder.Entity<Subject>().HasQueryFilter(e => e.TenantId == _tenant.Name);

            modelBuilder.Entity<NonTeacher>().HasKey(e => e.Id);
            modelBuilder.Entity<NonTeacher>().Property(e => e.TenantId).IsRequired();
            modelBuilder.Entity<NonTeacher>().HasQueryFilter(e => e.TenantId == _tenant.Name);

            modelBuilder.Entity<Student>().HasKey(e => e.Id);
            modelBuilder.Entity<Student>().Property(e => e.TenantId).IsRequired();
            modelBuilder.Entity<Student>().HasQueryFilter(e => e.TenantId == _tenant.Name);

            modelBuilder.Entity<SchoolFee>().HasKey(e => e.Id);
            modelBuilder.Entity<SchoolFee>().Property(e => e.TenantId).IsRequired();
            modelBuilder.Entity<SchoolFee>().Property(e =>  e.FeeAmount).HasPrecision(18, 2);
            modelBuilder.Entity<SchoolFee>().Property(e =>  e.TotalFees).HasPrecision(18, 2);
            modelBuilder.Entity<SchoolFee>().HasQueryFilter(e => e.TenantId == _tenant.Name);

            modelBuilder.Entity<Expense>().HasKey(e => e.Id);
            modelBuilder.Entity<Expense>().Property(e => e.TenantId).IsRequired();
            modelBuilder.Entity<Expense>().HasQueryFilter(e => e.TenantId == _tenant.Name);

            modelBuilder.Entity<Salary>().HasKey(e => e.Id);
            modelBuilder.Entity<Salary>().Property(e => e.TenantId).IsRequired();
            modelBuilder.Entity<Salary>().Property(e => e.AmountPaid).HasPrecision(18, 2);
            modelBuilder.Entity<Salary>().HasQueryFilter(e => e.TenantId == _tenant.Name);

            modelBuilder.Entity<Attendance>().HasKey(e => e.Id);
            modelBuilder.Entity<Attendance>().Property(e => e.TenantId).IsRequired();
            modelBuilder.Entity<Attendance>().HasQueryFilter(e => e.TenantId == _tenant.Name);

            modelBuilder.Entity<Result>().HasKey(e => e.Id);
            modelBuilder.Entity<Result>().Property(e => e.TenantId).IsRequired();
            modelBuilder.Entity<Result>().HasQueryFilter(e => e.TenantId == _tenant.Name);

            modelBuilder.Entity<SMSMessage>().HasKey(e => e.Id);
            modelBuilder.Entity<SMSMessage>().Property(e => e.TenantId).IsRequired();
            modelBuilder.Entity<SMSMessage>().HasQueryFilter(e => e.TenantId == _tenant.Name);

            modelBuilder.Entity<ClientPayment>().HasKey(e => e.Id);
            modelBuilder.Entity<ClientPayment>().Property(e => e.TenantId).IsRequired();
            modelBuilder.Entity<ClientPayment>().Property(e => e.AmountPaid).HasPrecision(18, 2);
            modelBuilder.Entity<ClientPayment>().HasQueryFilter(e => e.TenantId == _tenant.Name);

            modelBuilder.Entity<Subscription>().HasKey(e => e.Id);
            modelBuilder.Entity<Subscription>().Property(e => e.TenantId).IsRequired();
            modelBuilder.Entity<Subscription>().HasQueryFilter(e => e.TenantId == _tenant.Name);

            modelBuilder.Entity<Report>().HasKey(e => e.Id);
            modelBuilder.Entity<Report>().Property(e => e.TenantId).IsRequired();
            modelBuilder.Entity<Report>().HasQueryFilter(e => e.TenantId == _tenant.Name);
        }

        //public DbSet<Goods> Goods { get; set; } = null!;

        /* protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
             base.OnModelCreating(modelBuilder);

             modelBuilder.Entity<Goods>().HasKey(e => e.Id);
             modelBuilder.Entity<Goods>().Property(e => e.Name).IsRequired().HasMaxLength(100);
             modelBuilder.Entity<Goods>().Property(e => e.TenantId).IsRequired();
             modelBuilder.Entity<Goods>().HasQueryFilter(e => e.TenantId == _tenant.Name);
         }*/

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>().Where(e => e.State == EntityState.Added))
            {
                entry.Entity.TenantId = _tenant.Name;
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
