select s.id, s.StyleName, sl.*, l.* from styles s
join styleLabor sl on sl.StyleId = s.id
join Labor l on sl.LaborId = l.id
where s.StyleName like 'ba%'

select s.id, s.StyleName, sm.*, m.* from styles s
join StyleMisc sm on sm.StyleId = s.id
join Misc m on sm.MiscId = m.id
where s.StyleName like 'ba%'
/* bad =33079 */

select * from styleLabor sl
join Labor l on sl.LaborId = l.id
