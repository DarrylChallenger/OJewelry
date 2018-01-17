/* Get Styles */
select * from Styles s
	join StyleComponents sc on s.Id = sc.StyleId
	join Components c on c.id = sc.ComponentId

