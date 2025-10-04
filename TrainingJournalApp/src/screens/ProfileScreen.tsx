import React, { useState, useEffect } from 'react';
import {
  View,
  StyleSheet,
  ScrollView,
  Alert,
} from 'react-native';
import {
  Card,
  Title,
  Paragraph,
  Button,
  TextInput,
  Portal,
  Modal,
  Provider as PaperProvider,
  Divider,
} from 'react-native-paper';
import DatabaseService from '../services/DatabaseService';
import { UserWeight, CreateUserWeightData } from '../types';
import { Translations } from '../constants';
import { useTheme } from '../contexts/ThemeContext';
import { useLanguage } from '../contexts/LanguageContext';

const ProfileScreen: React.FC = () => {
  const { colors } = useTheme();
  const { translations } = useLanguage();
  
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
    currentWeight: {
      fontSize: 32,
      fontWeight: 'bold',
      color: colors.weightStats.current,
      textAlign: 'center',
    },
    lastUpdate: {
      fontSize: 14,
      color: colors.textSecondary,
      textAlign: 'center',
      marginTop: 8,
    },
    statsContainer: {
      flexDirection: 'row',
      justifyContent: 'space-around',
      marginTop: 16,
    },
    statItem: {
      alignItems: 'center',
    },
    statValue: {
      fontSize: 18,
      fontWeight: 'bold',
      color: colors.weightStats.current,
    },
    statLabel: {
      fontSize: 12,
      color: colors.textSecondary,
      marginTop: 4,
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
    centerContainer: {
      flex: 1,
      justifyContent: 'center',
      alignItems: 'center',
    },
    dateText: {
      color: colors.textSecondary,
      fontSize: 14,
      textAlign: 'center',
    },
    changeText: {
      fontSize: 16,
      fontWeight: 'bold',
      textAlign: 'center',
      marginTop: 8,
    },
    weightItem: {
      paddingVertical: 8,
    },
    weightValue: {
      fontSize: 18,
      fontWeight: 'bold',
      color: colors.weightStats.current,
    },
    notes: {
      color: colors.textSecondary,
      fontStyle: 'italic',
      fontSize: 12,
    },
    divider: {
      marginVertical: 8,
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
  const [weights, setWeights] = useState<UserWeight[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalVisible, setModalVisible] = useState(false);
  
  // Form state
  const [weight, setWeight] = useState('');
  const [date, setDate] = useState(new Date().toISOString().split('T')[0]);
  const [notes, setNotes] = useState('');

  useEffect(() => {
    loadWeights();
  }, []);

  const loadWeights = async () => {
    try {
      const data = await DatabaseService.getUserWeights();
      setWeights(data);
    } catch (error) {
      console.error('Błąd ładowania wagi:', error);
      Alert.alert(translations.common.error, translations.errors.loadingWeights);
    } finally {
      setLoading(false);
    }
  };

  const resetForm = () => {
    setWeight('');
    setDate(new Date().toISOString().split('T')[0]);
    setNotes('');
  };

  const openAddModal = () => {
    resetForm();
    setModalVisible(true);
  };

  const closeModal = () => {
    setModalVisible(false);
    resetForm();
  };

  const handleSave = async () => {
    if (!weight.trim()) {
      Alert.alert(translations.common.error, translations.errors.weightRequired);
      return;
    }

    const weightValue = parseFloat(weight);
    if (isNaN(weightValue) || weightValue <= 0) {
      Alert.alert(translations.common.error, translations.errors.weightInvalid);
      return;
    }

    try {
      const data: CreateUserWeightData = {
        weight: weightValue,
        date: date,
        notes: notes.trim() || undefined
      };

      await DatabaseService.createUserWeight(data);
      closeModal();
      loadWeights();
      Alert.alert(translations.common.success, translations.success.weightAdded);
    } catch (error) {
      console.error('Błąd zapisywania wagi:', error);
      Alert.alert(translations.common.error, translations.errors.savingWeight);
    }
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('pl-PL', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  };

  const getCurrentWeight = () => {
    if (weights.length === 0) return null;
    return weights[0]; // Najnowszy wpis (posortowane DESC)
  };

  const getWeightChange = () => {
    if (weights.length < 2) return null;
    const current = weights[0].weight;
    const previous = weights[1].weight;
    const change = current - previous;
    return {
      value: Math.abs(change),
      isPositive: change > 0,
      isZero: change === 0
    };
  };

  const currentWeight = getCurrentWeight();
  const weightChange = getWeightChange();

  if (loading) {
    return (
      <View style={styles.centerContainer}>
        <Title style={{ color: colors.textPrimary }}>Ładowanie...</Title>
      </View>
    );
  }

  return (
    <PaperProvider>
      <ScrollView style={styles.container}>
        <Card style={styles.card}>
          <Card.Content>
            <Title style={{ color: colors.textPrimary }}>Obecna waga</Title>
            {currentWeight ? (
              <View>
                <Paragraph style={styles.currentWeight}>
                  {currentWeight.weight} kg
                </Paragraph>
                <Paragraph style={styles.dateText}>
                  Ostatnia aktualizacja: {formatDate(currentWeight.date)}
                </Paragraph>
                {weightChange && !weightChange.isZero && (
                  <Paragraph style={[
                    styles.changeText,
                    { color: weightChange.isPositive ? '#f44336' : '#4CAF50' }
                  ]}>
                    {weightChange.isPositive ? '+' : '-'}{weightChange.value.toFixed(1)} kg 
                    od poprzedniego pomiaru
                  </Paragraph>
                )}
              </View>
            ) : (
              <Paragraph style={{ color: colors.textSecondary }}>Brak danych o wadze</Paragraph>
            )}
          </Card.Content>
          <Card.Actions>
            <Button mode="contained" onPress={openAddModal}>
              Dodaj wagę
            </Button>
          </Card.Actions>
        </Card>

        <Card style={styles.card}>
          <Card.Content>
            <Title style={{ color: colors.textPrimary }}>Statystyki</Title>
            <View style={styles.statsContainer}>
              <View style={styles.statItem}>
                <Title style={styles.statValue}>{weights.length}</Title>
                <Paragraph style={{ color: colors.textSecondary }}>Łącznie pomiarów</Paragraph>
              </View>
              {weights.length > 0 && (
                <View style={styles.statItem}>
                  <Title style={styles.statValue}>
                    {weights.reduce((min, w) => Math.min(min, w.weight), weights[0].weight).toFixed(1)} kg
                  </Title>
                  <Paragraph style={{ color: colors.textSecondary }}>Najniższa waga</Paragraph>
                </View>
              )}
              {weights.length > 0 && (
                <View style={styles.statItem}>
                  <Title style={styles.statValue}>
                    {weights.reduce((max, w) => Math.max(max, w.weight), weights[0].weight).toFixed(1)} kg
                  </Title>
                  <Paragraph style={{ color: colors.textSecondary }}>Najwyższa waga</Paragraph>
                </View>
              )}
            </View>
          </Card.Content>
        </Card>

        <Card style={styles.card}>
          <Card.Content>
            <Title style={{ color: colors.textPrimary }}>Ostatnie pomiary</Title>
            {weights.length === 0 ? (
              <Paragraph style={{ color: colors.textSecondary }}>Brak pomiarów</Paragraph>
            ) : (
              weights.slice(0, 5).map((w, index) => (
                <View key={w.id}>
                  <View style={styles.weightItem}>
                    <View>
                      <Paragraph style={styles.weightValue}>{w.weight} kg</Paragraph>
                      <Paragraph style={styles.dateText}>{formatDate(w.date)}</Paragraph>
                      {w.notes && (
                        <Paragraph style={styles.notes}>{w.notes}</Paragraph>
                      )}
                    </View>
                  </View>
                  {index < Math.min(weights.length, 5) - 1 && <Divider style={styles.divider} />}
                </View>
              ))
            )}
          </Card.Content>
        </Card>
      </ScrollView>

      <Portal>
        <Modal
          visible={modalVisible}
          onDismiss={closeModal}
          contentContainerStyle={styles.modalContainer}
        >
          <Title style={{ color: colors.textPrimary }}>Dodaj wagę</Title>
          
          <TextInput
            label="Waga (kg)"
            value={weight}
            onChangeText={setWeight}
            style={styles.input}
            mode="outlined"
            keyboardType="numeric"
            placeholder="np. 75.5"
          />
          
          <TextInput
            label={translations.profile.date}
            value={date}
            onChangeText={setDate}
            style={styles.input}
            mode="outlined"
            keyboardType="numeric"
            placeholder="YYYY-MM-DD"
          />

          <TextInput
            label="Notatki (opcjonalne)"
            value={notes}
            onChangeText={setNotes}
            style={styles.input}
            mode="outlined"
            multiline
            numberOfLines={3}
          />

          <View style={styles.modalActions}>
            <Button onPress={closeModal} style={styles.modalButton}>
              Anuluj
            </Button>
            <Button mode="contained" onPress={handleSave} style={styles.modalButton}>
              Zapisz
            </Button>
          </View>
        </Modal>
      </Portal>
    </PaperProvider>
  );
};


export default ProfileScreen;
