/**
 * Funkcje pomocnicze do tłumaczenia enumów
 * Mapowanie wartości enumów na polskie tłumaczenia
 */

import { MuscleGroup, MuscleGroupRole } from '../types';
import { Translations } from './Translations';

/**
 * Tłumaczy grupę mięśniową na polski
 */
export const translateMuscleGroup = (muscleGroup: MuscleGroup): string => {
  const translations: Record<MuscleGroup, string> = {
    [MuscleGroup.Chest]: Translations.muscleGroups.chest,
    [MuscleGroup.Back]: Translations.muscleGroups.back,
    [MuscleGroup.Biceps]: Translations.muscleGroups.biceps,
    [MuscleGroup.Triceps]: Translations.muscleGroups.triceps,
    [MuscleGroup.Shoulders]: Translations.muscleGroups.shoulders,
    [MuscleGroup.Legs]: Translations.muscleGroups.legs,
    [MuscleGroup.Glutes]: Translations.muscleGroups.glutes,
    [MuscleGroup.Calves]: Translations.muscleGroups.calves,
    [MuscleGroup.Abs]: Translations.muscleGroups.abs,
    [MuscleGroup.Forearms]: Translations.muscleGroups.forearms,
    [MuscleGroup.Traps]: Translations.muscleGroups.traps,
    [MuscleGroup.Lats]: Translations.muscleGroups.lats,
  };
  
  return translations[muscleGroup] || muscleGroup;
};

/**
 * Tłumaczy rolę grupy mięśniowej na polski
 */
export const translateMuscleGroupRole = (role: MuscleGroupRole): string => {
  const translations: Record<MuscleGroupRole, string> = {
    [MuscleGroupRole.Primary]: Translations.muscleGroupRoles.primary,
    [MuscleGroupRole.Secondary]: Translations.muscleGroupRoles.secondary,
  };
  
  return translations[role] || role;
};

/**
 * Zwraca kolor dla grupy mięśniowej
 */
export const getMuscleGroupColor = (muscleGroup: MuscleGroup): string => {
  const colors: Record<MuscleGroup, string> = {
    [MuscleGroup.Chest]: 'rgba(233, 30, 99, 1)',
    [MuscleGroup.Back]: 'rgba(156, 39, 176, 1)',
    [MuscleGroup.Biceps]: 'rgba(63, 81, 181, 1)',
    [MuscleGroup.Triceps]: 'rgba(33, 150, 243, 1)',
    [MuscleGroup.Shoulders]: 'rgba(0, 188, 212, 1)',
    [MuscleGroup.Legs]: 'rgba(76, 175, 80, 1)',
    [MuscleGroup.Glutes]: 'rgba(139, 195, 74, 1)',
    [MuscleGroup.Calves]: 'rgba(205, 220, 57, 1)',
    [MuscleGroup.Abs]: 'rgba(255, 235, 59, 1)',
    [MuscleGroup.Forearms]: 'rgba(255, 152, 0, 1)',
    [MuscleGroup.Traps]: 'rgba(255, 87, 34, 1)',
    [MuscleGroup.Lats]: 'rgba(121, 85, 72, 1)',
  };
  
  return colors[muscleGroup] || 'rgba(117, 117, 117, 1)';
};

/**
 * Zwraca kolor dla roli grupy mięśniowej
 */
export const getMuscleGroupRoleColor = (role: MuscleGroupRole): string => {
  const colors: Record<MuscleGroupRole, string> = {
    [MuscleGroupRole.Primary]: 'rgba(76, 175, 80, 1)',
    [MuscleGroupRole.Secondary]: 'rgba(255, 152, 0, 1)',
  };
  
  return colors[role] || 'rgba(117, 117, 117, 1)';
};

/**
 * Zwraca wszystkie dostępne grupy mięśniowe z tłumaczeniami
 */
export const getMuscleGroupOptions = (): Array<{ value: MuscleGroup; label: string }> => {
  return Object.values(MuscleGroup).map(group => ({
    value: group,
    label: translateMuscleGroup(group)
  }));
};

/**
 * Zwraca wszystkie dostępne role grup mięśniowych z tłumaczeniami
 */
export const getMuscleGroupRoleOptions = (): Array<{ value: MuscleGroupRole; label: string }> => {
  return Object.values(MuscleGroupRole).map(role => ({
    value: role,
    label: translateMuscleGroupRole(role)
  }));
};
