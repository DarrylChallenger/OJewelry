/* Find Comapnies w/o stone | finding vendors */
select 'FV_'+c.Name as Name, c.Id from Companies c
left  join Vendors v on v.CompanyId = c.id
where v.id is null 
UNION
select 'SV_'+c.Name as Name, c.Id from Companies c
left  join Vendors v on v.CompanyId = c.id
where v.id is null

/* add dummy vendors into table */
begin transaction
insert into Vendors (Name, CompanyId, Type_Type)
SELECT 'FV_'+name ,id, 5 
FROM Companies as c
WHERE NOT EXISTS (SELECT * from Vendors as v where v.CompanyId = c.id and v.Type_Type = 5)  
UNION
SELECT 'SV_'+name, id, 3 
FROM Companies as c
WHERE NOT EXISTS (SELECT * from Vendors as v where v.CompanyId = c.id and v.Type_Type = 3)  
commit transaction

/* Check */
select * from Vendors
select c.Id, c.Name, v.* from Companies c
left  join Vendors v on v.CompanyId = c.id

select * from Vendors where Type_Type <> 0

/* Check */
select * from stones s
join vendors v on v.CompanyId = s.CompanyId 
where vendorId is null and v.Type_Type = 3
select * from findings f
join vendors v on v.CompanyId = f.CompanyId 
where vendorId is null and v.Type_Type = 5

/* Assign stone vendor to stones */
begin tran
update stones set vendorId = vid from
(
select v.id as vid, s.id as sid from stones s
join vendors v on v.CompanyId = s.CompanyId 
where vendorId is null and v.Type_Type = 3
) t where id = sid
select * from stones 
commit tran

/* Assign findings vendor to findings */
begin tran
update findings set vendorId = vid from
(
select v.id as vid, f.id as fid from findings f
join vendors v on v.CompanyId = f.CompanyId 
where vendorId is null and v.Type_Type = 5
) t where id = fid
select * from findings 
commit tran
