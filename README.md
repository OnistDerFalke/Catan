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



