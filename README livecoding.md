# kata-LegacyTrain

Kata on how to refactor a typical legacy code base (directly inspired
by [Emily Bache's kata](https://github.com/emilybache/KataTrainReservation).

Too many projects have layered-based...

## Contexte:

SSII a gagn� un appel d'offre pour mise en oeuvre rapide d'un logiciel de r�servation de si�ges dans les trains.
Apr�s avoir d�velopp� une premi�re verison de l'appli, la SSII a continu�e a faire evoluer le syst�me jusqu'� arriver �
une situation de blocage: le client demande une modification de l'algoithme de reservation ce qui semble impossible � la
SSII (qui plus est, � perdu entre-temps tous ses d�veloppeurs partis faire autre chose de plus int�ressant).
La SSII a jet�e l'�ponge en produisant un avenant/devis hors de prix pour le client qui nous sollicite pour "reprendre
le dossier".

Nous arrivons donc sur une code base assez moche, pour laquelle nous n'avons plus de d�veloppeurs pour nous expliquer
leurs intentions initiales et justifier de leurs choix. Heureusement pour nous, le client mets � notre disposition un
expert du domaine pendant 3 heures pour r�pondre � nos questions.

On est assez inquiet par la difficult� potentielle, mais comme on est joeur on a accept� cette mission mais on compte
sur vous pour nous aider.

La nouvelle feature est de supporter une autre fa�on de reserver des places de trains pour un autre grand distributeur
qui n'aime pas du tout notre format JSON de retour => on va donc lui exposer un nouveau entry point pour supporter son
besoin.
TrainTrainCorp se rends compte qu'avec l'arriv�e de ce clients et les volumes �normes corrspondant attendus => mettre �
jour l'algo de r�servation en introduisant une nouvelle r�gle : "Dans l'id�al, ne pas charger les voitures du train �
plus de 70% de leur capacit�."

Couplet sur le contexte organisationel :

- On a externalis� le TrainDataService pour des questions de scalabilit�
- On a externalis� le BookingReferenceService pour r�pondre � une contrainte r�glementaire Europ�ene (trouver un truc
  rigolo et absurde).

## Description de l'architecture

On a r�cup�r� le diagramme suivant qui a l'air d'etre � jour et qu'on vous commente.

Montrer dessin.

## Etapes

1. On rajoute des tests d'acceptance sur les uses cases intiaux (Pas plus de 70% du train et pas de reservation a cheval
   sur 2 voitures)
    - Surprise : la deuxieme regle (non chevauchement) n'est pas impl�ment�e... Discussion avec l'expert qui n'en croit
      pas ses yeux.
    - On propose au client d'impl�menter cette r�gle en m�me temps que la nouvelle feature (la r�gle sera valable
      quelque soit les modalit�s de r�servation (historique et nouveau).

2. On rajoute 1 test d'acceptance sur la nouvelle feature -> nouveau format de retour sur scenarii avec moins de 70% de
   charge par voiture

3. On fait �merger le concept de Coach qui n'existait pas dans le code existant
4. On s�pare bien le code du domaine (plus du tout an�mique) avec le code technique
5. On extrait le format de s�rialization du domaine m�tier en le situant dans des adapteurs � la p�riph�rie du syst�me (
   archi hexagonale FTW)

En route :

- on se sera d�barrass� le l'�tat des trains persist�s/cach�s dans le service TrainTrain (� tord, car d'autres gens
  peuvent modifier les reservations des trains via le TrainData service)
- on se sera d�barrass� de l'ORM

Suite � notre refactoring, on peut commencer � avoir des discussions int�ressantes avec le m�tier sur un autre probl�me
qui le taraude: la modification de la topologies des trains � post�riori qui �tait probl�matique avant a cause du
caching des trains du service TrainTrain.

T-Shirts:

- le client � toujours raison (bleu acier)
- l'�quipe du Train-Train (rose p�tant)

sklmk
