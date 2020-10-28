using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using entitymodel.Annotations;


namespace entitymodel
{
    [DebuggerDisplay("Id={this.Id} type={this.Type}")]
    public class entity : INotifyPropertyChanged, IComparable<entity>, IComparable, IEquatable<entity>
    {

        #region Fields
        private long _id = 0;

        private DateTime _timestamp = DateTime.Now;
        private bool _isFixed = false; // фиксация данных,которые нельзя изменять после создания объекта
        protected Status _changed;
        //protected EntityType _type = EntityType.ent;
        #endregion

        #region Constructors

        public entity()
        {
            _changed = Status.newcreated;
            _id =conv.GenerateId(Type);
        }

        public entity(long newId) : this()
        {
            _id = newId;
        }

        ~entity()
        {
            Clear();
        }
        #endregion

        #region events

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Comparer

        public int CompareTo(entity other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            int idComparison = _id.CompareTo(other._id);
            if (idComparison != 0) return idComparison;
            return _timestamp.CompareTo(other._timestamp);
        }
        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            if (!(obj is entity)) throw new ArgumentException($"Object must be of type {nameof(entity)}");
            return CompareTo((entity)obj);
        }
        public static bool operator <(entity left, entity right)
        {
            return Comparer<entity>.Default.Compare(left, right) < 0;
        }
        public static bool operator >(entity left, entity right)
        {
            return Comparer<entity>.Default.Compare(left, right) > 0;
        }
        public static bool operator <=(entity left, entity right)
        {
            return Comparer<entity>.Default.Compare(left, right) <= 0;
        }
        public static bool operator >=(entity left, entity right)
        {
            return Comparer<entity>.Default.Compare(left, right) >= 0;
        }

        public bool Equals(entity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _id == other._id && _timestamp.Equals(other._timestamp);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as entity;
            return other != null && Equals(other);
        }
        public override int GetHashCode()
        {
            //unchecked { return (_id * 397) ^ _timestamp.GetHashCode(); }
            //return ((int)Type) * 100000000 + Id;
            return Id.GetHashCode();
        }
        public static bool operator ==(entity left, entity right) { return Equals(left, right); }
        public static bool operator !=(entity left, entity right) { return !Equals(left, right); }

        #endregion

        #region Propeties
       
        public virtual EntityType Type => EntityType.ent;

        public long Id
        {
            get => _id;
            set
            {
                if (!_isFixed) _id = value;
                else throw new Exception("Нельзя изменять номер записи");
            }
        }

        public DateTime CreationTime => _timestamp;

        public long TimeStamp
        {
            get => _timestamp.Ticks;
            set
            {
                if (!_isFixed) _timestamp = new DateTime(value);
                else throw new Exception("Нельзя изменять метку времени");
            }
        }

        public Status Changed
        {
            get => _changed;
            set
            {
                Status t = _changed;
                _changed = value;
                if ((t!=_changed)&&_changed==Status.deleted) OnPropertyChanged(nameof(Changed));
            }
        }

        public virtual string Value => ToString();

        #endregion

        #region methods

        public override string ToString() { return _id.ToString(); }

        /// <summary>
        /// Сброс в состояние "изменений нет" для данного объекта
        /// </summary>
        public virtual void Reset()
        {
            _changed = Status.not_changed;
            _isFixed = true;
            //CheckChildren();
        }

        public void Fix()
        {
            _isFixed = true;
        }

        public virtual void Clear()
        { }

        public virtual void ClearLinks() { }

        public virtual void PrepareForDelete() { }
        #endregion


    }
}
