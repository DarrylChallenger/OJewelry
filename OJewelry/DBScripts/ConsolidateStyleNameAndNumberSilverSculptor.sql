/*

1. Silver Sculptor cost sheets - can you move what is now under Style Name into Description (which is currently blank).
2. Rush cost sheets - currently Style # and Style name are teh same so one can just be deleted
3. SentiMetal - Style name can be deleted (it is just duplication of description)

*/


select * from Companies

select * from styles s
join Collections c on c.id = s.CollectionId
where c.CompanyId = 11


begin transaction
update styles set [Desc] = stylename where id in
(
select s.id from styles s
join Collections c on c.id = s.CollectionId
where c.CompanyId = 11
)
commit transaction