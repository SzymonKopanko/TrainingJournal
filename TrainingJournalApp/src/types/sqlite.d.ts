declare module 'react-native-sqlite-storage' {
  interface SQLiteDatabase {
    executeSql: (sql: string, params?: any[]) => Promise<[any, any]>;
    transaction: (fn: (tx: any) => void) => Promise<void>;
    close: () => Promise<void>;
  }

  interface SQLiteStatic {
    DEBUG: boolean;
    enablePromise: (enable: boolean) => void;
    openDatabase: (config: { name: string; location?: string }) => Promise<SQLiteDatabase>;
  }

  namespace SQLite {
    interface SQLiteDatabase {
      executeSql: (sql: string, params?: any[]) => Promise<[any, any]>;
      transaction: (fn: (tx: any) => void) => Promise<void>;
      close: () => Promise<void>;
    }
  }

  const SQLite: SQLiteStatic;
  export default SQLite;
}
