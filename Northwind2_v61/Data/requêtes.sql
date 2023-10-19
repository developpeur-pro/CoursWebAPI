
declare @idEmploye int = 11
declare @idAdresse uniqueidentifier

select @idAdresse = IdAdresse from Employes where Id = @idEmploye

delete from employes where id = @idEmploye
delete from Adresses where id = @idAdresse
delete from Affectations where IdEmploye = @idEmploye

select @idEmploye = max(Id) from Employes
DBCC CHECKIDENT ('Employes', RESEED, @idEmploye) 

select * from Employes

-- select * from Affectations where IdEmploye = 11

-- C15DF44B-0243-4573-BAE1-8DF2C267409D