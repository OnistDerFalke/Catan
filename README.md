# Catan
Projekt inżynierski dyplomowy.

<b>OnistDerFalke DevLog</b>:

<b>6 Lipiec 2022</b>
* Dla <span style="color: purple"><i>issue #36</i></span> poprawiłem jakość interfejsu z kartami: jak kliknie się jeszcze raz na zaznaczoną kartę, to się odznacza oraz po użyciu karty staje się
ona z powrotem mała. Do przetestowania. Jeśli chodzi o wielkość tej cyferki i gdzie ją umieścić na tych kartach to średnio pasuje inne położenie i trzeba to obgadać na discordzie bo nie chcę też wywalić
wielkiej cyfry, która będzie się zlewała z tłem mocno albo co gorsza zasłaniała tekst na karcie. Do ustalenia.
* Dla <span style="color: purple"><i>issue #81</i></span> zablokowałem zmienianie zakładek gdy wyskakuje okienko.
* Dla <span style="color: purple"><i>issue #56</i></span> pozwoliłem sobie na nieuzgodnioną zmianę. Jeśli bardzo będzie Wam przeszkadzać, to zrobie normalne. Ogólnie imo nie ma sensu robić handlu
i podziału handlu w różnych przyciskach. Po naciśnięciu handel pojawiają się dwa mniejsze przyciski z typem handlu. Możecie dać znać co o tym sądzicie. Według mnie to zapobiega troche
powstawaniu bałaganu w przyciskach. Redukujmy je do tylu, żeby użytkownik wiedział co się dzieje w tym interfejsie.

<b>5 Lipiec 2022</b>
* Dla <span style="color: purple"><i>issue #74</i></span> dodałem okienko wyświetlania karty, którą zakupił gracz. Wciśnięcie przycisku zatwierdź powoduje zniknięcie okna. Wydaje mi się, że działa, ale warto jeszcze potestować przy okazji robienia czegoś inego.

<b>4 Lipiec 2022</b>
* Dla <span style="color: purple"><i>issue #50</i></span> wziąłem grafiki pól od Gosii i podstawiłem je zamiast poprzednich. Dodatkowo wyodrębniłem złodzieja oraz podpiąłem samą
jego ikonkę do skryptu, który pokazuje go jedynie w sytuacji. gdy pole ma zmienną isThief ustawioną na wartość True. Grafiki są podmienione wszędzie - w scenie gry oraz menu głownego z tą różnicą, że prefaby
w menu głównym nie zawierają złodzieja. Więc ogólnie złodziej się przestawia jak trzeba imo. Można przetestować.
* Dodałem grafiki kart do UI. Trzeba ustalić co jak ma się dziać przy interakcji z nim, bo możliwe, że coś jest nie tak.

<b>3 Lipiec 2022</b>
* Podniosłem trochę te informacje. Ogólnie to jak chcesz po swojemu dopasować, to wchodzisz w scenę GameScreen, rozwijasz Board i na samym dole masz PortInfo schowane. W inspektorze odkryj je, żeby je widzieć, rozwiń i jak wejdziesz tam dalej to masz Canvas i w niej image oraz 
proportion text i operujesz wysokością tych elementów po prostu, albo jak chcesz dwa na raz to zmieniasz wysokość port info.
* Dla <span style="color: purple"><i>issue #81</i></span> wprowadziłem w kodzie system blokady interakcji z danymi elementami poprzez override metody sprawdzającej blokowanie. Teraz można wpisywać warunki blokowania elementów interaktywnych dla wszystkich elementów:

    - InteractiveElement->CheckBlockStatus() dla blokowania wszystkich elementów
    - InteractiveField>CheckBlockStatus() dla blokowania pól
    - InteractivePath->CheckBlockStatus() dla blokowania ścieżek
    - InteractiveJunction->>CheckBlockStatus() dla blokowania skrzyżowań
* Dla <span style="color: purple"><i>issue #81</i></span> zablokowałem wszelką interakcję przy rzucie kością (skorzystałem z warunku if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.GameSwitching && GameManager.CurrentDiceThrownNumber == 0 && GameManager.MovingUserMode == GameManager.MovingMode.Normal)) podanego
w issue #80).

<b>2 Lipiec 2022</b>
* Dla <span style="color: purple"><i>issue #79</i></span> naprawiłem bug poprzed odznaczenie elementu przed wejściem w fazę złodzieja. Nie zamykam, bo warto jeszcze przetestować, ale wydaje mi się, że działa git.
Jeśli się zgadzasz to możesz zamknąć issue.
* Dla <span style="color: purple"><i>issue #80</i></span> dodałem z podanym warunkiem informację o konieczności rzutu kością. Warunek wydaje się ok, ale możesz to jeszcze po swojemu zweryfikowac. Jeśli wymyślisz kolejne warunki
do tego, co ma się wypisać w innym miejscu, to dopisz tu po prostu.
* Dla <span style="color: purple"><i>issue #54</i></span> dodałem informacje w miejscach z Twojej zwróconej tablicy. Jednak coś tam z tymi pozycjami jest trochę nie tak. Już wszystko jest oskryptowane więc możesz
pokombinować co tam jest, że trochę źle się wyświetlają i to się będzie zmieniać już w grze, więc pewnie będzie łatwiej. Wysokość napisów i takie tam do ustalenia też. Pamiętajmy jednak, że im wyżej są napisy, tym
w perspektywie widać je w lekkim przesunięciu. Musimy to uwzględnić. Jeśli bardzo przeszkadzają te informacje w aktualnym stanie, to możesz je wyłączyć w SetPortInfo w BoardCreation na końcu dając false w info.SetActive.


<b>1 Lipiec 2022</b>
* Dla <span style="color: purple"><i>issue #54</i></span> dodałem odpowiednie rodzaje portów do skrzyżowań.

<b>30 Czerwiec 2022</b>
* Dla <span style="color: purple"><i>issue #47</i></span> poprawiłem problem z aktualizacją dostępnych surowców. Pierwszy problem pozostaje zagadką, bo siedziałem pół godziny rzucając po prostu kostką na każdym trybie
i wszystko działało, nic się nie wywalało. Jeśli problem po naprawieniu powyższego błędu wciąż będzie występował proszę o informację o trybie, liczbie graczy i wykonanych czynnościach, które do niego doprowadziły.
W innym wypadku ciężko mi zweryfikować, co nie działa, bo błędu nie widzę. Tzw. - "u mnie działa".
* Dla <span style="color: purple"><i>issue #78</i></span> blokuję przycisk następnej tury, gdy trzeba ruszyć złodziejem. Zamykam wątek.
* Dla <span style="color: purple"><i>issue #77</i></span> zakładka podświetla się na amen. Zamykam wątek.
* Dla <span style="color: purple"><i>issue #75</i></span> informacje wyświetlają się teraz w górnej części ekranu. Trzeba jeszcze przetestować, więc wątku nie zamykam.

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

<b>6 Lipiec 2022</b>
* _issue #56_: zamknęłam issue - jest ok
* _issue #36_: dodałam w komentarzu jeszcze jedną rzecz do zrobienia i odniosłam się do uwag
* _issue #81_: dodałam komentarz z propozycją rozwiązania problemu - co Ty na to?
* _issue #80_: dodałam napisy dla trybu zaawansowanego przy początkowym rozłożeniu budynków. Zamknęłam issue
* _issue #75_: zamknęłam issue, wydaje mi się, że jest ok, jak coś się znajdzie to będzie się dodawało później kolejne bugi, przy testowaniu gotowego projektu
* Poprawiłam to, że jak gracz wykorzysta jakąś kartę przed rzutem kością to informacja wraca do "rzutu kością" (wcześniej tak nie było).
    
<b>5 Lipiec 2022</b>
* _issue #74_: zamknęłam issue - jest ok
* _issue #36_: uwagi do tego issue (to co bym zmieniła i to co mi nie pasuje) znajduje się w komentarzu do issue - musimy to wspólnie obgadać, żeby określić co lepiej wygląda, bo może macie lepsze pomysły
* _issue #50_: zamknęłam issue - jest ok
* _issue #12_: zamknęłam issue - jest ok
* _issue #49_: zamknęłam issue - jest ok
* _issue #48_: zamknęłam issue - jest ok
* _issue #70_: poprawiłam algorytm rozkładania informacji - już jest git
* _issue #80_: dodałam informacje o fazach budowania lub handlu dla trybu podstawowego. Wprowadziłam dwa tryby - takie podstawowe (normalny, faza budowania oraz faza handlowania), do której gracza wraca po zakończeniu specjalnego ruchu np. budowy darmowej drogi, przestawienia złodzieja lub rzutu kością. Obie zmienne są w GameManager - **BasicMovingUserMode** i **MovingUserMode**. Trzeba jeszcze sprawdzić czy aby na pewno wszystko działa.
* _issue #81_: zablokowałam przyciski (zrobiłam podział na fazy też) - wydaje mi się, że jest dobrze, zostało jeszcze poblokować te interaktywne elementy np. wybranie drogi, skrzyżowania itp. Dodatkowo trzeba uniemożliwić zmianę zakładki, gdy jakiekolwiek okno jest otwarte (patrz: komentarz do issue)
* _issue #72_: zamknęłam issue

<b>2 Lipiec 2022</b>
* _issue #79_: zamknęłam issue - działa dobrze
* _issue #54_: poprawiłam pozycje informacji, ale średnio to wygląda przez podniesienie chyba tych napisów i ich wielkość, więc na razie możesz zmniejszyć wielkość troszkę i może podnieść, żeby te numerki nie były w wodzie... A resztę się ogarnie jak będą porty czyli ścieżki.

<b>1 Lipiec 2022</b>
* Dodałam obrót drożek (portów) w tablicy przechowującej x i z, teraz tez przechowują kąty.
* _issue #54_: Ustawiłam pozycje informacji o portach (tzn. napisu "3:1", a także odpowiedniego surowca z napisem "2:1") - zaktualizowałam issue
* Utworzyłam **_issue #80_** - muszę jeszcze pododawać resztę informacji, ale to się muszę zastanowić kiedy co wypisać
* _issue #42_: naprawiłam zliczanie i punktowanie zużytych kart rycerzy. Zamknęłam issue.
* Pochowałam okienka wyskakujące przy złodzieju, jeżeli się nie powinny pojawić tzn. jak było 0 elementów do pokazania okienko się pojawiało i po chwili znikało, co wyglądało źle. Przy wyborze graczy, od których się zabiera surowce usunęłam możliwość wybrania siebie oraz posortowałam graczy rosnąco (czasami się wyświetlało 3 i 2 zamiast 2 i 3)
* _issue #47_: wydaje mi się że naprawiłam bug, więc zamknęłam issue. Jak się natrafi na buga to się doda nowy issue
  
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
  



