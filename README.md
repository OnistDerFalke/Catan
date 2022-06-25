# Catan
Projekt inżynierski dyplomowy.

<b>OnistDerFalke DevLog</b>:

<b>25 Czerwiec 2022</b>
* Zmieniłem kolejność surowców w popupach monopolu i wynalazku na taką, jak jest w menu dolnym.
* Teraz w okienku invention przyciski plusa i minusa nie są zupełnie widoczne gdy nie można ich użyć, dzięki czemu gracz widzi jedynie dostępne opcje do naciśnięcia. Wydawało mi się to najlepsze rozwiązanie
i jest bardzo czytelne według mnie. Wątek <span style="color: purple"><i>issue #45</i></span> zamykam.
* Dla <span style="color: purple"><i>issue #21</i></span> dodałem to, że jak nie można użyć karty to totalnie nie można jej kliknąć. Nie wiem czy do końca o to ci chodziło więc <b>zostawiam ten wątek otwardy do sprawdzenia</b>
i jeśli będzie git to możesz go zamknąć, a jeśli nie to daj mi znać.

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
* Dla <span style="color: purple"><i>issue #44</i></span> wywołuję metodę <span style="color: green"><b><i>MonopolCardEffect</b></i></span> po zaakceptowaniu wybranego surowca. Dałabym tutaj kolejność surowców taką jaka jest w panelu użytkownika: drewno, glina, zboże, wełna, ruda żelaza, bo jest to trochę mylące teraz.
* Dla <span style="color: purple"><i>issue #45</i></span> wywołuję metodę <span style="color: green"><b><i>InventionCardEffect</b></i></span> po zaakceptowaniu wybranych surowców. Podobnie jak dla monopolu - dałabym tutaj kolejność surowców taką jaka jest w panelu użytkownika: drewno, glina, zboże, wełna, ruda żelaza, bo jest to trochę mylące teraz.
* Dla <span style="color: purple"><i>issue #43</i></span> poprawiłam działanie budowania darmowej drogi i aktywność przycisku budowania.
* Dla <span style="color: purple"><i>issue #73</i></span> utworzyłam klasę <span style="color: green"><b><i>Ports</b></i></span> oraz <span style="color: green"><b><i>PortDetails</b></i></span>, które przechowują informację o szczegółach handlu morskiego dla różnych portów. Gracz posiada zmienną ports, która posiada słownik portów (PortDetails -> bool), których wartości boolowskie oznaczają to czy dany rodzaj handlu morskiego jest dostępny dla gracza czy nie. W klasie PortDetails znajduje się zmienna <span style="color: green"><b><i>exchangeForOneResource</b></i></span>, które gracz musi oddać, aby otrzymać jeden surowiec wybranego rodzaju.
* Dla <span style="color: purple"><i>issue #59</i></span> stworzyłam metodę liczącą liczbę surowców, które można kupić na podstawie tych, które zostały wybrane do tej pory przez gracza oraz liczbę surowców, w których posiadaniu jest gracz, a które zostały wybrane nadmiarowo przez gracza. Działanie metody należy przetestować gdy będzie interfejs. Metoda ta to **CountTradeResources**, na razie jest w GameManager.
  



