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

        public string Text { get; set; }
    }
}
