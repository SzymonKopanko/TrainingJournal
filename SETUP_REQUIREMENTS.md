# TrainingJournal - Wymagania po resecie komputera

## 🚀 Aplikacje do zainstalowania

### **Podstawowe narzędzia:**
- **Git** - kontrola wersji
- **Node.js** (LTS) - dla React Native
- **npm** - menedżer pakietów Node.js

### **Android Development:**
- **Android Studio** - IDE dla Android
- **Java JDK** (OpenJDK 21 lub wyższy)
- **Android SDK** - Software Development Kit
- **Android NDK** - Native Development Kit

### **.NET Development:**
- **Visual Studio 2022** lub **Visual Studio Code**
- **.NET 8.0 SDK** - dla API backend
- **Entity Framework Core Tools**

### **Dodatkowe narzędzia:**
- **Postman** - testowanie API
- **GitHub CLI** - praca z GitHub z terminala

## 🔧 Konfiguracja środowiska

### **Zmienne środowiskowe:**
```
JAVA_HOME=C:\Program Files\Android\Android Studio\jbr
ANDROID_HOME=C:\Users\[USERNAME]\AppData\Local\Android\Sdk
PATH=%PATH%;%ANDROID_HOME%\platform-tools;%ANDROID_HOME%\tools
```

### **Git konfiguracja:**
```bash
git config --global user.name "Twoje Imię"
git config --global user.email "twoj@email.com"
```

## 📱 Testowanie na urządzeniu

### **Android:**
- Włącz **Developer Options** w telefonie
- Włącz **USB Debugging**
- Połącz telefon przez USB

### **React Native:**
```bash
npx react-native doctor  # Sprawdź konfigurację
npx react-native run-android  # Uruchom na telefonie
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
├── TrainingJournalApp/          # Frontend (React Native)
├── TrainingJournalApi.Tests/    # Testy API
└── README.md
```

## 🔗 Linki

- **GitHub:** https://github.com/skopa/TrainingJournal
- **API Docs:** https://localhost:7168/swagger (po uruchomieniu)
- **React Native Docs:** https://reactnative.dev/

## ⚠️ Ważne notatki

- **Dysk C:** Upewnij się że masz wystarczająco miejsca (min. 20GB wolnego)
- **Android SDK:** Instaluj na dysku z największą ilością miejsca
- **Node.js:** Użyj wersji LTS (Long Term Support)
- **Java:** Android Studio zawiera własną wersję JDK
