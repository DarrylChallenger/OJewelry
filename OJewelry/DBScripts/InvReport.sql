select s.Id, s.Stylenum, s.stylename, s.Quantity from styles as s

select  distinct c.Id, s.Id, s.[Desc], s.StyleNum, m.Quantity from styles as s
left outer join memo as m on s.id = m.StyleID
join Collections as c on s.CollectionId = c.Id
join Companies as cp on cp.Id = c.CompanyId
where c.CompanyId in(1, 2)
order by s.StyleNum

select comp.Id, s.Id, p.Id, s.stylenum, p.Name, m.Quantity from Companies as Comp
join Collections as coll on comp.Id = coll.CompanyId
join Styles as s on s.CollectionId = coll.Id
join Memo as m on m.StyleID = s.Id
join Presenters as p on p.Id = m.PresenterID
where p.CompanyId = 2
order by s.StyleNum

select p.CompanyId, p.id, m.StyleID, m.Quantity, p.Name from memo m join Presenters p on p.Id = m.PresenterID

select  s.id, p.Id, p.Name, s.Stylenum, s.Quantity as 'sQty', m.Quantity from styles as s
join memo as m on s.id = m.StyleID
join Presenters as p on p.id = m.PresenterID
order by p.Name, stylenum

