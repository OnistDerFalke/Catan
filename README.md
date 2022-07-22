# Catan
Projekt inżynierski dyplomowy.

<b>OnistDerFalke DevLog</b>:

<b>22 Lipiec 2022</b>
* Dla <span style="color: purple"><i>issue #89</i></span> dodałem warunek pojawiania się ikonki. Można przetestować i jak git, zamknąć issue.
* Dla <span style="color: purple"><i>issue #37</i></span> zmianiłem "Wioska" na "Osada". Tu chyba nie ma co testować, więc zamykam wątek.

<b>20 Lipiec 2022</b>
* Dla <span style="color: purple"><i>issue #62</i></span> dokończyłem okno końca gry. Jest aktualnie dostepne w trybie TEST. 
W dowolnym momencie gry naciskamy klawisz T i wyskakuje nam okno końca gry z aktualnymi wynikami, żeby nie trzeba było kończyć gry, żeby zobaczyć okno.

<b>19 Lipiec 2022</b>
* Dla <span style="color: purple"><i>issue #64</i></span> podłączyłem i oskryptowałem kolorowe UI z wynikami.
* Zrezygnowałem z odklikiwania zaznaczeń na oceanie. Musiałbym dodać collidery na UI i robić dodatkowy smietnik a średnio mi się chce. Może kiedyś jak starczy czasu.

<b>18 Lipiec 2022</b>
* Dla <span style="color: purple"><i>issue #90</i></span> naprawiłem bug, zamykam issue.
* Dla <span style="color: purple"><i>issue #89</i></span> dodałem pojawianie się ikonki przy budowaniu. Sory, że w tym tasku, ale nie mam własnego, a już nie chce mi się robić, a ten ściśle powiązany.
* Dla <span style="color: purple"><i>issue #37</i></span> poprawiłem buga, warto jeszcze potestować i jeśli ok zamknąć issue. 
* Dla <span style="color: purple"><i>issue #64</i></span> dodałem graficzny update do dolnego UI i zmieściłem tam widok punktacji. Jeszcze jest niepodłączony i nieoskryptowany.

<b>15 Lipiec 2022</b>
* Dla <span style="color: purple"><i>issue #54</i></span> dodałem porty na mapkę i rozmieszczanie ich. Coś jest nie tak tutaj, opis w issue.

<b>14 Lipiec 2022</b>
* Dodałem ikonki do GUI, dopracowałem okno handlu morskiego w nowym stylu (jeśli zaakceptujecie, to wkrótce wszystkie okna dostaną update)
* Dla <span style="color: purple"><i>issue #59</i></span> ukończyłem okno handlu morskiego ze wszystkimi interakcjami. Potrzeba poprawić algorytm obliczania nadmiarowych surowców. Wtedy sprawdzić poprawność działania całego okna jeszcze raz
na wypadek jakbym coś przeoczył.
* Poprawiłem/dopracowałem okno akceptacji oferty.

<b>13 Lipiec 2022</b>
* Dla <span style="color: purple"><i>issue #86</i></span> przerobiłem i zrefaktoryzowałem cały system interakcji, który będzie teraz łatwo rozszerzalny o dodawanie kolejnych interakcji. Dodałem również interakcję z polem wyboru złodzieja.
Podobnie jak pozostałe elementy podświetla się ono teraz na czarno. Niestety nie mogłem poblokować jednej rzeczy, bo metoda sypała nullami. Jeśli poprawisz ją/wyjaśnisz mi jaki był jej cel i może źle ją wywołuję, to daj znać i wtedy wrzuce
ten warunek.
* Dla <span style="color: purple"><i>issue #66</i></span> dodałem shader dla kostki ten, który wybrałyście.

<b>12 Lipiec 2022</b>
* Dla <span style="color: purple"><i>issue #86</i></span> dodałem aktualizowanie menu statycznego po najechaniu. Do tego podświetlenie elementu po najechaniu działa tylko wtedy gdy można wejść w interakcje z tym elementem.
To dość złożona operacja, żeby to zmienić, dlatego potrzebuję jeszcze trochę czasu na dopracowanie, ale wrzucam to w wielu commitach.

<b>11 Lipiec 2022</b>
* Dla <span style="color: purple"><i>issue #59</i></span> dodałem prototyp okienka bez implementacji po wybraniu handlu morskiego. Dalszy implement czeka, aż się zgadamy.
* Dla <span style="color: purple"><i>issue #66</i></span> dodałem shader dla kostki.

<b>10 Lipiec 2022</b>
* Dla <span style="color: purple"><i>issue #61</i></span> zablokowałem wysyłanie przez gracza oferty do samego siebie, wyzerowałem gracza po ponownym otwarciu okna, dodałem wartości potrzebne stronie operującej
oknem handlu:
  - Dla gracza, którego jest tura w oknie tworzenia oferty wymiany, pod surowcami, które ma oddać wyświetla się liczba dostępnych surowców,
  - Dla gracza, który w nie swojej turze steruje oknem potwierdzenia wymiany, pod surowcami, które ma oddać wyświetla się liczba posiadanych przez niego surowców (żeby mógł zrozumieć, dlaczego nie może zaakceptować lub
  podjąć decyzje, czy wymiana mu się opłaca).

<b>9 Lipiec 2022</b>
* Dla <span style="color: purple"><i>issue #61</i></span> utworzyłem całe okno do akceptowania wymiany wraz z blokowaniem przycisków i całą transakcją pomiędzy graczami. Jeśli mogę mieć uwagę co do samej wymiany, odpowiedzialność
za wymianę surowców nie powinna leżeć po stronie popupu, ponieważ jest to jedynie element interfejsu, który służy do komunikacji z użytkownikiem. Te cztery operacje dodawania i odejmowania powinny być zaimplementowane w logice (przejąć
odpowiedzialność za wymianę) w postaci jakiejś metody, którą tylko wywołamy w skrypcie okna jako znak, że należy dokonać teraz wymiany. Takie są poprawne praktyki według tego co kiedyś czytałem, ale co z tym zrobimy zależy tylko od nas.
Dwie minuty roboty, zero problematyczności. Czekam na jakąś opinię w tej sprawie. Zastanawiało mnie to, czy sprawdzanie, czy druga strona może zaakceptować powinno odbywać się już przy tworzeniu wymiany. Z jednej strony tak, bo potem gracz dostanie okno,
które może jedynie odrzucić. Z drugiej strony, pozostali gracze mogą się dowiedzieć o liczbie posiadanych surowców przez graczy pozostałych. Nie ma o tym nigdzie w issue, a dość poważnym problemem może być brak informacji o posiadanych surowcach przez
otrzymującego ofertę przy podejmowaniu decyzji akceptuj/odrzuć. Może warto by znów taką informację gdzieś wyświetlić (jak w którymś oknie poprzednio). Jako, że z tym issue jest wiele wątpliwości, które musimy uzgodnić - nie zamykam go i ustawiam na testy.
* Dla <span style="color: purple"><i>issue #85</i></span> zrobiłem skrypt środkujący popupy zgodnie z pozycją prawego UI. Uwaga - tabela wyników nie jest popupem, ma prawo zasłaniać wszystko, tak jest w większości gier. Można przetestować. Jeśli okej, to można zamknąć issue.

<b>8 Lipiec 2022</b>
* Dla <span style="color: purple"><i>issue #60</i></span> wprowadziłem limity surowców i aktywacje przycisku wymiany. Przycisk ten jeszcze nic nie robi, będzie pakował wybrane surowce i callował następny popup, ale
na razie nie dotykam bo git jest częściowo zajęty, żeby nie było konfliktów.
* Dla <span style="color: purple"><i>issue #87</i></span> poprawiłem te dwie rzeczy. Jeśli gdzieś się jeszcze to powtórzy, to dajcie znać. Warto by to przetestować jeszcze.
* Dla <span style="color: purple"><i>issue #88</i></span> naprawiłem bug i przetestowałem podstawiajac różne kombinacje wyników - ale warto się zawsze przyjrzeć tej kolejności przy okazji.

<b>7 Lipiec 2022</b>
* Dla <span style="color: purple"><i>issue #82</i></span> bug naprawiony, zamykam issue.
* Dla <span style="color: purple"><i>issue #36</i></span> przycisk się teraz odznacza, bug naprawiony. Nie zamykam issue, bo są tam inne treści jeszcze.
* Dla <span style="color: purple"><i>issue #60</i></span> dodałem okno z działającymi elementami, ale nie ma jeszcze ograniczeń wyboru surowców, ani możliwości zatwierdzania jeszcze.

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

<b>20 Lipiec 2022</b>
* _issue #89_: Warunek trzeba zmienić, tak to jest git wg mnie
* _issue #37_: Została jeszcze zmiana napisu - patrz komentarz do issue
* _issue #62_: Wygląda ok, nie wiem jak z tą grafiką - lepiej niech się Gosia może wypowie :D Naprawiłam ustawianie GameManager, żeby nie wywalały się błędy gdy się rozpocznie kolejną grę
* _issue #88_: Teraz mi się nie pokazuje ten błąd - zamknęłam issue
* _issue #86_: Zamknęłam issue
* _issue #64_: Zamknęłam issue

<b>15 Lipiec 2022</b>
* _issue #54_: Naprawiłam algorytm rozmieszczania portów (tych dróżek)
* _issue #59_: Naprawiłam liczenie dodatkowych surowców i surowców, które gracz może otrzymać. Zamknęłam issue
* _issue #73_: Zamknęłam issue
* _issue #90_: Znalazłam bug i utworzyłam issue
* _issue #87_: Zamknęłam issue
* _issue #54_: Zamknęłam issue
* _issue #55_: Zamknęłam issue
* _issue #60_: Zamknęłam issue
* _issue #70_: Zamknęłam issue
* _issue #71_: Zamknęłam issue

<b>13 Lipiec 2022</b>
* _issue #66_: zamknęłam issue
* _issue #86_: teraz elementy, na które się najeżdża dobrze się podświetlają, ale jest kolejny problem - szczegóły w komentarzu issue.

<b>9 Lipiec 2022</b>
* _issue #61_: przerzuciłam te cztery linijki dotyczące wymiany surowców do osobnej metody w GameManager -> odpowiedzi na wątpliwości i propozycje zmian + 2 bugi zapisałam w komentarzu do issue.

<b>8 Lipiec 2022</b>
* _issue #52_: wydaje mi się, że działa, metoda aktualizująca punkty (i sprawdzająca która droga jest najdłuższa i do jakiego gracza należy) znajduje się w **GameManager**, a wywołana jest przy każdym budowaniu drogi lub osady.
* Znalazłam błędy i utworzyłam dwa issue: _issue #87_ i _issue #88_. _Issue #88_ nie jest ważny, ale _issue #87_ dobrze by było gdybyś zerknął, bo prawdopodobnie kolejne popupy robisz na tej samej zasadzie, więc lepiej już teraz to poprawić i nie robić tych błędów dalej.
* _issue #61_: przygotowałam metody na dodawanie i odejmowanie surowców w postaci słownika - szczegóły działania w komentarzu do issue. Napisałam tam to, co należy wywołać zaraz po zaakceptowaniu propozycji.

<b>7 Lipiec 2022</b>
* _issue #36_: zamknęłam issue
* _issue #81_: przerzuciłam wszystkie zmienne boolowskie dotyczące popupów na słownik typu string -> bool i zamknęłam issue
* _issue #62_: gdy któryś z graczy zdobędzie min. 10 punktów to zmienna EndGame w GameManager zmienia się na true... (wartość ta ustawiana jest w klasie **ActionsContentNavigation** w metodzie **OnTurnSkipButton**, ponieważ sprawdzanie liczby punktów jest dopiero na koniec ruchu danego gracza)
* _issue #83_: poprawiłam warunki budowania drogi, wydaje mi się, że teraz jest dobrze. Zamknęłam issue

<b>6 Lipiec 2022</b>
* _issue #56_: zamknęłam issue - jest ok
* _issue #36_: dodałam w komentarzu jeszcze jedną rzecz do zrobienia i odniosłam się do uwag
* _issue #81_: dodałam komentarz z propozycją rozwiązania problemu - co Ty na to? Dodałam też zadanie dla @OnistDerFalke w komentarzu - przerzuciłam do funkcji **Available** w JunctionElement oraz PathElement sprawdzenie warunków czy gracz może wybudować drogę/budynek i na tej podstawie podświetla się przycisk Build. 
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
  



