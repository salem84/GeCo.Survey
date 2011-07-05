using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeCoSurvey.Domain;
using GeCoSurvey.Data.Infrastructure;
using GeCoSurvey.Data;

namespace GeCoSurvey.Service
{
    public interface ISurveyService
    {
        IEnumerable<Survey> GetSurveys(bool active);
        Survey GetSurvey(int id);
    }

    public class SurveyService : ISurveyService
    {
        private readonly ISurveyRepository reposSurvey;

        public SurveyService(ISurveyRepository reposSurvey)
        {
            this.reposSurvey = reposSurvey;
        }

        
        public IEnumerable<Survey> GetSurveys(bool active)
        {
            var surveys = reposSurvey.GetMany(s => s.Active == active);
            return surveys;
        }

        public Survey GetSurvey(int id)
        {
            var survey = reposSurvey.GetById(id);
            return survey;
        }
    }
}
