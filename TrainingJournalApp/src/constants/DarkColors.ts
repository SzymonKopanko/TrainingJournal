/**
 * Kolory aplikacji TrainingJournal - Tryb ciemny
 * Kolory dla ciemnego motywu aplikacji
 */

export const DarkColors = {
  // Kolory podstawowe - jaskrawy zielony jako główny
  primary: 'rgba(76, 175, 80, 1)',           // Jaskrawy zielony - główny kolor aplikacji
  primaryDark: 'rgba(56, 142, 60, 1)',       // Ciemniejszy zielony - dla hover/pressed states
  primaryLight: 'rgba(102, 187, 106, 1)',   // Jaśniejszy zielony - dla tła
  
  // Kolory akcentowe - jaskrawy zielony
  accent: 'rgba(76, 175, 80, 1)',            // Jaskrawy zielony - dla akcentów i przycisków akcji
  accentDark: 'rgba(56, 142, 60, 1)',        // Ciemniejszy zielony
  
  // Kolory statusu
  success: 'rgba(76, 175, 80, 1)',           // Jaskrawy zielony - sukces, pozytywne akcje
  successLight: 'rgba(102, 187, 106, 1)',    // Jaśniejszy zielony - tło sukcesu
  warning: 'rgba(255, 152, 0, 1)',           // Pomarańczowy - ostrzeżenia (bez zmian)
  warningLight: 'rgba(255, 183, 77, 1)',     // Jaśniejszy pomarańczowy - tło ostrzeżeń
  error: 'rgba(244, 67, 54, 1)',             // Czerwony - błędy, usuwanie (bez zmian)
  errorLight: 'rgba(239, 83, 80, 1)',        // Jaśniejszy czerwony - tło błędów
  info: 'rgba(76, 175, 80, 1)',              // Jaskrawy zielony - informacje
  
  // Kolory tekstu - białe i jasne szare
  textPrimary: 'rgba(255, 255, 255, 1)',     // Główny kolor tekstu - biały
  textSecondary: 'rgba(224, 224, 224, 1)',   // Drugorzędny tekst - jasny szary
  textDisabled: 'rgba(158, 158, 158, 1)',    // Wyłączony tekst - średni jasny szary
  textOnPrimary: 'rgba(0, 0, 0, 1)',         // Tekst na kolorze primary - czarny (na zielonym)
  textOnAccent: 'rgba(0, 0, 0, 1)',          // Tekst na kolorze accent - czarny (na zielonym)
  
  // Kolory tła - czarny i ciemne szare
  background: 'rgba(0, 0, 0, 1)',            // Główne tło aplikacji - czarny
  backgroundCard: 'rgba(33, 33, 33, 1)',     // Tło kart - ciemny szary
  backgroundOverlay: 'rgba(0, 0, 0, 0.8)',   // Tło overlay - półprzezroczysty czarny
  
  // Kolory granic i separatorów - ciemne szare
  border: 'rgba(66, 66, 66, 1)',             // Granice - ciemny szary
  borderLight: 'rgba(48, 48, 48, 1)',        // Jaśniejsze granice
  divider: 'rgba(66, 66, 66, 1)',            // Separatory - ciemny szary
  
  // Kolory cieni
  shadow: 'rgba(0, 0, 0, 0.4)',              // Cień - półprzezroczysty czarny
  shadowDark: 'rgba(0, 0, 0, 0.6)',          // Ciemniejszy cień
  
  // Kolory dla grup mięśniowych (jaśniejsze dla ciemnego tła)
  muscleGroups: {
    chest: 'rgba(255, 64, 129, 1)',          // Różowy - klatka piersiowa
    back: 'rgba(186, 104, 200, 1)',          // Jaśniejszy fioletowy - plecy
    biceps: 'rgba(100, 115, 255, 1)',        // Jaśniejszy indigo - biceps
    triceps: 'rgba(100, 181, 246, 1)',       // Jaśniejszy niebieski - triceps
    shoulders: 'rgba(77, 208, 225, 1)',      // Jaśniejszy cyjan - barki
    legs: 'rgba(129, 199, 132, 1)',          // Jaśniejszy zielony - nogi
    glutes: 'rgba(174, 213, 129, 1)',        // Jaśniejszy zielony - pośladki
    calves: 'rgba(220, 231, 117, 1)',        // Jaśniejsza limonka - łydki
    abs: 'rgba(255, 245, 157, 1)',           // Jaśniejszy żółty - brzuch
    forearms: 'rgba(255, 183, 77, 1)',       // Jaśniejszy pomarańczowy - przedramiona
    traps: 'rgba(255, 138, 101, 1)',         // Jaśniejszy pomarańczowy - kaptury
    lats: 'rgba(161, 136, 127, 1)',          // Jaśniejszy brązowy - najszersze grzbietu
  },
  
  // Kolory dla ról grup mięśniowych
  muscleGroupRoles: {
    primary: 'rgba(129, 199, 132, 1)',       // Jaśniejszy zielony - grupa główna
    secondary: 'rgba(255, 183, 77, 1)',      // Jaśniejszy pomarańczowy - grupa pomocnicza
  },
  
  // Kolory dla statystyk wagi
  weightStats: {
    current: 'rgba(76, 175, 80, 1)',         // Jaskrawy zielony - obecna waga
    increase: 'rgba(244, 67, 54, 1)',        // Czerwony - przyrost wagi (bez zmian)
    decrease: 'rgba(76, 175, 80, 1)',        // Jaskrawy zielony - spadek wagi
    neutral: 'rgba(158, 158, 158, 1)',       // Średni jasny szary - brak zmian
  },
  
  // Kolory dla nawigacji
  navigation: {
    active: 'rgba(76, 175, 80, 1)',         // Jaskrawy zielony - aktywna zakładka
    inactive: 'rgba(158, 158, 158, 1)',      // Średni jasny szary - nieaktywna zakładka
    background: 'rgba(33, 33, 33, 1)',      // Ciemny szary - tło nawigacji
  },
  
  // Kolory dla przycisków
  buttons: {
    primary: 'rgba(76, 175, 80, 1)',        // Jaskrawy zielony - główny przycisk
    secondary: 'rgba(158, 158, 158, 1)',     // Średni jasny szary - drugorzędny przycisk
    outlined: 'rgba(33, 33, 33, 1)',         // Ciemny szary - przycisk outlined
    danger: 'rgba(244, 67, 54, 1)',          // Czerwony - przycisk usuwania (bez zmian)
    success: 'rgba(76, 175, 80, 1)',        // Jaskrawy zielony - przycisk sukcesu
    disabled: 'rgba(97, 97, 97, 1)',         // Ciemny szary - wyłączony przycisk
  },
  
  // Kolory dla inputów
  inputs: {
    border: 'rgba(66, 66, 66, 1)',           // Ciemny szary - granica inputu
    borderFocused: 'rgba(76, 175, 80, 1)',   // Jaskrawy zielony - granica przy focusie
    background: 'rgba(33, 33, 33, 1)',       // Ciemny szary - tło inputu
    placeholder: 'rgba(158, 158, 158, 1)',   // Średni jasny szary - placeholder
    error: 'rgba(244, 67, 54, 1)',           // Czerwony - błąd w inputcie (bez zmian)
  },
  
  // Kolory dla kart
  cards: {
    background: 'rgba(33, 33, 33, 1)',       // Ciemny szary - tło karty
    border: 'rgba(66, 66, 66, 1)',           // Ciemny szary - granica karty
    shadow: 'rgba(0, 0, 0, 0.3)',            // Cień karty
  },
  
  // Kolory dla modali
  modal: {
    background: 'rgba(33, 33, 33, 1)',       // Ciemny szary - tło modala
    overlay: 'rgba(0, 0, 0, 0.7)',           // Półprzezroczysty - tło overlay
    border: 'rgba(66, 66, 66, 1)',           // Ciemny szary - granica modala
  },
  
  // Kolory dla FAB (Floating Action Button)
  fab: {
    background: 'rgba(76, 175, 80, 1)',      // Jaskrawy zielony - tło FAB
    icon: 'rgba(0, 0, 0, 1)',                 // Czarny - ikona FAB (na zielonym tle)
    shadow: 'rgba(0, 0, 0, 0.5)',            // Cień FAB
  },
  
  // Kolory dla chipów
  chips: {
    primary: 'rgba(76, 175, 80, 1)',         // Jaskrawy zielony - tło chipa primary
    secondary: 'rgba(158, 158, 158, 1)',      // Średni jasny szary - tło chipa secondary
    success: 'rgba(76, 175, 80, 1)',         // Jaskrawy zielony - tło chipa success
    warning: 'rgba(255, 152, 0, 1)',         // Pomarańczowy - tło chipa warning (bez zmian)
    error: 'rgba(244, 67, 54, 1)',          // Czerwony - tło chipa error (bez zmian)
  },
} as const;
