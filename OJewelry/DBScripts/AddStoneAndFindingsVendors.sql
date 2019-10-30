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

select * from Vendors
select c.Id, c.Name, v.* from Companies c
left  join Vendors v on v.CompanyId = c.id

