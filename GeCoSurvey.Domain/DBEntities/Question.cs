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

        public string Testo { get; set; }

        public virtual ICollection<SubQuestion> Children { get; set; }
        
        public virtual Competenza Competenza { get; set; }
        public int CompetenzaId { get; set; }
    }
}
