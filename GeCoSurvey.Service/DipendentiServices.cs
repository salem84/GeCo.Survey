using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using GeCoSurvey.Domain;
using GeCoSurvey.Data.Infrastructure;
using GeCoSurvey.Data;

namespace GeCoSurvey.Service
{
    public interface IDipendentiService
    {
        Dipendente SalvaDipendente(Dipendente d);
        List<LivelloConoscenza> GetLivelliConoscenza();
    }


    public class DipendentiService : IDipendentiService
    {
        private readonly IRepository<Dipendente> reposDipendenti;
        private readonly IRepository<LivelloConoscenza> reposLivelloConoscenza;
        private readonly IRepository<ConoscenzaCompetenza> reposConoscenze;
        private readonly IUnitOfWork unitOfWork;

        public DipendentiService(IRepository<Dipendente> reposDipendenti,
            IRepository<LivelloConoscenza> reposLivelloConoscenza,
            IRepository<ConoscenzaCompetenza> reposConoscenze,
            IUnitOfWork unitOfWork)
        {
            this.reposDipendenti = reposDipendenti;
            this.reposLivelloConoscenza = reposLivelloConoscenza;
            this.reposConoscenze = reposConoscenze;
            this.unitOfWork = unitOfWork;

        }


        /// <summary>
        /// Devo rielaborare l'oggetto per evitare creazione di duplicati di conoscenze su DB
        /// </summary>
        /// <param name="dipendente"></param>
        public Dipendente SalvaDipendente(Dipendente d)
        {
            if (d.Id == 0)
                return CreaDipendente(d);
            else
                return AggiornaDipendente(d);
        }


        private Dipendente AggiornaDipendente(Dipendente d)
        {
            int idLivelloInsuff = reposLivelloConoscenza.Get(lc => lc.Titolo == Tipologiche.Livello.INSUFFICIENTE).Id;          

            Dipendente dipendente = reposDipendenti.Get(dip => dip.Id == d.Id);

            dipendente.Matricola = d.Matricola;
            dipendente.Cognome = d.Cognome;
            dipendente.Nome = d.Nome;
            dipendente.DataNascita = d.DataNascita;

            for (int i = 0; i < d.Conoscenze.Count; i++ )
            {
                ConoscenzaCompetenza c = d.Conoscenze.ToList()[i];

                //e salvo solo quelle diverse da 0
                if (c.LivelloConoscenzaId != idLivelloInsuff)
                {
                    ConoscenzaCompetenza conoscenza;
                    conoscenza = dipendente.Conoscenze.SingleOrDefault(con => con.CompetenzaId == c.CompetenzaId);
                    if (conoscenza == null)
                    {
                        conoscenza = new ConoscenzaCompetenza();
                        conoscenza.CompetenzaId = c.CompetenzaId;
                        dipendente.Conoscenze.Add(conoscenza);
                    }

                    conoscenza.LivelloConoscenzaId = c.LivelloConoscenzaId;
                }
                else
                {
                    //E' una di quelle che erano presenti in precedenza e sono state settate a 0 per essere cancellate
                    var conosc = dipendente.Conoscenze.SingleOrDefault(con => con.CompetenzaId == c.CompetenzaId);
                    if (conosc != null)
                    {
                        dipendente.Conoscenze.Remove(conosc);
                        var cc = reposConoscenze.Get(con => con.Id == conosc.Id);
                        reposConoscenze.Delete(cc);
                    }
                }
            }

            unitOfWork.Commit();

            return dipendente;
        }

        private Dipendente CreaDipendente(Dipendente d)
        {
            int idLivelloInsuff = reposLivelloConoscenza.Get(lc => lc.Titolo == Tipologiche.Livello.INSUFFICIENTE).Id;

            //Controllo se l'id è uguale a 0
            //Ricreo l'oggetto
                        
            Dipendente dipendente = new Dipendente();
            dipendente.Matricola = d.Matricola;
            dipendente.Cognome = d.Cognome;
            dipendente.Nome = d.Nome;
            dipendente.DataNascita = d.DataNascita;
            
            dipendente.Conoscenze = new List<ConoscenzaCompetenza>();

            //Mi scorro tutte le conoscenze
            foreach (var c in d.Conoscenze)
            {
                //e salvo solo quelle diverse da 0
                //if (c.LivelloConoscenza.Titolo != Tipologiche.Livello.INSUFFICIENTE)
                if (c.LivelloConoscenzaId != idLivelloInsuff)
                {
                    ConoscenzaCompetenza conoscenza = new ConoscenzaCompetenza();

                    conoscenza.LivelloConoscenzaId = c.LivelloConoscenzaId;
                    conoscenza.CompetenzaId = c.CompetenzaId;
                    dipendente.Conoscenze.Add(conoscenza);
                }
            }

            
            reposDipendenti.Add(dipendente);
            unitOfWork.Commit();

            return dipendente;
        }

        public List<LivelloConoscenza> GetLivelliConoscenza()
        {
            var result = reposLivelloConoscenza.GetAll();
            return result.ToList();
        }


        /*public void EliminaDipendente(int id)
        {
            //repos.Attach(dipToRemove);
            Dipendente dipToRemove = repos.Single(d => d.Id == id);
            repos.Delete(dipToRemove);

            uow.Commit();
        }

        public IQueryable GetDipendenti()
        {
            IRepository<Dipendente> repository = new BaseRepository<Dipendente>(null);
            return repository.AsQueryable();
        }

        public IList<Dipendente> GetDipendenti(Expression<Func<Dipendente, bool>> where)
        {
            var repos = ServiceLocator.Current.GetInstance<IRepository<Dipendente>>();
            var lista = repos.AsQueryable().AsExpandable().Where(where).ToList();
            //var lista = repos.GetAll().Where(where.Compile());
            
            return lista.ToList();
        }


        public Dipendente CaricaDipendente(int id)
        {
            //TODO attenzione ritorna proxy
            var repos = ServiceLocator.Current.GetInstance<IRepository<Dipendente>>();
            /*var result = repos.Include(a => a.Conoscenze.Select(c => c.Competenza))
                    .Include(a => a.Conoscenze.Select(c => c.LivelloConoscenza))
                    .Include(a => a.Conoscenze.Select(c => c.Competenza.TipologiaCompetenza))
                    .SingleOrDefault(a => a.Id == id);*/
       /*     var result = repos.SingleOrDefault(a => a.Id == id);

            return result;
        }*/

        
    }
}
