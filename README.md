## Code associé au cours sur les Web API ASP.Net Core avec Entity Framework Core

Accès à la formation sur [Dyma.fr](https://dyma.fr/aspnetcore)

- v34 : 1ère partie du modèle EF Northwind2 avec tables Employes, Adresses, Affectations, Territoires et Regions, sans les relations entre tables 
- v36 : Ajout des relations entre les tables Employes, Adresses, Affectations, Territoires et Regions dans le modèle EF. Permet d'illustrer tous les types de relations.
- v44 : Création d'une classe de migration pour créer la base. Ajout d'un script SQL à exécuter directement sur la base pour créer un jeu de données
- v53 : Création du contrôleur `EmployeController` avec actions de lecture pour les employés et leur territoires d'affectation. Création du service métier `ServiceEmployes` exploité par le contrôleur.
- v57 : Ajout d'exemples de sélection, filtrage et tri sur les actions de lecture. Ajout de propriétés de navigation et utilisation dans les actions de lecture pour récupérer des données associées.
- v61 : Création du reste du modèle EF (tables produits, clients, commandes...) + jeu de données.  
Ajout des actions traitant les requêtes POST sur les employés, adresses et affectations pour illustrer la validation de modèle et les formats de donénes JSON, UrlEncoded et FormData
