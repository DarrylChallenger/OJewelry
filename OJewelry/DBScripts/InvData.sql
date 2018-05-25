select comp.Name, sty.Id, StyleName, coll.Name as CollName, p.Name as LocName, sty.Quantity as SQuant, m.Quantity as LQuant from styles as sty
join memo as m on m.StyleID = sty.Id
join Presenters as p on p.Id = m.PresenterID
join Collections as coll on sty.CollectionId = coll.Id
join Companies as comp on coll.CompanyId = comp.Id
where sty.StyleName = 'lct1'

select comp.Id, comp.Name from Companies as comp
select coll.Id, coll.Name from Collections as coll

select comp.Name, coll.Name from Companies as comp
join Collections as coll on comp.Id = coll.CompanyId
where comp.Id = 7
