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

select * from "AuthorRecipe"

select "Author"."FirstName", "Author"."LastName", "Recipe"."Title"
from "AuthorRecipe"
inner join "Author" on "AuthorRecipe"."Id" = "Author"."Id"
inner join "Recipe" on "AuthorRecipe"."Id" = "Recipe"."Id";