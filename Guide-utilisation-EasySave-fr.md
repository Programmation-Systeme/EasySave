# Guide d'utilisation EasySave V3.0
## Présentation
Ceci est un guide utilisateur des fonctionnalités du logiciel EasySave V3.0.
La fonctionnalité principale du logiciel est de créer des travaux de sauvegardes, c'est-à-dire de dupliquer des fichiers d'un dossier vers un autre dossier et de permettre une mise à jour/synchronisation simplifiée des fichiers.
EasySave permet de configurer une infinité de travaux de sauvegarde.

## 1. Naviguer dans le logiciel

Pour naviguer dans le logiciel, l'espace latéral gauche propose 4 menus distincts : "Accueil", "Créer", "Options" et "Exécution".

## 2. Fermer le logiciel

Il est possible de quitter le logiciel en cliquant sur la croix en haut à droite.

## 3. Sélectionner une langue

La langue peut être changée en allant dans le menu "Options" et en sélectionnant la langue souhaitée (français ou anglais). 

## 4. Voir et interagir avec les travaux de sauvegarde

Le menu "Accueil" montre les différents travaux de sauvegarde existants et permet d'interagir avec en cliquant pour en sélectionner un ou plusieurs et en cliquant par la suite sur "Exécuter" ou "Supprimer".

## 5. Créer un travail de sauvegarde

Le menu "Créer" permet de configurer un nouveau travail de sauvegarde, il suffit de cliquer sur "Sélectionner le dossier source" pour choisir le dossier qui sera copié et cliquer sur "Sélectionner le dossier de destination" pour choisir où ce dossier sera copié. Il faut également renseigner le type de sauvegarde qui sera appliqué à ce travail, complète (tous les fichiers sont même ceux non modifiés depuis la dernière sauvegarde seront copiés) et différentielle (seulement les fichiers ayant été modifiés entre la dernière sauvegarde et celle-ci seront copiés).
Pour finaliser la création, il suffit de cliquer sur "Ajouter".

## 6. Exécution d'un travail de sauvegarde

Un travail de sauvegarde en cours d'exécution peut être visualisé dans le menu "Exécution", où il peut être annulé en cliquant sur un travail puis sur la croix à droite, ou stoppé puis repris en cliquant sur le bouton pause puis continuer.

Si plusieurs travaux de sauvegarde sont exécutés en même temps, les opérations se réalisent en parallèle (c'est-à-dire en même temps).

Tous les fichiers avec pour extension une extension ajoutée dans les extensions à chiffrer dans le menu "Options" seront chiffrés lors de l'exécution d'un travail de sauvegarde.

Tous les fichiers avec pour extension une extension ajoutée dans les extensions à prioriser dans le menu "Options" seront copiés en premier lors de l'exécution d'un travail de sauvegarde.

## 7. Options configurables
Il est possible de configurer et changer plusieurs paramètres différentes dans le menu "Options" :
- La langue (choix entre français et anglais)
- Le format de logs (entre json et xml)
- L'ajout d'extensions de fichier qui doivent être chiffrés
- Le nom du processus métier, qui si en cours d'exécution empêchera tout travail de sauvegarde de s'exécuter
- Une taille maximale en Ko (kilo-octets), qui permettra si un fichier de taille égale ou supérieure est rencontré, de réduire la bande passante en arrêtant temporairement les autres travaux en cours d'exécution jusqu'à ce que ce fichier soit transféré.
- L'ajout d'extensions de fichiers priorisés qui doivent être traités en premier lors d'une sauvegarde

## 8. Logs

Les logs permettent d'avoir un historique des différentes sauvegardes faites à l'aide de EasySave.
Le log garde des informations générales telles que :
- La taille du fichier
- Le chemin du fichier source
- La date à laquelle la dernière mise à jour a été faite
...
Les nouvelles informations sont ajoutées à la fin du fichier log central à chaque mise à jour lancée.
Le fichier log peut être retrouvé à la racine du projet dans le dossier "LogDirectory", séparé du code source du logiciel.
