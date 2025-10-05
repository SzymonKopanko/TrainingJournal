// Enums
export enum MuscleGroup {
  Chest = 'Chest',
  Back = 'Back',
  Biceps = 'Biceps',
  Triceps = 'Triceps',
  Shoulders = 'Shoulders',
  Legs = 'Legs',
  Glutes = 'Glutes',
  Calves = 'Calves',
  Abs = 'Abs',
  Forearms = 'Forearms',
  Traps = 'Traps',
  Lats = 'Lats'
}

export enum MuscleGroupRole {
  Primary = 'Primary',
  Secondary = 'Secondary'
}

// Main Types
export interface Exercise {
  id: number;
  name: string;
  description?: string;
  bodyWeightPercentage: number; // procent masy ciała podnoszonej podczas ćwiczenia
  createdAt: string;
  muscleGroups: ExerciseMuscleGroup[];
}

export interface ExerciseMuscleGroup {
  id: number;
  exerciseId: number;
  muscleGroup: MuscleGroup;
  role: MuscleGroupRole;
}

export interface Training {
  id: number;
  name: string;
  description?: string;
  date: string;
  createdAt: string;
  exercises: TrainingExercise[];
}

export interface TrainingExercise {
  id: number;
  trainingId: number;
  exerciseId: number;
  exercise: Exercise;
  order: number;
  sets: ExerciseSet[];
}

export interface ExerciseSet {
  id: number;
  trainingExerciseId: number;
  reps: number;
  weight: number;
  restTime?: number; // w sekundach
  notes?: string;
  completedAt?: string;
}

export interface ExerciseEntry {
  id: number;
  exerciseId: number;
  exercise: Exercise;
  date: string;
  sets: ExerciseSet[];
  notes?: string;
}

export interface UserWeight {
  id: number;
  weight: number;
  date: string;
  notes?: string;
}

// Form Types
export interface CreateExerciseData {
  name: string;
  description?: string;
  bodyWeightPercentage: number;
  muscleGroups: {
    muscleGroup: MuscleGroup;
    role: MuscleGroupRole;
  }[];
}

export interface CreateTrainingData {
  name: string;
  description?: string;
  date: string;
  exercises: {
    exerciseId: number;
    order: number;
    sets: {
      reps: number;
      weight: number;
      restTime?: number;
    }[];
  }[];
}

export interface CreateExerciseSetData {
  trainingExerciseId: number;
  reps: number;
  weight: number;
  restTime?: number;
  notes?: string;
}

export interface CreateUserWeightData {
  weight: number;
  date: string;
  notes?: string;
}
