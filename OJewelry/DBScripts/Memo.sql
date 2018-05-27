select com.Id, com.Name, p.name as LocName, s.StyleName, m.* from Memo m
join Presenters p on p.Id = m.PresenterID
join Companies com on com.id = p.CompanyId
join Styles s on s.Id = m.StyleID
where com.Id = 1 and styleID  >= 308 and StyleID <=312
order by date, StyleID
-- where styleID = 20122 and PresenterID = 9005


select com.Id, com.Name, p.name, s.id as styleID, s.StyleName, count(m.StyleID) from Memo m
join Presenters p on p.Id = m.PresenterID
join Companies com on com.id = p.CompanyId
join Styles s on s.Id = m.StyleID
-- where styleID = 20122 and PresenterID = 9005
--where com.id = 8
group by m.StyleID, p.Id, p.Name, com.Id, com.Name, s.id, s.StyleName
order by count(m.StyleID) desc