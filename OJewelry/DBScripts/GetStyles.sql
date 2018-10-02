/* Get Styles */
select * from Styles s
	join StyleComponents sc on s.Id = sc.StyleId
	join Components c on c.id = sc.ComponentId

select com.Id, com.Name, cli.* from Companies com
join Clients cli on cli.CompanyID = com.id
where com.Id = 10006

select * from styles s
where s.id =1

select * from castings c
where c.id = 2

select * from castings
update Castings set MetalWtUnitId = 1 where MetalWtUnitId =0