create table "Author"(

	"Id" serial primary key,
	"Uuid" uuid,
	"FirstName" varchar(30) Not null,
	"LastName" Varchar(30) Not null,
	"DateOfBirth" date,
	"Bio" varchar(1200),
	"IsActive" bool not null,
	"DateCreated" date not null,
	"DateUpdated" date not null
	
);

select * from "Author";

delete from "Author" where "Id" = 6;

create table "Recipe"(

	"Id" serial primary key,
	"Title" varchar(200) not null,
	"Subtitle" varchar(200) not null,
	"Text" varchar(4000) not null,
	"IsActive" bool not null,
	"DateCreated" date not null,
	"DateUpdated" date not null
);

create table "AuthorRecipe"(

	"Id" serial primary key,
	"RecipeId" int not null,
	"AuthorId" int not null
	
);

alter table "Author" alter column "DateOfBirth" type timestamp;
alter table "Author" alter column "DateCreated" type timestamp;
alter table "Author" alter column "DateUpdated" type timestamp;


alter table "AuthorRecipe" add constraint "Fk_AuthorRecipe_Author_AuthorId" foreign key ("AuthorId") references "Author" ("Id");

alter table "AuthorRecipe" add constraint "Fk_AuthorRecipe_Recepie_RecepieId" foreign key ("RecipeId") references "Recipe" ("Id");
