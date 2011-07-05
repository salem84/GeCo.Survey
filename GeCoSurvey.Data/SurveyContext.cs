using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using GeCoSurvey.Domain;

namespace GeCoSurvey.Data
{
    public class SurveyContext : DbContext
    {
       // public MyFinanceContext() : base("MyFinance") { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<SurveySession> Sessions { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
             modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .Map(m =>
            {
                m.ToTable("RoleMemberships");
                m.MapLeftKey("UserName");
                m.MapRightKey("RoleName");
            });
        }
    }

    public class SurveyContextInitializer : DropCreateDatabaseIfModelChanges<SurveyContext>
    {
        protected override void Seed(SurveyContext context)
        {
            var roles = new List<Role>{
                new Role{RoleName = "Administrator"},
                new Role{RoleName = "User"}               
            };

            roles.ForEach(r => context.Roles.Add(r));

            var survey = new Survey { Name = "Test", Active = true };
            context.Surveys.Add(survey);

            //---
            var question = new Question { SurveyId = survey.Id, Text="Prima domanda" };
            context.Questions.Add(question);

            var subQuestions = new List<SubQuestion>{
                new SubQuestion{QuestionId = question.Id, Text = "Alto"},
                new SubQuestion{QuestionId = question.Id, Text = "Medio"},
                new SubQuestion{QuestionId = question.Id, Text = "Basso"},
                new SubQuestion{QuestionId = question.Id, Text = "Nullo"},
            };

            question.Children = subQuestions;

            context.Questions.Add(question);

            //--
            var question2 = new Question { SurveyId = survey.Id, Text = "Seconda domanda" };
            context.Questions.Add(question);

            var subQuestions2 = new List<SubQuestion>{
                new SubQuestion{QuestionId = question2.Id, Text = "Grande"},
                new SubQuestion{QuestionId = question2.Id, Text = "Cosìcosì"},
                new SubQuestion{QuestionId = question2.Id, Text = "Piccolo"},
                new SubQuestion{QuestionId = question2.Id, Text = "Non ci sta"},
            };

            question2.Children = subQuestions2;

            context.Questions.Add(question2);
        }
    }
}
