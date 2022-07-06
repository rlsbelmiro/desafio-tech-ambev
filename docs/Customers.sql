create table Customers
(
	Id int not null primary key auto_increment,
	Name varchar(200) not null,
	Email varchar(200) not null,
	Document varchar(20) not null,
	Active bit not null default(1),
	CreatedAt datetime not null,
	CreatedBy varchar(200),
	UpdatedAt datetime null,
	UpdatedBy varchar(200) null
)