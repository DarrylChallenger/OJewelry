

select * from Castings c
left outer join StyleCastings sc on c.id = sc.CastingId
where sc.id is null

select * from Stones s
left outer join StyleStone ss on s.id = ss.StoneId
where ss.id is null

select * from Findings f
left outer join StyleFinding sf on f.id = sf.FindingId
where sf.id is null

select * from labor l
left outer join stylelabor sl on l.id = sl.laborId
where sl.id is null

select * from Misc m
left outer join StyleMisc sm on m.id = sm.MiscId
where sm.id is null



delete from castings where id in (select c.id from Castings c
left outer join StyleCastings sc on c.id = sc.CastingId
where sc.id is null)

delete from stones where id in (select s.id from Stones s
left outer join StyleStone ss on s.id = ss.StoneId
where ss.id is null)

delete from findings where id in (select f.id from Findings f
left outer join StyleFinding sf on f.id = sf.FindingId
where sf.id is null)

delete from labor where id in (select l.id from labor l
left outer join stylelabor sl on l.id = sl.laborId
where sl.id is null) 

delete from misc where id in (select m.id from Misc m
left outer join StyleMisc sm on m.id = sm.MiscId
where sm.id is null)
