using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using GeCoSurvey.Domain;

namespace GeCoSurvey.Data
{
    public class SurveyContextInitializer : //DropCreateDatabaseAlways<SurveyContext>
        DropCreateDatabaseIfModelChanges<SurveyContext>
    {
        protected override void Seed(SurveyContext context)
        {
            (new InitializeDB(context)).InitalizeAll();
            
            
            var roles = new List<Role>{
                new Role{RoleName = "Administrator"},
                new Role{RoleName = "User"}               
            };

            roles.ForEach(r => context.Roles.Add(r));

            var survey = new Survey { Name = "Test", Active = true };
            context.Surveys.Add(survey);

            //---
            int compId = context.Competenze.Single(c => c.Titolo == "Normative Tecniche").Id;
            var question = new Question
            {
                SurveyId = survey.Id,
                Testo = "Prima domanda",
                CompetenzaId = compId
            };
            context.Questions.Add(question);

            var subQuestions = new List<SubQuestion>{
                new SubQuestion{QuestionId = question.Id, Testo = "Alto", LivelloConoscenzaId = 5},
                new SubQuestion{QuestionId = question.Id, Testo = "Medio", LivelloConoscenzaId = 4},
                new SubQuestion{QuestionId = question.Id, Testo = "Basso", LivelloConoscenzaId = 3},
                new SubQuestion{QuestionId = question.Id, Testo = "Bassissimo", LivelloConoscenzaId = 2},
                new SubQuestion{QuestionId = question.Id, Testo = "nn ce sta", LivelloConoscenzaId = 1},
            };

            question.Children = subQuestions;

            context.Questions.Add(question);

            //--
            compId = context.Competenze.Single(c => c.Titolo == "Normative Qualità").Id;
            var question2 = new Question
            {
                SurveyId = survey.Id,
                Testo = "Seconda domanda",
                CompetenzaId = compId
            };
            context.Questions.Add(question);

            var subQuestions2 = new List<SubQuestion>{
                new SubQuestion{QuestionId = question2.Id, Testo = "Grande", LivelloConoscenzaId = 5},
                new SubQuestion{QuestionId = question2.Id, Testo = "Cosìcosì", LivelloConoscenzaId = 4},
                new SubQuestion{QuestionId = question2.Id, Testo = "Piccolo", LivelloConoscenzaId = 3},
                new SubQuestion{QuestionId = question2.Id, Testo = "boh", LivelloConoscenzaId = 2},
                new SubQuestion{QuestionId = question2.Id, Testo = "Non ci sta manco qua", LivelloConoscenzaId = 1},
            };

            question2.Children = subQuestions2;

            context.Questions.Add(question2);



            context.ResponsabiliDipendenti.Add(new ResponsabiliDipendenti { Dipendente = "d1", Responsabile = "r1" });
            context.ResponsabiliDipendenti.Add(new ResponsabiliDipendenti { Dipendente = "d2", Responsabile = "r1" });
            context.ResponsabiliDipendenti.Add(new ResponsabiliDipendenti { Dipendente = "d3", Responsabile = "r1" });

            context.SaveChanges();
        }
    }
}
