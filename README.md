## Code du cours sur les Web API ASP.Net Core avec Entity Framework Core

Accès à la formation sur [Dyma.fr](https://dyma.fr/aspnetcore)

### v34 - Modèle EF
1ère partie du modèle EF Northwind2 avec tables Employes, Adresses, Affectations, Territoires et Regions, sans les relations entre tables 

### v36 - Modèle EF - Relations
Ajout des relations entre les tables Employes, Adresses, Affectations, Territoires et Regions dans le modèle EF. Permet d'illustrer tous les types de relations.

### v44 - Migration
Création d'une classe de migration pour créer la base. Ajout d'un script SQL à exécuter directement sur la base pour créer un jeu de données

### v53 - Requêtes GET
Création du contrôleur `EmployeController` avec actions de lecture pour les employés et leur territoires d'affectation. Création du service métier `ServiceEmployes` exploité par le contrôleur.

### v57 - Requêtes GET avec propriétés de navigation
Ajout d'exemples de sélection, filtrage et tri sur les actions de lecture. Ajout de propriétés de navigation et utilisation dans les actions de lecture pour récupérer des données associées.

### v61 - Requêtes POST - validation de modèle
Ajout des actions traitant les requêtes POST sur les employés, adresses et affectations pour illustrer la validation de modèle et les formats de données JSON, UrlEncoded et FormData.  
Ajout d'une entité DTO `FormEmploye` pour la récup des notes et photos sous formes de fichiers.

### v63 - Requêtes POST - traitement
Création du reste du modèle EF (tables produits, clients, commandes...) + jeu de données.  
Ajout du contrôleur et du service Commandes pour illustrer les différents types de traitements de requêtes POST (ajout de données autonomes ou référençant des données déjà existantes dans la base)

### v72 - Gestion des erreurs de base de données
Ajout de la gestion des erreurs de base de données par une méthode d'extension sur `ControllerBase`, puis par un middleware.

### v73 - Journalisation et validation
Mise en place de la journalisation dans les contrôleurs et dans le middleware.  
Illustration de la validation automatique par ajout d'attributs et d'une méthode de validation sur les entités `Employe` et `Adresse`.  
Implémentation manuelle de règles de validation pour la création de commandes.

### v91 - Requêtes DELETE, PUT et PATCH
Complétion des contrôleurs existants pour gérer la suppression et la modification de données.  
Ajout des contrôleurs Produit et Client pour illustrer  la modification.