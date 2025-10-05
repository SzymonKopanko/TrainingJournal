/**
 * TrainingJournal App
 * Aplikacja do zarządzania dziennikiem treningowym
 *
 * @format
 */

import React, { useEffect, useState } from 'react';
import { StatusBar, StyleSheet, useColorScheme } from 'react-native';
import { SafeAreaProvider } from 'react-native-safe-area-context';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createStackNavigator } from '@react-navigation/stack';
import { Provider as PaperProvider } from 'react-native-paper';
import Icon from 'react-native-vector-icons/MaterialIcons';

import DatabaseService from './src/services/DatabaseService';
import ExercisesScreen from './src/screens/ExercisesScreen';
import EditExerciseScreen from './src/screens/EditExerciseScreen';
import TrainingsScreen from './src/screens/TrainingsScreen';
import HistoryScreen from './src/screens/HistoryScreen';
import ProfileScreen from './src/screens/ProfileScreen';
import SettingsScreen from './src/screens/SettingsScreen';
import { Colors, Translations } from './src/constants';
import { ThemeProvider, useTheme } from './src/contexts/ThemeContext';
import { LanguageProvider, useLanguage } from './src/contexts/LanguageContext';

const Tab = createBottomTabNavigator();
const Stack = createStackNavigator();

function ExercisesStack() {
  return (
    <Stack.Navigator
      screenOptions={{
        headerShown: false, // Usuń header
      }}
    >
      <Stack.Screen 
        name="ExercisesList" 
        component={ExercisesScreen}
      />
      <Stack.Screen 
        name="EditExercise" 
        component={EditExerciseScreen}
      />
    </Stack.Navigator>
  );
}

function AppContent() {
  const { isDark, colors } = useTheme();
  const { translations } = useLanguage();
  const [isDatabaseReady, setIsDatabaseReady] = useState(false);

  useEffect(() => {
    initializeDatabase();
  }, []);

  const initializeDatabase = async () => {
    try {
      await DatabaseService.initDatabase();
      setIsDatabaseReady(true);
      console.log('Aplikacja gotowa do użycia');
    } catch (error) {
      console.error('Błąd inicjalizacji aplikacji:', error);
    }
  };

  if (!isDatabaseReady) {
    return null; // Można dodać loading screen
  }

  return (
    <SafeAreaProvider>
      <PaperProvider theme={isDark ? { dark: true } : { dark: false }}>
        <NavigationContainer>
          <StatusBar barStyle={isDark ? 'light-content' : 'dark-content'} />
          <Tab.Navigator
            screenOptions={({ route }) => ({
              tabBarIcon: ({ focused, color, size }) => {
                let iconName: string;

                switch (route.name) {
                  case 'Exercises':
                    iconName = 'fitness-center';
                    break;
                  case 'Trainings':
                    iconName = 'directions-run';
                    break;
                  case 'History':
                    iconName = 'history';
                    break;
                  case 'Profile':
                    iconName = 'person';
                    break;
                  case 'Settings':
                    iconName = 'settings';
                    break;
                  default:
                    iconName = 'help';
                }

                return <Icon name={iconName} size={size} color={color} />;
              },
              tabBarActiveTintColor: colors.navigation.active,
              tabBarInactiveTintColor: colors.navigation.inactive,
              tabBarStyle: {
                backgroundColor: colors.navigation.background,
                borderTopColor: colors.border,
              },
              headerStyle: {
                backgroundColor: colors.primary,
              },
              headerTintColor: colors.textOnPrimary,
              headerTitleStyle: {
                fontWeight: 'bold',
              },
            })}
          >
            <Tab.Screen 
              name="Exercises" 
              component={ExercisesStack}
              options={{ title: translations.navigation.exercises }}
            />
            <Tab.Screen 
              name="Trainings" 
              component={TrainingsScreen}
              options={{ title: translations.navigation.trainings }}
            />
            <Tab.Screen 
              name="History" 
              component={HistoryScreen}
              options={{ title: translations.navigation.history }}
            />
            <Tab.Screen 
              name="Profile" 
              component={ProfileScreen}
              options={{ title: translations.navigation.profile }}
            />
            <Tab.Screen 
              name="Settings" 
              component={SettingsScreen}
              options={{ title: translations.settings.title }}
            />
          </Tab.Navigator>
        </NavigationContainer>
      </PaperProvider>
    </SafeAreaProvider>
  );
}

function App() {
  return (
    <LanguageProvider>
      <ThemeProvider>
        <AppContent />
      </ThemeProvider>
    </LanguageProvider>
  );
}

export default App;
