select sty.Id, StyleName, coll.Name as CollName, p.Name as LocName, sty.Quantity as SQuant, m.Quantity as LQuant from styles as sty
join memo as m on m.StyleID = sty.Id
join Presenters as p on p.Id = m.PresenterID
join Collections as coll on sty.CollectionId = coll.Id
where sty.StyleName = 'lct1'

