alter table "Author" add column "RoleId" int;
alter table "Author" add column "Username" varchar(50);
alter table "Author" add column "Password" varchar(255);



select * from "Author";

ALTER TABLE "Author"
    ADD CONSTRAINT "Fk_Author_Role_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Role"("Id");

Update "Author" set "Username" = 'user4'
where  "Id" = 4;

Update "Author" set "Password" = 
	'$2a$12$0C5R86XUeICNiTkBrMiG3u1g0ZdcDwIQ9JNOkH9iFhaBz5CfPTKWq'
where "Id" = 4;

