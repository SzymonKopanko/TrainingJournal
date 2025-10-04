/**
 * Eksport wszystkich stałych aplikacji
 * Centralne miejsce importu kolorów, tłumaczeń i innych stałych
 */

export { Colors, getColors, LightColors, DarkColors } from './Colors';
export { Translations, getTranslations, PolishTranslations, EnglishTranslations } from './Translations';
export type { Language } from './Translations';
export {
  translateMuscleGroup,
  translateMuscleGroupRole,
  getMuscleGroupColor,
  getMuscleGroupRoleColor,
  getMuscleGroupOptions,
  getMuscleGroupRoleOptions,
} from './EnumTranslations';

// Re-export typów
export type {
  ColorKey,
  MuscleGroupColorKey,
  MuscleGroupRoleColorKey,
  WeightStatsColorKey,
  NavigationColorKey,
  ButtonColorKey,
  InputColorKey,
  CardColorKey,
  ModalColorKey,
  FabColorKey,
  ChipColorKey,
} from './Colors';

export type {
  TranslationKey,
  NavigationTranslationKey,
  ExerciseTranslationKey,
  TrainingTranslationKey,
  HistoryTranslationKey,
  MuscleGroupTranslationKey,
  MuscleGroupRoleTranslationKey,
  SuccessTranslationKey,
  ErrorTranslationKey,
  ConfirmationTranslationKey,
  CommonTranslationKey,
  DateFormatTranslationKey,
  ValidationTranslationKey,
  PlaceholderTranslationKey,
  TooltipTranslationKey,
  AccessibilityTranslationKey,
} from './Translations';
