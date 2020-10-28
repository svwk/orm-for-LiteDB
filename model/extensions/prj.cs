using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace entitymodel
{
    [DebuggerDisplay("Id={this.Id} Name={this.Name} type={this.Type}")]
    public class prj : entity, IComparable<prj>, IEquatable<prj>
    {
        #region Fields

        private string _name     = "";
        private string _oboznach = "";
        private string _folder   = "";

        private sor _sor;
        private long _sorId;

        private bindviewcond<smt> _smts;
        #endregion

        #region Constructors

        public prj() => Init();
        public prj(int newId) : base(newId) => Init();
        private void Init()
        {
            
        }

        public prj(sor sorEntity)
        {
            Init();
            _sor = sorEntity;
            _sorId = sorEntity.Id;
        }

        #endregion

        #region Comparer

        public int CompareTo(prj other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            int nameComparison = string.Compare(_name, other._name, StringComparison.CurrentCultureIgnoreCase);
            if (nameComparison != 0) return nameComparison;
            int oboznachComparison = string.Compare(_oboznach, other._oboznach, StringComparison.CurrentCultureIgnoreCase);
            if (oboznachComparison != 0) return oboznachComparison;
            return string.Compare(_folder, other._folder, StringComparison.CurrentCultureIgnoreCase);
        }
        public new int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            if (!(obj is prj)) throw new ArgumentException($"Object must be of type {nameof(prj)}");
            return CompareTo((prj) obj);
        }
        public static bool operator <(prj left, prj right)
        {
            return Comparer<prj>.Default.Compare(left, right) < 0;
        }
        public static bool operator >(prj left, prj right)
        {
            return Comparer<prj>.Default.Compare(left, right) > 0;
        }
        public static bool operator <=(prj left, prj right)
        {
            return Comparer<prj>.Default.Compare(left, right) <= 0;
        }
        public static bool operator >=(prj left, prj right)
        {
            return Comparer<prj>.Default.Compare(left, right) >= 0;
        }
        public bool Equals(prj other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_name, other._name, StringComparison.CurrentCultureIgnoreCase) && string.Equals(_oboznach, other._oboznach, StringComparison.CurrentCultureIgnoreCase) && string.Equals(_folder, other._folder, StringComparison.CurrentCultureIgnoreCase);
            //return base.Equals(other) && string.Equals(_name, other._name, StringComparison.CurrentCultureIgnoreCase) && string.Equals(_oboznach, other._oboznach, StringComparison.CurrentCultureIgnoreCase) && string.Equals(_folder, other._folder, StringComparison.CurrentCultureIgnoreCase);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as prj;
            return other != null && Equals(other);
        }
        public override int GetHashCode()
        {
            /*unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (_name     != null ? StringComparer.CurrentCultureIgnoreCase.GetHashCode(_name) : 0);
                hashCode = (hashCode * 397) ^ (_oboznach != null ? StringComparer.CurrentCultureIgnoreCase.GetHashCode(_oboznach) : 0);
                hashCode = (hashCode * 397) ^ (_folder   != null ? StringComparer.CurrentCultureIgnoreCase.GetHashCode(_folder) : 0);
                return hashCode;
            }*/
            return base.GetHashCode();
        }
        public static bool operator ==(prj left, prj right) { return Equals(left, right); }
        public static bool operator !=(prj left, prj right) { return !Equals(left, right); }

        #endregion

        #region Events

 

        #endregion


        #region Propeties

        public override EntityType       Type      => EntityType.prj;

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

        public string Oboznach
        {
            get => _oboznach;
            set
            {
                value = conv.ClearString(value);
                if (_oboznach == value) return;
                if ( Changed == Status.not_changed) Changed = Status.edited;
                _oboznach = value;
                OnPropertyChanged(nameof(Oboznach));
            }
        }
        public string Folder
        {
            get => _folder;
            set
            {
                value = value.Trim();
                if (_folder == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _folder = value;
                OnPropertyChanged(nameof(Folder));
            }
        }
        public sor Sor
        {
            get => _sor;
            set
            {
                if (value == null || _sorId == value.Id)_sor = value;
            }
        }
        public long SorId
        {
            get => _sorId;
            set
            {
                if (_sorId == value) return;
                if ( Changed == Status.not_changed) Changed = Status.edited;
                _sorId = value;
                if (_sor?.Id != _sorId) _sor = null;
                OnPropertyChanged(nameof(SorId));
            }
        }
        public bindviewcond<smt> CSmts
        {
            get => _smts;
            set
            {
                if (value != null) value.IsCascading = true;
                _smts = value;
            }
        }

        //public bindview<pls> CPlss
        //{
        //    get => this[EntityType.pls] as bindview<pls>;
        //    set
        //    {
        //        value.IsCascading    = true;
        //        this[EntityType.pls] = value;
        //    }
        //}

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"{Oboznach}. {Name}";
        }

        public override void Clear()
        {
            base.Clear();
            _sor = null;
            _smts = null;
            _sorId = -1;
            _folder = "";
            _name = "";
            _oboznach = "";
            _changed = Status.not_changed;
        }

        public override void ClearLinks() { _sor = null;
            _smts = null;
        }

        public override void PrepareForDelete()
        {
            if (_smts != null && _smts.Count > 0)
            {
                _smts.IsCascading = true;
                for (int i = 0; i < _smts.Count;) _smts.Remove(_smts[i]);
            }
            ClearLinks();
        }
        #endregion

        //public new sor Parent { get; set; }
        //public new prjs Children { get; set; }


    }
}