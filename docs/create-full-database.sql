create database DevAmbev;
use DevAmbev;
CREATE TABLE Users
(
	Id int not null primary key auto_increment,
	Name varchar(200) not null,
	Email varchar(200) not null,
	Password varchar(1000) not null,
	CreatedAt datetime not null,
	UpdatedAt datetime null,
	CreatedBy varchar(200) not null,
	UpdatedBy varchar(200) null,
	Active bit default(1) not null
);
create table Products
(
	Id int not null primary key auto_increment,
	Name varchar(200) not null,
	Description varchar(1000) not null,
	Price decimal(18,2),
	Active bit not null default(1),
	Quantity int not null,
	CreatedAt datetime not null,
	CreatedBy varchar(200),
	UpdatedAt datetime null,
	UpdatedBy varchar(200) null
);
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
);
create table Orders
(
	Id int not null primary key auto_increment,
	Amount decimal(18,2) not null,
	CustomerId int not null,
	UserId int not null,
	Active bit not null default(1),
	CreatedAt datetime not null,
	CreatedBy varchar(200),
	UpdatedAt datetime null,
	UpdatedBy varchar(200) null,
	constraint FK_Customer foreign key(CustomerId) references Customers(Id),
	constraint FK_User foreign key(UserId) references Users(Id)
);
create table OrderItems
(
	Id int not null primary key auto_increment,
	UnityPrice decimal(18,2) not null,
	Quantity int not null,
	TotalPrice decimal(18,2) not null,
	ProductId int not null,
	OrderId int not null, 
	CreatedAt datetime not null,
	CreatedBy varchar(200),
	UpdatedAt datetime null,
	UpdatedBy varchar(200) null,
	constraint FK_Product foreign key(ProductId) references Products(Id),
	constraint FK_Order foreign key(OrderId) references Orders(Id)
);