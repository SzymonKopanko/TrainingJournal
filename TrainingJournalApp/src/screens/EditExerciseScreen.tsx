import React, { useState, useEffect } from 'react';
import {
  View,
  StyleSheet,
  ScrollView,
  Alert,
  Text,
} from 'react-native';
import {
  Card,
  Title,
  Button,
  TextInput,
  Provider as PaperProvider,
  RadioButton,
} from 'react-native-paper';
import { useNavigation, useRoute } from '@react-navigation/native';
import DatabaseService from '../services/DatabaseService';
import { Exercise, CreateExerciseData, MuscleGroup, MuscleGroupRole } from '../types';
import { 
  translateMuscleGroup, 
  translateMuscleGroupRole,
  getMuscleGroupRoleColor 
} from '../constants';
import { useTheme } from '../contexts/ThemeContext';
import { useLanguage } from '../contexts/LanguageContext';

const EditExerciseScreen: React.FC = () => {
  const navigation = useNavigation();
  const route = useRoute();
  const { colors } = useTheme();
  const { translations } = useLanguage();
  
  // Pobierz exercise z parametrów lub null dla nowego ćwiczenia
  const { exercise, onExerciseSaved } = route.params as { 
    exercise: Exercise | null; 
    onExerciseSaved?: () => void;
  };
  const isEditing = !!exercise;
  
  const styles = StyleSheet.create({
    container: {
      flex: 1,
      backgroundColor: colors.background,
    },
    scrollContainer: {
      padding: 16,
    },
    card: {
      marginBottom: 16,
      elevation: 2,
      backgroundColor: colors.cards.background,
      borderColor: colors.cards.border,
    },
    input: {
      marginBottom: 16,
    },
    selectedMuscleGroupItem: {
      flexDirection: 'row',
      justifyContent: 'space-between',
      alignItems: 'center',
      marginBottom: 8,
      padding: 12,
      backgroundColor: colors.cards.background,
      borderRadius: 8,
      borderWidth: 1,
      borderColor: colors.border,
    },
    radioItem: {
      flexDirection: 'row',
      alignItems: 'center',
      marginVertical: 4,
    },
    removeButton: {
      marginTop: 16,
    },
    addButton: {
      marginTop: 8,
    },
    actions: {
      flexDirection: 'row',
      justifyContent: 'space-between',
      marginTop: 24,
      paddingHorizontal: 16,
      paddingBottom: 16,
    },
    actionButton: {
      flex: 1,
      marginHorizontal: 8,
    },
  });

  // Form state
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [bodyWeightPercentage, setBodyWeightPercentage] = useState('0');
  const [selectedMuscleGroups, setSelectedMuscleGroups] = useState<{ muscleGroup: MuscleGroup; role: MuscleGroupRole }[]>([]);
  const [newMuscleGroup, setNewMuscleGroup] = useState<MuscleGroup | null>(null);
  const [newMuscleGroupRole, setNewMuscleGroupRole] = useState<MuscleGroupRole | null>(null);

  useEffect(() => {
    if (exercise) {
      setName(exercise.name);
      setDescription(exercise.description || '');
      setBodyWeightPercentage(exercise.bodyWeightPercentage.toString());
      setSelectedMuscleGroups(exercise.muscleGroups.map((emg: any) => ({
        muscleGroup: emg.muscleGroup,
        role: emg.role
      })));
    }
  }, [exercise]);

  const addMuscleGroup = () => {
    if (!newMuscleGroup || !newMuscleGroupRole) {
      Alert.alert(translations.common.error, translations.errors.muscleGroupRequired);
      return;
    }
    
    setSelectedMuscleGroups([...selectedMuscleGroups, {
      muscleGroup: newMuscleGroup,
      role: newMuscleGroupRole
    }]);
    
    // Resetuj wybory
    setNewMuscleGroup(null);
    setNewMuscleGroupRole(null);
  };

  const removeMuscleGroup = (index: number) => {
    setSelectedMuscleGroups(selectedMuscleGroups.filter((_, i) => i !== index));
  };

  const handleSave = async () => {
    if (!name.trim()) {
      Alert.alert(translations.common.error, translations.errors.exerciseNameRequired);
      return;
    }

    if (selectedMuscleGroups.length === 0) {
      Alert.alert(translations.common.error, translations.errors.muscleGroupRequired);
      return;
    }

    const bodyWeightValue = parseFloat(bodyWeightPercentage);
    if (isNaN(bodyWeightValue) || bodyWeightValue < 0) {
      Alert.alert(translations.common.error, 'Procent masy ciała musi być liczbą większą lub równą 0');
      return;
    }

    try {
      const data: CreateExerciseData = {
        name: name.trim(),
        description: description.trim() || undefined,
        bodyWeightPercentage: bodyWeightValue,
        muscleGroups: selectedMuscleGroups
      };

      if (isEditing) {
        await DatabaseService.updateExercise(exercise.id, data);
      } else {
        await DatabaseService.createExercise(data);
      }
      
      Alert.alert(translations.common.success, isEditing ? translations.success.exerciseUpdated : translations.success.exerciseAdded, [
        { 
          text: translations.common.ok, 
          onPress: () => {
            // Wywołaj callback jeśli istnieje
            if (onExerciseSaved) {
              onExerciseSaved();
            }
            navigation.goBack();
          }
        }
      ]);
    } catch (error) {
      console.error('Błąd zapisywania ćwiczenia:', error);
      Alert.alert(translations.common.error, translations.errors.savingExercise);
    }
  };

  const handleCancel = () => {
    navigation.goBack();
  };

  return (
    <PaperProvider>
      <View style={styles.container}>
        <ScrollView style={styles.scrollContainer}>
          {/* Podstawowe informacje */}
          <Card style={styles.card}>
            <Card.Content>
              <Title style={{ color: colors.textPrimary }}>
                {isEditing ? translations.exercises.editExercise : translations.exercises.addExercise}
              </Title>
              
              <TextInput
                label={translations.exercises.exerciseName}
                value={name}
                onChangeText={setName}
                style={styles.input}
                mode="outlined"
                textColor={colors.textPrimary}
                placeholderTextColor={colors.inputs.placeholder}
                outlineColor={colors.inputs.border}
                activeOutlineColor={colors.inputs.borderFocused}
              />
              
              <TextInput
                label={translations.exercises.exerciseDescription}
                value={description}
                onChangeText={setDescription}
                style={styles.input}
                mode="outlined"
                multiline
                numberOfLines={3}
                textColor={colors.textPrimary}
                placeholderTextColor={colors.inputs.placeholder}
                outlineColor={colors.inputs.border}
                activeOutlineColor={colors.inputs.borderFocused}
              />
              
              <TextInput
                label={translations.exercises.bodyWeightPercentage}
                value={bodyWeightPercentage}
                onChangeText={setBodyWeightPercentage}
                style={styles.input}
                mode="outlined"
                keyboardType="numeric"
                textColor={colors.textPrimary}
                placeholderTextColor={colors.inputs.placeholder}
                outlineColor={colors.inputs.border}
                activeOutlineColor={colors.inputs.borderFocused}
                placeholder="0"
              />
              <Text style={{ color: colors.textSecondary, fontSize: 12, marginTop: -8, marginBottom: 8 }}>
                {translations.exercises.bodyWeightPercentageDescription}
              </Text>
            </Card.Content>
          </Card>

          {/* Grupy mięśniowe */}
          <Card style={styles.card}>
            <Card.Content>
              <Title style={{ color: colors.textPrimary }}>{translations.exercises.muscleGroups}</Title>
              
              {/* Lista wybranych grup mięśniowych */}
              <View style={{ marginTop: 16 }}>
                {selectedMuscleGroups.map((mg, index) => (
                  <View key={index} style={styles.selectedMuscleGroupItem}>
                    <Text style={{ color: colors.textPrimary }}>
                      {translateMuscleGroup(mg.muscleGroup)} ({translateMuscleGroupRole(mg.role)})
                    </Text>
                    <Button
                      mode="outlined"
                      onPress={() => removeMuscleGroup(index)}
                      style={styles.removeButton}
                      textColor={colors.error}
                      buttonColor={colors.buttons.outlined}
                      compact
                    >
                      {translations.common.delete}
                    </Button>
                  </View>
                ))}
              </View>

              {/* Dodawanie nowej grupy mięśniowej */}
              {selectedMuscleGroups.length < Object.values(MuscleGroup).length && (
                <View style={{ marginTop: 16 }}>
                  <Text style={{ color: colors.textSecondary, marginBottom: 8 }}>
                    {translations.exercises.addMuscleGroup}:
                  </Text>
                  
                  <Text style={{ color: colors.textSecondary, fontSize: 12, marginBottom: 8 }}>
                    {translations.exercises.muscleGroup}:
                  </Text>
                  <RadioButton.Group 
                    onValueChange={(value) => setNewMuscleGroup(value as MuscleGroup)} 
                    value={newMuscleGroup || ''}
                  >
                    {Object.values(MuscleGroup)
                      .filter(group => !selectedMuscleGroups.some(mg => mg.muscleGroup === group))
                      .map(group => (
                        <View key={group} style={styles.radioItem}>
                          <RadioButton value={group} color={colors.primary} />
                          <Text style={{ color: colors.textPrimary, marginLeft: 8 }}>
                            {translateMuscleGroup(group)}
                          </Text>
                        </View>
                      ))}
                  </RadioButton.Group>

                  <Text style={{ color: colors.textSecondary, fontSize: 12, marginTop: 16, marginBottom: 8 }}>
                    {translations.exercises.role}:
                  </Text>
                  <RadioButton.Group 
                    onValueChange={(value) => setNewMuscleGroupRole(value as MuscleGroupRole)} 
                    value={newMuscleGroupRole || ''}
                  >
                    {Object.values(MuscleGroupRole).map(role => (
                      <View key={role} style={styles.radioItem}>
                        <RadioButton value={role} color={colors.primary} />
                        <Text style={{ color: colors.textPrimary, marginLeft: 8 }}>
                          {translateMuscleGroupRole(role)}
                        </Text>
                      </View>
                    ))}
                  </RadioButton.Group>

                  <Button
                    mode="contained"
                    onPress={addMuscleGroup}
                    style={styles.addButton}
                    textColor={colors.textOnPrimary}
                    buttonColor={colors.primary}
                    disabled={!newMuscleGroup || !newMuscleGroupRole}
                  >
                    {translations.exercises.addMuscleGroup}
                  </Button>
                </View>
              )}
            </Card.Content>
          </Card>
        </ScrollView>

        {/* Przyciski akcji */}
        <View style={styles.actions}>
          <Button 
            onPress={handleCancel} 
            style={styles.actionButton}
            textColor={colors.textPrimary}
            buttonColor={colors.buttons.outlined}
          >
            {translations.common.cancel}
          </Button>
          <Button 
            mode="contained" 
            onPress={handleSave} 
            style={styles.actionButton}
            textColor={colors.textOnPrimary}
            buttonColor={colors.primary}
          >
            {translations.common.save}
          </Button>
        </View>
      </View>
    </PaperProvider>
  );
};

export default EditExerciseScreen;
