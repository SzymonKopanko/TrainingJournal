import SQLite from 'react-native-sqlite-storage';
import { 
  Exercise, 
  Training, 
  ExerciseSet, 
  ExerciseEntry, 
  UserWeight,
  CreateExerciseData,
  CreateTrainingData,
  CreateExerciseSetData,
  CreateUserWeightData,
  MuscleGroup,
  MuscleGroupRole
} from '../types';

// Konfiguracja SQLite
SQLite.DEBUG = true;
SQLite.enablePromise(true);

class DatabaseService {
  private db: SQLite.SQLiteDatabase | null = null;

  async initDatabase(): Promise<void> {
    try {
      this.db = await SQLite.openDatabase({
        name: 'TrainingJournal.db',
        location: 'default',
      });

      await this.createTables();
      console.log('Baza danych zainicjalizowana pomyślnie');
    } catch (error) {
      console.error('Błąd inicjalizacji bazy danych:', error);
      throw error;
    }
  }

  private async createTables(): Promise<void> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    // Tabela Exercises
    await this.db.executeSql(`
      CREATE TABLE IF NOT EXISTS exercises (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        name TEXT NOT NULL,
        description TEXT,
        created_at DATETIME DEFAULT CURRENT_TIMESTAMP
      )
    `);

    // Tabela ExerciseMuscleGroups
    await this.db.executeSql(`
      CREATE TABLE IF NOT EXISTS exercise_muscle_groups (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        exercise_id INTEGER NOT NULL,
        muscle_group TEXT NOT NULL,
        role TEXT NOT NULL,
        FOREIGN KEY (exercise_id) REFERENCES exercises (id) ON DELETE CASCADE
      )
    `);

    // Tabela Trainings
    await this.db.executeSql(`
      CREATE TABLE IF NOT EXISTS trainings (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        name TEXT NOT NULL,
        description TEXT,
        date DATETIME NOT NULL,
        created_at DATETIME DEFAULT CURRENT_TIMESTAMP
      )
    `);

    // Tabela TrainingExercises
    await this.db.executeSql(`
      CREATE TABLE IF NOT EXISTS training_exercises (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        training_id INTEGER NOT NULL,
        exercise_id INTEGER NOT NULL,
        order_index INTEGER NOT NULL,
        FOREIGN KEY (training_id) REFERENCES trainings (id) ON DELETE CASCADE,
        FOREIGN KEY (exercise_id) REFERENCES exercises (id) ON DELETE CASCADE
      )
    `);

    // Tabela ExerciseSets
    await this.db.executeSql(`
      CREATE TABLE IF NOT EXISTS exercise_sets (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        training_exercise_id INTEGER NOT NULL,
        reps INTEGER NOT NULL,
        weight REAL NOT NULL,
        rest_time INTEGER,
        notes TEXT,
        completed_at DATETIME,
        FOREIGN KEY (training_exercise_id) REFERENCES training_exercises (id) ON DELETE CASCADE
      )
    `);

    // Tabela ExerciseEntries
    await this.db.executeSql(`
      CREATE TABLE IF NOT EXISTS exercise_entries (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        exercise_id INTEGER NOT NULL,
        date DATETIME NOT NULL,
        notes TEXT,
        FOREIGN KEY (exercise_id) REFERENCES exercises (id) ON DELETE CASCADE
      )
    `);

    // Tabela UserWeights
    await this.db.executeSql(`
      CREATE TABLE IF NOT EXISTS user_weights (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        weight REAL NOT NULL,
        date DATETIME NOT NULL,
        notes TEXT
      )
    `);
  }

  // Exercise CRUD
  async createExercise(data: CreateExerciseData): Promise<Exercise> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    const result = await this.db.executeSql(
      'INSERT INTO exercises (name, description) VALUES (?, ?)',
      [data.name, data.description || null]
    );

    const exerciseId = result[0].insertId;
    
    // Dodaj grupy mięśniowe
    for (const muscleGroup of data.muscleGroups) {
      await this.db.executeSql(
        'INSERT INTO exercise_muscle_groups (exercise_id, muscle_group, role) VALUES (?, ?, ?)',
        [exerciseId, muscleGroup.muscleGroup, muscleGroup.role]
      );
    }

    return this.getExerciseById(exerciseId);
  }

  async getExercises(): Promise<Exercise[]> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    const result = await this.db.executeSql(`
      SELECT e.*, 
             emg.muscle_group, 
             emg.role 
      FROM exercises e
      LEFT JOIN exercise_muscle_groups emg ON e.id = emg.exercise_id
      ORDER BY e.name
    `);

    const exercisesMap = new Map<number, Exercise>();
    
    for (let i = 0; i < result[0].rows.length; i++) {
      const row = result[0].rows.item(i);
      const exerciseId = row.id;

      if (!exercisesMap.has(exerciseId)) {
        exercisesMap.set(exerciseId, {
          id: exerciseId,
          name: row.name,
          description: row.description,
          createdAt: row.created_at,
          muscleGroups: []
        });
      }

      if (row.muscle_group) {
        exercisesMap.get(exerciseId)!.muscleGroups.push({
          id: row.id,
          exerciseId: exerciseId,
          muscleGroup: row.muscle_group as MuscleGroup,
          role: row.role as MuscleGroupRole
        });
      }
    }

    return Array.from(exercisesMap.values());
  }

  async getExerciseById(id: number): Promise<Exercise> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    const result = await this.db.executeSql(`
      SELECT e.*, 
             emg.id as emg_id,
             emg.muscle_group, 
             emg.role 
      FROM exercises e
      LEFT JOIN exercise_muscle_groups emg ON e.id = emg.exercise_id
      WHERE e.id = ?
    `, [id]);

    if (result[0].rows.length === 0) {
      throw new Error('Ćwiczenie nie znalezione');
    }

    const exercise: Exercise = {
      id: result[0].rows.item(0).id,
      name: result[0].rows.item(0).name,
      description: result[0].rows.item(0).description,
      createdAt: result[0].rows.item(0).created_at,
      muscleGroups: []
    };

    for (let i = 0; i < result[0].rows.length; i++) {
      const row = result[0].rows.item(i);
      if (row.muscle_group) {
        exercise.muscleGroups.push({
          id: row.emg_id,
          exerciseId: exercise.id,
          muscleGroup: row.muscle_group as MuscleGroup,
          role: row.role as MuscleGroupRole
        });
      }
    }

    return exercise;
  }

  async updateExercise(id: number, data: CreateExerciseData): Promise<Exercise> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    await this.db.executeSql(
      'UPDATE exercises SET name = ?, description = ? WHERE id = ?',
      [data.name, data.description || null, id]
    );

    // Usuń stare grupy mięśniowe
    await this.db.executeSql('DELETE FROM exercise_muscle_groups WHERE exercise_id = ?', [id]);

    // Dodaj nowe grupy mięśniowe
    for (const muscleGroup of data.muscleGroups) {
      await this.db.executeSql(
        'INSERT INTO exercise_muscle_groups (exercise_id, muscle_group, role) VALUES (?, ?, ?)',
        [id, muscleGroup.muscleGroup, muscleGroup.role]
      );
    }

    return this.getExerciseById(id);
  }

  async deleteExercise(id: number): Promise<void> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    await this.db.executeSql('DELETE FROM exercises WHERE id = ?', [id]);
  }

  // Training CRUD
  async createTraining(data: CreateTrainingData): Promise<Training> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    const result = await this.db.executeSql(
      'INSERT INTO trainings (name, description, date) VALUES (?, ?, ?)',
      [data.name, data.description || null, data.date]
    );

    const trainingId = result[0].insertId;

    // Dodaj ćwiczenia do treningu
    for (const exerciseData of data.exercises) {
      const teResult = await this.db.executeSql(
        'INSERT INTO training_exercises (training_id, exercise_id, order_index) VALUES (?, ?, ?)',
        [trainingId, exerciseData.exerciseId, exerciseData.order]
      );

      const trainingExerciseId = teResult[0].insertId;

      // Dodaj serie
      for (const setData of exerciseData.sets) {
        await this.db.executeSql(
          'INSERT INTO exercise_sets (training_exercise_id, reps, weight, rest_time) VALUES (?, ?, ?, ?)',
          [trainingExerciseId, setData.reps, setData.weight, setData.restTime || null]
        );
      }
    }

    return this.getTrainingById(trainingId);
  }

  async getTrainings(): Promise<Training[]> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    const result = await this.db.executeSql(`
      SELECT t.*, 
             te.id as te_id,
             te.exercise_id,
             te.order_index,
             e.name as exercise_name,
             e.description as exercise_description,
             e.created_at as exercise_created_at
      FROM trainings t
      LEFT JOIN training_exercises te ON t.id = te.training_id
      LEFT JOIN exercises e ON te.exercise_id = e.id
      ORDER BY t.date DESC, te.order_index
    `);

    const trainingsMap = new Map<number, Training>();

    for (let i = 0; i < result[0].rows.length; i++) {
      const row = result[0].rows.item(i);
      const trainingId = row.id;

      if (!trainingsMap.has(trainingId)) {
        trainingsMap.set(trainingId, {
          id: trainingId,
          name: row.name,
          description: row.description,
          date: row.date,
          createdAt: row.created_at,
          exercises: []
        });
      }

      if (row.te_id) {
        const training = trainingsMap.get(trainingId)!;
        const existingExercise = training.exercises.find(ex => ex.id === row.te_id);
        
        if (!existingExercise) {
          training.exercises.push({
            id: row.te_id,
            trainingId: trainingId,
            exerciseId: row.exercise_id,
            exercise: {
              id: row.exercise_id,
              name: row.exercise_name,
              description: row.exercise_description,
              createdAt: row.exercise_created_at,
              muscleGroups: []
            },
            order: row.order_index,
            sets: []
          });
        }
      }
    }

    return Array.from(trainingsMap.values());
  }

  async getTrainingById(id: number): Promise<Training> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    const result = await this.db.executeSql(`
      SELECT t.*, 
             te.id as te_id,
             te.exercise_id,
             te.order_index,
             e.name as exercise_name,
             e.description as exercise_description,
             e.created_at as exercise_created_at,
             es.id as es_id,
             es.reps,
             es.weight,
             es.rest_time,
             es.notes,
             es.completed_at
      FROM trainings t
      LEFT JOIN training_exercises te ON t.id = te.training_id
      LEFT JOIN exercises e ON te.exercise_id = e.id
      LEFT JOIN exercise_sets es ON te.id = es.training_exercise_id
      WHERE t.id = ?
      ORDER BY te.order_index, es.id
    `, [id]);

    if (result[0].rows.length === 0) {
      throw new Error('Trening nie znaleziony');
    }

    const training: Training = {
      id: result[0].rows.item(0).id,
      name: result[0].rows.item(0).name,
      description: result[0].rows.item(0).description,
      date: result[0].rows.item(0).date,
      createdAt: result[0].rows.item(0).created_at,
      exercises: []
    };

    const exercisesMap = new Map<number, any>();

    for (let i = 0; i < result[0].rows.length; i++) {
      const row = result[0].rows.item(i);
      
      if (row.te_id && !exercisesMap.has(row.te_id)) {
        exercisesMap.set(row.te_id, {
          id: row.te_id,
          trainingId: training.id,
          exerciseId: row.exercise_id,
          exercise: {
            id: row.exercise_id,
            name: row.exercise_name,
            description: row.exercise_description,
            createdAt: row.exercise_created_at,
            muscleGroups: []
          },
          order: row.order_index,
          sets: []
        });
      }

      if (row.es_id) {
        const exercise = exercisesMap.get(row.te_id);
        if (exercise) {
          exercise.sets.push({
            id: row.es_id,
            trainingExerciseId: row.te_id,
            reps: row.reps,
            weight: row.weight,
            restTime: row.rest_time,
            notes: row.notes,
            completedAt: row.completed_at
          });
        }
      }
    }

    training.exercises = Array.from(exercisesMap.values());
    return training;
  }

  async deleteTraining(id: number): Promise<void> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    await this.db.executeSql('DELETE FROM trainings WHERE id = ?', [id]);
  }

  // UserWeight CRUD
  async createUserWeight(data: CreateUserWeightData): Promise<UserWeight> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    const result = await this.db.executeSql(
      'INSERT INTO user_weights (weight, date, notes) VALUES (?, ?, ?)',
      [data.weight, data.date, data.notes || null]
    );

    return {
      id: result[0].insertId,
      weight: data.weight,
      date: data.date,
      notes: data.notes
    };
  }

  async getUserWeights(): Promise<UserWeight[]> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    const result = await this.db.executeSql(`
      SELECT * FROM user_weights 
      ORDER BY date DESC
    `);

    const weights: UserWeight[] = [];
    for (let i = 0; i < result[0].rows.length; i++) {
      const row = result[0].rows.item(i);
      weights.push({
        id: row.id,
        weight: row.weight,
        date: row.date,
        notes: row.notes
      });
    }

    return weights;
  }

  async deleteUserWeight(id: number): Promise<void> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    await this.db.executeSql('DELETE FROM user_weights WHERE id = ?', [id]);
  }

  // ExerciseSet CRUD
  async updateExerciseSet(id: number, data: Partial<CreateExerciseSetData>): Promise<void> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    const fields = [];
    const values = [];

    if (data.reps !== undefined) {
      fields.push('reps = ?');
      values.push(data.reps);
    }
    if (data.weight !== undefined) {
      fields.push('weight = ?');
      values.push(data.weight);
    }
    if (data.restTime !== undefined) {
      fields.push('rest_time = ?');
      values.push(data.restTime);
    }
    if (data.notes !== undefined) {
      fields.push('notes = ?');
      values.push(data.notes);
    }

    if (fields.length === 0) return;

    values.push(id);

    await this.db.executeSql(
      `UPDATE exercise_sets SET ${fields.join(', ')} WHERE id = ?`,
      values
    );
  }

  async markSetCompleted(id: number): Promise<void> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    await this.db.executeSql(
      'UPDATE exercise_sets SET completed_at = CURRENT_TIMESTAMP WHERE id = ?',
      [id]
    );
  }
}

export default new DatabaseService();
