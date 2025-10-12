import React, { useState, useEffect } from 'react';
import {
  View,
  StyleSheet,
  FlatList,
  Alert,
  RefreshControl,
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
  List,
} from 'react-native-paper';
import { useNavigation, useRoute, useFocusEffect } from '@react-navigation/native';
import DatabaseService from '../services/DatabaseService';
import { Training, Exercise, TrainingExercise } from '../types';
import { translateMuscleGroup, translateMuscleGroupRole } from '../constants';
import { useTheme } from '../contexts/ThemeContext';
import { useLanguage } from '../contexts/LanguageContext';

const TrainingDetailsScreen: React.FC = () => {
  const navigation = useNavigation();
  const route = useRoute();
  const { colors } = useTheme();
  const { translations } = useLanguage();
  
  const { training } = route.params as { training: Training };
  
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
    exerciseItem: {
      flexDirection: 'row',
      justifyContent: 'space-between',
      alignItems: 'center',
      paddingVertical: 8,
      paddingHorizontal: 16,
      backgroundColor: colors.cards.background,
      borderRadius: 8,
      marginBottom: 8,
      borderWidth: 1,
      borderColor: colors.border,
    },
    muscleGroupChip: {
      marginRight: 4,
      marginBottom: 4,
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
  });

  const [trainingDetails, setTrainingDetails] = useState<Training | null>(null);
  const [availableExercises, setAvailableExercises] = useState<Exercise[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [modalVisible, setModalVisible] = useState(false);
  const [selectedExercise, setSelectedExercise] = useState<Exercise | null>(null);

  useEffect(() => {
    loadTrainingDetails();
  }, []);

  useFocusEffect(
    React.useCallback(() => {
      loadTrainingDetails();
    }, [])
  );

  const loadTrainingDetails = async () => {
    try {
      setLoading(true);
      const [trainingsData, exercisesData] = await Promise.all([
        DatabaseService.getTrainings(),
        DatabaseService.getExercises()
      ]);
      
      const currentTraining = trainingsData.find(t => t.id === training.id);
      setTrainingDetails(currentTraining || training);
      setAvailableExercises(exercisesData);
    } catch (error) {
      console.error('Błąd ładowania szczegółów treningu:', error);
      Alert.alert(translations.common.error, translations.errors.loadingTrainings);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  const handleRefresh = () => {
    setRefreshing(true);
    loadTrainingDetails();
  };

  const openAddExerciseModal = () => {
    setModalVisible(true);
  };

  const closeModal = () => {
    setModalVisible(false);
    setSelectedExercise(null);
  };

  const handleAddExercise = async () => {
    if (!selectedExercise || !trainingDetails) {
      Alert.alert(translations.common.error, translations.trainings.selectExerciseMessage);
      return;
    }

    try {
      await DatabaseService.addExerciseToTraining(trainingDetails.id, selectedExercise.id);
      Alert.alert(translations.common.success, 'Ćwiczenie zostało dodane do treningu');
      closeModal();
      loadTrainingDetails();
    } catch (error) {
      console.error('Błąd dodawania ćwiczenia:', error);
      Alert.alert(translations.common.error, error.message || 'Błąd podczas dodawania ćwiczenia');
    }
  };

  const handleRemoveExercise = (trainingExercise: TrainingExercise) => {
      Alert.alert(
      translations.trainings.removeExercise,
      `${translations.confirmations.removeExerciseFromTrainingMessage} "${trainingExercise.exercise.name}"?`,
      [
        { text: translations.common.cancel, style: 'cancel' },
        {
          text: translations.common.delete,
          style: 'destructive',
          onPress: async () => {
            try {
              await DatabaseService.removeExerciseFromTraining(trainingExercise.id);
              Alert.alert(translations.common.success, 'Ćwiczenie zostało usunięte z treningu');
              loadTrainingDetails();
            } catch (error) {
              console.error('Błąd usuwania ćwiczenia:', error);
              Alert.alert(translations.common.error, error.message || 'Błąd podczas usuwania ćwiczenia');
            }
          }
        }
      ]
    );
  };

  const renderMuscleGroupChip = (muscleGroup: any, role: any, index: number) => (
    <Chip
      key={index}
      style={[
        styles.muscleGroupChip,
        { backgroundColor: colors.primary }
      ]}
      textStyle={{ color: colors.textOnPrimary }}
      compact
    >
      {translateMuscleGroup(muscleGroup)} ({translateMuscleGroupRole(role)})
    </Chip>
  );

  const renderExercise = ({ item }: { item: TrainingExercise }) => (
    <View style={styles.exerciseItem}>
      <View style={{ flex: 1 }}>
        <Title style={{ color: colors.textPrimary, fontSize: 16 }}>
          {item.exercise.name}
        </Title>
        {item.exercise.description && (
          <Paragraph style={{ color: colors.textSecondary, fontSize: 12 }}>
            {item.exercise.description}
          </Paragraph>
        )}
        <View style={{ flexDirection: 'row', flexWrap: 'wrap', marginTop: 4 }}>
          {item.exercise.muscleGroups.map((emg: any, index: number) =>
            renderMuscleGroupChip(emg.muscleGroup, emg.role, index)
          )}
        </View>
        <Paragraph style={{ color: colors.textSecondary, fontSize: 12, marginTop: 4 }}>
          Serii: {item.sets.length}
        </Paragraph>
      </View>
      <Button
        mode="outlined"
        onPress={() => handleRemoveExercise(item)}
        textColor={colors.error}
        buttonColor={colors.buttons.outlined}
        compact
      >
        {translations.common.delete}
      </Button>
    </View>
  );

  const renderAvailableExercise = ({ item }: { item: Exercise }) => (
    <List.Item
      title={item.name}
      description={item.description}
      onPress={() => setSelectedExercise(item)}
      style={{
        backgroundColor: selectedExercise?.id === item.id ? colors.primary : 'transparent',
      }}
      titleStyle={{
        color: selectedExercise?.id === item.id ? colors.textOnPrimary : colors.textPrimary,
      }}
      descriptionStyle={{
        color: selectedExercise?.id === item.id ? colors.textOnPrimary : colors.textSecondary,
      }}
    />
  );

  if (loading) {
    return (
      <View style={[styles.container, { justifyContent: 'center', alignItems: 'center' }]}>
        <Title style={{ color: colors.textPrimary }}>{translations.common.loading}</Title>
      </View>
    );
  }

  if (!trainingDetails) {
    return (
      <View style={[styles.container, { justifyContent: 'center', alignItems: 'center' }]}>
        <Title style={{ color: colors.textPrimary }}>Trening nie został znaleziony</Title>
      </View>
    );
  }

  return (
    <PaperProvider>
      <View style={styles.container}>
        <FlatList
          data={trainingDetails.exercises}
          renderItem={renderExercise}
          keyExtractor={(item) => item.id.toString()}
          refreshControl={
            <RefreshControl refreshing={refreshing} onRefresh={handleRefresh} />
          }
          contentContainerStyle={styles.scrollContainer}
          ListHeaderComponent={
            <Card style={styles.card}>
              <Card.Content>
                <Title style={{ color: colors.textPrimary }}>{trainingDetails.name}</Title>
                {trainingDetails.description && (
                  <Paragraph style={{ color: colors.textSecondary }}>
                    {trainingDetails.description}
                  </Paragraph>
                )}
                <Paragraph style={{ color: colors.textSecondary, fontSize: 12, marginTop: 8 }}>
                  Data: {new Date(trainingDetails.createdAt).toLocaleDateString('pl-PL')}
                </Paragraph>
                <Paragraph style={{ color: colors.textSecondary, fontSize: 12 }}>
                  Ćwiczeń: {trainingDetails.exercises.length}
                </Paragraph>
              </Card.Content>
            </Card>
          }
          ListEmptyComponent={
            <Card style={styles.card}>
              <Card.Content>
                <Title style={{ color: colors.textPrimary }}>{translations.trainings.noExercises}</Title>
                <Paragraph style={{ color: colors.textSecondary }}>
                  {translations.trainings.noExercisesDescription}
                </Paragraph>
              </Card.Content>
            </Card>
          }
        />
        
        <FAB
          style={styles.fab}
          icon="plus"
          onPress={openAddExerciseModal}
          label={translations.trainings.addExercise}
        />

        <Portal>
          <Modal
            visible={modalVisible}
            onDismiss={closeModal}
            contentContainerStyle={styles.modalContainer}
          >
            <Title style={{ color: colors.textPrimary }}>{translations.trainings.selectExercise}</Title>
            
            <FlatList
              data={availableExercises.filter(exercise => 
                !trainingDetails?.exercises.some(te => te.exerciseId === exercise.id)
              )}
              renderItem={renderAvailableExercise}
              keyExtractor={(item) => item.id.toString()}
              style={{ maxHeight: 300, marginVertical: 16 }}
            />

            <View style={styles.modalActions}>
              <Button 
                onPress={closeModal} 
                style={styles.modalButton}
                textColor={colors.textPrimary}
                buttonColor={colors.buttons.outlined}
              >
                {translations.common.cancel}
              </Button>
              <Button 
                mode="contained" 
                onPress={handleAddExercise} 
                style={styles.modalButton}
                disabled={!selectedExercise}
                textColor={colors.textOnPrimary}
                buttonColor={colors.primary}
              >
                {translations.trainings.add}
              </Button>
            </View>
          </Modal>
        </Portal>
      </View>
    </PaperProvider>
  );
};

export default TrainingDetailsScreen;
