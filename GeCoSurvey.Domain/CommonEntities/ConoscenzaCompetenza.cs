using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace GeCoSurvey.Domain
{
    public class ConoscenzaCompetenza : BaseIdentityModel<ConoscenzaCompetenza>
    {
        public int CompetenzaId { get; set; }
        public virtual Competenza Competenza { get; set; }
        public int LivelloConoscenzaId { get; set; }
        public virtual LivelloConoscenza LivelloConoscenza { get; set; }

        public int ConoscitoreId { get; set; }
        public virtual Anagrafica Conoscitore { get; set; }
        //public int Indice 
        //{
        //    get { return Competenza.Peso * this.LivelloConoscenza.Valore; }
        //}
    }
}