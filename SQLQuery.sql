go
use ExamBookLibraryDB
go

update Users
set IsAdmin = 1
where id = 1

select * from Users
select * from People
select * from Books
select * from Authors
select * from Publishers
select * from Actions
select * from Books where User_Id is not NULL