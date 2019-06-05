select c.Name, st.Image, * from styles st
join collections c on st.CollectionId = c.Id
where stylename like 'RNG - Morg%' and st.id = 612