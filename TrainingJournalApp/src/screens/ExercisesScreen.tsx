import React, { useState, useEffect } from 'react';
import {
  View,
  StyleSheet,
  FlatList,
  Alert,
  RefreshControl,
  Text,
} from 'react-native';
import {
  Card,
  Title,
  Paragraph,
  Button,
  FAB,
  Chip,
  Portal,
  Modal,
  TextInput,
  Provider as PaperProvider,
  RadioButton,
} from 'react-native-paper';
import { useNavigation } from '@react-navigation/native';
import type { StackNavigationProp } from '@react-navigation/stack';

type RootStackParamList = {
  EditExercise: { 
    exercise: Exercise | null;
    onExerciseSaved?: () => void;
  };
};

type ExercisesScreenNavigationProp = StackNavigationProp<RootStackParamList, 'EditExercise'>;
import DatabaseService from '../services/DatabaseService';
import { Exercise, CreateExerciseData, MuscleGroup, MuscleGroupRole } from '../types';
import { 
  translateMuscleGroup, 
  translateMuscleGroupRole,
  getMuscleGroupRoleColor 
} from '../constants';
import { useTheme } from '../contexts/ThemeContext';
import { useLanguage } from '../contexts/LanguageContext';

const ExercisesScreen: React.FC = () => {
  const navigation = useNavigation<ExercisesScreenNavigationProp>();
  const { colors } = useTheme();
  const { translations } = useLanguage();
  const [exercises, setExercises] = useState<Exercise[]>([]);
  
  const styles = StyleSheet.create({
    container: {
      flex: 1,
      backgroundColor: colors.background,
    },
    centerContainer: {
      flex: 1,
      justifyContent: 'center',
      alignItems: 'center',
    },
    listContainer: {
      padding: 16,
    },
    card: {
      marginBottom: 16,
      elevation: 2,
      backgroundColor: colors.cards.background,
      borderColor: colors.cards.border,
    },
    chip: {
      marginRight: 8,
      marginBottom: 8,
    },
    muscleGroupsContainer: {
      flexDirection: 'row',
      flexWrap: 'wrap',
      marginTop: 8,
    },
    fab: {
      position: 'absolute',
      margin: 16,
      right: 0,
      bottom: 0,
      backgroundColor: colors.fab.background,
    },
    modalContainer: {
      backgroundColor: colors.modal.background,
      padding: 20,
      margin: 20,
      borderRadius: 8,
      borderColor: colors.modal.border,
    },
    input: {
      marginBottom: 16,
    },
    button: {
      marginTop: 16,
    },
    sectionTitle: {
      marginTop: 16,
      marginBottom: 8,
    },
    addButton: {
      marginTop: 8,
    },
    modalActions: {
      flexDirection: 'row',
      justifyContent: 'space-between',
      marginTop: 24,
    },
    modalButton: {
      flex: 1,
      marginHorizontal: 8,
    },
    muscleGroupItem: {
      marginBottom: 16,
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
  });
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [modalVisible, setModalVisible] = useState(false);
  const [editingExercise, setEditingExercise] = useState<Exercise | null>(null);
  
  // Form state
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [selectedMuscleGroups, setSelectedMuscleGroups] = useState<{ muscleGroup: MuscleGroup; role: MuscleGroupRole }[]>([]);

  useEffect(() => {
    loadExercises();
  }, []);

  const loadExercises = async () => {
    try {
      const data = await DatabaseService.getExercises();
      setExercises(data);
    } catch (error) {
      console.error('Błąd ładowania ćwiczeń:', error);
      Alert.alert(translations.common.error, translations.errors.loadingExercises);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  const handleRefresh = () => {
    setRefreshing(true);
    loadExercises();
  };

  const resetForm = () => {
    setName('');
    setDescription('');
    setSelectedMuscleGroups([]);
    setEditingExercise(null);
  };

  const openAddModal = () => {
    resetForm();
    setModalVisible(true);
  };

  const openEditModal = (exercise: Exercise) => {
    setName(exercise.name);
    setDescription(exercise.description || '');
    setSelectedMuscleGroups(exercise.muscleGroups.map(emg => ({
      muscleGroup: emg.muscleGroup,
      role: emg.role
    })));
    setEditingExercise(exercise);
    setModalVisible(true);
  };

  const closeModal = () => {
    setModalVisible(false);
    resetForm();
  };

  const addMuscleGroup = () => {
    // Znajdź pierwszą dostępną grupę mięśniową
    const availableGroups = Object.values(MuscleGroup).filter(group => 
      !selectedMuscleGroups.some(mg => mg.muscleGroup === group)
    );
    
    if (availableGroups.length === 0) {
      Alert.alert(translations.common.info, 'Wszystkie grupy mięśniowe zostały już dodane');
      return;
    }
    
    setSelectedMuscleGroups([...selectedMuscleGroups, {
      muscleGroup: availableGroups[0],
      role: MuscleGroupRole.Primary
    }]);
  };

  const removeMuscleGroup = (index: number) => {
    setSelectedMuscleGroups(selectedMuscleGroups.filter((_, i) => i !== index));
  };

  const updateMuscleGroup = (index: number, field: 'muscleGroup' | 'role', value: any) => {
    const updated = [...selectedMuscleGroups];
    updated[index] = { ...updated[index], [field]: value };
    setSelectedMuscleGroups(updated);
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

    try {
      const data: CreateExerciseData = {
        name: name.trim(),
        description: description.trim() || undefined,
        bodyWeightPercentage: 0, // Domyślna wartość
        muscleGroups: selectedMuscleGroups
      };

      if (editingExercise) {
        await DatabaseService.updateExercise(editingExercise.id, data);
      } else {
        await DatabaseService.createExercise(data);
      }

      closeModal();
      loadExercises();
      Alert.alert(translations.common.success, editingExercise ? translations.success.exerciseUpdated : translations.success.exerciseAdded);
    } catch (error) {
      console.error('Błąd zapisywania ćwiczenia:', error);
      Alert.alert(translations.common.error, translations.errors.savingExercise);
    }
  };

  const handleDelete = (exercise: Exercise) => {
    Alert.alert(
      translations.confirmations.deleteExercise,
      `${translations.confirmations.deleteExerciseMessage} "${exercise.name}"?`,
      [
        { text: translations.common.cancel, style: 'cancel' },
        {
          text: translations.common.delete,
          style: 'destructive',
          onPress: async () => {
            try {
              await DatabaseService.deleteExercise(exercise.id);
              loadExercises();
              Alert.alert(translations.common.success, translations.success.exerciseDeleted);
            } catch (error) {
              console.error('Błąd usuwania ćwiczenia:', error);
              Alert.alert(translations.common.error, translations.errors.deletingExercise);
            }
          }
        }
      ]
    );
  };

  const renderMuscleGroupChip = (muscleGroup: MuscleGroup, role: MuscleGroupRole, index: number) => {
    const roleColor = getMuscleGroupRoleColor(role);
    const translatedMuscleGroup = translateMuscleGroup(muscleGroup);
    const translatedRole = translateMuscleGroupRole(role);

    return (
      <Chip
        key={index}
        style={[styles.chip, { backgroundColor: roleColor }]}
        textStyle={{ color: colors.textOnPrimary }}
      >
        {translatedMuscleGroup} ({translatedRole})
      </Chip>
    );
  };

  const renderExercise = ({ item }: { item: Exercise }) => (
    <Card style={styles.card}>
      <Card.Content>
        <Title style={{ color: colors.textPrimary }}>{item.name}</Title>
        {item.description && <Paragraph style={{ color: colors.textSecondary }}>{item.description}</Paragraph>}
        <View style={styles.muscleGroupsContainer}>
          {item.muscleGroups.map((emg, index) => 
            renderMuscleGroupChip(emg.muscleGroup, emg.role, index)
          )}
        </View>
      </Card.Content>
      <Card.Actions>
        <Button onPress={() => navigation.navigate('EditExercise', { 
          exercise: item, 
          onExerciseSaved: loadExercises 
        })}>
          {translations.common.edit}
        </Button>
        <Button onPress={() => handleDelete(item)} textColor={colors.error}>
          {translations.common.delete}
        </Button>
      </Card.Actions>
    </Card>
  );

  if (loading) {
    return (
      <View style={styles.centerContainer}>
        <Title style={{ color: colors.textPrimary }}>Ładowanie...</Title>
      </View>
    );
  }

  return (
    <PaperProvider>
      <View style={styles.container}>
        <FlatList
          data={exercises}
          renderItem={renderExercise}
          keyExtractor={(item) => item.id.toString()}
          refreshControl={
            <RefreshControl refreshing={refreshing} onRefresh={handleRefresh} />
          }
          contentContainerStyle={styles.listContainer}
        />
        
        <FAB
          style={styles.fab}
          icon="plus"
          onPress={() => navigation.navigate('EditExercise', { 
            exercise: null, 
            onExerciseSaved: loadExercises 
          })}
          label={translations.exercises.addExercise}
        />

      </View>
    </PaperProvider>
  );
};


export default ExercisesScreen;
