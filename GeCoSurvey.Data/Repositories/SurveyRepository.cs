using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeCoSurvey.Domain;
using GeCoSurvey.Data.Infrastructure;
using System.Data;

namespace GeCoSurvey.Data
{
    public class SurveyRepository : RepositoryBase<Survey>//, ISurveyRepository
    {
        public SurveyRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        

    }

    /*public interface ISurveyRepository : IRepository<Survey>
    {
               
    }*/
}
