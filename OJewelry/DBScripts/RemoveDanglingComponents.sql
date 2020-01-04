begin tran
delete from Castings where id in (
select c.id from Castings c
left outer join StyleCastings sc on sc.CastingId = c.id
where CastingId is null)
commit tran

begin tran
delete from Stones where id in (
select x.id from  Stones x
left outer join StyleStone s on s.stoneid = x.id
where stoneId is null)
commit tran

begin tran
delete from Findings where id in (
select x.id from  Findings x
left outer join StyleFinding s on s.FindingId = x.id
where FindingId is null)
commit tran

begin tran
delete from Labor where id in (
select x.id from  Labor x
left outer join StyleLabor s on s.LaborId = x.id
where LaborId is null)
commit tran

begin tran
delete from LaborTable where id in (
select x.id from  LaborTable x
left outer join StyleLaborTable s on s.LaborTableId = x.id
where LaborTableId is null)
commit tran

begin tran
delete from Misc where id in (
select x.id from  Misc x
left outer join StyleMisc s on s.MiscId = x.id
where MiscId is null)
commit tran

