CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

INSERT INTO
	"Author" (
		"Uuid",
		"FirstName",
		"LastName",
		"DateOfBirth",
		"Bio",
		"IsActive",
		"DateCreated",
		"DateUpdated"
	)
VALUES
	(
	uuid_generate_v4(),
	'Petar',
	'Petrinja',
	'1984-01-21',
	'Krkan mekog srca!',
	'true',
	'2024-03-06',	
	'2024-03-06'
	);

INSERT INTO
	"Author" (
		"Uuid",
		"FirstName",
		"LastName",
		"DateOfBirth",
		"Bio",
		"IsActive",
		"DateCreated",
		"DateUpdated"
	)
VALUES
	(
	uuid_generate_v4(),
	'Marko',
	'Filetini',
	'1982-06-13',
	'Prvo radno iskustvo stekao u kuhinjama putnickih brodova gdje je i formirao svoj stil, fuzija azijske i mediteranske kuhinje.  ',
	'true',
	'2024-03-06',	
	'2024-03-06'
	), 
	(
	uuid_generate_v4(),
	'Ivo',
	'Ivic',
	'1974-04-17',
	'Prasci, svinje, gujde ...',
	'true',
	'2024-03-06',	
	'2024-03-06'
	);

INSERT INTO
	"Author" (
		"Uuid",
		"FirstName",
		"LastName",
		"DateOfBirth",
		"Bio",
		"IsActive",
		"DateCreated",
		"DateUpdated"
	)
VALUES
	(
	uuid_generate_v4(),
	'Petko',
	'Odrezak',
	'1982-06-13',
	'Bio',
	'false',
	'2024-03-06',	
	'2024-03-06'
	); 

Update "Author" Set "FirstName" = 'Marko' where "Id" = 8;

	select * from "Author";
	select * from "Author" where "IsActive" = true;

select "FirstName", "LastName" from "Author" where "Uuid" = 'bfde91db-9c0e-4548-af43-ed8030ae7153';

	