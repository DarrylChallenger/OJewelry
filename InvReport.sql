select s.Stylenum, s.stylename, s.Quantity from styles as s

select  distinct s.Id, s.StyleNum, s.Quantity from styles as s
join memo as m on s.id = m.StyleID
join Presenters as p on p.id = m.PresenterID
order by s.StyleNum

select  distinct p.Name from styles as s
join memo as m on s.id = m.StyleID
join Presenters as p on p.id = m.PresenterID
order by p.Name

select  s.Stylenum, p.Name, m.Quantity from styles as s
join memo as m on s.id = m.StyleID
join Presenters as p on p.id = m.PresenterID
order by stylenum, p.Name

