# TrainingJournalApi.Tests

Projekt testowy dla TrainingJournalApi zawierający testy jednostkowe dla kontrolerów.

## Struktura testów

### SimpleAccountControllerTests
Testy dla kontrolera AccountController:
- `Register_WithValidData_ReturnsOk` - test rejestracji z poprawnymi danymi
- `Register_WithInvalidData_ReturnsBadRequest` - test rejestracji z niepoprawnymi danymi
- `Logout_ReturnsOk` - test wylogowania

### SimpleExercisesControllerTests
Testy dla kontrolera ExercisesController:
- `GetExercises_WithoutAuthentication_ReturnsUnauthorized` - test pobierania ćwiczeń bez uwierzytelnienia
- `AddExercise_WithoutAuthentication_ReturnsUnauthorized` - test dodawania ćwiczenia bez uwierzytelnienia
- `GetExerciseById_WithoutAuthentication_ReturnsUnauthorized` - test pobierania ćwiczenia po ID bez uwierzytelnienia
- `UpdateExercise_WithoutAuthentication_ReturnsUnauthorized` - test aktualizacji ćwiczenia bez uwierzytelnienia
- `DeleteExercise_WithoutAuthentication_ReturnsUnauthorized` - test usuwania ćwiczenia bez uwierzytelnienia

### SimpleTrainingsControllerTests
Testy dla kontrolera TrainingsController:
- `GetTrainings_WithoutAuthentication_ReturnsMethodNotAllowed` - test pobierania treningów bez uwierzytelnienia
- `GetTraining_WithoutAuthentication_ReturnsMethodNotAllowed` - test pobierania treningu po ID bez uwierzytelnienia
- `CreateTraining_WithoutAuthentication_ReturnsMethodNotAllowed` - test tworzenia treningu bez uwierzytelnienia
- `UpdateTraining_WithoutAuthentication_ReturnsMethodNotAllowed` - test aktualizacji treningu bez uwierzytelnienia
- `DeleteTraining_WithoutAuthentication_ReturnsMethodNotAllowed` - test usuwania treningu bez uwierzytelnienia

## Uruchamianie testów

```bash
dotnet test
```

## Uruchamianie konkretnych testów

```bash
# Wszystkie testy dla AccountController
dotnet test --filter "SimpleAccountControllerTests"

# Wszystkie testy dla ExercisesController
dotnet test --filter "SimpleExercisesControllerTests"

# Wszystkie testy dla TrainingsController
dotnet test --filter "SimpleTrainingsControllerTests"
```

## Technologie

- **xUnit** - framework testowy
- **Microsoft.AspNetCore.Mvc.Testing** - testowanie aplikacji ASP.NET Core
- **Microsoft.EntityFrameworkCore.InMemory** - baza danych w pamięci do testów
- **Microsoft.AspNetCore.Identity** - uwierzytelnianie i autoryzacja

## Konfiguracja testów

Testy używają:
- Bazy danych InMemory dla izolacji testów
- Uproszczonej konfiguracji Identity (bez wymagań dotyczących haseł)
- Wyłączonego logowania podczas testów
- Automatycznego czyszczenia bazy danych po każdym teście
