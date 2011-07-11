using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace GeCoSurvey.Domain
{
    public class SubQuestion
    {
        public int Id { get; set; }

        //[ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }
        public int? QuestionId { get; set; }

        public string Testo { get; set; }
        
        public virtual LivelloConoscenza LivelloConoscenza { get; set; }
        public int LivelloConoscenzaId { get; set; }
    }
}
