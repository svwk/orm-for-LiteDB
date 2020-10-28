using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace entitymodel
{
    [DebuggerDisplay("Id={this.Id} Name={this.Name} type={this.Type}")]
    public class pls : entity, IComparable<pls>, IEquatable<pls>
    {
        #region Fields

        private string _name   = "";
        private string _adress = "";

        private dog _dog;
        private long _dogId;
        private bindviewcond<sor> _sors;

        #endregion

        #region Constructors

        public pls() => Init();
        public pls(int newId) : base(newId) => Init();
        private void Init()
        {
 
        }

        public pls(dog dogEntity)
        {
            Init();
            _dog = dogEntity;
            _dogId = dogEntity.Id;
        }

        #endregion

        #region Comparer

        public int CompareTo(pls other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            int nameComparison = string.Compare(_name, other._name, StringComparison.CurrentCultureIgnoreCase);
            if (nameComparison != 0) return nameComparison;
            return string.Compare(_adress, other._adress, StringComparison.CurrentCultureIgnoreCase);
        }
        public new int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            if (!(obj is pls)) throw new ArgumentException($"Object must be of type {nameof(pls)}");
            return CompareTo((pls) obj);
        }
        public static bool operator <(pls left, pls right)
        {
            return Comparer<pls>.Default.Compare(left, right) < 0;
        }
        public static bool operator >(pls left, pls right)
        {
            return Comparer<pls>.Default.Compare(left, right) > 0;
        }
        public static bool operator <=(pls left, pls right)
        {
            return Comparer<pls>.Default.Compare(left, right) <= 0;
        }
        public static bool operator >=(pls left, pls right)
        {
            return Comparer<pls>.Default.Compare(left, right) >= 0;
        }
        public bool Equals(pls other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_name, other._name, StringComparison.CurrentCultureIgnoreCase) && string.Equals(_adress, other._adress, StringComparison.CurrentCultureIgnoreCase);
            //return base.Equals(other) && string.Equals(_name, other._name, StringComparison.CurrentCultureIgnoreCase) && string.Equals(_adress, other._adress, StringComparison.CurrentCultureIgnoreCase);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as pls;
            return other != null && Equals(other);
        }
        public override int GetHashCode()
        {
            /*unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (_name   != null ? StringComparer.CurrentCultureIgnoreCase.GetHashCode(_name) : 0);
                hashCode = (hashCode * 397) ^ (_adress != null ? StringComparer.CurrentCultureIgnoreCase.GetHashCode(_adress) : 0);
                return hashCode;
            }*/
            return base.GetHashCode();
        }
        public static bool operator ==(pls left, pls right) { return Equals(left, right); }
        public static bool operator !=(pls left, pls right) { return !Equals(left, right); }

        #endregion

        #region Events

        #endregion

        #region Propeties

        public override EntityType       Type       => EntityType.pls;

        public string Name
        {
            get => _name;
            set
            {
                value = conv.ClearString(value);
                if (_name == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Adress
        {
            get => _adress;
            set
            {
                value = conv.ClearString(value);
                if (_adress == value) return;
                if ( Changed == Status.not_changed) Changed = Status.edited;
                _adress = value;
                OnPropertyChanged(nameof(Adress));
            }
        }

        public bindviewcond<sor> CSors
        {
            get => _sors;
            set
            {
                if (value != null) value.IsCascading    = true;
                _sors = value;
            }
        }
        public dog Dog
        {
            get => _dog;
            set
            {
                if (value == null || value.Id==_dogId) _dog = value;
            }
        }

        public long DogId
        {
            get => _dogId;
            set
            {
                if (_dogId == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _dogId = value;
                if (_dog?.Id != _dogId) _dog= null;
                OnPropertyChanged(nameof(DogId));
            }
        }

        #endregion

        #region Methods

        public override void Clear()
        {
            base.Clear();
            _changed      = Status.not_changed;

            _dog = null;
            _dogId = -1;
            _adress = "";
            _name = "";
            _sors = null;
        }

        public override string ToString()
        {
            return $"{Name}. - {Adress}";
        }

        public override void ClearLinks() { _dog = null;
            _sors = null;
        }

        public override void PrepareForDelete()
        {
            if (_sors != null && _sors.Count>0)
            {
                _sors.IsCascading = true;
                for (int i = 0; i < _sors.Count;) _sors.Remove(_sors[i]);
                //foreach (var v in _sors)
                //{
                //    _sors.Remove(v);
                //}

            }
            ClearLinks();
        }
        //public bool CheckDouble(pls otherItem)
        //{
        //    if (otherItem.Name == _name && otherItem.Adress == _adress && otherItem.DogId == _dogId) return true;
        //    return false;

        //}
        #endregion

        //public new dog Parent { get; set; }
        //public new sors Children { get; set; }

    }
}