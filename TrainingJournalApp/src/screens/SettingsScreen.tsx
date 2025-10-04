import React, { useState } from 'react';
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
  List,
  Switch,
  RadioButton,
  Portal,
  Modal,
  Provider as PaperProvider,
  Divider,
} from 'react-native-paper';
import { Translations, Language } from '../constants';
import { useTheme, ThemeMode } from '../contexts/ThemeContext';
import { useLanguage } from '../contexts/LanguageContext';

const SettingsScreen: React.FC = () => {
  const { themeMode, setThemeMode, colors } = useTheme();
  const { language, translations, setLanguage } = useLanguage();
  const [themeModalVisible, setThemeModalVisible] = useState(false);
  const [languageModalVisible, setLanguageModalVisible] = useState(false);
  
  const styles = StyleSheet.create({
    container: {
      flex: 1,
      backgroundColor: colors.background,
    },
    card: {
      margin: 16,
      marginBottom: 8,
      elevation: 2,
      backgroundColor: colors.cards.background,
      borderColor: colors.cards.border,
    },
    modalContainer: {
      backgroundColor: colors.modal.background,
      padding: 20,
      margin: 20,
      borderRadius: 8,
      borderColor: colors.modal.border,
    },
    modalDescription: {
      marginBottom: 16,
      color: colors.textSecondary,
    },
    radioItem: {
      flexDirection: 'row',
      alignItems: 'center',
      marginVertical: 8,
    },
    radioLabel: {
      marginLeft: 8,
      fontSize: 16,
    },
    modalActions: {
      flexDirection: 'row',
      justifyContent: 'flex-end',
      marginTop: 24,
    },
    modalButton: {
      minWidth: 100,
    },
  });

  const handleThemeChange = (mode: string) => {
    setThemeMode(mode as ThemeMode);
    setThemeModalVisible(false);
  };

  const handleLanguageChange = (lang: string) => {
    setLanguage(lang as Language);
    setLanguageModalVisible(false);
  };

  const handleClearData = () => {
    Alert.alert(
      translations.settings.clearData,
      translations.settings.clearDataDescription,
      [
        { text: translations.common.cancel, style: 'cancel' },
        {
          text: translations.settings.clear,
          style: 'destructive',
          onPress: () => {
            // TODO: Implement clear data functionality
            Alert.alert(translations.common.info, translations.settings.clearDataDescription);
          }
        }
      ]
    );
  };

  const handleExportData = () => {
    // TODO: Implement export data functionality
    Alert.alert(translations.common.info, 'Export functionality will be available in the next version');
  };

  const handleImportData = () => {
    // TODO: Implement import data functionality
    Alert.alert(translations.common.info, 'Import functionality will be available in the next version');
  };

  const getThemeLabel = (mode: ThemeMode): string => {
    switch (mode) {
      case 'light':
        return translations.settings.lightTheme;
      case 'dark':
        return translations.settings.darkTheme;
      case 'system':
        return translations.settings.systemTheme;
      default:
        return mode;
    }
  };

  const getCurrentThemeLabel = (): string => {
    return getThemeLabel(themeMode);
  };

  const getLanguageLabel = (lang: Language): string => {
    switch (lang) {
      case 'pl':
        return 'Polski';
      case 'en':
        return 'English';
      default:
        return lang;
    }
  };

  const getCurrentLanguageLabel = (): string => {
    return getLanguageLabel(language);
  };

  return (
    <PaperProvider>
      <ScrollView style={styles.container}>
        {/* Wygląd */}
        <Card style={styles.card}>
          <Card.Content>
            <Title style={{ color: colors.textPrimary }}>{translations.settings.appearance}</Title>
            <List.Item
              title={translations.settings.theme}
              description={getCurrentThemeLabel()}
              left={(props) => <List.Icon {...props} icon="theme-light-dark" />}
              right={(props) => <List.Icon {...props} icon="chevron-right" />}
              onPress={() => setThemeModalVisible(true)}
            />
          </Card.Content>
        </Card>

        {/* Język */}
        <Card style={styles.card}>
          <Card.Content>
            <Title style={{ color: colors.textPrimary }}>{translations.settings.language}</Title>
            <List.Item
              title={getCurrentLanguageLabel()}
              description={translations.settings.languageDescription}
              left={(props) => <List.Icon {...props} icon="translate" />}
              right={(props) => <List.Icon {...props} icon="chevron-right" />}
              onPress={() => setLanguageModalVisible(true)}
            />
          </Card.Content>
        </Card>

        {/* Dane */}
        <Card style={styles.card}>
          <Card.Content>
            <Title style={{ color: colors.textPrimary }}>{translations.settings.data}</Title>
            <List.Item
              title={translations.settings.exportData}
              description={translations.settings.exportData}
              left={(props) => <List.Icon {...props} icon="export" />}
              right={(props) => <List.Icon {...props} icon="chevron-right" />}
              onPress={handleExportData}
            />
            <Divider />
            <List.Item
              title={translations.settings.importData}
              description={translations.settings.importData}
              left={(props) => <List.Icon {...props} icon="import" />}
              right={(props) => <List.Icon {...props} icon="chevron-right" />}
              onPress={handleImportData}
            />
            <Divider />
            <List.Item
              title={translations.settings.clearData}
              description={translations.settings.clearDataDescription}
              left={(props) => <List.Icon {...props} icon="delete" color={colors.error} />}
              right={(props) => <List.Icon {...props} icon="chevron-right" />}
              onPress={handleClearData}
            />
          </Card.Content>
        </Card>

        {/* O aplikacji */}
        <Card style={styles.card}>
          <Card.Content>
            <Title style={{ color: colors.textPrimary }}>{translations.settings.about}</Title>
            <List.Item
              title={translations.settings.version}
              description="1.0.0"
              left={(props) => <List.Icon {...props} icon="information" />}
            />
            <Divider />
            <List.Item
              title={translations.settings.developer}
              description="skopa"
              left={(props) => <List.Icon {...props} icon="account" />}
            />
            <Divider />
            <List.Item
              title={translations.settings.privacy}
              description={translations.settings.privacy}
              left={(props) => <List.Icon {...props} icon="shield-account" />}
              right={(props) => <List.Icon {...props} icon="chevron-right" />}
              onPress={() => Alert.alert(translations.common.info, 'Privacy policy will be available in the next version')}
            />
            <Divider />
            <List.Item
              title={translations.settings.terms}
              description={translations.settings.terms}
              left={(props) => <List.Icon {...props} icon="file-document" />}
              right={(props) => <List.Icon {...props} icon="chevron-right" />}
              onPress={() => Alert.alert(translations.common.info, 'Terms of service will be available in the next version')}
            />
          </Card.Content>
        </Card>
      </ScrollView>

      {/* Modal wyboru motywu */}
      <Portal>
        <Modal
          visible={themeModalVisible}
          onDismiss={() => setThemeModalVisible(false)}
          contentContainerStyle={styles.modalContainer}
        >
          <Title style={{ color: colors.textPrimary }}>{translations.settings.theme}</Title>
          <Paragraph style={styles.modalDescription}>
            {translations.settings.themeDescription}
          </Paragraph>

          <RadioButton.Group onValueChange={handleThemeChange} value={themeMode}>
            <View style={styles.radioItem}>
              <RadioButton value="light" />
              <Paragraph style={styles.radioLabel}>
                {translations.settings.lightTheme}
              </Paragraph>
            </View>
            <View style={styles.radioItem}>
              <RadioButton value="dark" />
              <Paragraph style={styles.radioLabel}>
                {translations.settings.darkTheme}
              </Paragraph>
            </View>
            <View style={styles.radioItem}>
              <RadioButton value="system" />
              <Paragraph style={styles.radioLabel}>
                {translations.settings.systemTheme}
              </Paragraph>
            </View>
          </RadioButton.Group>

          <View style={styles.modalActions}>
            <Button onPress={() => setThemeModalVisible(false)} style={styles.modalButton}>
              {translations.common.cancel}
            </Button>
          </View>
        </Modal>
      </Portal>

      {/* Modal wyboru języka */}
      <Portal>
        <Modal
          visible={languageModalVisible}
          onDismiss={() => setLanguageModalVisible(false)}
          contentContainerStyle={styles.modalContainer}
        >
          <Title style={{ color: colors.textPrimary }}>{translations.settings.language}</Title>
          <Paragraph style={styles.modalDescription}>
            {translations.settings.languageDescription}
          </Paragraph>

          <RadioButton.Group onValueChange={handleLanguageChange} value={language}>
            <View style={styles.radioItem}>
              <RadioButton value="pl" />
              <Paragraph style={styles.radioLabel}>
                Polski
              </Paragraph>
            </View>
            <View style={styles.radioItem}>
              <RadioButton value="en" />
              <Paragraph style={styles.radioLabel}>
                English
              </Paragraph>
            </View>
          </RadioButton.Group>

          <View style={styles.modalActions}>
            <Button onPress={() => setLanguageModalVisible(false)} style={styles.modalButton}>
              {translations.common.cancel}
            </Button>
          </View>
        </Modal>
      </Portal>
    </PaperProvider>
  );
};

export default SettingsScreen;
