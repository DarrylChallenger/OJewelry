select * from LaborTable
select * from styles where stylename like '%qwerty%'

select s.id, s.stylenum, s.JewelryTypeId, sl.*  from styles s
join stylelaborTable sl on sl.StyleId = s.id
where s.id = 49095


select s.id, s.stylenum, s.JewelryTypeId, sl.*, l.id, l.Name from styles s
join stylelabor sl on sl.StyleId = s.id
join labor l on l.Id = sl.LaborId
where s.id = 49097

select s.id, s.stylenum, s.JewelryTypeId, sl.*, l.id, l.Name  from styles s
join stylelaborTable sl on sl.StyleId = s.id
join LaborTable l on l.Id = sl.LaborTableId
where s.id = 49097

update StyleLaborTable set LaborTableId = 5013 where styleid = 49095