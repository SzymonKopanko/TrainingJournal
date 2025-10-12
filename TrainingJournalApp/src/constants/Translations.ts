/**
 * Tłumaczenia aplikacji TrainingJournal
 * Centralne miejsce definicji wszystkich tekstów używanych w aplikacji
 * Automatycznie wybiera tłumaczenia dla określonego języka
 */

import { EnglishTranslations } from './EnglishTranslations';

export type Language = 'pl' | 'en';

/**
 * Zwraca tłumaczenia dla określonego języka
 */
export const getTranslations = (language: Language) => {
  return language === 'en' ? EnglishTranslations : PolishTranslations;
};

/**
 * Polskie tłumaczenia (domyślne)
 */
export const PolishTranslations = {
  // Nawigacja
  navigation: {
    exercises: 'Ćwiczenia',
    trainings: 'Treningi',
    history: 'Historia',
    profile: 'Profil',
    settings: 'Ustawienia',
  },
  
  // Ekran ćwiczeń
  exercises: {
    title: 'Ćwiczenia',
    addExercise: 'Dodaj ćwiczenie',
    editExercise: 'Edytuj ćwiczenie',
    deleteExercise: 'Usuń ćwiczenie',
    exerciseName: 'Nazwa ćwiczenia',
    exerciseDescription: 'Opis (opcjonalny)',
    bodyWeightPercentage: 'Procent masy ciała',
    bodyWeightPercentageDescription: 'Procent masy ciała podnoszonej podczas ćwiczenia (np. 0.8 = 80%)',
    muscleGroups: 'Grupy mięśniowe',
    muscleGroup: 'Grupa mięśniowa',
    role: 'Rola',
    addMuscleGroup: 'Dodaj grupę mięśniową',
    noExercises: 'Brak ćwiczeń',
    noExercisesDescription: 'Nie masz jeszcze żadnych ćwiczeń.',
    noExercisesAction: 'Dodaj pierwsze ćwiczenie.',
    loading: 'Ładowanie...',
    refresh: 'Odśwież',
    save: 'Zapisz',
    cancel: 'Anuluj',
    edit: 'Edytuj',
    delete: 'Usuń',
  },
  
  // Ekran treningów
  trainings: {
    title: 'Treningi',
    addTraining: 'Dodaj trening',
    editTraining: 'Edytuj trening',
    deleteTraining: 'Usuń trening',
    trainingName: 'Nazwa treningu',
    trainingDescription: 'Opis (opcjonalny)',
    trainingDate: 'Data',
    exercisesCount: 'Ćwiczenia',
    noTrainings: 'Brak treningów',
    noTrainingsDescription: 'Nie masz jeszcze żadnych treningów.',
    noTrainingsAction: 'Dodaj pierwszy trening.',
    loading: 'Ładowanie...',
    refresh: 'Odśwież',
    save: 'Zapisz',
    cancel: 'Anuluj',
    edit: 'Edytuj',
    delete: 'Usuń',
    details: 'Szczegóły',
    noExercises: 'Brak ćwiczeń',
    noExercisesDescription: 'Dodaj ćwiczenia do tego treningu używając przycisku +',
    addExercise: 'Dodaj ćwiczenie',
    selectExercise: 'Wybierz ćwiczenie',
    add: 'Dodaj',
    removeExercise: 'Usuń ćwiczenie',
    selectExerciseMessage: 'Wybierz ćwiczenie',
  },
  
  // Ekran historii
  history: {
    title: 'Historia',
    noData: 'Brak danych',
    noDataDescription: 'Nie masz jeszcze żadnych wpisów wagi.',
    noDataAction: 'Dodaj pierwszy wpis w zakładce Profil.',
    loading: 'Ładowanie...',
    refresh: 'Odśwież',
    delete: 'Usuń',
  },
  
  // Ekran profilu
  profile: {
    title: 'Profil',
    currentWeight: 'Obecna waga',
    lastUpdate: 'Ostatnia aktualizacja',
    weightChange: 'od poprzedniego pomiaru',
    addWeight: 'Dodaj wagę',
    statistics: 'Statystyki',
    totalMeasurements: 'Łącznie pomiarów',
    lowestWeight: 'Najniższa waga',
    highestWeight: 'Najwyższa waga',
    recentMeasurements: 'Ostatnie pomiary',
    noMeasurements: 'Brak pomiarów',
    weight: 'Waga (kg)',
    date: 'Data',
    notes: 'Notatki (opcjonalne)',
    loading: 'Ładowanie...',
    save: 'Zapisz',
    cancel: 'Anuluj',
    delete: 'Usuń',
  },
  
  // Ekran ustawień
  settings: {
    title: 'Ustawienia',
    appearance: 'Wygląd',
    theme: 'Motyw',
    lightTheme: 'Jasny',
    darkTheme: 'Ciemny',
    systemTheme: 'Systemowy',
    themeDescription: 'Wybierz motyw aplikacji',
    language: 'Język',
    languageDescription: 'Wybierz język aplikacji',
    data: 'Dane',
    exportData: 'Eksportuj dane',
    importData: 'Importuj dane',
    clearData: 'Wyczyść wszystkie dane',
    clearDataDescription: 'Usuwa wszystkie ćwiczenia, treningi i pomiary wagi',
    about: 'O aplikacji',
    version: 'Wersja',
    developer: 'Deweloper',
    privacy: 'Prywatność',
    terms: 'Warunki użytkowania',
    loading: 'Ładowanie...',
    save: 'Zapisz',
    cancel: 'Anuluj',
    confirm: 'Potwierdź',
    export: 'Eksportuj',
    import: 'Importuj',
    clear: 'Wyczyść',
  },
  
  // Grupy mięśniowe
  muscleGroups: {
    chest: 'Klatka piersiowa',
    back: 'Plecy',
    biceps: 'Biceps',
    triceps: 'Triceps',
    shoulders: 'Barki',
    legs: 'Nogi',
    glutes: 'Pośladki',
    calves: 'Łydki',
    abs: 'Brzuch',
    forearms: 'Przedramiona',
    traps: 'Kaptury',
    lats: 'Najszersze grzbietu',
  },
  
  // Role grup mięśniowych
  muscleGroupRoles: {
    primary: 'Główna',
    secondary: 'Pomocnicza',
  },
  
  // Komunikaty sukcesu
  success: {
    exerciseAdded: 'Ćwiczenie dodane',
    exerciseUpdated: 'Ćwiczenie zaktualizowane',
    exerciseDeleted: 'Ćwiczenie usunięte',
    trainingAdded: 'Trening dodany',
    trainingUpdated: 'Trening zaktualizowany',
    trainingDeleted: 'Trening usunięty',
    weightAdded: 'Waga dodana',
    weightDeleted: 'Wpis usunięty',
  },
  
  // Komunikaty błędów
  errors: {
    exerciseNameRequired: 'Nazwa ćwiczenia jest wymagana',
    muscleGroupRequired: 'Wybierz przynajmniej jedną grupę mięśniową',
    trainingNameRequired: 'Nazwa treningu jest wymagana',
    weightRequired: 'Waga jest wymagana',
    weightInvalid: 'Wprowadź poprawną wartość wagi',
    bodyWeightPercentageInvalid: 'Procent masy ciała musi być liczbą większą lub równą 0',
    loadingExercises: 'Nie udało się załadować ćwiczeń',
    loadingTrainings: 'Nie udało się załadować treningów',
    loadingWeights: 'Nie udało się załadować historii wagi',
    savingExercise: 'Nie udało się zapisać ćwiczenia',
    savingTraining: 'Nie udało się zapisać treningu',
    savingWeight: 'Nie udało się zapisać wagi',
    deletingExercise: 'Nie udało się usunąć ćwiczenia',
    deletingTraining: 'Nie udało się usunąć treningu',
    deletingWeight: 'Nie udało się usunąć wpisu',
    databaseInit: 'Błąd inicjalizacji aplikacji',
  },
  
  // Potwierdzenia
  confirmations: {
    deleteExercise: 'Usuń ćwiczenie',
    deleteExerciseMessage: 'Czy na pewno chcesz usunąć ćwiczenie',
    deleteTraining: 'Usuń trening',
    deleteTrainingMessage: 'Czy na pewno chcesz usunąć trening',
    deleteWeight: 'Usuń wpis wagi',
    deleteWeightMessage: 'Czy na pewno chcesz usunąć wpis z',
    removeExerciseFromTraining: 'Usuń ćwiczenie z treningu',
    removeExerciseFromTrainingMessage: 'Czy na pewno chcesz usunąć ćwiczenie z treningu',
  },
  
  // Ogólne
  common: {
    yes: 'Tak',
    no: 'Nie',
    ok: 'OK',
    cancel: 'Anuluj',
    confirm: 'Potwierdź',
    delete: 'Usuń',
    edit: 'Edytuj',
    save: 'Zapisz',
    back: 'Wstecz',
    loading: 'Ładowanie...',
    error: 'Błąd',
    success: 'Sukces',
    info: 'Informacja',
    warning: 'Ostrzeżenie',
    required: 'Wymagane',
    optional: 'Opcjonalne',
    today: 'Dzisiaj',
    yesterday: 'Wczoraj',
    tomorrow: 'Jutro',
    polish: 'Polski',
    english: 'English',
  },
  
  // Formatowanie dat
  dateFormat: {
    short: 'DD.MM.YYYY',
    long: 'DD MMMM YYYY',
    time: 'HH:mm',
    dateTime: 'DD.MM.YYYY HH:mm',
  },
  
  // Walidacja
  validation: {
    required: 'To pole jest wymagane',
    email: 'Wprowadź poprawny adres email',
    minLength: 'Minimum {count} znaków',
    maxLength: 'Maksimum {count} znaków',
    numeric: 'Wprowadź liczbę',
    positive: 'Wprowadź liczbę dodatnią',
    date: 'Wprowadź poprawną datę',
    weight: 'Wprowadź poprawną wagę (np. 75.5)',
  },
  
  // Placeholdery
  placeholders: {
    exerciseName: 'np. Przysiad',
    exerciseDescription: 'Opis ćwiczenia...',
    trainingName: 'np. Trening nóg',
    trainingDescription: 'Opis treningu...',
    weight: 'np. 75.5',
    notes: 'Dodatkowe notatki...',
    search: 'Szukaj...',
  },
  
  // Tooltips
  tooltips: {
    addExercise: 'Dodaj nowe ćwiczenie',
    addTraining: 'Dodaj nowy trening',
    addWeight: 'Dodaj pomiar wagi',
    edit: 'Edytuj',
    delete: 'Usuń',
    refresh: 'Odśwież dane',
    save: 'Zapisz zmiany',
    cancel: 'Anuluj zmiany',
  },
  
  // Dostępność
  accessibility: {
    exerciseCard: 'Karta ćwiczenia',
    trainingCard: 'Karta treningu',
    weightCard: 'Karta wagi',
    addButton: 'Przycisk dodawania',
    editButton: 'Przycisk edycji',
    deleteButton: 'Przycisk usuwania',
    saveButton: 'Przycisk zapisywania',
    cancelButton: 'Przycisk anulowania',
    refreshButton: 'Przycisk odświeżania',
    tabNavigation: 'Nawigacja zakładek',
    modal: 'Okno dialogowe',
    input: 'Pole wprowadzania',
    picker: 'Wybór opcji',
  },
} as const;

/**
 * Domyślne tłumaczenia (polskie)
 */
export const Translations = PolishTranslations;

// Re-export tłumaczeń dla łatwego dostępu
export { EnglishTranslations };

// Typy dla TypeScript
export type TranslationKey = keyof typeof PolishTranslations;
export type NavigationTranslationKey = keyof typeof PolishTranslations.navigation;
export type ExerciseTranslationKey = keyof typeof PolishTranslations.exercises;
export type TrainingTranslationKey = keyof typeof PolishTranslations.trainings;
export type HistoryTranslationKey = keyof typeof PolishTranslations.history;
export type SettingsTranslationKey = keyof typeof PolishTranslations.settings;
export type MuscleGroupTranslationKey = keyof typeof PolishTranslations.muscleGroups;
export type MuscleGroupRoleTranslationKey = keyof typeof PolishTranslations.muscleGroupRoles;
export type SuccessTranslationKey = keyof typeof PolishTranslations.success;
export type ErrorTranslationKey = keyof typeof PolishTranslations.errors;
export type ConfirmationTranslationKey = keyof typeof PolishTranslations.confirmations;
export type CommonTranslationKey = keyof typeof PolishTranslations.common;
export type DateFormatTranslationKey = keyof typeof PolishTranslations.dateFormat;
export type ValidationTranslationKey = keyof typeof PolishTranslations.validation;
export type PlaceholderTranslationKey = keyof typeof PolishTranslations.placeholders;
export type TooltipTranslationKey = keyof typeof PolishTranslations.tooltips;
export type AccessibilityTranslationKey = keyof typeof PolishTranslations.accessibility;
