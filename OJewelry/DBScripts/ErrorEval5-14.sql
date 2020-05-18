select * from companies
-- Multi Gems stones don't have endors
select '['+s.name+']', '['+h.Name+']', '['+StoneSize+']', '['+v.Name+']', v.Id from stones s
join Shapes h on s.ShapeId = h.id
join Vendors v on s.VendorId = v.id
where s.companyid = 9

select * from styles s
join Collections c on c.id = s.CollectionId
where c.CompanyId = 11

select * from vendors where id = 2015

update vendors set name = 'Multi Gems' where id = 2015

select c.name, * from vendors v
join Companies c on v.CompanyId = c.id
where v.name = ''
