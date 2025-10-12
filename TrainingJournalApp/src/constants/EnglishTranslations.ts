/**
 * English translations for TrainingJournal app
 * Central place for all English text used in the application
 */

export const EnglishTranslations = {
  // Navigation
  navigation: {
    exercises: 'Exercises',
    trainings: 'Trainings',
    history: 'History',
    profile: 'Profile',
    settings: 'Settings',
  },
  
  // Exercises screen
  exercises: {
    title: 'Exercises',
    addExercise: 'Add Exercise',
    editExercise: 'Edit Exercise',
    exerciseName: 'Exercise Name',
    exerciseDescription: 'Description (optional)',
    bodyWeightPercentage: 'Body Weight Percentage',
    bodyWeightPercentageDescription: 'Percentage of body weight lifted during exercise (e.g. 0.8 = 80%)',
    muscleGroups: 'Muscle Groups',
    muscleGroup: 'Muscle Group',
    role: 'Role',
    addMuscleGroup: 'Add Muscle Group',
    primaryMuscle: 'Primary',
    secondaryMuscle: 'Secondary',
    save: 'Save',
    cancel: 'Cancel',
    edit: 'Edit',
    delete: 'Delete',
    loading: 'Loading...',
    refresh: 'Refresh',
  },
  
  // Trainings screen
  trainings: {
    title: 'Trainings',
    addTraining: 'Add Training',
    editTraining: 'Edit Training',
    deleteTraining: 'Delete Training',
    trainingName: 'Training Name',
    trainingDescription: 'Description (optional)',
    trainingDate: 'Date',
    exercisesCount: 'Exercises',
    description: 'Description (optional)',
    date: 'Date',
    exercises: 'Exercises',
    addExercise: 'Add Exercise',
    noTrainings: 'No Trainings',
    noTrainingsDescription: 'You don\'t have any trainings yet.',
    noTrainingsAction: 'Add your first training.',
    loading: 'Loading...',
    refresh: 'Refresh',
    save: 'Save',
    cancel: 'Cancel',
    edit: 'Edit',
    delete: 'Delete',
    details: 'Details',
    noExercises: 'No Exercises',
    noExercisesDescription: 'Add exercises to this training using the + button',
    selectExercise: 'Select Exercise',
    add: 'Add',
    removeExercise: 'Remove Exercise',
    selectExerciseMessage: 'Select an exercise',
  },
  
  // History screen
  history: {
    title: 'History',
    weightHistory: 'Weight History',
    noData: 'No data available',
    noDataDescription: 'Start tracking your weight to see your progress here',
    noDataAction: 'Add your first weight measurement',
    loading: 'Loading...',
    refresh: 'Refresh',
    delete: 'Delete',
  },
  
  // Profile screen
  profile: {
    title: 'Profile',
    currentWeight: 'Current Weight',
    lastUpdate: 'Last Update',
    weightChange: 'from previous measurement',
    addWeight: 'Add Weight',
    statistics: 'Statistics',
    totalMeasurements: 'Total Measurements',
    lowestWeight: 'Lowest Weight',
    highestWeight: 'Highest Weight',
    recentMeasurements: 'Recent Measurements',
    noMeasurements: 'No Measurements',
    weight: 'Weight (kg)',
    date: 'Date',
    notes: 'Notes (optional)',
    loading: 'Loading...',
    save: 'Save',
    cancel: 'Cancel',
    delete: 'Delete',
  },
  
  // Settings screen
  settings: {
    title: 'Settings',
    appearance: 'Appearance',
    theme: 'Theme',
    lightTheme: 'Light',
    darkTheme: 'Dark',
    systemTheme: 'System',
    themeDescription: 'Choose application theme',
    language: 'Language',
    languageDescription: 'Choose application language',
    data: 'Data',
    exportData: 'Export Data',
    importData: 'Import Data',
    clearData: 'Clear All Data',
    clearDataDescription: 'Removes all exercises, trainings and weight measurements',
    about: 'About',
    version: 'Version',
    developer: 'Developer',
    privacy: 'Privacy',
    terms: 'Terms of Service',
    loading: 'Loading...',
    save: 'Save',
    cancel: 'Cancel',
    confirm: 'Confirm',
    export: 'Export',
    import: 'Import',
    clear: 'Clear',
  },
  
  // Muscle groups
  muscleGroups: {
    chest: 'Chest',
    back: 'Back',
    biceps: 'Biceps',
    triceps: 'Triceps',
    shoulders: 'Shoulders',
    legs: 'Legs',
    glutes: 'Glutes',
    calves: 'Calves',
    abs: 'Abs',
    forearms: 'Forearms',
    traps: 'Traps',
    lats: 'Lats',
  },
  
  // Muscle group roles
  muscleGroupRoles: {
    primary: 'Primary',
    secondary: 'Secondary',
  },
  
  // Success messages
  success: {
    exerciseAdded: 'Exercise added',
    exerciseUpdated: 'Exercise updated',
    exerciseDeleted: 'Exercise deleted',
    trainingAdded: 'Training added',
    trainingUpdated: 'Training updated',
    trainingDeleted: 'Training deleted',
    weightAdded: 'Weight added',
    weightDeleted: 'Entry deleted',
  },
  
  // Error messages
  errors: {
    loadingExercises: 'Error loading exercises',
    loadingTrainings: 'Error loading trainings',
    loadingWeights: 'Error loading weights',
    exerciseNameRequired: 'Exercise name is required',
    trainingNameRequired: 'Training name is required',
    muscleGroupRequired: 'At least one muscle group is required',
    weightRequired: 'Weight is required',
    weightInvalid: 'Please enter a valid weight',
    bodyWeightPercentageInvalid: 'Body weight percentage must be a number greater than or equal to 0',
    savingExercise: 'Error saving exercise',
    savingTraining: 'Error saving training',
    savingWeight: 'Error saving weight',
    deletingExercise: 'Error deleting exercise',
    deletingTraining: 'Error deleting training',
    deletingWeight: 'Error deleting weight',
  },
  
  // Confirmation dialogs
  confirmations: {
    deleteExercise: 'Delete Exercise',
    deleteExerciseMessage: 'Are you sure you want to delete this exercise? This action cannot be undone.',
    deleteTraining: 'Delete Training',
    deleteTrainingMessage: 'Are you sure you want to delete this training? This action cannot be undone.',
    deleteWeight: 'Delete Weight Entry',
    deleteWeightMessage: 'Are you sure you want to delete this weight entry? This action cannot be undone.',
    removeExerciseFromTraining: 'Remove Exercise from Training',
    removeExerciseFromTrainingMessage: 'Are you sure you want to remove this exercise from the training?',
    cancelChanges: 'Cancel Changes',
    cancelChangesMessage: 'Are you sure you want to cancel? All unsaved changes will be lost.',
  },
  
  // Validation messages
  validation: {
    required: 'This field is required',
    invalidEmail: 'Please enter a valid email address',
    passwordTooShort: 'Password must be at least 8 characters',
    passwordsDoNotMatch: 'Passwords do not match',
    invalidWeight: 'Please enter a valid weight',
    invalidDate: 'Please enter a valid date',
  },
  
  // Placeholders
  placeholders: {
    exerciseName: 'Enter exercise name',
    exerciseDescription: 'Enter exercise description',
    trainingName: 'Enter training name',
    trainingDescription: 'Enter training description',
    weight: 'Enter weight',
    notes: 'Enter notes',
    search: 'Search...',
  },
  
  // Common words
  common: {
    yes: 'Yes',
    no: 'No',
    ok: 'OK',
    cancel: 'Cancel',
    confirm: 'Confirm',
    delete: 'Delete',
    edit: 'Edit',
    save: 'Save',
    back: 'Back',
    loading: 'Loading...',
    error: 'Error',
    success: 'Success',
    info: 'Information',
    warning: 'Warning',
    required: 'Required',
    optional: 'Optional',
    today: 'Today',
    yesterday: 'Yesterday',
    tomorrow: 'Tomorrow',
    thisWeek: 'This Week',
    thisMonth: 'This Month',
    thisYear: 'This Year',
    polish: 'Polish',
    english: 'English',
  },
  
  // Accessibility labels
  accessibility: {
    addExercise: 'Add new exercise',
    addTraining: 'Add new training',
    addWeight: 'Add weight measurement',
    editExercise: 'Edit exercise',
    editTraining: 'Edit training',
    deleteExercise: 'Delete exercise',
    deleteTraining: 'Delete training',
    deleteWeight: 'Delete weight entry',
    refresh: 'Refresh data',
    search: 'Search',
    filter: 'Filter',
    sort: 'Sort',
    close: 'Close',
    back: 'Back',
    next: 'Next',
    previous: 'Previous',
    done: 'Done',
  },
} as const;

// TypeScript types
export type EnglishTranslationKey = keyof typeof EnglishTranslations;
export type EnglishNavigationTranslationKey = keyof typeof EnglishTranslations.navigation;
export type EnglishExerciseTranslationKey = keyof typeof EnglishTranslations.exercises;
export type EnglishTrainingTranslationKey = keyof typeof EnglishTranslations.trainings;
export type EnglishHistoryTranslationKey = keyof typeof EnglishTranslations.history;
export type EnglishSettingsTranslationKey = keyof typeof EnglishTranslations.settings;
export type EnglishMuscleGroupTranslationKey = keyof typeof EnglishTranslations.muscleGroups;
export type EnglishMuscleGroupRoleTranslationKey = keyof typeof EnglishTranslations.muscleGroupRoles;
export type EnglishSuccessTranslationKey = keyof typeof EnglishTranslations.success;
export type EnglishErrorTranslationKey = keyof typeof EnglishTranslations.errors;
export type EnglishConfirmationTranslationKey = keyof typeof EnglishTranslations.confirmations;
export type EnglishValidationTranslationKey = keyof typeof EnglishTranslations.validation;
export type EnglishPlaceholderTranslationKey = keyof typeof EnglishTranslations.placeholders;
export type EnglishCommonTranslationKey = keyof typeof EnglishTranslations.common;
export type EnglishAccessibilityTranslationKey = keyof typeof EnglishTranslations.accessibility;
