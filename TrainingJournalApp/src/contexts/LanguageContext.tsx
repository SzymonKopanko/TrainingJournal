/**
 * Kontekst zarządzania językiem aplikacji
 * Pozwala na przełączanie między językami polskim i angielskim
 */

import React, { createContext, useContext, useEffect, useState, ReactNode } from 'react';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { getTranslations, Language } from '../constants/Translations';

interface LanguageContextType {
  language: Language;
  translations: ReturnType<typeof getTranslations>;
  setLanguage: (language: Language) => void;
}

const LanguageContext = createContext<LanguageContextType | undefined>(undefined);

const LANGUAGE_STORAGE_KEY = '@training_journal_language';

interface LanguageProviderProps {
  children: ReactNode;
}

export const LanguageProvider: React.FC<LanguageProviderProps> = ({ children }) => {
  const [language, setLanguageState] = useState<Language>('pl');
  const [isLoaded, setIsLoaded] = useState(false);

  // Pobierz odpowiednie tłumaczenia dla aktualnego języka
  const translations = getTranslations(language);

  // Załaduj zapisany język przy starcie aplikacji
  useEffect(() => {
    loadLanguage();
  }, []);

  // Zapisz język gdy się zmieni
  useEffect(() => {
    if (isLoaded) {
      saveLanguage(language);
    }
  }, [language, isLoaded]);

  const loadLanguage = async () => {
    try {
      const savedLanguage = await AsyncStorage.getItem(LANGUAGE_STORAGE_KEY);
      if (savedLanguage && ['pl', 'en'].includes(savedLanguage)) {
        setLanguageState(savedLanguage as Language);
      }
    } catch (error) {
      console.error('Błąd ładowania języka:', error);
    } finally {
      setIsLoaded(true);
    }
  };

  const saveLanguage = async (lang: Language) => {
    try {
      await AsyncStorage.setItem(LANGUAGE_STORAGE_KEY, lang);
    } catch (error) {
      console.error('Błąd zapisywania języka:', error);
    }
  };

  const setLanguage = (lang: Language) => {
    setLanguageState(lang);
  };

  const value: LanguageContextType = {
    language,
    translations,
    setLanguage,
  };

  return (
    <LanguageContext.Provider value={value}>
      {children}
    </LanguageContext.Provider>
  );
};

export const useLanguage = (): LanguageContextType => {
  const context = useContext(LanguageContext);
  if (context === undefined) {
    throw new Error('useLanguage must be used within a LanguageProvider');
  }
  return context;
};
