using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace entitymodel
{
    [DebuggerDisplay("Id={this.Id} Name={this.Name} type={this.Type}")]
    public class rab : entity, IComparable<rab>, IEquatable<rab>
    {
        #region Fields
        private string _name;
        private string _originalname;
        private int    _position;
        private double _count;
        private decimal _costtotal;
        private decimal _cost;
        private decimal _costtotalnds;


        private wgr  _wgr;
        private long _wgrId = -1;

        #endregion

        #region Constructors

        public rab() => Init();
        public rab(int newId) : base(newId) => Init();

        private void Init()
        {

        }
        public rab(wgr wgrEntity)
        {
            Init();
            _wgr   = wgrEntity;
            _wgrId = wgrEntity.Id;
        }

        #endregion

        #region Comparer

        public int CompareTo(rab other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var entityComparison = base.CompareTo(other);
            if (entityComparison != 0) return entityComparison;
            int nameComparison = string.Compare(_name, other._name, StringComparison.CurrentCultureIgnoreCase);
            if (nameComparison != 0) return nameComparison;
            int originalnameComparison = string.Compare(_originalname, other._originalname, StringComparison.CurrentCultureIgnoreCase);
            if (originalnameComparison != 0) return originalnameComparison;
            int costComparison = _cost.CompareTo(other._cost);
            if (costComparison != 0) return costComparison;
            return _count.CompareTo(other._count);
        }
        public new int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            if (!(obj is rab)) throw new ArgumentException($"Object must be of type {nameof(rab)}");
            return CompareTo((rab)obj);
        }
        public static bool operator <(rab left, rab right)
        {
            return Comparer<rab>.Default.Compare(left, right) < 0;
        }
        public static bool operator >(rab left, rab right)
        {
            return Comparer<rab>.Default.Compare(left, right) > 0;
        }
        public static bool operator <=(rab left, rab right)
        {
            return Comparer<rab>.Default.Compare(left, right) <= 0;
        }
        public static bool operator >=(rab left, rab right)
        {
            return Comparer<rab>.Default.Compare(left, right) >= 0;
        }
        public bool Equals(rab other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && string.Equals(_name, other._name, StringComparison.CurrentCultureIgnoreCase) && string.Equals(_originalname, other._originalname, StringComparison.CurrentCultureIgnoreCase) && _cost == other._cost && _count.Equals(other._count);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as rab;
            return other != null && Equals(other);
        }
        public override int GetHashCode()
        {
            //unchecked
            //{
            //    int hashCode = base.GetHashCode();
            //    hashCode = (hashCode * 397) ^ (_name != null ? StringComparer.CurrentCultureIgnoreCase.GetHashCode(_name) : 0);
            //    hashCode = (hashCode * 397) ^ (_originalname != null ? StringComparer.CurrentCultureIgnoreCase.GetHashCode(_originalname) : 0);
            //    hashCode = (hashCode * 397) ^ _cost.GetHashCode();
            //    hashCode = (hashCode * 397) ^ _count.GetHashCode();
            //    return hashCode;
            //}
            return base.GetHashCode();
        }
        public static bool operator ==(rab left, rab right) { return Equals(left, right); }
        public static bool operator !=(rab left, rab right) { return !Equals(left, right); }


        #endregion


        #region Propeties
        public override EntityType Type => EntityType.rab;

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
        public string Originalname
        {
            get => _originalname;
            set
            {
                value = conv.ClearString(value);
                if (_originalname == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _originalname = value;
                OnPropertyChanged(nameof(Originalname));
            }
        }
        public int Position
        {
            get => _position;
            set
            {
                if (_position == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }
        public double Count
        {
            get => _count;
            set
            {
                if ((long)(_count*100000 )== (long)(value * 100000)) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _count = value;
                OnPropertyChanged(nameof(Count));
            }
        }
        public decimal Costtotal
        {
            get => _costtotal;
            set
            {
                if (_costtotal == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _costtotal = value;
                OnPropertyChanged(nameof(Costtotal));
            }
        }
        public decimal Cost
        {
            get => _cost;
            set
            {
                if (_cost == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _cost = value;
                OnPropertyChanged(nameof(Cost));
            }
        }
        public decimal Costtotalnds
        {
            get => _costtotalnds;
            set
            {
                if (_costtotalnds == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _costtotalnds = value;
                OnPropertyChanged(nameof(Costtotalnds)); 
            }
        }
        public wgr Wgr
        {
            get => _wgr;
            set
            {
                if (value == null || _wgrId == value.Id) _wgr = value;
            }
        }
        public long WgrId
        {
            get => _wgrId;
            set
            {
                if (_wgrId == value) return;
                if (Changed       == Status.not_changed) Changed = Status.edited;
                _wgrId = value;
                if (_wgr?.Id != _wgrId) _wgr = null;
                OnPropertyChanged(nameof(WgrId));
            }

        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"{_position}. {_name}";
        }

        public override void Clear()
        {
            base.Clear();
            _wgr   = null;
            _wgrId = -1;
            //_plss         = null;
            _position = -1;
            _name     = "";
            _count = 0;
            _cost = 0;
            _costtotal = 0;
            _costtotalnds = 0;
            _originalname = "";
            _changed  = Status.not_changed;
        }
        public override void ClearLinks()
        {
            _wgr = null;
            //_plss = null;
        }

        public override void PrepareForDelete()
        {
            //if (_plss != null && _plss.Count > 0)
            //{
            //    _plss.IsCascading = true;
            //    for (int i = 0; i < _plss.Count;) _plss.Remove(_plss[i]);

            //    //foreach (var v in _plss) _plss.Remove(v);
            //}
            ClearLinks();
        }
        #endregion


    }
}