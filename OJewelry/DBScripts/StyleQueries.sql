﻿select * from styles

select * from styles s
join StyleFinding sf on s.id = sf.StyleId
join Findings f on f.id = sf.FindingId

select * from stones
select * from styles s
join StyleStone ss on s.id = ss.StyleId
join Stones st on st.id = ss.StoneId

select * from stylefinding
select * from Findings
