insert into "AuthorRecepie"("RecepieId", "AuthorId") 
values (
	1,1
),
(
	2,2
),
(
	3,3
);

select * from "AuthorRecepie"

select "Author"."FirstName", "Author"."LastName", "Recepie"."Title"
from "AuthorRecepie"
inner join "Author" on "AuthorRecepie"."Id" = "Author"."Id"
inner join "Recepie" on "AuthorRecepie"."Id" = "Recepie"."Id";