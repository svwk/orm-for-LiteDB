using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace entitymodel
{
    [DebuggerDisplay("Id={this.Id} Name={this.Name} type={this.Type}")]
    public class zak : entity, IComparable<zak>, IEquatable<zak>
    {

        #region Fields
        private string _rekvizity = "";
        private string _name = "";

        private bindviewcond<dog> _dogs;
        #endregion

        #region constructors
        public zak() => Init();
        public zak(int newId) : base(newId) => Init();
        private void Init()
        {

        }
        #endregion

        #region Comparer

        public bool Equals(zak other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            //return base.Equals(other) && string.Equals(_name, other._name, StringComparison.OrdinalIgnoreCase) && string.Equals(_rekvizity, other._rekvizity, StringComparison.OrdinalIgnoreCase);
            return string.Equals(_name, other._name, StringComparison.CurrentCultureIgnoreCase) && string.Equals(_rekvizity, other._rekvizity, StringComparison.CurrentCultureIgnoreCase);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((zak) obj);
        }
        public override int GetHashCode()
        {
            /*unchecked
            {
                //int hashCode = base.GetHashCode();
                //hashCode = (hashCode * 397) ^ (_name      != null ? StringComparer.CurrentCultureIgnoreCase.GetHashCode(_name) : 0);
                //hashCode = (hashCode * 397) ^ (_rekvizity != null ? StringComparer.CurrentCultureIgnoreCase.GetHashCode(_rekvizity) : 0);
                //return hashCode;
                
            }*/
            return base.GetHashCode();
        }
        public static bool operator ==(zak left, zak right) { return Equals(left, right); }
        public static bool operator !=(zak left, zak right) { return !Equals(left, right); }
        public int CompareTo(zak other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;

            int rekvizityComparison = string.Compare(_name, other._name, StringComparison.CurrentCultureIgnoreCase);
            if (rekvizityComparison != 0) return rekvizityComparison;
            return  string.Compare(_rekvizity, other._rekvizity, StringComparison.CurrentCultureIgnoreCase);
        }
        public new int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            if (!(obj is zak)) throw new ArgumentException($"Object must be of type {nameof(zak)}");
            return CompareTo((zak) obj);
        }
        public static bool operator <(zak left, zak right)
        {
            return Comparer<zak>.Default.Compare(left, right) < 0;
        }
        public static bool operator >(zak left, zak right)
        {
            return Comparer<zak>.Default.Compare(left, right) > 0;
        }
        public static bool operator <=(zak left, zak right)
        {
            return Comparer<zak>.Default.Compare(left, right) <= 0;
        }
        public static bool operator >=(zak left, zak right)
        {
            return Comparer<zak>.Default.Compare(left, right) >= 0;
        }

        #endregion

        #region Events

        #endregion

        #region Propeties
        public override EntityType Type => EntityType.zak;
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
        public string Rekvizity
        {
            get => _rekvizity;
            set
            {
                value = conv.ClearString(value);
                if (_rekvizity == value) return;

                    if (Changed == Status.not_changed) Changed = Status.edited;
                    _rekvizity = value;
                    OnPropertyChanged(nameof(Rekvizity));
                
            }
        }

        public bindviewcond<dog> CDogs
        {
            get => _dogs;
            set
            {
                if (value!=null) value.IsCascading    = true;
                _dogs = value;
            }
        }

        #endregion

        #region Methods
        public override string ToString()
        {
            return $"{Name}";
        }

        public override void ClearLinks() { _dogs = null; }
        public override void PrepareForDelete()
        {
            if (_dogs != null && _dogs.Count>0) 
            {
                _dogs.IsCascading = true;
                for (int i = 0; i < _dogs.Count;) _dogs.Remove(_dogs[i]);
                //foreach (var v in _dogs) _dogs.Remove(v);
            }
            ClearLinks();
        }
        #endregion

    }
}