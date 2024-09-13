use NotePad
go

select *
from Users;

select * 
from Notes;

insert into NotePad.dbo.Notes
(Title, Description, CreatedAt, LastUpdatedAt, UserId) values 
('First note', 'First note description', '2024-09-13', '2024-09-13', 1),
('Second note', 'Second note description', '2024-09-13', '2024-09-13', 1),
('Third note', 'Third note description', '2024-09-13', '2024-09-13', 1),
('Fourth note', 'Fourth note description', '2024-09-13', '2024-09-13', 1),
('First note two', 'First note description', '2024-09-13', '2024-09-13', 2),
('Second note two', 'Second note description', '2024-09-13', '2024-09-13', 2),
('Third note two', 'Third note description', '2024-09-13', '2024-09-13', 2);


select 
	u.Id,
	u.Name,
	u.Email,
	n.Id,
	n.Title, 
	n.Description
from Users u
inner join Notes n
on u.Id = n.UserId;