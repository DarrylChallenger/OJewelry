select co.name, c.name, * from styles sty 
join collections c on c.id = sty.collectionId
join Companies co on co.id = c.CompanyId
where sty.id not in
(select s.id from Styles s
left join StyleLabor sl on sl.StyleId = s.id
left outer join Labor l on l.id = sl.LaborId
where l.Name = 'FINISHING LABOR')
order by co.name, c.name


select * from Styles s
left join StyleLabor sl on sl.StyleId = s.id
left outer join Labor l on l.id = sl.LaborId
where l.Name = 'FINISHING LABOR'

select * from Styles s
left join StyleLabor sl on sl.StyleId = s.id
--left outer join Labor l on l.id = sl.LaborId
where s.id=32079

select * from StyleLabor where StyleId = 32079
