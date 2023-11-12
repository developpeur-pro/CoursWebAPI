
declare @idEmploye int = 10
declare @idAdresse uniqueidentifier

select @idAdresse = IdAdresse from Employes where Id = @idEmploye

delete from employes where id = @idEmploye
delete from Adresses where id = @idAdresse
delete from Affectations where IdEmploye = @idEmploye

select @idEmploye = max(Id) from Employes
DBCC CHECKIDENT ('Employes', RESEED, @idEmploye) 

select * from Employes

-- C15DF44B-0243-4573-BAE1-8DF2C267409D

-- Supprssion d'une commande et de ses lignes
declare @idCom int = 831
delete from Commandes where id = @idCom
select @idCom = max(Id) from Commandes
DBCC CHECKIDENT ('Commandes', RESEED, @idCom) 

select * from Commandes where id = @idCom

select max(id) from Commandes