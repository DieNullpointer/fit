# Spengergasse FIT

Klone das Repository mit folgendem Befehl :

```
git clone https://github.com/DieNullpointer/fit.git
```

## Kurzbeschreibung

## Teammitglieder

| Name                        | Email                    | Aufgabenbereich |
| --------------------------- | ------------------------ | --------------- |
| Bastian _Seidl_, 3CHIF      | sei20375@spengergasse.at | -               |
| Maximilian _Schwarz_, 3CHIF | sch22834@spengergasse.at | -               |
| Philip _Schrenk_, 3CHIF     | sch22538@spengergasse.at | -               |
| Louis _Muhr_, 3CHIF         | muh22378@spengergasse.at | -               |

## Voraussetzungen

Das Projekt verwendet .NET in der Version 6 sowie Docker. Prüfe mit folgendem Befehlen, ob die .NET
SDK in der Version 6 oder 7 oder Docker am Rechner installiert ist:

```
dotnet --version
docker --version
```

## Start der App

Führe das Startskript aus, um den Container anzulegen und die ASP.NET Core Webapi zu starten.

**Windows**

```
startServer.cmd
```

**macOS, Linux**

```
chmod a+x startServer.sh
./startServer.sh
```

![](./spengerlogo.svg)

### Projektbeschreibung

PROJEKT FIT:

- einladung mit link auf seite - email
- anmelden linux server: docker
- willkommenschrift mit fit name
- anmelden: firmenname, firmenadresse, landfeld, plz, rechnungsanschrift, gleich-wie obrige adresse checkbox - einfach copy,
  ansprechpartenr 1:n - mehrere : titel; VN; NN; Telnr; moblNr; email; funktion; checkbox: hauptansprechpartner - MUSS,
- uploadfelder mit pdf für unterschriebene vereibarung, MB begrenzung < 30MB, mehrere uploads mit attatchments möglich, kommt noch mehr
- auswahl der pakete: konfigurierbar als admin, nur eins zum auswählen,
  paket: name; preis; zuordnung zum fit 1:n;
  aus adminsicht bei jedem paketfeld sichtbar ja/nein auswählen für anmelder
- bei durchführung bekommen alle ansprechpartenr anmeldebestätigung
- admin seite: history anschaubar, neuer fit anlegen: datum; name;
- datenhaltung braucht schnittstelle nach außen (zugriff für herr vogl)
- adminseite nicht vergessen (vorerst ohne ad von schule)
- design der spengergasse

zusammenfassung:
fits erstellen, zuordnen, anmelden, + nächste kommende veranstaltung sehen
DOCS nicht vergessen
fit.spengergasse.at bestehende seit
