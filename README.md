# Bin2Squid 🎮

Un mini-jeu multijoueur inspiré de "Squid Game" développé en Unity pour un casino en ligne.

## 📋 Table des matières

- [Aperçu](#aperçu)
- [Fonctionnalités](#fonctionnalités)
- [Architecture](#architecture)
- [Installation](#installation)
- [Configuration](#configuration)
- [Structure du projet](#structure-du-projet)
- [Contributeurs](#contributeurs)

---

## 📖 Aperçu

**Bin2Squid** est un jeu de plateforme multijoueur où les joueurs participent à des défis de sélection de blocs colorés. Le jeu combine:
- Un système de **matchmaking** et création de salons
- Un **chat** en temps réel entre joueurs
- Un système de **monnaie virtuelle** avec coûts d'entrée
- Une **mécanique de gameplay** basée sur la sélection de bonnes couleurs
- Un **système de classement** avec profils joueurs

**Plateforme**: Windows (exécutable compilé fourni: `Bin2SquidWindows/`)

---

## ✨ Fonctionnalités

### Authentification & Profil
- ✅ Système de login/signup avec **PlayFab**
- ✅ Profil joueur avec gestion de la monnaie
- ✅ Stockage des données utilisateur dans le cloud

### Lobbies & Salons
- ✅ Création de salons publics/privés
- ✅ Définition du coût d'entrée et capacité du salon
- ✅ Affichage des salons disponibles
- ✅ Rejoindre/quitter des salons
- ✅ Salle d'attente avec vue sur les autres joueurs

### Social
- ✅ Système d'amis
- ✅ Chat lobby en temps réel
- ✅ Statuts joueurs (en ligne, en jeu, etc.)

### Gameplay
- ✅ Plateforme avec blocs colorés à surmonter
- ✅ Sélection du bon bloc à chaque niveau
- ✅ Conditions de victoire/défaite
- ✅ HUD avec indices visuels
- ✅ Synchronisation multijoueur en temps réel

---

## 🏗 Architecture

Le projet utilise une architecture **client-serveur** avec:

### Backend
- **PlayFab** (Microsoft Azure)
  - Authentification utilisateur
  - Stockage des données de profil
  - Gestion des statistiques
  
- **Photon PUN 2** (Exit Games)
  - Réseau multijoueur temps réel
  - Synchronisation des états de jeu
  - RPC calls entre clients

### Frontend
- **Unity** (moteur de jeu)
- **C#** (langage principal)

### Structure Client

```
Assets/Scripts/
├── Menu/
│   ├── Auth/                    # Authentification (Login/Signup)
│   ├── PhotonManager.cs         # Gestion Photon
│   ├── PlayfabManager.cs        # Gestion PlayFab
│   ├── Room/
│   │   ├── RoomCreation/        # Création de salons
│   │   ├── RoomDisplay/         # Affichage des salons
│   │   └── Profile/             # Profil joueur
│   ├── WaitingRoom/             # Salle d'attente
│   ├── Chat/                    # Système de chat
│   └── Friend/                  # Gestion des amis
│
└── InGame/
    ├── InGameManager.cs         # Orchestration du gameplay
    ├── Blocs/
    │   ├── BlocsManager.cs       # Gestion des blocs
    │   └── BlocItem.cs           # Bloc individuel
    └── Hud/
        ├── HudManager.cs         # Interface utilisateur
        └── WinLoseCondition.cs   # Conditions de fin
```

---

## 🚀 Installation

### Prérequis
- **Unity 2022 LTS ou supérieur** (version recommandée)
- **.NET Framework 4.7.1+**
- **Photon PUN 2** (inclus dans le projet)
- **PlayFab SDK** (inclus dans le projet)

### Étapes

1. **Cloner le projet**
   ```bash
   git clone <repository-url>
   cd Bin2Squid/Bin2Squid
   ```

2. **Ouvrir avec Unity**
   - Ouvrir Unity Hub
   - Cliquer sur "Open" → sélectionner le dossier `Bin2Squid`

3. **Attendre la compilation**
   - Unity va compiler tous les scripts automatiquement

4. **Vérifier les configurations**
   - Ouvrir `Window > Photon Unity Networking > Highlight Server Settings`
   - Ouvrir `Window > PlayFab > Settings` et vérifier les clés API

---

## ⚙️ Configuration

### Configuration Photon

1. **Créer un compte Photon** (gratuit): https://www.photonengine.com
2. **Récupérer votre App ID**
3. **Dans Unity**: `Assets > Photon > Resources > PhotonServerSettings.asset`
4. **Coller l'App ID** dans le champ correspondant

### Configuration PlayFab

1. **Créer un compte PlayFab** (gratuit): https://developer.playfab.com
2. **Créer un titre**
3. **Dans Unity**: `Assets > PlayFab > Resources > PlayFabSettings.json`
4. **Remplir les identifiants PlayFab**

### Données de Salon

Les salons stockent:
- Nom et capacité
- Coût d'entrée (propriété personnalisée: `RoomAmountofMoney`)
- Propriétaires et liste des joueurs

---

## 📁 Structure du Projet

```
Bin2Squid/
├── Assets/
│   ├── Photon/                  # SDK Photon PUN 2
│   ├── PlayFabSDK/              # SDK PlayFab
│   ├── PlayFabEditorExtensions/ # Outils PlayFab pour l'éditeur
│   ├── Resources/               # Ressources (sprites, audio)
│   ├── Scenes/                  # Scènes Unity
│   │   ├── Menu.unity
│   │   └── InGame.unity
│   ├── Scripts/                 # Code source (voir section Architecture)
│   └── TextMesh Pro/            # Polices et ressources TMPro
│
├── Bin2SquidWindows/            # Build Windows compilée
│   ├── Bin2Squid_Data/
│   └── MonoBleedingEdge/
│
├── Library/                     # Cache Unity (ne pas modifier)
├── Logs/                        # Logs d'exécution
├── Packages/                    # Dépendances package
├── ProjectSettings/             # Paramètres du projet
├── Temp/                        # Fichiers temporaires
│
├── Bin2Squid.sln               # Solution Visual Studio
├── README.md                    # Ce fichier
└── *.csproj                     # Fichiers de projet C#
```

---

## 🎮 Comment Jouer

1. **Lancer le jeu** (depuis l'exécutable Windows ou Unity)
2. **S'authentifier** (login ou création de compte)
3. **Créer ou rejoindre une salle**
4. **Attendre les autres joueurs** dans la salle d'attente
5. **Participer au défi** de sélection de blocs
6. **Avancer** à travers les niveaux successifs
7. **Remporter** les récompenses en cas de victoire

---

## 🔧 Technologies Utilisées

| Technologie | Utilisation | Version |
|-------------|-------------|---------|
| **Unity** | Moteur de jeu | 2022+ |
| **C#** | Langage de programmation | 9.0+ |
| **Photon PUN 2** | Multijoueur temps réel | 2.x |
| **PlayFab** | Backend et données | SDK intégré |
| **TextMesh Pro** | Rendu de texte avancé | Inclus |

---

## 📊 Système de Monnaie

- Les joueurs possèdent un solde virtuel
- Chaque salon a un **coût d'entrée** défini par le créateur
- Le système valide que le joueur a assez de fonds avant d'entrer
- Les gains/pertes se reflètent dans le profil après chaque partie

---

## 🐛 Dépannage

### Photon ne se connecte pas
- Vérifier que l'App ID Photon est correct
- Vérifier la connexion Internet
- Vérifier les paramètres firewall

### PlayFab retourne une erreur
- Vérifier les clés API PlayFab
- Vérifier que le titre PlayFab est créé
- Consulter les logs PlayFab dans le dashboard

### Les salons ne s'affichent pas
- Vérifier que vous êtes connecté au lobby Photon
- Essayer de relancer le jeu

---

## 📈 Améliorations Futures

- [ ] Système de classement global (leaderboard)
- [ ] Récompenses et achievements
- [ ] Différents modes de jeu
- [ ] Support mobile
- [ ] Cosmétiques achetables
- [ ] Système de niveaux de difficulté
- [ ] Spectateur en direct

---

## 👥 Contributeurs

Développé par l'équipe Epitech pour le projet **Bin2Squid**.

---

## 📄 Licence

Ce projet est développé à titre éducatif/professionnel.

---

