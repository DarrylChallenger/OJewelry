select jt.*, st.* from styles st
join JewelryTypes jt on jt.Id = st.JewelryTypeId
where jt.Id in(1,2, 5005)

select * from JewelryTypes where Id in(1,2, 5005)

select * from Cost
delete from Cost where companyId = 1
