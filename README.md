# ProjektNet - WorkshopManager

Aplikacja webowa Asp.NET Core do zarządzania warsztatem samochodwym.
Adres github: https://github.com/tostrowski8080/ProjektNet

## Funkcje aplikacji

- Rejstracja i logowanie z podziałem na role
- Blokada dostępu w zależności od roli
- Tworzenie klientów, pojazdów, zleceń dla recepcjonisty
- Zarządzanie przypisanymi zleceniami przez mechaników
- Zarządzanie częściami przez pracowników - admin dodaje części do katalogu, recepcjonista zmienia ich ilość, mechanik używa je do czynności naprawczych
- Możliwość połączenia klienta warsztatu do jego konta na stronie
- Generowanie raportów PDF
- Dodawanie zdjęć pojazdów przez użytkowników z połączonymi kontami
- Dane są mapowane przez Mapperly

## Role

- Klient - konto klienta warsztatu, powinien zostać przypisane przez pracownika do klienta z bazy danych
    - Dodawanie zdjęć do swoich pojazdów
    - Dodawanie komentarzy do zleceń
- Recepcjonista - konto od zarządzania działaniem warsztatu
    - Dodawanie Klientów
    - Dodawanie Pojazdów do klientów
    - Dodawanie Zleceń do pojazdów
    - Przypisywanie Klientów z bazy danych do konta na stronie
    - Modyfikowanie ilości części w składzie
    - Generowanie raportu PDF z napraw klienta
- Mechanik - konto dla mechaników zapisywania swoich działań
    - Zmiana statusu zleceń przypisanych do mechanika
    - Dodawanie czynności do zlecenia
    - Dodawanie wymaganych części do czynności
- Admin - konto z wszystkimi uprawnieniami
    - Wszystkie uprawnienia innych ról
    - Zmiana ról dla kont
    - Dodawanie części do katalogu
    - Generowanie miesięcznego PDF z napraw

## Struktura projektu

```
/WorkshopManager
├── Controllers/
├── DTOs/
├── Models/
├── Services/
├── Mappers/             // Mapperly mappery
├── PdfRaports/          // klasy QuestPDF do generowania raportów
├── Pages/               // Razor Pages do interakcji z funkcjonalnościami
├── Views/
├── wwwroot/
│   └── uploads/         // zdjęcia pojazdów
├── Data/
├── Program.cs
```

## BackgroundService - raporty e-mail

Co godzine program wysyła raport e-mail na wskazany adres - można zmienić adres w klasie OpenOrderReportBackgroundService.cs

## Github Actions - CI/CD

Repozytorium zawiera zautomatyzowany proces CI/CD za pomocą GitHub Actions.

Workflow wykonuje:

1. `dotnet restore` – przywracanie zależności
2. `dotnet build` – budowanie aplikacji
3. `dotnet test` – uruchamianie testów jednostkowych

Uruchamia się automatycznie po push lub pull request do gałęzi main.

Aby uruchomić lokalnie należy zaktualizować appsettings.json z własną lokalną bazą danych.