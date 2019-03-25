namespace Bank.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BankDB : DbContext
    {
        public BankDB()
            : base("name=BankDB")
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<AuditTrail> AuditTrails { get; set; }
        public virtual DbSet<CreditScore> CreditScore { get; set; }
        public virtual DbSet<EmailConfirmation> EmailConfirmation { get; set; }
        public virtual DbSet<LoanApprovalRule> LoanApprovalRules { get; set; }
        public virtual DbSet<LoanRequest> LoanRequest { get; set; }
        public virtual DbSet<LoanResult> LoanResult { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .Property(e => e.Address1)
                .IsFixedLength();

            modelBuilder.Entity<Address>()
                .Property(e => e.City)
                .IsFixedLength();

            modelBuilder.Entity<Address>()
                .Property(e => e.State)
                .IsFixedLength();

            modelBuilder.Entity<Address>()
                .Property(e => e.ZipCode)
                .IsFixedLength();

            modelBuilder.Entity<EmailConfirmation>()
                .Property(e => e.Token)
                .IsFixedLength();

            modelBuilder.Entity<LoanApprovalRule>()
                .Property(e => e.LoanType)
                .IsFixedLength();

            modelBuilder.Entity<LoanRequest>()
                .Property(e => e.SocialSecurity)
                .IsFixedLength();

            modelBuilder.Entity<LoanRequest>()
                .Property(e => e.Employer)
                .IsFixedLength();

            modelBuilder.Entity<LoanRequest>()
                .Property(e => e.JobTitle)
                .IsFixedLength();

            modelBuilder.Entity<LoanRequest>()
                .Property(e => e.EmploymentType)
                .IsFixedLength();

            modelBuilder.Entity<LoanRequest>()
                .Property(e => e.Status)
                .IsFixedLength();

            modelBuilder.Entity<LoanRequest>()
                .Property(e => e.LoanType)
                .IsFixedLength();

            modelBuilder.Entity<LoanResult>()
                .Property(e => e.Decision)
                .IsFixedLength();

            modelBuilder.Entity<LoanResult>()
                .Property(e => e.Comments)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.FirstName)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.LastName)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.MiddleName)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Telephone)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsFixedLength();

            /*
            //JDR: All these remove, have been repaced by methods within their classes
            modelBuilder.Entity<LoanRequest>()
                .HasMany(e => e.AuditTrails)
                .WithRequired(e => e.LoanRequest)
                .HasForeignKey(e => e.RequestID)
                .WillCascadeOnDelete(false);

             modelBuilder.Entity<User>()
                .HasMany(e => e.CreditScores)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

             modelBuilder.Entity<User>()
                .HasMany(e => e.EmailConfirmations)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

             modelBuilder.Entity<LoanRequest>()
                .HasMany(e => e.LoanResults)
                .WithRequired(e => e.LoanRequest)
                .HasForeignKey(e => e.RequestID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.LoanRequests)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
            */
        }
    }
}
