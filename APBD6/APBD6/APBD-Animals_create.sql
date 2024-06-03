CREATE TABLE Animal (
    IdAnimal int  NOT NULL IDENTITY,
    Name nvarchar(200)  NOT NULL,
    Description nvarchar(200)  NULL,
    Category nvarchar(200)  NOT NULL,
    Area nvarchar(200)  NOT NULL,
    CONSTRAINT Animal_pk PRIMARY KEY  (IdAnimal)
);


