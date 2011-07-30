﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeCoSurvey.Domain;

namespace GeCoSurvey.Data
{
    public class DipendentiDefault
    {
        private SurveyContext context;

        public DipendentiDefault(SurveyContext context)
        {
            this.context = context;
        }

        class D
        {
            public string t { get; set; }
            public string v { get; set; }
            public string mg { get; set; }
        }

        private Dipendente SalvaDipendente(D[] lista, string matricola, string cognome, string nome, string ruolo)
        {
            /*var reposDipendenti = ServiceLocator.Current.GetInstance<IRepository<Dipendente>>();

            var reposComp = ServiceLocator.Current.GetInstance<IRepository<Competenza>>();
            var reposLivelli = ServiceLocator.Current.GetInstance<IRepository<LivelloConoscenza>>();
            var uow = ServiceLocator.Current.GetInstance<IUnitOfWork>();*/

            /*var reposDipendenti = new BaseRepository<Dipendente>(context);
            var reposRuoli = new BaseRepository<Ruolo>(context);
            var reposComp = new BaseRepository<Competenza>(context);
            var reposLivelli = new BaseRepository<LivelloConoscenza>(context);
            var uow = new UnitOfWork(context);*/

            List<ConoscenzaCompetenza> conoscenze = new List<ConoscenzaCompetenza>();

            foreach (var elemento in lista)
            {
                ConoscenzaCompetenza conoscenza = new ConoscenzaCompetenza();
                conoscenza.Competenza = context.Competenze.Single(c => c.Titolo == elemento.t && c.TipologiaCompetenza.MacroGruppo == elemento.mg);
                conoscenza.LivelloConoscenza = context.LivelliConoscenza.Single(lc => lc.Titolo == elemento.v);
                conoscenze.Add(conoscenza);
            }

            var dipendente = new Dipendente()
            {
                Matricola = matricola,
                Cognome = cognome,
                Nome = nome,
                Conoscenze = conoscenze
            };

            dipendente.RuoloInAziendaId = context.Ruoli.Single(r => r.Titolo == ruolo).Id;


            if (context.Dipendenti.SingleOrDefault(d => d.Cognome == dipendente.Cognome) == null)
            {
                context.Dipendenti.Add(dipendente);
                //uow.Commit();
                context.SaveChanges();
            }

            return dipendente;

        }


        //Punteggi osservati dal foglio Responsabile Ufficio Tecnico
        public Dipendente SalvaDipendente1()
        {
            var lista = new[] 
            {
                #region COMPETENZE TECNICHE

                new D() { t="Normative Tecniche", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Normative Qualità", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Normative di Settore", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Normative Ambientali", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Normative di Sicurezza", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Caratteristiche dei Materiali", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Macchinari idonei all'esecuzione", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Contabilità Lavori", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_TECNICO },

                //NON c'è sul foglio
                new D() { t="Processo realizzazione lavori speciali", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                              
                new D() { t="Planning breve-medio periodo", v=Tipologiche.Livello.SUFFICIENTE, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Planning medio-lungo periodo", v=Tipologiche.Livello.SUFFICIENTE, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Planning operativo movimentazione risorse", v=Tipologiche.Livello.SUFFICIENTE, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Monitoraggio e rilievo dell'opera eseguita", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Elaborazione preventivi ed offerte", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Incidenza dei costi", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                
                new D() { t="Analisi scostamenti", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Tecniche di misurazione", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Tecnica di confezionamento dei c.b. e cementizi", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Tecniche di esecuzione in presenza di traffico ", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },

                //Non c'è sul foglio
                new D() { t="Planning Operativo Movimentazione Macchine", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_TECNICO },

                #endregion

                #region COMPETENZE COMPORTAMENTALI

                new D() { t="Integrazione", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="TeamWork", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Gestione delle Risorse Umane", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Leadership", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },

                new D() { t="Comunicazione", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Assertività", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Negoziazione", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Networking", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },

                new D() { t="Capacità di Analisi", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Problem solving", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Visione d'insieme", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Orientamento al cliente", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },

                new D() { t="Orientamento al risultato", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Responsabilità", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Efficienza", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Proattività", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                
                #endregion

                #region HR DISCREZIONALI

                new D() { t="Assessment", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_DISCREZIONALE},
                new D() { t="Considerazioni Gestionali", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_DISCREZIONALE},

                #endregion

                #region COMPETENZE COMPORTAMENTALI

                new D() { t="Integrazione", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="TeamWork", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Gestione delle Risorse Umane", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Leadership", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },

                new D() { t="Comunicazione", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Assertività", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Negoziazione", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Networking", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },

                new D() { t="Capacità di Analisi", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Problem solving", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Visione d'insieme", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Orientamento al cliente", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },

                new D() { t="Orientamento al risultato", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Responsabilità", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Efficienza", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Proattività", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                
                #endregion
            };

            return SalvaDipendente(lista, "101", "Conti", "Giorgio", "Capo Cantiere Manutenzione");
            
        }

        public Dipendente SalvaDipendente2()
        {
            var lista = new[] 
            {
                #region COMPETENZE TECNICHE

                new D() { t="Normative Tecniche", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Normative Qualità", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Normative di Settore", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Normative Ambientali", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Normative di Sicurezza", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Caratteristiche dei Materiali", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Macchinari idonei all'esecuzione", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Contabilità Lavori", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_TECNICO },

                //NON c'è sul foglio
                new D() { t="Processo realizzazione lavori speciali", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                              
                
                new D() { t="Analisi scostamenti", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Tecniche di misurazione", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Tecnica di confezionamento dei c.b. e cementizi", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Tecniche di esecuzione in presenza di traffico ", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },

                //Non c'è sul foglio
                new D() { t="Planning Operativo Movimentazione Macchine", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_TECNICO },

                #endregion

                #region COMPETENZE COMPORTAMENTALI

                new D() { t="Integrazione", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="TeamWork", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Gestione delle Risorse Umane", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Leadership", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },

                new D() { t="Comunicazione", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Assertività", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Negoziazione", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Networking", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },

                new D() { t="Capacità di Analisi", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Problem solving", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Visione d'insieme", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Orientamento al cliente", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },

                new D() { t="Orientamento al risultato", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Responsabilità", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Efficienza", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Proattività", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                
                #endregion

                #region HR DISCREZIONALI

                new D() { t="Assessment", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_DISCREZIONALE},
                new D() { t="Considerazioni Gestionali", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_DISCREZIONALE},

                #endregion

                #region COMPETENZE COMPORTAMENTALI

                new D() { t="Integrazione", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="TeamWork", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Gestione delle Risorse Umane", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Leadership", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },

                new D() { t="Comunicazione", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Assertività", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Negoziazione", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Networking", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },

                new D() { t="Capacità di Analisi", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Problem solving", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Visione d'insieme", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Orientamento al cliente", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },

                new D() { t="Orientamento al risultato", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Responsabilità", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Efficienza", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Proattività", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                
                #endregion
            };

            return SalvaDipendente(lista, "102", "Celli", "Marco", "Direttore Cantiere Manutenzione");

        }


        public Dipendente SalvaDipendente3()
        {
            var lista = new[] 
            {
                #region COMPETENZE TECNICHE

                new D() { t="Normative Tecniche", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Normative Qualità", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Normative di Settore", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Normative Ambientali", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Normative di Sicurezza", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Caratteristiche dei Materiali", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Macchinari idonei all'esecuzione", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Contabilità Lavori", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_TECNICO },

                //NON c'è sul foglio
                new D() { t="Processo realizzazione lavori speciali", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                              
                
                new D() { t="Analisi scostamenti", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                
                new D() { t="Tecniche di esecuzione in presenza di traffico ", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },

                //Non c'è sul foglio
                new D() { t="Planning Operativo Movimentazione Macchine", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_TECNICO },

                #endregion

                #region COMPETENZE COMPORTAMENTALI

                new D() { t="Integrazione", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="TeamWork", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Gestione delle Risorse Umane", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Leadership", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },

                new D() { t="Comunicazione", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Assertività", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Negoziazione", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Networking", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },

                new D() { t="Capacità di Analisi", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Problem solving", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Visione d'insieme", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Orientamento al cliente", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },

                new D() { t="Orientamento al risultato", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Responsabilità", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Efficienza", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Proattività", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                
                #endregion

                #region HR DISCREZIONALI

                new D() { t="Assessment", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_DISCREZIONALE},
                new D() { t="Considerazioni Gestionali", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_DISCREZIONALE},

                #endregion

                #region COMPETENZE COMPORTAMENTALI

                new D() { t="Integrazione", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="TeamWork", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Gestione delle Risorse Umane", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Leadership", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },

                new D() { t="Comunicazione", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Assertività", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Negoziazione", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Networking", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },

                new D() { t="Capacità di Analisi", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Problem solving", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Visione d'insieme", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Orientamento al cliente", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },

                new D() { t="Orientamento al risultato", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Responsabilità", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Efficienza", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Proattività", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                
                #endregion
            };

            return SalvaDipendente(lista, "103", "Cacci", "Franco", "Capo Cantiere Infrastrutture");

        }

        public Dipendente SalvaDipendente4()
        {
            var lista = new[] 
            {
                #region COMPETENZE TECNICHE

                new D() { t="Normative Tecniche", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Normative Qualità", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Normative di Settore", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Normative Ambientali", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Normative di Sicurezza", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Caratteristiche dei Materiali", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Macchinari idonei all'esecuzione", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                new D() { t="Contabilità Lavori", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_TECNICO },

                //NON c'è sul foglio
                new D() { t="Processo realizzazione lavori speciali", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                              
                
                new D() { t="Analisi scostamenti", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },
                
                new D() { t="Tecniche di esecuzione in presenza di traffico ", v=Tipologiche.Livello.DISCRETO, mg=Tipologiche.Macrogruppi.MG_TECNICO },

                //Non c'è sul foglio
                new D() { t="Planning Operativo Movimentazione Macchine", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_TECNICO },

                #endregion

                #region COMPETENZE COMPORTAMENTALI

                new D() { t="Integrazione", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="TeamWork", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Gestione delle Risorse Umane", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Leadership", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },

                new D() { t="Comunicazione", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Assertività", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Negoziazione", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Networking", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },

                new D() { t="Capacità di Analisi", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Problem solving", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Visione d'insieme", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Orientamento al cliente", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },

                new D() { t="Orientamento al risultato", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Responsabilità", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Efficienza", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                new D() { t="Proattività", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_COMPORTAMENTALE },
                
                #endregion

                #region HR DISCREZIONALI

                new D() { t="Assessment", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_DISCREZIONALE},
                new D() { t="Considerazioni Gestionali", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_DISCREZIONALE},

                #endregion

                #region COMPETENZE COMPORTAMENTALI

                new D() { t="Integrazione", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="TeamWork", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Gestione delle Risorse Umane", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Leadership", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },

                new D() { t="Comunicazione", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Assertività", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Negoziazione", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Networking", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },

                new D() { t="Capacità di Analisi", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Problem solving", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Visione d'insieme", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Orientamento al cliente", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },

                new D() { t="Orientamento al risultato", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Responsabilità", v=Tipologiche.Livello.OTTIMO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Efficienza", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                new D() { t="Proattività", v=Tipologiche.Livello.BUONO, mg=Tipologiche.Macrogruppi.MG_HR_COMPORTAMENTALE },
                
                #endregion
            };

            return SalvaDipendente(lista, "104", "Pomi", "Luca", "Direttore Cantiere Infrastrutture");

        }
    }
}
