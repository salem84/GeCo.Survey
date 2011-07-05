using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeCoSurvey.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        SurveyContext Get();
    }
}
