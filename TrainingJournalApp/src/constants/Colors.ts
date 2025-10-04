/**
 * Kolory aplikacji TrainingJournal
 * Centralne miejsce definicji wszystkich kolorów używanych w aplikacji
 * Automatycznie wybiera kolory dla trybu jasnego lub ciemnego
 */

import { LightColors } from './LightColors';
import { DarkColors } from './DarkColors';

export type ColorTheme = 'light' | 'dark';

/**
 * Zwraca kolory dla określonego motywu
 */
export const getColors = (theme: ColorTheme) => {
  return theme === 'dark' ? DarkColors : LightColors;
};

/**
 * Kolory dla trybu jasnego (domyślne)
 */
export const Colors = LightColors;

// Re-export kolorów dla łatwego dostępu
export { LightColors, DarkColors };

// Typy dla TypeScript
export type ColorKey = keyof typeof LightColors;
export type MuscleGroupColorKey = keyof typeof LightColors.muscleGroups;
export type MuscleGroupRoleColorKey = keyof typeof LightColors.muscleGroupRoles;
export type WeightStatsColorKey = keyof typeof LightColors.weightStats;
export type NavigationColorKey = keyof typeof LightColors.navigation;
export type ButtonColorKey = keyof typeof LightColors.buttons;
export type InputColorKey = keyof typeof LightColors.inputs;
export type CardColorKey = keyof typeof LightColors.cards;
export type ModalColorKey = keyof typeof LightColors.modal;
export type FabColorKey = keyof typeof LightColors.fab;
export type ChipColorKey = keyof typeof LightColors.chips;
