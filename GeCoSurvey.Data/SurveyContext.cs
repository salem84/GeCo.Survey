using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using GeCoSurvey.Domain;
using System.Data.Entity.Infrastructure;

namespace GeCoSurvey.Data
{
    public class SurveyContext : DbContext
    {
        // public MyFinanceContext() : base("MyFinance") { }
        //public DbSet<User> Users { get; set; }
        //public DbSet<Role> Roles { get; set; }

        public DbSet<Dipendente> Dipendenti { get; set; }
        public DbSet<Ruolo> Ruoli { get; set; }
        public DbSet<Competenza> Competenze { get; set; }
        //public DbSet<Area> Aree { get; set; }
        public DbSet<LivelloConoscenza> LivelliConoscenza { get; set; }
        public DbSet<TipologiaCompetenza> TipologieCompetenze { get; set; }
        public DbSet<ConoscenzaCompetenza> ConoscenzaCompetenze { get; set; }
        public DbSet<Parametro> Parametri { get; set; }



        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<SurveySession> Sessions { get; set; }
        //public DbSet<ResponsabiliDipendenti> ResponsabiliDipendenti { get; set; }


        
        public virtual void Commit()
        {
            base.SaveChanges();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

            CreaModelloComune(modelBuilder);

            /*modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .Map(m =>
                {
                    m.ToTable("RoleMemberships");
                    m.MapLeftKey("UserName");
                    m.MapRightKey("RoleName");
                });*/

            modelBuilder.Entity<SurveySession>()
                .HasRequired(session => session.Survey)
                .WithMany(survey => survey.SurveySessions)
                .WillCascadeOnDelete(false);

            /*modelBuilder.Entity<ResponsabiliDipendenti>()
                .HasKey(e => new { e.Responsabile, e.Dipendente });*/
        }

        private void CreaModelloComune(DbModelBuilder modelBuilder)
        {
            modelBuilder.ComplexType<Area>();

            modelBuilder.Entity<Competenza>()
                .Map(m => m.ToTable("Competenze"));

            modelBuilder.Entity<ConoscenzaCompetenza>()
                .Map(m => m.ToTable("ConoscenzeCompetenza"));
            //.HasRequired(m => m.Dotato).WithRequiredPrincipal().WillCascadeOnDelete();


            /*modelBuilder.Entity<Ruolo>()
                .Map(m =>m.ToTable("Ruoli"));*/
            modelBuilder.Entity<Anagrafica>()
               .Map(m => m.ToTable("Anagrafica"))
               .Map<Dipendente>(m => m.Requires("Tipo").HasValue("DIP"))
               .Map<Ruolo>(m => m.Requires("Tipo").HasValue("ROLE"));


            modelBuilder.Entity<LivelloConoscenza>()
                .Map(m => m.ToTable("LivelliConoscenza"));


            modelBuilder.Entity<TipologiaCompetenza>()
                .Map(m => m.ToTable("TipologieCompetenza"));


            modelBuilder.Entity<Parametro>()
                .HasKey(m => m.Nome)
                .Map(m => m.ToTable("Parametri"));
        }
    }

    
    
}
