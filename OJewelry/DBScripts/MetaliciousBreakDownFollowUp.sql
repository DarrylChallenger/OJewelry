DECLARE @Company int
SET @Company = 4

--select * from Companies

select * from styles s
join Collections c on c.id = s.CollectionId
where c.companyid = @Company

select s.id, s.StyleName, s.[desc], s.MetalWtNote, cas.Name, cas.Qty, v.Name as VendorName from styles s
join Collections c on c.id = s.CollectionId
join StyleCastings sc on s.id = sc.StyleId
join Castings cas on sc.CastingId = cas.Id
LEFT OUTER join Vendors v on v.Id = cas.VendorId
where c.companyid = @Company

select s.id, s.StyleName, s.[desc], sto.Name, ss.Qty, v.Name as VendorName from styles s
join Collections c on c.id = s.CollectionId
join StyleStone ss on s.id = ss.StyleId
join Stones sto on ss.stoneId = sto.Id
LEFT OUTER join Vendors v on v.Id = sto.VendorId
where c.companyid = @Company

select s.id, s.StyleName, s.[desc], fin.Name, sf.Qty, v.Name as VendorName from styles s
join Collections c on c.id = s.CollectionId
join StyleFinding sf on s.id = sf.StyleId
join Findings fin on sf.FindingId = fin.Id
LEFT OUTER join Vendors v on v.Id = fin.VendorId
where c.companyid = @Company

select s.id, s.StyleName, s.[desc], lab.Name, lab.Qty, v.Name as VendorName from styles s
join Collections c on c.id = s.CollectionId
join StyleLabor sl on s.id = sl.StyleId
join Labor lab on sl.LaborId = lab.Id
LEFT OUTER join Vendors v on v.Id = lab.VendorId
where c.companyid = @Company

select s.id, s.StyleName, s.[desc], lab.Name, slt.Qty, v.Name as VendorName from styles s
join Collections c on c.id = s.CollectionId
join StyleLaborTable slt on s.id = slt.StyleId
join LaborTable lab on slt.LaborTableId = lab.id
LEFT OUTER join Vendors v on v.Id = lab.VendorId
where c.companyid = @Company

