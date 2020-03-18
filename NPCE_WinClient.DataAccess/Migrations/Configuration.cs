namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NPCE_WinClient.DataAccess.NpceDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(NPCE_WinClient.DataAccess.NpceDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            context.TipoServizio.AddOrUpdate(ts => ts.Descrizione,
                new Model.TipoServizio { Descrizione="Posta4" },
                new Model.TipoServizio { Descrizione = "Posta1" }
                );

            var allAnagrafiche = context.Anagrafica.ToList();

            if (allAnagrafiche != null)
            {
                context.Anagrafica.RemoveRange(allAnagrafiche);
            }
            context.SaveChanges();
            context.Anagrafica.AddOrUpdate(m => m.Cognome,
                new Model.Anagrafica
                {
                    Nome = "Paolo",
                    Cognome = "Rossi",
                    RagioneSociale = "Paolo Rossi SpA",
                    DUG = "Via",
                    Citta = "ROMA",
                    Cap = "00144",
                    Toponimo = "dei ciclamini",
                    NumeroCivico = "180",
                    Provincia = "RM",
                    Stato = "ITALIA"
                },

                new Model.Anagrafica
                {
                    Nome = "Matteo",
                    Cognome = "Bianchi",
                    DUG = "Via",
                    Citta = "AGRIGENTO",
                    Cap = "00144",
                    Toponimo = "DEI TULIPANI",
                    NumeroCivico = "180",
                    Provincia = "AG",
                    Stato = "ITALIA"
                },
                new Model.Anagrafica
                {
                    Nome = "Paolo",
                    Cognome = "Verdi",
                    DUG = "Via",
                    Citta = "TERNI",
                    Cap = "00144",
                    Toponimo = "delle Rose",
                    NumeroCivico = "18",
                    Provincia = "TR",
                    Stato = "ITALIA"
                }
                );
        }
    }
}
