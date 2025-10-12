// Enums
export enum MuscleGroup {
  Chest = 'Chest',
  Back = 'Back',
  FrontDeltoid = 'FrontDeltoid',
  MiddleDeltoid = 'MiddleDeltoid',
  RearDeltoid = 'RearDeltoid',
  Biceps = 'Biceps',
  Triceps = 'Triceps',
  Quads = 'Quads',
  Hamstrings = 'Hamstrings',
  Glutes = 'Glutes',
  Calves = 'Calves',
  Abs = 'Abs',
  Forearms = 'Forearms',
  Cardio = 'Cardio'
}

export enum MuscleGroupRole {
  Primary = 'Primary',
  Secondary = 'Secondary'
}

// Main Types
export interface Exercise {
  id: number;
  name: string;
  description: string;
  bodyWeightPercentage: number; // procent masy ciała podnoszonej podczas ćwiczenia
  createdAt: string;
  updatedAt?: string;
  muscleGroups: ExerciseMuscleGroup[];
}

export interface ExerciseMuscleGroup {
  id: number;
  exerciseId: number;
  muscleGroup: MuscleGroup;
  role: MuscleGroupRole;
  createdAt: string;
  updatedAt?: string;
}

export interface Training {
  id: number;
  name: string;
  description?: string;
  createdAt: string;
  updatedAt?: string;
  exercises: TrainingExercise[];
}

export interface TrainingExercise {
  id: number;
  trainingId: number;
  exerciseId: number;
  exercise: Exercise;
  order: number;
  notes?: string;
  createdAt: string;
  updatedAt?: string;
  sets: ExerciseSet[];
}

export interface ExerciseSet {
  id: number;
  exerciseEntryId: number;
  order: number;
  reps: number;
  weight: number;
  rir: number; // reps in reserve
  createdAt: string;
  updatedAt?: string;
}

export interface ExerciseEntry {
  id: number;
  exerciseId: number;
  exercise: Exercise;
  notes: string;
  createdAt: string;
  updatedAt?: string;
  sets: ExerciseSet[];
}

export interface UserWeight {
  id: number;
  weight: number;
  weightedAt: string;
  createdAt: string;
  updatedAt?: string;
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
  exercises: {
    exerciseId: number;
    order: number;
    notes?: string;
  }[];
}

export interface CreateExerciseSetData {
  exerciseEntryId: number;
  order: number;
  reps: number;
  weight: number;
  rir: number;
}

export interface CreateUserWeightData {
  weight: number;
  weightedAt: string;
}
