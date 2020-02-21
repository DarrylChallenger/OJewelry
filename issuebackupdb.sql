/*backup */

select s.*, ss.Name from stones s
join shapes ss on s.ShapeId = ss.id 
where s.companyid = 4

select * from stones where companyid = 4

select * from vendors where companyid = 4

select * from findings where companyid = 4

select * from companies

select * from StyleStone

select * from stones where Companyid <> 4
select * from findings where Companyid <> 4
select * from findings
select * from shapes