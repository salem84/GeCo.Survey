using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace GeCoSurvey.Domain
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        public virtual Survey Survey { get; set; }
        public int SurveyId { get; set; }

        /*[ForeignKey("Id")]
        public virtual Question Parent { get; set; }
        public int? ParentId { get; set; }*/

        public string Text { get; set; }

        public virtual ICollection<SubQuestion> Children { get; set; }
    }
}
