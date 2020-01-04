select * from Castings c
left outer join StyleCastings sc on sc.CastingId = c.id
where CastingId is null

select * from  Stones x
left outer join StyleStone s on s.stoneid = x.id
where stoneId is null

select * from  Findings x
left outer join StyleFinding s on s.FindingId = x.id
where FindingId is null

select * from  Labor x
left outer join StyleLabor s on s.LaborId = x.id
where LaborId is null

select * from  LaborTable x
left outer join StyleLaborTable s on s.LaborTableId = x.id
where LaborTableId is null

select * from  Misc x
left outer join StyleMisc s on s.MiscId = x.id
where MiscId is null

