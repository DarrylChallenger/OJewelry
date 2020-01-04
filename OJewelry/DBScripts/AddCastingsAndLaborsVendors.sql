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
select * from vendors where Name like 'cv%'
select * from vendors where Name like 'lv%'
