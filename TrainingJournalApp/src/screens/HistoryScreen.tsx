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
  Chip,
  Provider as PaperProvider,
} from 'react-native-paper';
import DatabaseService from '../services/DatabaseService';
import { UserWeight, CreateUserWeightData } from '../types';
import { Translations } from '../constants';
import { useTheme } from '../contexts/ThemeContext';
import { useLanguage } from '../contexts/LanguageContext';

const HistoryScreen: React.FC = () => {
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
      padding: 20,
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
    weightValue: {
      fontSize: 18,
      fontWeight: 'bold',
      color: colors.weightStats.current,
    },
    weightChip: {
      backgroundColor: colors.chips.primary,
      marginTop: 8,
    },
    dateText: {
      fontSize: 12,
      color: colors.textSecondary,
      marginTop: 4,
    },
    emptyContainer: {
      flex: 1,
      justifyContent: 'center',
      alignItems: 'center',
      padding: 32,
    },
    weightHeader: {
      flexDirection: 'row',
      justifyContent: 'space-between',
      alignItems: 'center',
      marginBottom: 8,
    },
    dateChip: {
      backgroundColor: colors.chips.primary,
    },
    notes: {
      color: colors.textSecondary,
      fontStyle: 'italic',
    },
  });
  const [weights, setWeights] = useState<UserWeight[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);

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
      setRefreshing(false);
    }
  };

  const handleRefresh = () => {
    setRefreshing(true);
    loadWeights();
  };

  const handleDelete = (weight: UserWeight) => {
    Alert.alert(
      translations.confirmations.deleteWeight,
      `${translations.confirmations.deleteWeightMessage} ${formatDate(weight.date)}?`,
      [
        { text: translations.common.cancel, style: 'cancel' },
        {
          text: translations.common.delete,
          style: 'destructive',
          onPress: async () => {
            try {
              await DatabaseService.deleteUserWeight(weight.id);
              loadWeights();
              Alert.alert(translations.common.success, translations.success.weightDeleted);
            } catch (error) {
              console.error('Błąd usuwania wpisu:', error);
              Alert.alert(translations.common.error, translations.errors.deletingWeight);
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

  const renderWeight = ({ item }: { item: UserWeight }) => (
    <Card style={styles.card}>
      <Card.Content>
        <View style={styles.weightHeader}>
          <Title style={styles.weightValue}>{item.weight} kg</Title>
          <Chip style={styles.dateChip}>{formatDate(item.date)}</Chip>
        </View>
        {item.notes && <Paragraph style={styles.notes}>{item.notes}</Paragraph>}
      </Card.Content>
      <Card.Actions>
        <Button onPress={() => handleDelete(item)} textColor={colors.buttons.danger}>
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
        {weights.length === 0 ? (
        <View style={styles.emptyContainer}>
          <Title style={{ color: colors.textPrimary }}>{translations.history.noData}</Title>
          <Paragraph style={{ color: colors.textSecondary }}>{translations.history.noDataDescription}</Paragraph>
          <Paragraph style={{ color: colors.textSecondary }}>{translations.history.noDataAction}</Paragraph>
        </View>
        ) : (
          <FlatList
            data={weights}
            renderItem={renderWeight}
            keyExtractor={(item) => item.id.toString()}
            refreshControl={
              <RefreshControl refreshing={refreshing} onRefresh={handleRefresh} />
            }
            contentContainerStyle={styles.listContainer}
          />
        )}
      </View>
    </PaperProvider>
  );
};


export default HistoryScreen;
