SELECT r.id IdRichiesta, 
        rd.ID IdDocumento,
		R.IdRichiesta GuidRichiesta,
		r.codicestatocorrente StatoRichiesta
      ,[CodiceStatoCorrenteDocumento] StatoDocumento
      ,[DataStatoCorrenteDocumento]
      ,[URLDocumento]
      ,[FormatoDocumento],
	   tr.Codice TipoRecapito,
	   di.*
  FROM [NPCEVisure].[dbo].[RichiesteDocumenti] rd
  inner join Richieste r on (r.Id=rd.IDRichiesta)
  inner join TipiRecapiti tr on (tr.ID= r.IDTipoRecapito)
  left outer join DocumentiInfo di on (di.RichiesteDocumentiId=rd.ID)
  where r.IdRichiesta = '3889fc5d-dd1b-4dd5-9783-2df781c19cd3'