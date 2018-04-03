select * from AspNetUsers as u
select * from AspNetRoles as r
select ur.*, u.Username, u.Email, r.Name from AspNetUserRoles as ur
join AspNetUsers as u on u.id = ur.UserId
join AspNetRoles as r on r.id = ur.RoleId
