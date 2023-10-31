-- Suppression d'un employé avec son adresse et ses affectations
-- et réinitialisation du compteur auto-incrémenté pour les id
declare @idEmploye int = 10
declare @idAdresse uniqueidentifier

select @idAdresse = IdAdresse from Employes where Id = @idEmploye

delete from Affectations where IdEmploye = @idEmploye
delete from employes where id = @idEmploye
delete from Adresses where id = @idAdresse

select @idEmploye = max(Id) from Employes
DBCC CHECKIDENT ('Employes', RESEED, @idEmploye) 

select * from Employes

-- C15DF44B-0243-4573-BAE1-8DF2C267409D

-- Suppression d'une commande et de ses lignes
-- et réinitialisation du compteur auto-incrémenté pour les id
declare @idCom int = 831
delete from Commandes where id = @idCom
select @idCom = max(Id) from Commandes
DBCC CHECKIDENT ('Commandes', RESEED, @idCom) 

select * from Commandes