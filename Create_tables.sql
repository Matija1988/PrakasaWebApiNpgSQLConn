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


create table "Recepie"(

	"Id" serial primary key,
	"Title" varchar(200) not null,
	"Subtitle" varchar(200) not null,
	"Text" varchar(4000) not null,
	"IsActive" bool not null,
	"DateCreated" date not null,
	"DateUpdated" date not null
);

create table "AuthorRecepie"(

	"Id" serial primary key,
	"RecepieId" int not null,
	"AuthorId" int not null
	
);

alter table "AuthorRecepie" add constraint "Fk_AuthorRecepie_Author_AuthorId" foreign key ("AuthorId") references "Author" ("Id");

alter table "AuthorRecepie" add constraint "Fk_AuthorRecepie_Recepie_RecepieId" foreign key ("RecepieId") references "Recepie" ("Id");

