CREATE TABLE Users
(
	Id int not null primary key,
	Name varchar(200) not null,
	Email varchar(200) not null,
	Password varchar(1000) not null,
	CreatedAt datetime not null,
	UpdatedAt datetime null,
	CreatedBy varchar(200) not null,
	UpdatedBy varchar(200) not null,
	Active bit default(1) not null
)