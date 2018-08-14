select * from styles

select * from styles s
join StyleFinding sf on s.id = sf.StyleId
join Findings f on f.id = sf.FindingId

select * from stones
select * from styles s
join StyleStone ss on s.id = ss.StyleId
join Stones st on st.id = ss.StoneId

select * from stylestone
select * from stylefinding
select * from Findings

select * from shapes

select * from Labor
select * from StyleLabor

select * from Companies
update stones set CompanyId =1
