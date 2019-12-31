/* Find Comapnies w/o castings | labors vendors */
select 'CV_'+c.Name as Name, c.Id from Companies c
left  join Vendors v on v.CompanyId = c.id
where v.id is null 
UNION
select 'LV_'+c.Name as Name, c.Id from Companies c
left  join Vendors v on v.CompanyId = c.id
where v.id is null

/* add dummy vendors into table */
begin transaction
insert into Vendors (Name, CompanyId, Type_Type)
SELECT 'CV_'+name ,id, 8 
FROM Companies as c
WHERE NOT EXISTS (SELECT * from Vendors as v where v.CompanyId = c.id and v.Type_Type = 8)  
UNION
SELECT 'LV_'+name, id, 16 
FROM Companies as c
WHERE NOT EXISTS (SELECT * from Vendors as v where v.CompanyId = c.id and v.Type_Type = 16)  
commit transaction
rollback transaction
/* Check */
select * from Vendors
select c.Id, c.Name, v.* from Companies c
left  join Vendors v on v.CompanyId = c.id

select * from Vendors where Type_Type <> 0

/* Check */
select * from castings c
join vendors v on v.CompanyId = c.CompanyId 
where vendorId is null and v.Type_Type = 8
select * from labors l
join vendors v on v.CompanyId = l.CompanyId 
where vendorId is null and v.Type_Type = 16

/* Assign stone vendor to castings */
begin tran
update castings set vendorId = vid from
(
select v.id as vid, c.id as cid from castings c
join vendors v on v.CompanyId = c.CompanyId 
where vendorId is null and v.Type_Type = 16
) t where id = cid
select * from castings 
commit tran

/* Assign findings vendor to labors */
begin tran
update labors set vendorId = vid from
(
select v.id as vid, l.id as lid from labors l
join vendors v on v.CompanyId = l.CompanyId 
where vendorId is null and v.Type_Type = 16
) t where id = lid
select * from labors 
commit tran


