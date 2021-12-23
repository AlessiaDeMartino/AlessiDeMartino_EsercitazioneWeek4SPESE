create database GestioneSpese;

--Spese
--• Id (int, PK, auto-incrementale)
--• Data (datetime)
--• CategoriaId (int, FK)
--• Descrizione (varchar(500))
--• Utente (varchar(100))
--• Importo (decimal)
--• Approvato (bit)

--Categorie
--• Id (int, PK, auto-incrementale)
--• Categoria (varchar(100))
create table Categorie (
Id int primary key identity (1,1),
Categoria varchar(100) not null,
);

create table Spese (
Id int primary key identity (1,1),
DataSpesa datetime,
Descrizione varchar(500) not null,
Utente varchar(100) not null,
Importo decimal (4,2) not null,
Approvato bit not null,
CategoriaId int foreign key references Categorie(Id),
);

--select s.CategoriaId, sum(s.Importo)
--from spese s
--where s.CategoriaId=1
--group by s.CategoriaId

select spese.CategoriaId, sum(spese.Importo) as TotaleSpese
from spese
group by spese.CategoriaId

select spese.CategoriaId, sum(spese.Importo) as TotaleSpese
from spese join categorie on spese.CategoriaId=categorie.Id
group by spese.CategoriaId







