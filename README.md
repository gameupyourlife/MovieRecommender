# Informatik Movie Recommender
## Idee

Als der ursprüngliche Plan mit einem Lüftercontroller auf Grund der Architektur von Windows ins Wasser fiel, entschied ich mich für eine Movie Recommender da ich darüber einen Artikel gelesen hatte und Lust auf so ein Projekt bekam.

## Ziel

Das klare Ziel meinerseits war es einen Prototypen in der Konsole mit C# zum laufen zu bekommen, welcher einem User Filme empfiehlt und den User Filme bewerten lassen kann, alles mit einem User Management System.

## Verlauf
Um das Neuronale Netzwerk zu erstellen habe ich mich für die ML.NET Libary entschieden, da diese eine ausgezeichnete Dokumentation besitzt und einfach zu integrieren ist.
Das MLM konnte ich dank der Dokumentation in kurzer Zeit erstellen und in einen lauffähigen Zustand bringen.
Die Trainingsdaten habe ich mit Hilfe einer extra dafür eigens geschriebenen Funktion zufällig erstellen lassen. Diese sollten provisorisch für den Prototypen dienen, da dieser eher als proof of concept dienen soll anstelle einer voll funktionsfähigen und akkuraten Releasversion.
Nachdem das Grundgerüst stand, ging es an die ersten Funktionen des Programms. 
Doch für jegliche Funktionalität muss ich Daten aus CSV Dateien auslesen und daten in CSV Dateien schreiben können. Hier nutze ich einen CSV Parser mit welchem man leicht CSV Dateien bearbeiten und auslesen kann.
Nachdem diese Funktionalität stand, versuchte ich mich an einem Top Ten System, welches auch soweit reibungslos funktionierte. 
Nun war es Zeit für das User Management System, welches das Herz des Programms darstellt. Dieses habe ich auf banalem Weg realisiert. Die Nutzer und Passwörter werden in einer ungesicherten CSV Datei gespeichert, welche bei dem Login ausgelesen und abgeglichen wird. Der eingeloggte Nutzer wird als Objekt gespeichert und somit im Programm durchgegeben. 
Auf dieser nun etablierten Basis funktionieren alle weiteren Funktionen, welche das Programm bietet.
Die Movie Recommend Funktion, also die Möglichkeit eines Nutzers gesehene Filme von 1-5 zu bewerten war die erste Funktion, welche nach Errichtung der Basis etabliert wurde. Sie ist essenziell für die Nutzbarkeit des Programmes, da ohne ihr das Programm keine Daten hat, von welchen es arbeiten kann. Diese Funktion basierte ursprünglich auf einfachen ReadLine Methoden. Die erfassten Werte wurden mit der Datenbank an Filmtiteln, welche ich aus dem Internet habe, verglichen und bei einem Treffer in eine CSV geschrieben.
Bei der Top Ten Funktion gab es in der ursprünglichen Verfassung ein Problem, welches mit dem CSV Parser zusammenhing. So las der CSV Parser die CSV lediglich einmal durch, weswegen die meisten der Ergebnisse nicht ausgegeben wurden, da ich den Abgleich immer ein nach dem andern durchführte und nicht alle parallel.
Bei der Löschung von Accounts, welche nicht fehlen darf, gab es ein weiteres Problem mit dem CSV Parser. Da es keine, mir bekannte, gute und einfach umsetzbare Möglichkeit in C# gibt einzelne Reihen aus einer CSV Datei zu löschen musste ich die ganze CSV einmal auslesen, und die Reihen, welche ich löschen möchte, überspringen. Die ausgelesenen Daten überschreiben dann die alten und löschen damit effektiv die übersprungenen Datensätze. Da ich aber vergaß das der Header nicht wie sonst üblich übersprungen werden kann, sondern ebenfalls ausgelesen und abgespeichert werden muss, wurde dies erst nicht getan und die erste Reihe wurde immer wieder gelöscht.
Nachdem dieses Problem gelöst wurde, indem die erste Zeile mitgelesen und gespeichert wurde, war das Programm soweit fertig, das visuelle Überarbeitungen an der Reihe standen.
Hierfür habe ich eine C# Libary gefunden welcher viele Möglichkeiten in der Console bietet. Mit Hilfe von Specter.Console konnte den Text Input welcher ich von dem User brauche vereinfachen durch das Text Promt Feature. Auch die Navigation lies sich durch das Selection Feature der Libary deutlich vereinfachen und verbessern. Des weiteren ermöglicht die Libary noch andere Graphische Verbesserung welche ich zum teil in das Programm einbauen konnte.

Mögliche Verbesserungen, welche man in der Zukunft anstreben könnte, sind das Integrieren von Multithreading um die Prozesse zu beschleunigen, die Verbesserung der Sicherheit der Account Daten durch z.B. das hashen der Passwörter sowie das Refactoring des Codes um diesen anschaulicher, übersichtlicher und wartbarer zu gestalten.
