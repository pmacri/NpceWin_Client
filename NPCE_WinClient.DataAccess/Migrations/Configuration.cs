﻿namespace NPCE_WinClient.DataAccess.Migrations
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
            context.VisureFormatoDocumento.AddOrUpdate(fd => fd.Descrizione,
                new Model.VisureFormatoDocumento { Id = "xml", Descrizione = "XML" },
                new Model.VisureFormatoDocumento { Id = "pdf", Descrizione = "PDF" }
                );

            context.VisureTipoDocumento.AddOrUpdate(fd => fd.Descrizione,
                new Model.VisureTipoDocumento { Id = "C", Descrizione = "Certificato Camerale" },
                new Model.VisureTipoDocumento { Id = "V", Descrizione = "Visura" }
                );

            context.VisureTipoRecapito.AddOrUpdate(fd => fd.Descrizione,
               new Model.VisureTipoRecapito { Id = "S", Descrizione = "POSTA PRIORITARIA" },
               new Model.VisureTipoRecapito { Id = "E", Descrizione = "E-MAIL" },
               new Model.VisureTipoRecapito { Id = "R", Descrizione = "RACCOMANDATA" },
               new Model.VisureTipoRecapito { Id = "D", Descrizione = "DOWNLOAD" },
               new Model.VisureTipoRecapito { Id = "P", Descrizione = "PEC" }
               );

            context.VisureCodiceDocumento.AddOrUpdate(fd => fd.Descrizione,
               new Model.VisureCodiceDocumento { Id = "CART", Descrizione = "CART", VisureTipoDocumentoId="C" },
               new Model.VisureCodiceDocumento { Id = "CRIA", Descrizione = "CRIA", VisureTipoDocumentoId = "C" },
               new Model.VisureCodiceDocumento { Id = "CRIM", Descrizione = "CRIM", VisureTipoDocumentoId = "C" },
               new Model.VisureCodiceDocumento { Id = "CRIS", Descrizione = "CRIS", VisureTipoDocumentoId = "C" },

            new Model.VisureCodiceDocumento { Id = "ATTI", Descrizione = "ATTI", VisureTipoDocumentoId = "V" },
            new Model.VisureCodiceDocumento { Id = "BICM", Descrizione = "BICM", VisureTipoDocumentoId = "V" },
            new Model.VisureCodiceDocumento { Id = "FASC", Descrizione = "FASC", VisureTipoDocumentoId = "V" },
            new Model.VisureCodiceDocumento { Id = "RIPR", Descrizione = "RIPR", VisureTipoDocumentoId = "V" },
            new Model.VisureCodiceDocumento { Id = "SCPE", Descrizione = "SCPE", VisureTipoDocumentoId = "V" },
            new Model.VisureCodiceDocumento { Id = "SCSC", Descrizione = "SCSC", VisureTipoDocumentoId = "V" },
            new Model.VisureCodiceDocumento { Id = "SCSO", Descrizione = "SCSO", VisureTipoDocumentoId = "V" },
            new Model.VisureCodiceDocumento { Id = "TRSF", Descrizione = "TRSF", VisureTipoDocumentoId = "V" },
            new Model.VisureCodiceDocumento { Id = "VISO", Descrizione = "VISO", VisureTipoDocumentoId = "V" },
            new Model.VisureCodiceDocumento { Id = "VISS", Descrizione = "VISS", VisureTipoDocumentoId = "V" }
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
                },
                new Model.Anagrafica
                {
                    Nome = "DITTA ",
                    Cognome = "MARKET",
                    DUG = "Viale",
                    Citta = "ROMA",
                    Cap = "00144",
                    Toponimo = "EUROPA",
                    NumeroCivico = "180",
                    Provincia = "RM",
                    Stato = "ITALIA"
                }
                );

            context.StatoServizio.AddOrUpdate(ss => ss.Descrizione,
                new Model.StatoServizio { Descrizione = "Salvato" }
                );
        }
    }
}
