# TrainingJournal API

Aplikacja do zarządzania dziennikiem treningowym - API backend napisane w ASP.NET Core 8.0.

## 🏗️ Architektura

### Główne komponenty:
- **ASP.NET Core 8.0** - framework webowy
- **Entity Framework Core** - ORM do zarządzania bazą danych
- **SQL Server** - baza danych
- **ASP.NET Core Identity** - uwierzytelnianie i autoryzacja
- **Swagger/OpenAPI** - dokumentacja API

### Struktura projektu:
```
TrainingJournal/
├── TrainingJournalApi/           # Główny projekt API
│   ├── Controllers/              # Kontrolery API
│   ├── Models/                   # Modele danych
│   ├── DTOs/                     # Data Transfer Objects
│   ├── Data/                     # Kontekst bazy danych
│   ├── Migrations/               # Migracje EF Core
│   └── Program.cs               # Konfiguracja aplikacji
├── TrainingJournalApi.Tests/     # Projekt testowy
│   ├── SimpleAccountControllerTests.cs
│   ├── SimpleExercisesControllerTests.cs
│   ├── SimpleTrainingsControllerTests.cs
│   └── README.md
└── README.md                     # Ten plik
```

## 🚀 Uruchamianie

### Wymagania:
- .NET 8.0 SDK
- SQL Server (lokalny lub Express)
- Visual Studio 2022 lub VS Code

### Instalacja:
1. Sklonuj repozytorium
2. Otwórz terminal w katalogu `TrainingJournalApi`
3. Zaktualizuj bazę danych:
   ```bash
   dotnet ef database update
   ```
4. Uruchom aplikację:
   ```bash
   dotnet run
   ```

### Dostęp do API:
- **API**: `https://localhost:7000` (lub port z `launchSettings.json`)
- **Swagger UI**: `https://localhost:7000/swagger`

## 📊 Modele danych

### Główne encje:
- **ApplicationUser** - użytkownicy (rozszerza IdentityUser)
- **Exercise** - ćwiczenia
- **ExerciseEntry** - wpisy ćwiczeń (sesje treningowe)
- **ExerciseSet** - serie ćwiczeń
- **Training** - treningi
- **TrainingExercise** - ćwiczenia w treningach
- **ExerciseMuscleGroup** - grupy mięśniowe ćwiczeń
- **UserWeight** - historia wagi użytkownika

### Enums:
- **MuscleGroup** - grupy mięśniowe (Chest, Back, Biceps, etc.)
- **MuscleGroupRole** - rola grupy mięśniowej (Primary, Secondary)

## 🔐 Uwierzytelnianie

API używa ASP.NET Core Identity z cookies:
- **Rejestracja**: `POST /api/Account/register`
- **Logowanie**: `POST /api/Account/login`
- **Wylogowanie**: `POST /api/Account/logout`
- **Profil**: `GET /api/Account/profile`

## 📋 Endpointy API

### Account Controller
- `POST /api/Account/register` - rejestracja użytkownika
- `POST /api/Account/login` - logowanie
- `POST /api/Account/logout` - wylogowanie
- `GET /api/Account/profile` - profil użytkownika

### Exercises Controller
- `GET /api/Exercises` - lista ćwiczeń użytkownika
- `GET /api/Exercises/{id}` - szczegóły ćwiczenia
- `POST /api/Exercises` - dodanie ćwiczenia
- `PUT /api/Exercises/{id}` - aktualizacja ćwiczenia
- `DELETE /api/Exercises/{id}` - usunięcie ćwiczenia

### Trainings Controller (wymaga autoryzacji)
- `GET /api/Trainings` - lista treningów użytkownika
- `GET /api/Trainings/{id}` - szczegóły treningu
- `POST /api/Trainings` - utworzenie treningu
- `PUT /api/Trainings/{id}` - aktualizacja treningu
- `DELETE /api/Trainings/{id}` - usunięcie treningu

### Inne kontrolery:
- **ExerciseEntriesController** - zarządzanie wpisami ćwiczeń
- **ExerciseSetsController** - zarządzanie seriami ćwiczeń
- **TrainingExercisesController** - zarządzanie ćwiczeniami w treningach
- **UserWeightsController** - historia wagi użytkownika
- **ExerciseMuscleGroupsController** - grupy mięśniowe ćwiczeń
- **EnumsController** - dostęp do enumów

## 🧪 Testy

Projekt zawiera testy jednostkowe dla głównych kontrolerów:

```bash
# Uruchomienie wszystkich testów
dotnet test

# Testy konkretnego kontrolera
dotnet test --filter "SimpleAccountControllerTests"
dotnet test --filter "SimpleExercisesControllerTests"
dotnet test --filter "SimpleTrainingsControllerTests"
```

### Pokrycie testów:
- **AccountController** - rejestracja, logowanie, wylogowanie
- **ExercisesController** - zabezpieczenia endpointów
- **TrainingsController** - zabezpieczenia endpointów

## 🗄️ Baza danych

### Konfiguracja:
- **Provider**: SQL Server
- **Connection String**: `appsettings.json`
- **Migracje**: Entity Framework Core Migrations

### Seed data:
- Przykładowy użytkownik: `seeduser@example.com`
- Przykładowe ćwiczenia: Przysiad, Martwy ciąg, Wyciskanie sztangi
- Przykładowa waga: 80.0 kg

### Migracje:
```bash
# Lista migracji
dotnet ef migrations list

# Zastosowanie migracji
dotnet ef database update

# Utworzenie nowej migracji
dotnet ef migrations add NazwaMigracji
```

## 🔧 Konfiguracja

### appsettings.json:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=SK_LENOVO\\SQLEXPRESS;Initial Catalog=TrainingJournalApi_data;Integrated Security=True;Pooling=False;Encrypt=False;Trust Server Certificate=True"
  }
}
```

### Wymagania haseł (Program.cs):
- Minimum 8 znaków
- Wymagane: cyfry, małe litery, wielkie litery, znaki specjalne
- Unikalny email

## 📝 DTOs

### Główne DTO:
- **UserDto** - dane użytkownika
- **RegisterDto** - rejestracja
- **LoginDto** - logowanie
- **ExerciseDto** - ćwiczenie
- **TrainingDto** - trening
- **ExerciseSetDto** - seria ćwiczenia
- **UserWeightDto** - waga użytkownika

## 🚀 Deployment

### Lokalne uruchomienie:
1. Upewnij się, że SQL Server działa
2. Zaktualizuj connection string w `appsettings.json`
3. Zastosuj migracje: `dotnet ef database update`
4. Uruchom: `dotnet run`

### Docker (opcjonalnie):
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY . /app
WORKDIR /app
EXPOSE 80
ENTRYPOINT ["dotnet", "TrainingJournalApi.dll"]
```

## 🤝 Contributing

1. Fork projektu
2. Utwórz branch dla nowej funkcji (`git checkout -b feature/nowa-funkcja`)
3. Commit zmian (`git commit -am 'Dodaj nową funkcję'`)
4. Push do brancha (`git push origin feature/nowa-funkcja`)
5. Utwórz Pull Request

## 📞 Kontakt

W przypadku pytań lub problemów, utwórz issue w repozytorium.
