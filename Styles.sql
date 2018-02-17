select sty.id, sty.StyleName, c.* from Styles as sty
join StyleCastings as sc on sty.id = sc.StyleId
join Castings as c on c.id = sc.CastingId
where sty.id = 3008

select sty.id, sty.StyleName, c.* from Styles as sty
join StyleComponents as sc on sty.id = sc.StyleId
join Components as c on c.id = sc.ComponentId
where sty.id = 3008

select * from stylecomponents where StyleId =3008

select sty.id, sty.StyleName from Styles as sty where sty.id = 3008

select sty.id, sty.StyleName  from Styles as sty
join StyleCastings as sc on sty.id = sc.StyleId
where sty.id = 3008

select * from castings where id = 2003

select * from Components where id = 1009