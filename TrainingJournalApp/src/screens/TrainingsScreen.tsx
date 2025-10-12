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
} from 'react-native-paper';
import DatabaseService from '../services/DatabaseService';
import { Training, Exercise, CreateTrainingData } from '../types';
import { Colors, Translations } from '../constants';
import { useTheme } from '../contexts/ThemeContext';
import { useLanguage } from '../contexts/LanguageContext';
import { useNavigation } from '@react-navigation/native';
import type { StackNavigationProp } from '@react-navigation/stack';

type RootStackParamList = {
  TrainingDetails: {
    training: Training;
  };
};

type TrainingsScreenNavigationProp = StackNavigationProp<RootStackParamList, 'TrainingDetails'>;

const TrainingsScreen: React.FC = () => {
  const navigation = useNavigation<TrainingsScreenNavigationProp>();
  const { colors } = useTheme();
  const { translations } = useLanguage();
  
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
    exercisesList: {
      marginTop: 8,
    },
    exerciseItem: {
      marginBottom: 2,
    },
    dateText: {
      fontSize: 12,
      color: colors.textSecondary,
      marginTop: 4,
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
  const [trainings, setTrainings] = useState<Training[]>([]);
  const [exercises, setExercises] = useState<Exercise[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [modalVisible, setModalVisible] = useState(false);
  const [editingTraining, setEditingTraining] = useState<Training | null>(null);
  
  // Form state
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [date, setDate] = useState(new Date().toISOString().split('T')[0]);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const [trainingsData, exercisesData] = await Promise.all([
        DatabaseService.getTrainings(),
        DatabaseService.getExercises()
      ]);
      setTrainings(trainingsData);
      setExercises(exercisesData);
    } catch (error) {
      console.error('Błąd ładowania danych:', error);
      Alert.alert(translations.common.error, translations.errors.loadingTrainings);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  const handleRefresh = () => {
    setRefreshing(true);
    loadData();
  };

  const resetForm = () => {
    setName('');
    setDescription('');
    setDate(new Date().toISOString().split('T')[0]);
    setEditingTraining(null);
  };

  const openAddModal = () => {
    resetForm();
    setModalVisible(true);
  };

  const openEditModal = (training: Training) => {
    setName(training.name);
    setDescription(training.description || '');
    setDate(training.date.split('T')[0]);
    setEditingTraining(training);
    setModalVisible(true);
  };

  const closeModal = () => {
    setModalVisible(false);
    resetForm();
  };

  const handleSave = async () => {
    if (!name.trim()) {
      Alert.alert(translations.common.error, translations.errors.trainingNameRequired);
      return;
    }

    try {
      const data: CreateTrainingData = {
        name: name.trim(),
        description: description.trim() || undefined,
        date: date,
        exercises: [] // Na razie puste, można rozszerzyć później
      };

      if (editingTraining) {
        // TODO: Implement update training
        Alert.alert(translations.common.info, 'Training updates will be available in the next version');
      } else {
        await DatabaseService.createTraining(data);
      }

      closeModal();
      loadData();
      Alert.alert(translations.common.success, editingTraining ? translations.success.trainingUpdated : translations.success.trainingAdded);
    } catch (error) {
      console.error('Błąd zapisywania treningu:', error);
      Alert.alert(translations.common.error, translations.errors.savingTraining);
    }
  };

  const handleDelete = (training: Training) => {
    Alert.alert(
      translations.confirmations.deleteTraining,
      `${translations.confirmations.deleteTrainingMessage} "${training.name}"?`,
      [
        { text: translations.common.cancel, style: 'cancel' },
        {
          text: translations.common.delete,
          style: 'destructive',
          onPress: async () => {
            try {
              await DatabaseService.deleteTraining(training.id);
              loadData();
              Alert.alert(translations.common.success, translations.success.trainingDeleted);
            } catch (error) {
              console.error('Błąd usuwania treningu:', error);
              Alert.alert(translations.common.error, translations.errors.deletingTraining);
            }
          }
        }
      ]
    );
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('pl-PL', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  };

  const renderTraining = ({ item }: { item: Training }) => (
    <Card style={styles.card}>
      <Card.Content>
        <Title style={{ color: colors.textPrimary }}>{item.name}</Title>
        {item.description && <Paragraph style={{ color: colors.textSecondary }}>{item.description}</Paragraph>}
        <Paragraph style={styles.dateText}>
          {translations.trainings.trainingDate}: {formatDate(item.date)}
        </Paragraph>
        <View style={styles.exercisesList}>
          {item.exercises.length > 0 ? (
            item.exercises.map((trainingExercise, index) => (
              <View key={trainingExercise.id} style={styles.exerciseItem}>
                <Text style={{ color: colors.textSecondary, fontSize: 12 }}>
                  {index + 1}. {trainingExercise.exercise.name}
                </Text>
              </View>
            ))
          ) : (
            <Text style={{ color: colors.textSecondary, fontSize: 12, fontStyle: 'italic' }}>
              {translations.trainings.noExercises}
            </Text>
          )}
        </View>
      </Card.Content>
      <Card.Actions>
        <Button 
          onPress={() => navigation.navigate('TrainingDetails', { training: item })}
          textColor={colors.buttons.primary}
          buttonColor={colors.buttons.outlined}
        >
          {translations.trainings.details}
        </Button>
        <Button 
          onPress={() => openEditModal(item)}
          textColor={colors.buttons.primary}
          buttonColor={colors.buttons.outlined}
        >
          {translations.common.edit}
        </Button>
        <Button 
          onPress={() => handleDelete(item)} 
          textColor={colors.error}
          buttonColor={colors.buttons.outlined}
        >
          {translations.common.delete}
        </Button>
      </Card.Actions>
    </Card>
  );

  if (loading) {
    return (
      <View style={styles.centerContainer}>
        <Title style={{ color: colors.textPrimary }}>{translations.common.loading}</Title>
      </View>
    );
  }

  return (
    <PaperProvider>
      <View style={styles.container}>
        <FlatList
          data={trainings}
          renderItem={renderTraining}
          keyExtractor={(item) => item.id.toString()}
          refreshControl={
            <RefreshControl refreshing={refreshing} onRefresh={handleRefresh} />
          }
          contentContainerStyle={styles.listContainer}
        />
        
        <FAB
          style={styles.fab}
          icon="plus"
          onPress={openAddModal}
          label={translations.trainings.addTraining}
        />

        <Portal>
          <Modal
            visible={modalVisible}
            onDismiss={closeModal}
            contentContainerStyle={styles.modalContainer}
          >
            <Title style={{ color: colors.textPrimary }}>{editingTraining ? translations.trainings.edit : translations.trainings.addTraining}</Title>
            
            <TextInput
              label={translations.trainings.trainingName}
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
              label={translations.trainings.trainingDescription}
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
              label={translations.trainings.trainingDate}
              value={date}
              onChangeText={setDate}
              style={styles.input}
              mode="outlined"
              keyboardType="numeric"
              placeholder="YYYY-MM-DD"
              textColor={colors.textPrimary}
              placeholderTextColor={colors.inputs.placeholder}
              outlineColor={colors.inputs.border}
              activeOutlineColor={colors.inputs.borderFocused}
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
                onPress={handleSave} 
                style={styles.modalButton}
                textColor={colors.textOnPrimary}
                buttonColor={colors.primary}
              >
                {translations.common.save}
              </Button>
            </View>
          </Modal>
        </Portal>
      </View>
    </PaperProvider>
  );
};


export default TrainingsScreen;
