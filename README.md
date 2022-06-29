# Catan
Projekt inżynierski dyplomowy.

<b>OnistDerFalke DevLog</b>:

<b>29 Czerwiec 2022</b>
* Dla <span style="color: purple"><i>issue #51</i></span> skończyłem cały interfejs wyboru gracza, który ma oddać surowce. Jest podłączony do przycisku przemieszczenia złodzieja tak, że wyświetla
się od razu po naciśnięciu. Przycisk potwierdzenia odblokowuje się dopiero po dokonaniu wyboru. Wybór gracza zwracany jest w postaci jego indeksu. Można go gdzieś przekazać na początku metody <b>OnConfirmButton w skrypcie ThiefPlayerChoicePopupController (znajduje się tam TODO, wartość jest w zmiennej chosenPlayerIndex)</b>.
Zauważyłem też (być może bug, ale nie jestem pewny), że w trybie zaawansowanym gdy uruchamiam test, nie wszyscy gracze wybierają surowiec do oddania (dobrze by to wyjaśnić, poczekam aż się do tego odniesiesz zanim
coś zacznę z tym robić, bo może tak ma być, a ja o czymś nie wiem).
* Dla <span style="color: purple"><i>issue #76</i></span> poprawiłem cennik.
* Dla <span style="color: purple"><i>issue #28</i></span> poprawiłem mechanizm działania UI i podświetlanie zakładek.


<b>27 Czerwiec 2022</b>
* Dla <span style="color: purple"><i>issue #47</i></span> dodałem info, że gracz, który oddaje surowce wie ile aktualnie ich ma. Dostepność wyświetla się poniżej i oznacza liczbę dostępnego surowca minus liczba tego surowca,
który już oddał (jeśli cofnie oddanie minusem do zera, to liczba będzie oznaczała ile ma tego surowca w magazynie).
* Dla <span style="color: purple"><i>issue #50</i></span> numerek nad złodziejem znika i nie można wysunąć w górę pola, na którym jest złodziej przy przestawianiu (testowałem to, ale warto by to jeszcze dobrze sprawdzić).
* Dla <span style="color: purple"><i>issue #50</i></span> częściowo zrobiłem okno wyboru sąsiadującego gracza - brakuje interakcji przycisków samego umiejscowienia wywołania tego. Wkrótce dodam.

<b>25 Czerwiec 2022</b>
* Zmieniłem kolejność surowców w popupach monopolu i wynalazku na taką, jak jest w menu dolnym.
* Teraz w okienku invention przyciski plusa i minusa nie są zupełnie widoczne gdy nie można ich użyć, dzięki czemu gracz widzi jedynie dostępne opcje do naciśnięcia. Wydawało mi się to najlepsze rozwiązanie
i jest bardzo czytelne według mnie. Wątek <span style="color: purple"><i>issue #45</i></span> zamykam.
* Dla <span style="color: purple"><i>issue #21</i></span> dodałem to, że jak nie można użyć karty to totalnie nie można jej kliknąć. Nie wiem czy do końca o to ci chodziło więc <b>zostawiam ten wątek otwarty do sprawdzenia</b>
i jeśli będzie git to możesz go zamknąć, a jeśli nie to daj mi znać.
* Dla <span style="color: purple"><i>issue #47</i></span> dodałem to okienko do wybierania nadmiarowych surowców. <b>Pytanie czy gracz, którego tura jest aktualnie też ma
oddawać surowce, czy nie?</b>. Plusy i minusy powinny działać git. Napisałem test <span style="color: green"><b><i>TestThiefPayPopup</b></i></span>, ktory można wywołać z klasy PopupWindowsController
(wysyłam zakomentowany, jak się odkomentuje to można przetestować okienko w sytuacji gdy był rycerz lub nie było rycerza). Musiałem przenieść odpowiedzialność na sam kontroler okna, bo klasa player nie
jest monobebechem, więc wywołanie musiałoby się odbywać przez asynchroniczne c# requesty, a async c# i unity za bardzo się nie lubią i to jest bardzo zły pomysł. Jak coś to pytaj jeśli czegoś nie dopowiedziałem,
bo to było trochę przenoszenia i zmian, więc może nie być takie oczywiste.
* Naprawiłem bug z właścicielem aktualnie zaznaczanego elementu (nie zmieniał się z powrotem na "Brak" po wybraniu neutralnego elementu).
* Dla <span style="color: purple"><i>issue #50</i></span> dodałem możliwość wyboru nowego pola dla złodzieja i naciśnięcie przycisku potwierdzającego wybór. <b>Nie jest zaimplementowane jeszcze przestawianie złodzieja</b>, a jedynie sama operacja
wyboru pola i zatwierdzenia. W fazie przemieszczania złodzieja klikalne są tylko pola i działają na tej samej zasadzie jak normalnie inne interaktywne elementy.

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

<b>29 Czerwiec 2022</b>
* _issue #51_: Dodałam funkcję w GameManagerze zwracają listę graczy, którzy sąsiadują z wybranym polem oraz po wyborze gracza dodałam losowanie surowca. Zamknęłam issue.
* _issue #47_: Przetestowałam i znalazłam dwa błędy - szczegóły opisane w komentarzu do tego issue.
* _issue #42_: Zliczanie wykorzystanych kart rycerzy - coś się źle liczy, muszę to DOKOŃCZYĆ.
  
<b>26 Czerwiec 2022</b>
* Dodałam odblokowanie możliwości użycia karty w kolejnej turze (zapomniałam po zakończeniu ruchu aktualizować zmiennej canUseCard na true) - dotyczy to ograniczenia do użycia tylko jednej karty w trakcie ruchu.
* Zamknęłam <span style="color: purple"><i>issue #35</i></span> - karty są dobrze blokowane.
* Dla _issue #47_ Zrobiłam tak, żeby już się pojawiało to okno tam gdzie powinno w grze (po wyrzuceniu 7 oczek), więc test już jest niepotrzebny, ale nie usuwam go. Dałabym też coś, żeby gracz wiedział ile ma maksymalnie surowców w tym oknie (szczególy w komentarzu do issue).
* Do _issue #50_ dodałam komentarz, żeby wyłączyć widoczność numerka nad polami jeśli stoi na nim złodziej oraz żeby uwzględnić to, że jeżeli złodziej stoi na jakimś polu, to gracz nie może wybrać tego pola.
* Postawiłam też złodzieja na pustyni na początku gry.
  
<b>24 Czerwiec 2022</b>
* Dla <span style="color: purple"><i>issue #21</i></span> podstawiłam poprawne wartości w wyświetlanych numerach wybudowanych liczb przez danego gracza i zamknęłam issue.
* Dla <span style="color: purple"><i>issue #36</i></span> podpięłam wywołania odpowiednich funkcji w zależności od wybranej karty (switch znajduje się w metodzie <span style="color: green"><b><i>UseCard</b></i></span> w klasie Player)
* Dla <span style="color: purple"><i>issue #44</i></span> wywołuję metodę <span style="color: green"><b><i>MonopolCardEffect</b></i></span> po zaakceptowaniu wybranego surowca. Dałabym tutaj kolejność surowców taką jaka jest w panelu użytkownika: drewno, glina, zboże, wełna, ruda żelaza, bo jest to trochę mylące teraz.
* Dla <span style="color: purple"><i>issue #45</i></span> wywołuję metodę <span style="color: green"><b><i>InventionCardEffect</b></i></span> po zaakceptowaniu wybranych surowców. Podobnie jak dla monopolu - dałabym tutaj kolejność surowców taką jaka jest w panelu użytkownika: drewno, glina, zboże, wełna, ruda żelaza, bo jest to trochę mylące teraz.
* Dla <span style="color: purple"><i>issue #43</i></span> poprawiłam działanie budowania darmowej drogi i aktywność przycisku budowania.
* Dla <span style="color: purple"><i>issue #73</i></span> utworzyłam klasę <span style="color: green"><b><i>Ports</b></i></span> oraz <span style="color: green"><b><i>PortDetails</b></i></span>, które przechowują informację o szczegółach handlu morskiego dla różnych portów. Gracz posiada zmienną ports, która posiada słownik portów (PortDetails -> bool), których wartości boolowskie oznaczają to czy dany rodzaj handlu morskiego jest dostępny dla gracza czy nie. W klasie PortDetails znajduje się zmienna <span style="color: green"><b><i>exchangeForOneResource</b></i></span>, które gracz musi oddać, aby otrzymać jeden surowiec wybranego rodzaju.
* Dla <span style="color: purple"><i>issue #59</i></span> stworzyłam metodę liczącą liczbę surowców, które można kupić na podstawie tych, które zostały wybrane do tej pory przez gracza oraz liczbę surowców, w których posiadaniu jest gracz, a które zostały wybrane nadmiarowo przez gracza. Działanie metody należy przetestować gdy będzie interfejs. Metoda ta to **CountTradeResources**, na razie jest w GameManager.
  



