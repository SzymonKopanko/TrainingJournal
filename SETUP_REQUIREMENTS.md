# TrainingJournal API - Wymagania po resecie komputera

## 🚀 Aplikacje do zainstalowania

### **Podstawowe narzędzia:**
- **Git** - kontrola wersji

### **.NET Development:**
- **Visual Studio 2022** lub **Visual Studio Code**
- **.NET 8.0 SDK** - dla API backend
- **Entity Framework Core Tools**

### **Dodatkowe narzędzia:**
- **Postman** - testowanie API
- **GitHub CLI** - praca z GitHub z terminala

## 🔧 Konfiguracja środowiska

### **Git konfiguracja:**
```bash
git config --global user.name "Twoje Imię"
git config --global user.email "twoj@email.com"
```

## 🌐 API Backend

### **Uruchomienie API:**
```bash
cd TrainingJournalApi
dotnet run
```
- API będzie dostępne na: `https://localhost:7168`
- Swagger UI: `https://localhost:7168/swagger`

## 📁 Struktura projektu

```
TrainingJournal/
├── TrainingJournalApi/          # Backend API (.NET)
├── TrainingJournalApi.Tests/    # Testy API
└── README.md
```

## 🔗 Linki

- **GitHub:** https://github.com/skopa/TrainingJournal
- **API Docs:** https://localhost:7168/swagger (po uruchomieniu)

## ⚠️ Ważne notatki

- **Dysk C:** Upewnij się że masz wystarczająco miejsca (min. 5GB wolnego)
- **SQL Server:** Upewnij się że SQL Server jest zainstalowany i działa
- **.NET SDK:** Użyj wersji 8.0 lub wyższej
