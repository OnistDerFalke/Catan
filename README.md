# Catan
Projekt inżynierski dyplomowy.

<b>OnistDerFalke DevLog</b>:

<b>24 Czerwiec 2022</b>
* Dla <span style="color: purple"><i>issue #36</i></span> dodałem funkcjonalność wyboru karty poprzez kliknięcie na nią, a następnie przycisku użycia karty.
Wszystkie zmiany zachodzą w skrypcie CardsContentNavigation. Tam też znajduje się metoda <span style="color: green"><b><i>OnCardUseButtonClick</b></i></span>, w której switch pozwala
na wywołanie danego eventu w zależności od wybranej karty.
* Poblokowałem też możliwość wciśnięcia przycisku użycia karty w sytuacji gdy jest ona oznaczona jako zablokowana lub gdy gracz 
posiada mniej niż jedną kartę. Przycisk jest blokowany na starcie, po wybraniu karty - karta jest przypisywana do wybranej, ale przycisk
odblokuje się dopiero gdy zostaną spełnione powyższe warunki. Cały ten if siedzi w metodzie <span style="color: green"><b><i>ChooseCardButton</b></i></span> i tam jest ładnie to opisane
jeszcze w komentarzach.
* Dla <span style="color: purple"><i>issue #21</i></span> dodałem przyciski rozpoczęcia i końca handlu. W skrypcie ActionsContentNavigation
dodałem metodę <span style="color: green"><b><i>ManageButtonGrid</b></i></span> wywoływaną na początku, która w zależności od trybu rozgrywki chowa nadmiarowe przyciski
oraz modyfikuje UI (być może w przyszłości coś więcej). Zatem jeśli wybrano tryb zaawansowany - przycisk zakończenia handlu nie pojawia się, a co więcej - pozostałe przyciski przesuwane są
w dół aby zapobiec przerwie. Oba przyciski mają podpięte metody w ActionsContentNavigation (<span style="color: green"><b><i>OnTradeButton, OnEndTradeButton</b></i></span>). Metody te trzeba zaimplementować.
* Dla <span style="color: purple"><i>issue #21</i></span> dodałem pod przyciskiem budowania informacje o limitach budowli. Odpowiada za to skrypt BuildingsLimit. W nim w update dodałem aktualizowanie tekstów
i podstawiłem przykładowe liczby oraz limity zadeklarowałem lokalnie. Trzeba te limity skądś pobierać (np. GameManagera), więc wystarczy to tylko podłączyć i wszystko będzie działać. Ogólnie
zrobiłbym to i zliczał elementy listy po prostu, ale wiem, że będziesz coś kombinowała z blokowaniem przycisku gdy limit osiągnięto (albo nawet już to zrobiłaś), więc nie chcę wchodzić w drogę.
Jak zrobisz to po swojemu to tylko się to podłączy do tego skryptu i będzie wyświetlać.
* Dla <span style="color: purple"><i>issue #44</i></span> dodałem to okienko, odpala się je zmieniając zmienną MonopolPopupShown na true w GameManagerze (analogicznie będą działały inne przyszłe popupy).
Można wybierać surowiec, dopiero po wybraniu odblokowuje się możliwość zatwierdzenia. Przy najechaniu na przycisk - lekko się zwiększa. Jeśli wybierzemy surowiec - zwiększa się on znacznie.
Po zatwierdzeniu przycisk się blokuje a okno znika. W metodzie <span style="color: green"><b><i>OnConfirmButton</b></i></span> jest TODO i miejsce na przekazanie wyboru do jakiejś zmiennej np. w GameManagerze.
* Dla <span style="color: purple"><i>issue #45</i></span> dodałem okienko, w którym mozna wybrać (z ograniczniem do 2) surowce i przy spełnieniu warunku zatwierdzić. Wówczas okienko znika. Nie
wyróżniłem jeszcze nieaktywnych przycisków, ale w następnym kroku to zrobie. Oprócz tego wszystko jest. Zmienne <surowiec>Value pokazują jakie i w jakiej liczbie surowce wybrał gracz. W metodzie
<span style="color: green"><b><i>OnConfirmButton</b></i></span> jest miejsce, żeby sobie przerzucić gdzieś to info z tym co wybrano (np. GameManager).


<b>KingaW20 DevLog</b>:

<b>24 Czerwiec 2022</b>
* Dla <span style="color: purple"><i>issue #21</i></span> podstawiłam poprawne wartości w wyświetlanych numerach wybudowanych liczb przez danego gracza i zamknęłam issue.
* Dla <span style="color: purple"><i>issue #36</i></span> podpięłam wywołania odpowiednich funkcji w zależności od wybranej karty (switch znajduje się w metodzie <span style="color: green"><b><i>UseCard</b></i></span> w klasie Player)



