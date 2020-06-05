SELECT r.id IdRichiesta, 
        rd.ID IdDocumento,
		R.IdRichiesta GuidRichiesta
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
  where r.IdRichiesta = 'd1ab0000-2d01-c0b1-0000-0000000005b1'