using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace entitymodel
{
    [DebuggerDisplay("Id={this.Id} Name={this.Name} type={this.Type}")]
    public class wgr : entity, IComparable<wgr>, IEquatable<wgr>
    {
        #region Fields
        private string _name;
        private int _position;

        private smt  _smt;
        private long _smtId = -1;

        private bindviewcond<rab> _rabs;

        #endregion

        #region Constructors

        public wgr() => Init();
        public wgr(int newId) : base(newId) => Init();
        private void Init()
        {

        }
        public wgr(smt smtEntity)
        {
            Init();
            _smt   = smtEntity;
            _smtId = smtEntity.Id;
        }

        #endregion

        #region Comparer

        public int CompareTo(wgr other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var entityComparison = base.CompareTo(other);
            if (entityComparison != 0) return entityComparison;
            int positionComparison = _position.CompareTo(other._position);
            if (positionComparison != 0) return positionComparison;
            return string.Compare(_name, other._name, StringComparison.CurrentCultureIgnoreCase);
        }
        public new  int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            if (!(obj is wgr)) throw new ArgumentException($"Object must be of type {nameof(wgr)}");
            return CompareTo((wgr)obj);
        }
        public static bool operator <(wgr left, wgr right)
        {
            return Comparer<wgr>.Default.Compare(left, right) < 0;
        }
        public static bool operator >(wgr left, wgr right)
        {
            return Comparer<wgr>.Default.Compare(left, right) > 0;
        }
        public static bool operator <=(wgr left, wgr right)
        {
            return Comparer<wgr>.Default.Compare(left, right) <= 0;
        }
        public static bool operator >=(wgr left, wgr right)
        {
            return Comparer<wgr>.Default.Compare(left, right) >= 0;
        }
        public bool Equals(wgr other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && string.Equals(_name, other._name, StringComparison.CurrentCultureIgnoreCase) && _position == other._position;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as wgr;
            return other != null && Equals(other);
        }
        public override int GetHashCode()
        {
            //unchecked
            //{
            //    int hashCode = base.GetHashCode();
            //    hashCode = (hashCode * 397) ^ (_name != null ? StringComparer.CurrentCultureIgnoreCase.GetHashCode(_name) : 0);
            //    hashCode = (hashCode * 397) ^ _position;
            //    return hashCode;
            //}
            return base.GetHashCode();
        }
        public static bool operator ==(wgr left, wgr right) { return Equals(left, right); }
        public static bool operator !=(wgr left, wgr right) { return !Equals(left, right); }

        #endregion

        #region Propeties

        public override EntityType Type => EntityType.wgr;

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
        public long SmtId
        {
            get => _smtId;
            set
            {
                if (_smtId == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _smtId = value;
                if (_smt?.Id != _smtId) _smt = null;
                OnPropertyChanged(nameof(SmtId));
            }
        }
        public smt Smt
        {
            get => _smt;
            set
            {
                if (value == null || _smtId == value.Id) _smt = value;
            }
        }
        public bindviewcond<rab> CRabs
        {
            get => _rabs;
            set
            {
                if (value != null) value.IsCascading = true;
                _rabs = value;
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
            _smt          = null;
            _smtId        = -1;
            _rabs      = null;
            _position = -1;
            _name   = "";
            _changed      = Status.not_changed;
        }
        public override void ClearLinks()
        {
            _smt  = null;
            _rabs = null;
        }

        public override void PrepareForDelete()
        {
            if (_rabs != null && _rabs.Count > 0)
            {
                _rabs.IsCascading = true;
                for (int i = 0; i < _rabs.Count;) _rabs.Remove(_rabs[i]);

                //foreach (var v in _plss) _plss.Remove(v);
            }
            ClearLinks();
        }
        #endregion


    }
}