insert into "AuthorRecipe"("RecipeId", "AuthorId") 
values (
	4,1
),
(
	5,2
),
(
	6,3
);

insert into "AuthorRecipe"("RecipeId", "AuthorId") 
values (
	6,2
),
(
	7,2
),
(
	8,2
);


select * from "AuthorRecipe";

select * from "Recipe";

select * from "Author";

select "Author"."Uuid", "Author"."FirstName", "Author"."LastName", "Recipe"."Title"
from "AuthorRecipe"  
	 inner join "Author" on "AuthorRecipe"."AuthorId" = "Author"."Id"
 inner join "Recipe" on "AuthorRecipe"."RecipeId" = "Recipe"."Id" 
	where "Author"."FirstName" = 'Filetini' ;

select "Author"."FirstName", "Author"."LastName", "Recipe"."Title"
from "Author"
inner join "AuthorRecipe" on "Author"."Id" = "AuthorRecipe"."AuthorId"
inner join "Recipe" on "Recipe"."Id" = "AuthorRecipe"."RecipeId"
	where "Author"."Uuid" = '13210dca-fc16-44ea-85fd-2e077b5c2e19';

select "Author"."FirstName", "Author"."LastName", "Recipe"."Title"
from "Author"
inner join "AuthorRecipe" on "Author"."Id" = "AuthorRecipe"."AuthorId"
inner join "Recipe" on "Recipe"."Id" = "AuthorRecipe"."RecipeId"
	where "Author"."FirstName" Like '%Ma%'
	and "Author"."LastName" like '%Fi%';