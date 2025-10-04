/**
 * Kolory aplikacji TrainingJournal - Tryb jasny
 * Kolory dla jasnego motywu aplikacji
 */

export const LightColors = {
  // Kolory podstawowe
  primary: 'rgba(33, 150, 243, 1)',           // Niebieski - główny kolor aplikacji
  primaryDark: 'rgba(25, 118, 210, 1)',      // Ciemniejszy niebieski - dla hover/pressed states
  primaryLight: 'rgba(187, 222, 251, 1)',    // Jaśniejszy niebieski - dla tła
  
  // Kolory akcentowe
  accent: 'rgba(255, 64, 129, 1)',           // Różowy - dla akcentów i przycisków akcji
  accentDark: 'rgba(245, 0, 87, 1)',         // Ciemniejszy różowy
  
  // Kolory statusu
  success: 'rgba(76, 175, 80, 1)',           // Zielony - sukces, pozytywne akcje
  successLight: 'rgba(200, 230, 201, 1)',    // Jaśniejszy zielony - tło sukcesu
  warning: 'rgba(255, 152, 0, 1)',           // Pomarańczowy - ostrzeżenia
  warningLight: 'rgba(255, 224, 178, 1)',    // Jaśniejszy pomarańczowy - tło ostrzeżeń
  error: 'rgba(244, 67, 54, 1)',             // Czerwony - błędy, usuwanie
  errorLight: 'rgba(255, 205, 210, 1)',      // Jaśniejszy czerwony - tło błędów
  info: 'rgba(33, 150, 243, 1)',             // Niebieski - informacje
  
  // Kolory tekstu
  textPrimary: 'rgba(33, 33, 33, 1)',        // Główny kolor tekstu - ciemny szary
  textSecondary: 'rgba(117, 117, 117, 1)',   // Drugorzędny tekst - średni szary
  textDisabled: 'rgba(189, 189, 189, 1)',    // Wyłączony tekst - jasny szary
  textOnPrimary: 'rgba(255, 255, 255, 1)',   // Tekst na kolorze primary - biały
  textOnAccent: 'rgba(255, 255, 255, 1)',    // Tekst na kolorze accent - biały
  
  // Kolory tła
  background: 'rgba(250, 250, 250, 1)',      // Główne tło aplikacji - bardzo jasny szary
  backgroundCard: 'rgba(255, 255, 255, 1)',  // Tło kart - biały
  backgroundOverlay: 'rgba(0, 0, 0, 0.5)',   // Tło overlay - półprzezroczysty czarny
  
  // Kolory granic i separatorów
  border: 'rgba(224, 224, 224, 1)',          // Granice - jasny szary
  borderLight: 'rgba(245, 245, 245, 1)',     // Jaśniejsze granice
  divider: 'rgba(224, 224, 224, 1)',         // Separatory - jasny szary
  
  // Kolory cieni
  shadow: 'rgba(0, 0, 0, 0.1)',              // Cień - półprzezroczysty czarny
  shadowDark: 'rgba(0, 0, 0, 0.2)',          // Ciemniejszy cień
  
  // Kolory dla grup mięśniowych
  muscleGroups: {
    chest: 'rgba(233, 30, 99, 1)',          // Różowy - klatka piersiowa
    back: 'rgba(156, 39, 176, 1)',          // Fioletowy - plecy
    biceps: 'rgba(63, 81, 181, 1)',         // Indigo - biceps
    triceps: 'rgba(33, 150, 243, 1)',       // Niebieski - triceps
    shoulders: 'rgba(0, 188, 212, 1)',      // Cyjan - barki
    legs: 'rgba(76, 175, 80, 1)',           // Zielony - nogi
    glutes: 'rgba(139, 195, 74, 1)',       // Jasny zielony - pośladki
    calves: 'rgba(205, 220, 57, 1)',       // Limonka - łydki
    abs: 'rgba(255, 235, 59, 1)',           // Żółty - brzuch
    forearms: 'rgba(255, 152, 0, 1)',       // Pomarańczowy - przedramiona
    traps: 'rgba(255, 87, 34, 1)',          // Głęboki pomarańczowy - kaptury
    lats: 'rgba(121, 85, 72, 1)',           // Brązowy - najszersze grzbietu
  },
  
  // Kolory dla ról grup mięśniowych
  muscleGroupRoles: {
    primary: 'rgba(76, 175, 80, 1)',        // Zielony - grupa główna
    secondary: 'rgba(255, 152, 0, 1)',      // Pomarańczowy - grupa pomocnicza
  },
  
  // Kolory dla statystyk wagi
  weightStats: {
    current: 'rgba(33, 150, 243, 1)',       // Niebieski - obecna waga
    increase: 'rgba(244, 67, 54, 1)',        // Czerwony - przyrost wagi
    decrease: 'rgba(76, 175, 80, 1)',        // Zielony - spadek wagi
    neutral: 'rgba(117, 117, 117, 1)',       // Szary - brak zmian
  },
  
  // Kolory dla nawigacji
  navigation: {
    active: 'rgba(33, 150, 243, 1)',        // Niebieski - aktywna zakładka
    inactive: 'rgba(117, 117, 117, 1)',     // Szary - nieaktywna zakładka
    background: 'rgba(255, 255, 255, 1)',   // Biały - tło nawigacji
  },
  
  // Kolory dla przycisków
  buttons: {
    primary: 'rgba(33, 150, 243, 1)',        // Niebieski - główny przycisk
    secondary: 'rgba(117, 117, 117, 1)',     // Szary - drugorzędny przycisk
    danger: 'rgba(244, 67, 54, 1)',          // Czerwony - przycisk usuwania
    success: 'rgba(76, 175, 80, 1)',         // Zielony - przycisk sukcesu
    disabled: 'rgba(189, 189, 189, 1)',      // Szary - wyłączony przycisk
  },
  
  // Kolory dla inputów
  inputs: {
    border: 'rgba(224, 224, 224, 1)',        // Szary - granica inputu
    borderFocused: 'rgba(33, 150, 243, 1)', // Niebieski - granica przy focusie
    background: 'rgba(255, 255, 255, 1)',    // Biały - tło inputu
    placeholder: 'rgba(189, 189, 189, 1)',   // Szary - placeholder
    error: 'rgba(244, 67, 54, 1)',           // Czerwony - błąd w inputcie
  },
  
  // Kolory dla kart
  cards: {
    background: 'rgba(255, 255, 255, 1)',    // Biały - tło karty
    border: 'rgba(224, 224, 224, 1)',        // Szary - granica karty
    shadow: 'rgba(0, 0, 0, 0.1)',            // Cień karty
  },
  
  // Kolory dla modali
  modal: {
    background: 'rgba(255, 255, 255, 1)',    // Biały - tło modala
    overlay: 'rgba(0, 0, 0, 0.5)',           // Półprzezroczysty - tło overlay
    border: 'rgba(224, 224, 224, 1)',        // Szary - granica modala
  },
  
  // Kolory dla FAB (Floating Action Button)
  fab: {
    background: 'rgba(33, 150, 243, 1)',     // Niebieski - tło FAB
    icon: 'rgba(255, 255, 255, 1)',          // Biały - ikona FAB
    shadow: 'rgba(0, 0, 0, 0.2)',            // Cień FAB
  },
  
  // Kolory dla chipów
  chips: {
    primary: 'rgba(227, 242, 253, 1)',        // Jasny niebieski - tło chipa primary
    secondary: 'rgba(243, 229, 245, 1)',    // Jasny fioletowy - tło chipa secondary
    success: 'rgba(232, 245, 232, 1)',        // Jasny zielony - tło chipa success
    warning: 'rgba(255, 243, 224, 1)',       // Jasny pomarańczowy - tło chipa warning
    error: 'rgba(255, 235, 238, 1)',         // Jasny czerwony - tło chipa error
  },
} as const;
