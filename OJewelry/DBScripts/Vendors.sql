/* add dummy vendor into */
insert into Vendors (Name, CompanyId)
select 'V_'+c.Name, c.Id from Companies c
left  join Vendors v on v.CompanyId = c.id
where v.id is null


select * from Vendors
select c.Name, c.Id, v.* from Companies c
left  join Vendors v on v.CompanyId = c.id
