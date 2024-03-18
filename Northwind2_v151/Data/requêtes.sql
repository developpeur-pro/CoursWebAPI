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
delete from Commandes where id > 830
declare @idCom as int
select @idCom = max(Id) from Commandes
DBCC CHECKIDENT ('Commandes', RESEED, @idCom) 

-- Nombre de produits par catégorie
select c.id, c.Nom, count(p.id)
from Categories c
left outer join Produits p on c.Id = p.IdCategorie
group by c.Id, c.Nom

select * from Produits p where p.Arrete = 'true'

-- Commandes et nombres de lignes associées
select C.Id, count(lc.idProduit) 
from Commandes c
left outer join LignesCommandes lc on c.Id = lc.IdCommande
group by C.Id
order by 1 desc

select * from Commandes where Id = 830
select * from LignesCommandes where IdCommande = 832

select IdFournisseur, id, nom, pu, UnitesEnStock, NiveauReappro, Arrete
from Produits p order by p.IdFournisseur, p.Id

select * from Clients c 
inner join Adresses a on c.IdAdresse = a.Id
where a.Pays = 'France'

select * from produits where id = 78

update Produits
set Nom = 'riz long', Version = '2025-01-03-16-15-865'
where Id = 77 and Version = '2025-01-03-16-30-456'