using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;


namespace dal
{
    public class litedb_dal : IDisposable
    {
        private ushort _dbVer = 0;
        private ConnectionString _connectionString;
        private LiteDatabase _db;
        private List<string> _collectionNames;
        private static bool _firstrun = true;

        public litedb_dal()
        {
            _connectionString = null;
            _db               = null;
            if (_firstrun)
            {
                _firstrun = false;

            }
        }

        public litedb_dal(string connectionString):this()
        {
            _connectionString = new ConnectionString(connectionString);
        }

        public litedb_dal(ConnectionString connectionString) : this()
        {
            _connectionString = connectionString;
        }

        public ConnectionString DbConnectionString
        {
            get => _connectionString;
            set
            {
                _connectionString = value;
                CloseDB();
            }
        }

        public string ConnectionStringToStr
        {
            get
            {
                if (_connectionString!=null) return _connectionString.ToString();
                return "";
            }
            set
            {
                _connectionString = new ConnectionString(value);
                CloseDB();
            }
        }


        public bool OpenDB()
        {
            CloseDB();
			if (_connectionString != null)
            {
                try
                {
                    _db = new LiteDatabase(_connectionString);
                }
                catch (Exception)
                {
                    throw;
                }
                if (_dbVer != _db.Engine.UserVersion)
                    {
                        throw new Exception("Неверная версия базы данных");
                    }

                try
                { 
                    //MapDB(BsonMapper.Global);

                    _collectionNames = _db.GetCollectionNames().ToList<string>();
                }
                catch (Exception)
                {
                    //Console.WriteLine(e);
                    throw;
                }
               if ((!_collectionNames.Any())&&(_dbVer<1)) { InitDB();}//Пустая бд
               // MakeIndexes();
                //Zaks = _db.GetCollection<model.zak>(ColNames.Zaks.ToString());

                return true;
             }
            return false;
        } 

        public void CloseDB()
        {
            _db?.Dispose();
            _db = null;
        }


        private void InitDB()
        {
            _db.Engine.UserVersion = _dbVer;
        }





        public List<T> LoadData<T>(string name)       
        {
            var col = _db?.GetCollection<T>(name);
            return col?.FindAll().ToList();
         }


        public void Insert<T>(string name, T item)
        {
            if (_db == null) return;
            var col = _db.GetCollection<T>(name);
            col.Insert(item);
        }

        public void Insert<T>(string name, IEnumerable<T> items)
        {
            if (_db == null) return;
            var col = _db.GetCollection<T>(name);
            col.Insert(items);
        }

        public void Update<T>(string name, T item)
        {
            if (_db == null) return;
            var col = _db.GetCollection<T>(name);
            col.Update(item);
        }

        public void Update<T>(string name, IEnumerable<T> items)
        {
            if (_db == null) return;
            var col = _db.GetCollection<T>(name);
            col.Update(items);
        }

        public void Upset<T>(string name, T item)
        {
            if (_db == null) return;
            var col = _db.GetCollection<T>(name);
            col.Upsert(item);
        }

        public void Upsert<T>(string name, IEnumerable<T> items)
        {
            if (_db == null) return;
            var col = _db.GetCollection<T>(name);
            col.Upsert(items);
        }

        public void Delete(string name, long id)
        {
            if (_db == null) return;
            var col = _db.GetCollection(name);
            col.Delete(id);
        }

        public bool FindById<T>(string name, long id,ref T value)
        {
            if (_db == null) return false;
            var col = _db.GetCollection<T>(name);
            value= col.FindById(id);
            return true;
        }

        public int Count(string name)
        {
            if (_db == null) return -1;
            var col = _db.GetCollection(name);
            return col.Count();
        }


        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов
        

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // освободить управляемое состояние (управляемые объекты).
                    
                }

                // освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // задать большим полям значение NULL.
                CloseDB();
                disposedValue = true;
            }
        }

        // переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
         ~litedb_dal() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
           Dispose(false);
         }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        void IDisposable.Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // раскомментировать следующую строку, если метод завершения переопределен выше.
             GC.SuppressFinalize(this);
        }

        #endregion


    }
}
