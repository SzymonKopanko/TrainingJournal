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
        description TEXT NOT NULL,
        body_weight_percentage REAL DEFAULT 0.0,
        created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
        updated_at DATETIME
      )
    `);

    // Migracje: dodaj kolumny jeśli nie istnieją
    await this.addColumnIfNotExists('exercises', 'body_weight_percentage', 'REAL DEFAULT 0.0');
    await this.addColumnIfNotExists('exercises', 'updated_at', 'DATETIME');
    await this.addColumnIfNotExists('exercise_muscle_groups', 'created_at', 'DATETIME');
    await this.addColumnIfNotExists('exercise_muscle_groups', 'updated_at', 'DATETIME');
    await this.addColumnIfNotExists('trainings', 'updated_at', 'DATETIME');
    await this.addColumnIfNotExists('training_exercises', 'notes', 'TEXT');
    await this.addColumnIfNotExists('training_exercises', 'created_at', 'DATETIME');
    await this.addColumnIfNotExists('training_exercises', 'updated_at', 'DATETIME');
    await this.addColumnIfNotExists('exercise_entries', 'created_at', 'DATETIME');
    await this.addColumnIfNotExists('exercise_entries', 'updated_at', 'DATETIME');
    await this.addColumnIfNotExists('user_weights', 'weighted_at', 'DATETIME');
    await this.addColumnIfNotExists('user_weights', 'created_at', 'DATETIME');
    await this.addColumnIfNotExists('user_weights', 'updated_at', 'DATETIME');

    // Inicjalizuj wartości created_at dla istniejących rekordów
    await this.initializeCreatedAtValues();

    // Tabela ExerciseMuscleGroups
    await this.db.executeSql(`
      CREATE TABLE IF NOT EXISTS exercise_muscle_groups (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        exercise_id INTEGER NOT NULL,
        muscle_group TEXT NOT NULL,
        role TEXT NOT NULL,
        created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
        updated_at DATETIME,
        FOREIGN KEY (exercise_id) REFERENCES exercises (id) ON DELETE CASCADE
      )
    `);

    // Tabela Trainings
    await this.db.executeSql(`
      CREATE TABLE IF NOT EXISTS trainings (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        name TEXT NOT NULL,
        description TEXT,
        created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
        updated_at DATETIME
      )
    `);

    // Tabela TrainingExercises
    await this.db.executeSql(`
      CREATE TABLE IF NOT EXISTS training_exercises (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        training_id INTEGER NOT NULL,
        exercise_id INTEGER NOT NULL,
        order_index INTEGER NOT NULL,
        notes TEXT,
        created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
        updated_at DATETIME,
        FOREIGN KEY (training_id) REFERENCES trainings (id) ON DELETE CASCADE,
        FOREIGN KEY (exercise_id) REFERENCES exercises (id) ON DELETE CASCADE
      )
    `);

    // Tabela ExerciseSets
    await this.db.executeSql(`
      CREATE TABLE IF NOT EXISTS exercise_sets (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        exercise_entry_id INTEGER NOT NULL,
        order_index INTEGER NOT NULL,
        reps INTEGER NOT NULL,
        weight REAL NOT NULL,
        rir INTEGER NOT NULL,
        created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
        updated_at DATETIME,
        FOREIGN KEY (exercise_entry_id) REFERENCES exercise_entries (id) ON DELETE CASCADE
      )
    `);

    // Tabela ExerciseEntries
    await this.db.executeSql(`
      CREATE TABLE IF NOT EXISTS exercise_entries (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        exercise_id INTEGER NOT NULL,
        notes TEXT NOT NULL,
        created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
        updated_at DATETIME,
        FOREIGN KEY (exercise_id) REFERENCES exercises (id) ON DELETE CASCADE
      )
    `);

    // Tabela UserWeights
    await this.db.executeSql(`
      CREATE TABLE IF NOT EXISTS user_weights (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        weight REAL NOT NULL,
        weighted_at DATETIME NOT NULL,
        created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
        updated_at DATETIME
      )
    `);
  }

  private async addColumnIfNotExists(tableName: string, columnName: string, columnDefinition: string): Promise<void> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    try {
      // Sprawdź czy kolumna istnieje
      const result = await this.db.executeSql(`PRAGMA table_info(${tableName})`);
      const columns = result[0].rows;
      
      let columnExists = false;
      for (let i = 0; i < columns.length; i++) {
        if (columns.item(i).name === columnName) {
          columnExists = true;
          break;
        }
      }

      // Dodaj kolumnę jeśli nie istnieje
      if (!columnExists) {
        await this.db.executeSql(`ALTER TABLE ${tableName} ADD COLUMN ${columnName} ${columnDefinition}`);
        console.log(`Dodano kolumnę ${columnName} do tabeli ${tableName}`);
      }
    } catch (error) {
      console.error(`Błąd podczas dodawania kolumny ${columnName}:`, error);
      // Nie rzucamy błędu, żeby nie blokować inicjalizacji
    }
  }

  private async initializeCreatedAtValues(): Promise<void> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    try {
      const currentTime = new Date().toISOString();
      
      // Inicjalizuj created_at dla exercise_muscle_groups
      await this.db.executeSql(`
        UPDATE exercise_muscle_groups 
        SET created_at = ? 
        WHERE created_at IS NULL
      `, [currentTime]);

      // Inicjalizuj created_at dla training_exercises
      await this.db.executeSql(`
        UPDATE training_exercises 
        SET created_at = ? 
        WHERE created_at IS NULL
      `, [currentTime]);

      // Inicjalizuj created_at dla exercise_entries
      await this.db.executeSql(`
        UPDATE exercise_entries 
        SET created_at = ? 
        WHERE created_at IS NULL
      `, [currentTime]);

      // Inicjalizuj created_at dla user_weights
      await this.db.executeSql(`
        UPDATE user_weights 
        SET created_at = ? 
        WHERE created_at IS NULL
      `, [currentTime]);

      console.log('Zainicjalizowano wartości created_at dla istniejących rekordów');
    } catch (error) {
      console.error('Błąd podczas inicjalizacji wartości created_at:', error);
      // Nie rzucamy błędu, żeby nie blokować inicjalizacji
    }
  }

  // Exercise CRUD
  async createExercise(data: CreateExerciseData): Promise<Exercise> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    const currentTime = new Date().toISOString();
    const result = await this.db.executeSql(
      'INSERT INTO exercises (name, description, body_weight_percentage, created_at) VALUES (?, ?, ?, ?)',
      [data.name, data.description || '', data.bodyWeightPercentage, currentTime]
    );

    const exerciseId = result[0].insertId;
    
    // Dodaj grupy mięśniowe
    for (const muscleGroup of data.muscleGroups) {
      await this.db.executeSql(
        'INSERT INTO exercise_muscle_groups (exercise_id, muscle_group, role, created_at) VALUES (?, ?, ?, ?)',
        [exerciseId, muscleGroup.muscleGroup, muscleGroup.role, currentTime]
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
          description: row.description || '',
          bodyWeightPercentage: row.body_weight_percentage,
          createdAt: row.created_at,
          updatedAt: row.updated_at,
          muscleGroups: []
        });
      }

      if (row.muscle_group) {
        exercisesMap.get(exerciseId)!.muscleGroups.push({
          id: row.id,
          exerciseId: exerciseId,
          muscleGroup: row.muscle_group as MuscleGroup,
          role: row.role as MuscleGroupRole,
          createdAt: row.created_at,
          updatedAt: row.updated_at
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
      bodyWeightPercentage: result[0].rows.item(0).body_weight_percentage,
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

    const currentTime = new Date().toISOString();
    await this.db.executeSql(
      'UPDATE exercises SET name = ?, description = ?, body_weight_percentage = ?, updated_at = ? WHERE id = ?',
      [data.name, data.description || '', data.bodyWeightPercentage, currentTime, id]
    );

    // Usuń stare grupy mięśniowe
    await this.db.executeSql('DELETE FROM exercise_muscle_groups WHERE exercise_id = ?', [id]);

    // Dodaj nowe grupy mięśniowe
    for (const muscleGroup of data.muscleGroups) {
      await this.db.executeSql(
        'INSERT INTO exercise_muscle_groups (exercise_id, muscle_group, role, created_at) VALUES (?, ?, ?, ?)',
        [id, muscleGroup.muscleGroup, muscleGroup.role, currentTime]
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

    const currentTime = new Date().toISOString();
    const result = await this.db.executeSql(
      'INSERT INTO trainings (name, description, created_at) VALUES (?, ?, ?)',
      [data.name, data.description || null, currentTime]
    );

    const trainingId = result[0].insertId;

    // Dodaj ćwiczenia do treningu
    for (const exerciseData of data.exercises) {
      await this.db.executeSql(
        'INSERT INTO training_exercises (training_id, exercise_id, order_index, notes, created_at) VALUES (?, ?, ?, ?, ?)',
        [trainingId, exerciseData.exerciseId, exerciseData.order, exerciseData.notes || null, currentTime]
      );
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
             e.body_weight_percentage as exercise_body_weight_percentage,
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
              bodyWeightPercentage: row.exercise_body_weight_percentage,
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

    const currentTime = new Date().toISOString();
    const result = await this.db.executeSql(
      'INSERT INTO user_weights (weight, weighted_at, created_at) VALUES (?, ?, ?)',
      [data.weight, data.weightedAt, currentTime]
    );

    return {
      id: result[0].insertId,
      weight: data.weight,
      weightedAt: data.weightedAt,
      createdAt: currentTime,
      updatedAt: undefined
    };
  }

  async getUserWeights(): Promise<UserWeight[]> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    const result = await this.db.executeSql(`
      SELECT * FROM user_weights 
      ORDER BY weighted_at DESC
    `);

    const weights: UserWeight[] = [];
    for (let i = 0; i < result[0].rows.length; i++) {
      const row = result[0].rows.item(i);
      weights.push({
        id: row.id,
        weight: row.weight,
        weightedAt: row.weighted_at,
        createdAt: row.created_at,
        updatedAt: row.updated_at
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

  // TrainingExercise CRUD
  async addExerciseToTraining(trainingId: number, exerciseId: number): Promise<TrainingExercise> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    // Sprawdź czy ćwiczenie już istnieje w treningu
    const [existingRows] = await this.db.executeSql(
      'SELECT id FROM training_exercises WHERE training_id = ? AND exercise_id = ?',
      [trainingId, exerciseId]
    );

    if (existingRows.rows.length > 0) {
      throw new Error('Ćwiczenie już istnieje w tym treningu');
    }

    // Pobierz maksymalny order dla tego treningu
    const [orderRows] = await this.db.executeSql(
      'SELECT MAX(order_index) as max_order FROM training_exercises WHERE training_id = ?',
      [trainingId]
    );

    const nextOrder = (orderRows.rows.item(0)?.max_order || 0) + 1;

    // Dodaj ćwiczenie do treningu
    const currentTime = new Date().toISOString();
    const [result] = await this.db.executeSql(
      'INSERT INTO training_exercises (training_id, exercise_id, order_index, created_at) VALUES (?, ?, ?, ?)',
      [trainingId, exerciseId, nextOrder, currentTime]
    );

    const trainingExerciseId = result.insertId;

    // Pobierz pełne dane ćwiczenia
    const [exerciseRows] = await this.db.executeSql(
      'SELECT * FROM exercises WHERE id = ?',
      [exerciseId]
    );

    if (exerciseRows.rows.length === 0) {
      throw new Error('Ćwiczenie nie zostało znalezione');
    }

    const exerciseRow = exerciseRows.rows.item(0);

    // Pobierz grupy mięśniowe dla ćwiczenia
    const [muscleGroupRows] = await this.db.executeSql(
      'SELECT * FROM exercise_muscle_groups WHERE exercise_id = ?',
      [exerciseId]
    );

    const muscleGroups = [];
    for (let i = 0; i < muscleGroupRows.rows.length; i++) {
      const row = muscleGroupRows.rows.item(i);
      muscleGroups.push({
        id: row.id,
        exerciseId: row.exercise_id,
        muscleGroup: row.muscle_group,
        role: row.role
      });
    }

    // Pobierz serie dla tego ćwiczenia w treningu
    const [setRows] = await this.db.executeSql(
      'SELECT * FROM exercise_sets WHERE training_exercise_id = ? ORDER BY id',
      [trainingExerciseId]
    );

    const sets = [];
    for (let i = 0; i < setRows.rows.length; i++) {
      const row = setRows.rows.item(i);
      sets.push({
        id: row.id,
        trainingExerciseId: row.training_exercise_id,
        reps: row.reps,
        weight: row.weight,
        restTime: row.rest_time,
        notes: row.notes,
        completedAt: row.completed_at
      });
    }

    return {
      id: trainingExerciseId,
      trainingId: trainingId,
      exerciseId: exerciseId,
      order: nextOrder,
      exercise: {
        id: exerciseRow.id,
        name: exerciseRow.name,
        description: exerciseRow.description,
        bodyWeightPercentage: exerciseRow.body_weight_percentage,
        createdAt: exerciseRow.created_at,
        muscleGroups: muscleGroups
      },
      sets: sets
    };
  }

  async removeExerciseFromTraining(trainingExerciseId: number): Promise<void> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    // Usuń wszystkie serie dla tego ćwiczenia w treningu
    await this.db.executeSql(
      'DELETE FROM exercise_sets WHERE training_exercise_id = ?',
      [trainingExerciseId]
    );

    // Usuń ćwiczenie z treningu
    await this.db.executeSql(
      'DELETE FROM training_exercises WHERE id = ?',
      [trainingExerciseId]
    );
  }

  async updateTrainingExerciseOrder(trainingExerciseId: number, newOrder: number): Promise<void> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    await this.db.executeSql(
      'UPDATE training_exercises SET order_index = ? WHERE id = ?',
      [newOrder, trainingExerciseId]
    );
  }

  async getTrainingExercises(trainingId: number): Promise<TrainingExercise[]> {
    if (!this.db) throw new Error('Baza danych nie jest zainicjalizowana');

    const [rows] = await this.db.executeSql(
      'SELECT te.*, e.name as exercise_name, e.description as exercise_description, ' +
      'e.body_weight_percentage as exercise_body_weight_percentage, e.created_at as exercise_created_at ' +
      'FROM training_exercises te ' +
      'JOIN exercises e ON te.exercise_id = e.id ' +
      'WHERE te.training_id = ? ' +
      'ORDER BY te.order_index',
      [trainingId]
    );

    const trainingExercises = [];
    for (let i = 0; i < rows.rows.length; i++) {
      const row = rows.rows.item(i);

      // Pobierz grupy mięśniowe dla ćwiczenia
      const [muscleGroupRows] = await this.db.executeSql(
        'SELECT * FROM exercise_muscle_groups WHERE exercise_id = ?',
        [row.exercise_id]
      );

      const muscleGroups = [];
      for (let j = 0; j < muscleGroupRows.rows.length; j++) {
        const mgRow = muscleGroupRows.rows.item(j);
        muscleGroups.push({
          id: mgRow.id,
          exerciseId: mgRow.exercise_id,
          muscleGroup: mgRow.muscle_group,
          role: mgRow.role
        });
      }

      // Pobierz serie dla tego ćwiczenia w treningu
      const [setRows] = await this.db.executeSql(
        'SELECT * FROM exercise_sets WHERE training_exercise_id = ? ORDER BY id',
        [row.id]
      );

      const sets = [];
      for (let j = 0; j < setRows.rows.length; j++) {
        const setRow = setRows.rows.item(j);
        sets.push({
          id: setRow.id,
          trainingExerciseId: setRow.training_exercise_id,
          reps: setRow.reps,
          weight: setRow.weight,
          restTime: setRow.rest_time,
          notes: setRow.notes,
          completedAt: setRow.completed_at
        });
      }

      trainingExercises.push({
        id: row.id,
        trainingId: row.training_id,
        exerciseId: row.exercise_id,
        order: row.order_index,
        exercise: {
          id: row.exercise_id,
          name: row.exercise_name,
          description: row.exercise_description,
          bodyWeightPercentage: row.exercise_body_weight_percentage,
          createdAt: row.exercise_created_at,
          muscleGroups: muscleGroups
        },
        sets: sets
      });
    }

    return trainingExercises;
  }
}

export default new DatabaseService();
