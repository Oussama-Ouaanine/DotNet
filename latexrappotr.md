
\documentclass[12pt,a4paper]{article}

% --- Encodage et langue ---
\usepackage[utf8]{inputenc}
\usepackage[T1]{fontenc}
\usepackage[french]{babel}

% --- Mise en page ---
\usepackage{geometry}
\geometry{margin=2.5cm}
\usepackage{parskip}
\setlength{\parskip}{6pt}
\usepackage{setspace}
\onehalfspacing

% --- Titres et liens ---
\usepackage{hyperref}
\hypersetup{colorlinks=true, linkcolor=blue, urlcolor=blue}

% --- Graphiques et diagrammes ---
\usepackage{tikz}
\usetikzlibrary{shapes, arrows.meta, positioning, fit, calc, shapes.multipart}

% --- Images ---
\usepackage{graphicx}
\graphicspath{{screenshots/}}

% --- Tableaux ---
\usepackage{booktabs}
\usepackage{array}

% --- Code source ---
\usepackage{listings}
\lstset{
  basicstyle=\ttfamily\small,
  breaklines=true,
  frame=single,
  language=[Sharp]C,
  keywordstyle=\color{blue}\bfseries,
  commentstyle=\color{gray}\itshape,
  stringstyle=\color{red}
}

% --- Couleurs ---
\usepackage{xcolor}
\definecolor{lightblue}{RGB}{173,216,230}
\definecolor{lightgreen}{RGB}{144,238,144}
\definecolor{lightyellow}{RGB}{255,255,224}

% --- Page de garde personnalisée ---
\begin{document}

\begin{titlepage}
  \centering
  
  % Logo / En-tête école
  \vspace*{1cm}
  {\Large \textbf{EMSI}}\\[0.3cm]
  {\large École Marocaine Des Sciences De L'Ingénieur}\\[0.5cm]
  \rule{\textwidth}{0.5pt}
  
  \vspace{2cm}
  
  % Titre du rapport
  {\Huge \textbf{Rapport de Projet}}\\[1cm]
  
  % Nom du projet
  {\LARGE \textbf{Système de Gestion de Bibliothèque}}\\[0.5cm]
  {\Large Développé avec ASP.NET Core MVC}\\[0.3cm]
  {\large \textit{Library Management System}}
  
  \vspace{2cm}
  
  % Illustration simple
  \begin{tikzpicture}
    \draw[fill=blue!20, draw=blue!50, thick] (0,0) rectangle (3,4);
    \draw[fill=white] (0.3,3.5) rectangle (2.7,2.5);
    \foreach \y in {2.2,1.9,1.6,1.3,1.0,0.7,0.4}
      \draw[thick] (0.5,\y) -- (2.5,\y);

  \end{tikzpicture}
  
  \vspace{2cm}
  
  % Auteurs
  {\Large \textbf{Réalisé par :}}\\[0.5cm]
  {\large
    Oussama \textsc{Ouaanine}
      \rule{\textwidth}{0.5pt}\\[0.3cm]
  {\large Année Universitaire 2024 -- 2025}\\[0.2cm]
  {\small Décembre 2025}
  }
  
  


\end{titlepage}

% Table des matières
\tableofcontents
\newpage

% =============================================================================
\section{Introduction}
% =============================================================================

La gestion efficace d'une bibliothèque requiert un système informatique robuste permettant de gérer les livres, les utilisateurs et les réservations de manière fluide et sécurisée. Ce rapport présente le développement d'un système de gestion de bibliothèque moderne développé avec ASP.NET Core MVC, offrant une interface web intuitive pour les administrateurs et les membres.

Le système implémente un modèle de contrôle d'accès basé sur les rôles (RBAC), distinguant deux types d'utilisateurs : les \textbf{administrateurs} qui gèrent le catalogue et les réservations, et les \textbf{clients} (membres) qui peuvent consulter le catalogue et réserver des livres.

L'application a été conçue pour être simple, performante et facile à maintenir, utilisant un stockage en mémoire pour les données et une architecture MVC claire séparant les préoccupations.

\subsection{Contexte du projet}

Les bibliothèques modernes nécessitent des outils numériques pour :
\begin{itemize}
  \item Gérer un catalogue de livres avec informations détaillées et couvertures
  \item Permettre aux membres de découvrir et réserver des ouvrages facilement
  \item Suivre l'état des réservations (en attente, approuvées, refusées, terminées)
  \item Offrir aux administrateurs des outils de gestion centralisés
  \item Assurer la sécurité des données et le contrôle d'accès approprié
\end{itemize}

% =============================================================================
\section{Objectifs du Projet}
% =============================================================================

Les objectifs principaux de ce système sont :

\begin{itemize}
  \item \textbf{Gestion du catalogue} : Permettre l'ajout, la modification et la suppression de livres avec images de couverture, catégories et métadonnées complètes.
  
  \item \textbf{Système de réservation} : Implémenter un workflow de réservation avec états multiples (En attente $\rightarrow$ Approuvée/Refusée $\rightarrow$ Terminée).
  
  \item \textbf{Contrôle d'accès} : Authentification basée sur les sessions avec distinction claire entre rôles administrateur et client.
  
  \item \textbf{Interface utilisateur moderne} : Conception responsive avec affichage en grille des livres, système de recherche et navigation intuitive.
  
  \item \textbf{Traçabilité} : Suivi complet de l'historique des réservations avec dates d'approbation, dates limites et retours.
  
  \item \textbf{Performance} : Architecture en mémoire avec services singleton garantissant des temps de réponse rapides.
\end{itemize}

% =============================================================================
\section{Description de l'Application}
% =============================================================================

\subsection{Fonctionnalités principales}

\subsubsection{Pour les visiteurs}
\begin{itemize}
  \item Consultation de la page d'accueil avec livres en vedette
  \item Création de compte membre
  \item Authentification sécurisée
\end{itemize}

\subsubsection{Pour les clients (membres)}
\begin{itemize}
  \item \textbf{Navigation du catalogue} : Parcourir les livres par catégorie avec affichage en grille
  \item \textbf{Recherche} : Rechercher des livres par titre ou auteur
  \item \textbf{Réservation} : Réserver des livres disponibles en un clic
  \item \textbf{Suivi} : Consulter l'état de leurs réservations (en attente, approuvées, historique)
  \item \textbf{Détails} : Voir les informations complètes d'un livre (description, ISBN, année de publication)
\end{itemize}

\subsubsection{Pour les administrateurs}
\begin{itemize}
  \item \textbf{Tableau de bord} : Vue d'ensemble avec statistiques (total livres, catégories, réservations actives, membres)
  \item \textbf{Gestion des livres} : Créer, modifier, supprimer des livres avec upload d'images de couverture
  \item \textbf{Gestion des catégories} : Créer et gérer les catégories de livres
  \item \textbf{Approbation des réservations} : Approuver ou refuser les demandes avec attribution de dates limites
  \item \textbf{Historique} : Consulter toutes les réservations avec filtres (statut, recherche) et alertes de retard
  \item \textbf{Gestion des clients} : Visualiser la liste des membres inscrits
\end{itemize}

\subsection{Workflow de réservation}

Le système implémente un workflow de réservation à états multiples :

\begin{enumerate}
  \item \textbf{Création} : Le client réserve un livre $\rightarrow$ Statut : \textit{Pending}
  \item \textbf{Décision administrative} : 
    \begin{itemize}
      \item Approbation : Admin assigne une date limite $\rightarrow$ Statut : \textit{Approved}
      \item Refus : Admin refuse la demande $\rightarrow$ Statut : \textit{Refused}
    \end{itemize}
  \item \textbf{Finalisation} : Admin marque le retour du livre $\rightarrow$ Statut : \textit{Completed}
\end{enumerate}

\subsection{Catalogue de livres}

Le système contient un catalogue préchargé de \textbf{50 livres} répartis dans 6 catégories :
\begin{itemize}
  \item Fiction (10 livres)
  \item Affaires/Business (8 livres)
  \item Technologie (7 livres)
  \item Bien-être (7 livres)
  \item Biographie (6 livres)
  \item Histoire (6 livres)
\end{itemize}

Chaque livre possède une image de couverture réelle (hébergée sur Amazon) pour une expérience visuelle enrichie.

% =============================================================================
\section{Captures d'écran de l'Application}
% =============================================================================

Cette section présente les principales interfaces de l'application en fonctionnement.

\subsection{Page d'accueil}

La page d'accueil présente une sélection de livres en vedette avec leurs couvertures, permettant aux visiteurs de découvrir le catalogue avant même de se connecter.

\begin{figure}[h]
\centering
\fbox{\parbox{0.9\textwidth}{
  \centering
  \vspace{3cm}
  \textit{[Insérer capture d'écran: Page d'accueil avec livres en vedette]}\\
  \texttt{http://localhost:5284}
  \vspace{3cm}
}}
\caption{Page d'accueil publique du système}
\label{fig:homepage}
\end{figure}

\subsection{Interface de connexion}

L'écran de connexion permet aux utilisateurs de s'authentifier en tant qu'administrateur ou client.

\begin{figure}[h]
\centering
\fbox{\parbox{0.9\textwidth}{
  \centering
  \vspace{3cm}
  \textit{[Insérer capture d'écran: Page de connexion]}\\
  \texttt{http://localhost:5284/Account/Login}
  \vspace{3cm}
}}
\caption{Interface de connexion avec validation de formulaire}
\label{fig:login}
\end{figure}

\newpage
\subsection{Interface client - Navigation du catalogue}

Les clients peuvent parcourir le catalogue organisé par catégories avec un affichage en grille moderne.

\begin{figure}[h]
\centering
\fbox{\parbox{0.9\textwidth}{
  \centering
  \vspace{3cm}
  \textit{[Insérer capture d'écran: Page Browse avec grille de livres]}\\
  \texttt{http://localhost:5284/Client/Browse}
  \vspace{3cm}
}}
\caption{Navigation du catalogue avec affichage en grille}
\label{fig:browse}
\end{figure}

\subsection{Interface client - Mes réservations}

Les clients peuvent consulter toutes leurs réservations organisées par statut (En attente, Approuvées, Historique).

\begin{figure}[h]
\centering
\fbox{\parbox{0.9\textwidth}{
  \centering
  \vspace{3cm}
  \textit{[Insérer capture d'écran: Page MyBookings]}\\
  \texttt{http://localhost:5284/Client/MyBookings}
  \vspace{3cm}
}}
\caption{Tableau de bord des réservations client}
\label{fig:mybookings}
\end{figure}

\newpage
\subsection{Interface administrateur - Tableau de bord}

Le tableau de bord administrateur affiche des statistiques clés et les activités récentes.

\begin{figure}[h]
\centering
\fbox{\parbox{0.9\textwidth}{
  \centering
  \vspace{3cm}
  \textit{[Insérer capture d'écran: Tableau de bord admin]}\\
  \texttt{http://localhost:5284/Admin}
  \vspace{3cm}
}}
\caption{Tableau de bord administrateur avec statistiques}
\label{fig:admin-dashboard}
\end{figure}

\subsection{Interface administrateur - Gestion des réservations}

L'interface d'approbation permet aux administrateurs de gérer les demandes de réservation avec attribution de dates limites.

\begin{figure}[h]
\centering
\fbox{\parbox{0.9\textwidth}{
  \centering
  \vspace{3cm}
  \textit{[Insérer capture d'écran: Page Bookings avec formulaire d'approbation]}\\
  \texttt{http://localhost:5284/Admin/Bookings}
  \vspace{3cm}
}}
\caption{Interface d'approbation des réservations}
\label{fig:admin-bookings}
\end{figure}

% =============================================================================
\section{Architecture Générale}
% =============================================================================

\subsection{Pattern MVC (Model-View-Controller)}

L'application suit rigoureusement le pattern architectural MVC :

\begin{center}
\begin{tikzpicture}[
  node distance=2cm,
  box/.style={rectangle, draw, rounded corners, minimum width=3cm, minimum height=1.2cm, align=center, fill=blue!10, thick},
  arrow/.style={-{Stealth}, thick}
]
  \node[box, fill=green!10] (view) at (0, 0) {\textbf{View}\\(Razor Pages)};
  \node[box, fill=blue!10] (controller) at (5, 0) {\textbf{Controller}\\(C\# Classes)};
  \node[box, fill=orange!10] (model) at (10, 0) {\textbf{Model}\\(Entities)};
  \node[box, fill=yellow!10] (service) at (5, -3) {\textbf{Service}\\(Business Logic)};
  
  \draw[arrow] (view) -- node[above] {1. Request} (controller);
  \draw[arrow] (controller) -- node[above] {2. Updates} (model);
  \draw[arrow] (controller) -- node[right] {3. Calls} (service);
  \draw[arrow] (service) -- node[below] {4. Manipulates} (model);
  \draw[arrow] (controller) -- node[below] {5. Returns} (view);
\end{tikzpicture}
\end{center}

\subsection{Structure des dossiers}

\begin{lstlisting}[language=bash, frame=none]
LibraryWebApp/
├── Controllers/               # Contrôleurs MVC
│   ├── HomeController.cs         # Page d'accueil
│   ├── AccountController.cs      # Authentification
│   ├── ClientController.cs       # Portail membre
│   └── AdminController.cs        # Portail admin
│
├── Models/                    # Entités du domaine
│   ├── User.cs                   # Utilisateur
│   ├── Book.cs                   # Livre
│   ├── Category.cs               # Catégorie
│   ├── Booking.cs                # Réservation
│   ├── BookingStatus.cs          # Énumération des états
│   └── ViewModels/               # ViewModels pour vues
│
├── Services/                  # Logique métier
│   ├── UserService.cs            # Gestion utilisateurs
│   ├── BookService.cs            # Gestion livres
│   ├── CategoryService.cs        # Gestion catégories
│   └── BookingService.cs         # Gestion réservations
│
├── Views/                     # Vues Razor
│   ├── Home/                     # Vues publiques
│   ├── Account/                  # Login/Register
│   ├── Client/                   # Interface client
│   ├── Admin/                    # Interface admin
│   └── Shared/                   # Layouts partagés
│
└── wwwroot/                   # Ressources statiques
    ├── css/                      # Feuilles de style
    ├── js/                       # Scripts JavaScript
    └── uploads/                  # Images uploadées
\end{lstlisting}

\subsection{Architecture en couches}

\begin{center}
\begin{tikzpicture}[
  layer/.style={rectangle, draw, minimum width=10cm, minimum height=1.2cm, align=center, thick},
  arrow/.style={-{Stealth}, thick}
]
  \node[layer, fill=blue!20] (presentation) at (0, 0) {\textbf{Couche Présentation} --- Controllers + Views (Razor)};
  \node[layer, fill=green!20] (business) at (0, -2) {\textbf{Couche Métier} --- Services (BookService, UserService, etc.)};
  \node[layer, fill=orange!20] (data) at (0, -4) {\textbf{Couche Données} --- In-Memory Storage (List<T> + Locks)};
  \node[layer, fill=purple!20] (domain) at (0, -6) {\textbf{Couche Domaine} --- Models (Book, User, Booking, etc.)};
  
  \draw[arrow] (presentation.south) -- (business.north);
  \draw[arrow] (business.south) -- (data.north);
  \draw[arrow, dashed] (presentation.east) -- ++(1.5,0) |- (domain.east);
  \draw[arrow, dashed] (business.east) -- ++(1,0) |- (domain.east);
\end{tikzpicture}
\end{center}

\subsection{Flux de données}

\begin{enumerate}
  \item L'utilisateur envoie une requête HTTP (ex: GET /Client/Browse)
  \item Le routeur ASP.NET Core achemine vers le contrôleur approprié
  \item Le contrôleur vérifie l'authentification via la session
  \item Le contrôleur appelle les services métier nécessaires
  \item Les services manipulent les données en mémoire (avec verrous thread-safe)
  \item Le contrôleur construit un ViewModel et retourne une vue Razor
  \item La vue est rendue en HTML et envoyée au client
\end{enumerate}

% =============================================================================
\section{Technologies Utilisées}
% =============================================================================

\begin{table}[h]
\centering
\begin{tabular}{@{}lll@{}}
\toprule
\textbf{Technologie} & \textbf{Version} & \textbf{Rôle} \\
\midrule
ASP.NET Core & 8.0 & Framework web backend \\
C\# & 12 & Langage de programmation \\
Razor Pages & --- & Moteur de templates \\
Bootstrap & 5.3 & Framework CSS responsive \\
jQuery & 3.x & Manipulation DOM \\
TikZ/LaTeX & --- & Génération de diagrammes UML \\
\bottomrule
\end{tabular}
\caption{Stack technologique du projet}
\end{table}

\subsection{Justification des choix}

\begin{itemize}
  \item \textbf{ASP.NET Core MVC} : Framework mature offrant une architecture MVC robuste, injection de dépendances native, et excellentes performances.
  
  \item \textbf{Stockage en mémoire} : Choix délibéré pour simplifier le déploiement et garantir des temps de réponse ultra-rapides. Les données sont initialisées au démarrage de l'application via des méthodes de seeding.
  
  \item \textbf{Services Singleton} : Pattern garantissant une instance unique partagée de chaque service, avec protection par verrous (locks) pour la thread-safety.
  
  \item \textbf{Authentification par session} : Solution légère sans framework d'authentification complexe, stockant Username, Email, Role et UserId dans des cookies de session avec timeout de 30 minutes.
  
  \item \textbf{Bootstrap 5} : Framework CSS moderne permettant une interface responsive et élégante avec peu de code personnalisé.
\end{itemize}

% =============================================================================
\section{Modélisation UML}
% =============================================================================

Cette section présente les diagrammes UML du système en respectant les normes UML 2.5.

\newpage
\subsection{Diagramme de cas d'utilisation}

Le diagramme suivant illustre les interactions entre les acteurs et le système.

\begin{center}
\begin{tikzpicture}[
  scale=0.85,
  every node/.style={transform shape},
  actor/.style={
    circle, 
    draw, 
    minimum size=0.6cm, 
    fill=yellow!20,
    thick
  },
  usecase/.style={
    ellipse, 
    draw, 
    minimum width=3cm, 
    minimum height=0.8cm, 
    align=center, 
    fill=lightblue,
    thick
  },
  system/.style={
    rectangle, 
    draw, 
    dashed, 
    rounded corners, 
    thick,
    inner sep=0.8cm
  },
  include/.style={
    -{Stealth}, 
    dashed,
    thick
  },
  extend/.style={
    -{Stealth}, 
    dashed,
    thick
  },
  assoc/.style={
    thick
  }
]
  
  % Acteurs
  \node[actor] (visiteur) at (-3, 6) {};
  \node[below=0.1cm of visiteur, font=\small] {\textbf{Visiteur}};
  
  \node[actor] (client) at (-3, 0) {};
  \node[below=0.1cm of client, font=\small] {\textbf{Client}};
  
  \node[actor] (admin) at (-3, -6) {};
  \node[below=0.1cm of admin, font=\small] {\textbf{Admin}};
  
  % Système
  \node[system, minimum width=11cm, minimum height=16cm] (sys) at (5, 0) {};
  \node[above] at (5, 8.5) {\textbf{Système de Gestion de Bibliothèque}};
  
  % Cas d'utilisation - Visiteur
  \node[usecase] (consulter) at (5, 6.5) {Consulter\\catalogue};
  \node[usecase] (inscrire) at (5, 5) {S'inscrire};
  \node[usecase] (connecter) at (5, 3.5) {Se connecter};
  
  % Cas d'utilisation - Client
  \node[usecase] (parcourir) at (5, 1.5) {Parcourir livres\\par catégorie};
  \node[usecase] (rechercher) at (5, 0) {Rechercher\\livre};
  \node[usecase] (reserver) at (5, -1.5) {Réserver\\livre};
  \node[usecase] (mesresa) at (5, -3) {Consulter mes\\réservations};
  
  % Cas d'utilisation - Admin
  \node[usecase] (dashboard) at (5, -4.5) {Accéder au\\tableau de bord};
  \node[usecase] (gererlivres) at (5, -6) {Gérer\\livres};
  \node[usecase] (gerercat) at (10, -6.5) {Gérer\\catégories};
  \node[usecase] (approuver) at (5, -7.5) {Approuver/Refuser\\réservations};
  
  % Relations Visiteur
  \draw[assoc] (visiteur) -- (consulter);
  \draw[assoc] (visiteur) -- (inscrire);
  \draw[assoc] (visiteur) -- (connecter);
  
  % Relations Client (héritage de Visiteur)
  \draw[-{Triangle[open, fill=white]}, thick] (client) -- (visiteur);
  \draw[assoc] (client) -- (parcourir);
  \draw[assoc] (client) -- (rechercher);
  \draw[assoc] (client) -- (reserver);
  \draw[assoc] (client) -- (mesresa);
  
  % Relations Admin
  \draw[assoc] (admin) -- (dashboard);
  \draw[assoc] (admin) -- (gererlivres);
  \draw[assoc] (admin) -- (gerercat);
  \draw[assoc] (admin) -- (approuver);
  \draw[assoc] (admin) -- (connecter);
  
  % Relation include
  \draw[include] (reserver) -- node[right, font=\tiny] {<<include>>} (connecter);
  \draw[include] (mesresa) -- node[right, font=\tiny] {<<include>>} (connecter);
  
\end{tikzpicture}
\end{center}

\textbf{Description des acteurs :}
\begin{itemize}
  \item \textbf{Visiteur} : Utilisateur non authentifié pouvant consulter le catalogue public.
  \item \textbf{Client} : Membre authentifié pouvant réserver des livres (hérite de Visiteur).
  \item \textbf{Administrateur} : Gestionnaire du système avec accès complet.
\end{itemize}

\newpage
\subsection{Diagramme de classes}

Le diagramme de classes présente la structure statique du système avec les entités principales et leurs relations.

\begin{center}
\begin{tikzpicture}[
  scale=0.7,
  every node/.style={transform shape},
  class/.style={
    rectangle split, 
    rectangle split parts=3,
    draw,
    thick,
    minimum width=4cm,
    text width=3.8cm,
    align=left,
    font=\small
  },
  classname/.style={
    fill=lightblue,
    font=\bfseries
  },
  arrow/.style={-{Stealth}, thick},
  depend/.style={-{Stealth}, dashed, thick},
  assoc/.style={thick}
]

  % User
  \node[class] (user) at (0, 0) {
    \nodepart{one} \centerline{\textbf{User}}
    \nodepart{two}
      - Id: string\\
      - UserId: int\\
      - Username: string\\
      - Email: string\\
      - Password: string\\
      - Role: string\\
      - RegistrationDate: DateTime
    \nodepart{three}
      + Authenticate(): bool\\
      + IsAdmin(): bool
  };
  
  % Book
  \node[class] (book) at (7, 0) {
    \nodepart{one} \centerline{\textbf{Book}}
    \nodepart{two}
      - Id: string\\
      - BookId: int\\
      - Title: string\\
      - Author: string\\
      - ISBN: string\\
      - CategoryId: string\\
      - CoverImageUrl: string\\
      - IsFeatured: bool\\
      - PublicationYear: int
    \nodepart{three}
      + GetDisplayInfo(): string
  };
  
  % Category
  \node[class] (category) at (14, 0) {
    \nodepart{one} \centerline{\textbf{Category}}
    \nodepart{two}
      - Id: string\\
      - Name: string\\
      - Description: string\\
      - Icon: string
    \nodepart{three}
      + GetBookCount(): int
  };
  
  % Booking
  \node[class] (booking) at (3.5, -8) {
    \nodepart{one} \centerline{\textbf{Booking}}
    \nodepart{two}
      - Id: string\\
      - BookingId: int\\
      - UserId: string\\
      - BookId: string\\
      - Status: BookingStatus\\
      - RequestedAt: DateTime\\
      - ApprovedAt: DateTime?\\
      - DueDate: DateTime?\\
      - ReturnedAt: DateTime?
    \nodepart{three}
      + IsOverdue(): bool\\
      + Approve(dueDate): void\\
      + Refuse(): void\\
      + Complete(): void
  };
  
  % BookingStatus Enum
  \node[class, minimum width=3cm] (status) at (10.5, -8) {
    \nodepart{one} \centerline{\textbf{<<enumeration>>}}
    \centerline{\textbf{BookingStatus}}
    \nodepart{two}
      Pending\\
      Approved\\
      Refused\\
      Completed
    \nodepart{three}
  };
  
  % Services
  \node[class, fill=green!10] (userservice) at (0, -15) {
    \nodepart{one} \centerline{\textbf{UserService}}
    \nodepart{two}
      - \_users: List<User>\\
      - \_mutex: object
    \nodepart{three}
      + Register(user): User\\
      + Authenticate(email, pwd): User\\
      + GetMembers(): IEnumerable<User>
  };
  
  \node[class, fill=green!10] (bookservice) at (7, -15) {
    \nodepart{one} \centerline{\textbf{BookService}}
    \nodepart{two}
      - \_books: List<Book>\\
      - \_mutex: object
    \nodepart{three}
      + GetAll(): IEnumerable<Book>\\
      + GetFeatured(n): IEnumerable<Book>\\
      + GetByCategory(id): IEnumerable<Book>\\
      + Search(term): IEnumerable<Book>\\
      + Create(book): void\\
      + Update(book): void\\
      + Delete(id): void
  };
  
  \node[class, fill=green!10] (bookingservice) at (14, -15) {
    \nodepart{one} \centerline{\textbf{BookingService}}
    \nodepart{two}
      - \_bookings: List<Booking>\\
      - \_mutex: object
    \nodepart{three}
      + CreateBooking(booking): Booking\\
      + ApproveBooking(id, date): bool\\
      + RefuseBooking(id): bool\\
      + MarkReturned(id): bool\\
      + GetForUser(userId): IEnumerable\\
      + GetOverdue(): IEnumerable
  };
  
  % Associations
  \draw[assoc] (user) -- node[above, sloped, font=\tiny] {1} 
                        node[below, sloped, font=\tiny] {0..*} 
                        (booking);
  \draw[assoc] (book) -- node[above, sloped, font=\tiny] {1} 
                        node[below, sloped, font=\tiny] {0..*} 
                        (booking);
  \draw[assoc] (category) -- node[above, sloped, font=\tiny] {1} 
                            node[below, sloped, font=\tiny] {0..*} 
                            (book);
  \draw[assoc] (booking) -- (status);
  
  % Dependencies (services -> models)
  \draw[depend] (userservice) -- (user);
  \draw[depend] (bookservice) -- (book);
  \draw[depend] (bookingservice) -- (booking);
  \draw[depend] (bookservice.east) -- ++(1,0) |- (category.south);
  
\end{tikzpicture}
\end{center}

\textbf{Relations principales :}
\begin{itemize}
  \item Un \textbf{User} peut avoir plusieurs \textbf{Bookings} (0..*).
  \item Un \textbf{Book} peut être associé à plusieurs \textbf{Bookings} (0..*).
  \item Un \textbf{Book} appartient à une seule \textbf{Category} (1).
  \item Un \textbf{Booking} possède un état défini par l'énumération \textbf{BookingStatus}.
  \item Les services dépendent des entités du domaine (relation de dépendance).
\end{itemize}

\newpage
\subsection{Diagramme de séquence : Réservation d'un livre}

Ce diagramme illustre le scénario complet de réservation d'un livre par un client.

\begin{center}
\begin{tikzpicture}[
  scale=0.75,
  every node/.style={transform shape},
  actor/.style={rectangle, draw, minimum width=1.5cm, minimum height=0.8cm, fill=yellow!20, thick},
  object/.style={rectangle, draw, minimum width=2.2cm, minimum height=0.8cm, fill=lightblue, thick},
  arrow/.style={-{Stealth}, thick},
  return/.style={-{Stealth}, dashed, thick},
  note/.style={font=\tiny}
]
  
  % Objets
  \node[actor] (client) at (0, 0) {Client};
  \node[object] (controller) at (3, 0) {ClientController};
  \node[object] (bookingserv) at (6.5, 0) {BookingService};
  \node[object] (bookserv) at (10, 0) {BookService};
  \node[object] (userserv) at (13.5, 0) {UserService};
  
  % Lignes de vie
  \draw[dashed] (client.south) -- ++(0, -16);
  \draw[dashed] (controller.south) -- ++(0, -16);
  \draw[dashed] (bookingserv.south) -- ++(0, -16);
  \draw[dashed] (bookserv.south) -- ++(0, -16);
  \draw[dashed] (userserv.south) -- ++(0, -16);
  
  % Messages
  \draw[arrow] (0, -1) -- node[above, note] {1: POST /Client/Reserve(bookId)} (3, -1);
  
  \draw[arrow] (3, -2) -- node[above, note] {2: GetString("UserId")} (3, -2.5);
  \draw[return] (3, -2.5) -- node[below, note] {userId} (3, -3);
  
  \draw[arrow] (3, -3.5) -- node[above, note] {3: GetById(bookId)} (10, -3.5);
  \draw[return] (10, -4) -- node[below, note] {book} (3, -4);
  
  \draw[arrow] (3, -4.5) -- node[above, note] {4: GetById(userId)} (13.5, -4.5);
  \draw[return] (13.5, -5) -- node[below, note] {user} (3, -5);
  
  \draw[arrow] (3, -6) -- node[above, note] {5: GetForUser(userId)} (6.5, -6);
  \draw[return] (6.5, -6.5) -- node[below, note] {existingBookings} (3, -6.5);
  
  \node[draw, rectangle, minimum width=2cm, minimum height=0.6cm, fill=lightyellow] at (3, -7.5) {\tiny Vérifier conflit};
  
  \draw[arrow] (3, -8.5) -- node[above, note] {6: CreateBooking(newBooking)} (6.5, -8.5);
  
  \node[draw, rectangle, minimum width=2cm, minimum height=0.6cm, fill=lightyellow] at (6.5, -9.5) {\tiny Ajouter à la liste};
  
  \draw[return] (6.5, -10.5) -- node[below, note] {booking} (3, -10.5);
  
  \draw[arrow] (3, -11.5) -- node[above, note] {7: TempData["Success"]} (3, -12);
  
  \draw[arrow] (3, -13) -- node[above, note] {8: RedirectToAction("MyBookings")} (0, -13);
  
  \draw[arrow] (0, -14) -- node[above, note] {9: GET /Client/MyBookings} (3, -14);
  
  \draw[return] (3, -15) -- node[below, note] {View(bookings)} (0, -15);
  
\end{tikzpicture}
\end{center}

\textbf{Description du flux :}
\begin{enumerate}
  \item Le client soumet une demande de réservation via un formulaire POST.
  \item Le contrôleur récupère l'identifiant utilisateur depuis la session.
  \item Le contrôleur vérifie que le livre existe via \texttt{BookService}.
  \item Le contrôleur vérifie que l'utilisateur existe via \texttt{UserService}.
  \item Le contrôleur vérifie les réservations existantes de l'utilisateur pour éviter les doublons.
  \item Si aucun conflit, une nouvelle réservation est créée avec statut \textit{Pending}.
  \item Un message de succès est enregistré dans \texttt{TempData}.
  \item Redirection vers la page "Mes Réservations".
  \item Affichage de toutes les réservations du client (Pending, Approved, History).
\end{enumerate}

\newpage
% =============================================================================
\section{Implémentation Technique}
% =============================================================================

\subsection{Gestion de la concurrence}

Chaque service utilise un verrou (\texttt{lock}) pour garantir la thread-safety lors des opérations de lecture/écriture sur les collections en mémoire :

\begin{lstlisting}
public class BookingService
{
    private readonly List<Booking> _bookings = new();
    private readonly object _mutex = new();
    
    public Booking CreateBooking(Booking booking)
    {
        lock (_mutex)
        {
            booking.Id = Guid.NewGuid().ToString();
            booking.Status = BookingStatus.Pending;
            booking.RequestedAt = DateTime.UtcNow;
            _bookings.Add(booking);
            return booking;
        }
    }
}
\end{lstlisting}

\subsection{Authentification par session}

L'authentification utilise le système de sessions ASP.NET Core avec timeout de 30 minutes :

\begin{lstlisting}
// Program.cs - Configuration
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// AccountController.cs - Login
HttpContext.Session.SetString("Username", user.Username);
HttpContext.Session.SetString("Email", user.Email);
HttpContext.Session.SetString("Role", user.Role);
HttpContext.Session.SetString("UserId", user.Id);
\end{lstlisting}

\subsection{Contrôle d'accès basé sur les rôles}

Chaque contrôleur implémente des méthodes de garde pour vérifier les autorisations :

\begin{lstlisting}
public class AdminController : Controller
{
    private IActionResult? EnsureAdmin()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Admin")
            return RedirectToAction("Login", "Account");
        return null;
    }
    
    public IActionResult Index()
    {
        var redirect = EnsureAdmin();
        if (redirect is not null) return redirect;
        
        // Logic for admin dashboard...
    }
}
\end{lstlisting}

\subsection{Affichage en grille responsive}

L'interface utilise Bootstrap 5 pour un affichage en grille responsive des livres :

\begin{lstlisting}[language=HTML]
<div class="row">
    @foreach (var book in Model)
    {
        <div class="col-md-3 mb-4">
            <div class="book-card">
                <div class="book-cover">
                    <img src="@book.CoverImageUrl" 
                         alt="@book.Title" />
                </div>
                <h5>@book.Title</h5>
                <p class="text-muted">@book.Author</p>
                <a href="/Client/Reserve/@book.Id" 
                   class="btn btn-primary">Reserver</a>
            </div>
        </div>
    }
</div>
\end{lstlisting}

% =============================================================================
\section{Fonctionnalités Avancées}
% =============================================================================

\subsection{Détection des retards}

Le système détecte automatiquement les réservations en retard :

\begin{lstlisting}
public IEnumerable<Booking> GetOverdue()
{
    lock (_mutex)
    {
        return _bookings
            .Where(b => b.Status == BookingStatus.Approved 
                     && b.DueDate.HasValue 
                     && b.DueDate.Value < DateTime.UtcNow)
            .ToList();
    }
}
\end{lstlisting}

\subsection{Système de filtrage et recherche}

L'interface administrateur offre des filtres multiples pour l'historique :

\begin{itemize}
  \item Filtrage par statut (Tous, Pending, Approved, Refused, Completed)
  \item Recherche par titre de livre ou nom d'utilisateur
  \item Alertes visuelles pour les retards (badge rouge)
\end{itemize}




\newpage
% =============================================================================
\section{Tests et Validation}
% =============================================================================

\subsection{Scénarios de test}

\begin{enumerate}
  \item \textbf{Test d'authentification}
  \begin{itemize}
    \item Login avec identifiants valides (admin@lumenlibrary.com / admin123)
    \item Login avec identifiants invalides
    \item Vérification de la redirection selon le rôle
    \item Timeout de session après 30 minutes
  \end{itemize}
  
  \item \textbf{Test du workflow de réservation}
  \begin{itemize}
    \item Client réserve un livre disponible $\rightarrow$ Statut Pending
    \item Admin approuve avec date limite $\rightarrow$ Statut Approved
    \item Admin refuse la demande $\rightarrow$ Statut Refused
    \item Admin marque retour $\rightarrow$ Statut Completed
  \end{itemize}
  
  \item \textbf{Test de sécurité}
  \begin{itemize}
    \item Accès direct à /Admin/Index sans authentification $\rightarrow$ Redirection
    \item Client tente d'accéder à /Admin/* $\rightarrow$ Refusé
    \item Validation anti-CSRF sur tous les formulaires POST
  \end{itemize}
  
  \item \textbf{Test de recherche et filtrage}
  \begin{itemize}
    \item Recherche par titre exact
    \item Recherche partielle (insensible à la casse)
    \item Filtrage par catégorie
    \item Combinaison recherche + filtre
  \end{itemize}
\end{enumerate}

\subsection{Comptes de test}

\begin{table}[h]
\centering
\begin{tabular}{@{}lll@{}}
\toprule
\textbf{Rôle} & \textbf{Email} & \textbf{Mot de passe} \\
\midrule
Administrateur & admin@lumenlibrary.com & admin123 \\
Client & maya@readers.com & reader123 \\
Client & leo@readers.com & reader123 \\
\bottomrule
\end{tabular}
\caption{Comptes préchargés pour tests}
\end{table}

\newpage
% =============================================================================
\section{Perspectives d'Amélioration}
% =============================================================================

\subsection{Améliorations techniques}

\begin{enumerate}
  \item \textbf{Persistance des données}
  \begin{itemize}
    \item Migration vers une base de données réelle (SQL Server, PostgreSQL, MongoDB)
    \item Implémentation du pattern Repository
    \item Entity Framework Core pour l'ORM
  \end{itemize}
  
  \item \textbf{Sécurité renforcée}
  \begin{itemize}
    \item Hachage des mots de passe avec BCrypt ou Argon2
    \item Authentification ASP.NET Core Identity
    \item JWT pour API REST
  \end{itemize}
  
  \item \textbf{Performance}
  \begin{itemize}
    \item Pagination des résultats
    \item Lazy loading des images
    \item Compression des réponses HTTP
  \end{itemize}
\end{enumerate}

\subsection{Fonctionnalités futures}

\begin{enumerate}
  \item \textbf{Notifications}
  \begin{itemize}
    \item Email lors de l'approbation d'une réservation
    \item Alertes de retour imminent (3 jours avant échéance)
    \item Notifications push pour les clients
  \end{itemize}
  
  \item \textbf{Statistiques avancées}
  \begin{itemize}
    \item Tableau de bord avec graphiques (Chart.js)
    \item Livres les plus empruntés
    \item Tendances de réservation par catégorie
    \item Export PDF/Excel des rapports
  \end{itemize}
  
  \item \textbf{Gestion des amendes}
  \begin{itemize}
    \item Calcul automatique des pénalités de retard
    \item Historique des paiements
    \item Blocage automatique des comptes en défaut
  \end{itemize}
  
  \item \textbf{Système de recommandation}
  \begin{itemize}
    \item Suggestions basées sur l'historique de lecture
    \item Livres similaires (même catégorie, même auteur)
    \item Système de notation et avis
  \end{itemize}
  
  \item \textbf{API REST}
  \begin{itemize}
    \item Endpoints pour application mobile
    \item Documentation Swagger/OpenAPI
    \item Authentification OAuth 2.0
  \end{itemize}
\end{enumerate}

\newpage
% =============================================================================
\section{Conclusion}
% =============================================================================

Ce projet a permis de développer un système de gestion de bibliothèque fonctionnel et complet, démontrant la maîtrise de plusieurs technologies et concepts clés du développement web moderne.

\subsection{Acquis techniques}

Les principaux acquis de ce projet sont :

\begin{itemize}
  \item \textbf{Architecture MVC} : Compréhension approfondie du pattern Modèle-Vue-Contrôleur avec ASP.NET Core, séparation claire des responsabilités entre les couches.
  
  \item \textbf{Gestion d'état} : Implémentation d'un système d'authentification basé sur les sessions avec contrôle d'accès par rôles (RBAC).
  
  \item \textbf{Programmation orientée objet} : Application des principes SOLID, injection de dépendances, pattern Singleton pour les services.
  
  \item \textbf{Conception UML} : Modélisation complète du système avec diagrammes de cas d'utilisation, de classes et de séquence conformes aux normes UML 2.5.
  
  \item \textbf{Interface utilisateur} : Création d'une interface responsive moderne avec Bootstrap 5, système de grille, et design inspiré d'Apple.
  
  \item \textbf{Gestion de la concurrence} : Utilisation de verrous (locks) pour garantir la thread-safety des opérations sur les données partagées.
\end{itemize}

\subsection{Méthodologie de développement}

Le projet a suivi une approche itérative :
\begin{enumerate}
  \item Définition des besoins et modélisation UML
  \item Mise en place de l'architecture de base (Models, Services)
  \item Implémentation des contrôleurs et logique métier
  \item Développement des vues Razor avec Bootstrap
  \item Tests manuels et ajustements
  \item Ajout de fonctionnalités avancées (upload images, filtres, détection retards)
  \item Refactoring et documentation du code
\end{enumerate}

\subsection{Bilan}

Le système répond aux objectifs initiaux en offrant :
\begin{itemize}
  \item Une plateforme complète de gestion de bibliothèque
  \item Un workflow de réservation robuste avec états multiples
  \item Une interface intuitive pour clients et administrateurs
  \item Un code maintenable et bien documenté
  \item Des performances excellentes grâce au stockage en mémoire
\end{itemize}

Ce projet constitue une base solide pouvant évoluer vers un système de production en intégrant les améliorations suggérées (base de données persistante, sécurité renforcée, API REST, notifications).

\vspace{1cm}

\begin{center}
\textit{« Une bibliothèque est une liberté. » --- Ursula K. Le Guin}
\end{center}

\newpage
% =============================================================================
\section{Bibliographie}
% =============================================================================

\begin{thebibliography}{99}

\bibitem{aspnetcore}
Microsoft. (2024). \textit{ASP.NET Core Documentation}. 
\texttt{https://docs.microsoft.com/aspnet/core}

\bibitem{csharp}
Microsoft. (2024). \textit{C\# Programming Guide}. 
\texttt{https://docs.microsoft.com/dotnet/csharp}

\bibitem{mvc}
Fowler, M. (2006). \textit{Patterns of Enterprise Application Architecture}. 
Addison-Wesley Professional.

\bibitem{uml}
Object Management Group. (2017). \textit{Unified Modeling Language (UML) Version 2.5.1}. 
\texttt{https://www.omg.org/spec/UML/2.5.1}

\bibitem{bootstrap}
Bootstrap Team. (2024). \textit{Bootstrap 5 Documentation}. 
\texttt{https://getbootstrap.com/docs/5.3}

\bibitem{razor}
Microsoft. (2024). \textit{Razor Pages in ASP.NET Core}. 
\texttt{https://docs.microsoft.com/aspnet/core/razor-pages}

\bibitem{dependencyinjection}
Freeman, A. (2022). \textit{Pro ASP.NET Core 6: Develop Cloud-Ready Web Applications}. 
Apress, 9th Edition.

\bibitem{threadsafety}
Albahari, J., \& Albahari, B. (2022). \textit{C\# 10 in a Nutshell: The Definitive Reference}. 
O'Reilly Media.

\bibitem{webdesign}
Duckett, J. (2011). \textit{HTML and CSS: Design and Build Websites}. 
Wiley.

\bibitem{softwarearchitecture}
Martin, R. C. (2017). \textit{Clean Architecture: A Craftsman's Guide to Software Structure and Design}. 
Prentice Hall.

\bibitem{designpatterns}
Gamma, E., Helm, R., Johnson, R., \& Vlissides, J. (1994). 
\textit{Design Patterns: Elements of Reusable Object-Oriented Software}. 
Addison-Wesley Professional.

\bibitem{restapi}
Fielding, R. T. (2000). \textit{Architectural Styles and the Design of Network-based Software Architectures}. 
Doctoral dissertation, University of California, Irvine.

\bibitem{authentication}
Microsoft. (2024). \textit{ASP.NET Core Security and Identity}. 
\texttt{https://docs.microsoft.com/aspnet/core/security}

\bibitem{entityframework}
Microsoft. (2024). \textit{Entity Framework Core Documentation}. 
\texttt{https://docs.microsoft.com/ef/core}

\bibitem{responsive}
Marcotte, E. (2011). \textit{Responsive Web Design}. 
A Book Apart.

\bibitem{agile}
Beck, K., et al. (2001). \textit{Manifesto for Agile Software Development}. 
\texttt{https://agilemanifesto.org}

\bibitem{solid}
Martin, R. C. (2000). \textit{Design Principles and Design Patterns}. 
Object Mentor.

\bibitem{sessionmanagement}
OWASP. (2024). \textit{Session Management Cheat Sheet}. 
\texttt{https://cheatsheetseries.owasp.org/cheatsheets/Session\_Management\_Cheat\_Sheet.html}

\end{thebibliography}

\vfill

\textbf{Fin du rapport}

\end{document}
```
