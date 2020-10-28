using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace entitymodel
{
    [DebuggerDisplay("Id={this.Id} Name={this.Name} type={this.Type}")]
    public class sor : entity, IComparable<sor>, IEquatable<sor>
    {

        #region Fields

        private string _name   = "";
        private string _gp     = "";
        private string _adress = "";


        private pls _pls;
        private long _plsId;

        private bindviewcond<prj> _prjs;
        #endregion

        #region Constructors

        public sor() => Init();
        public sor(int newId) : base(newId) => Init();
        private void Init()
        {
        }

        public sor(pls plsEntity)
        {
            Init();
            _pls = plsEntity;
            _plsId = plsEntity.Id;
        }

        #endregion

        #region Comparer

        public int CompareTo(sor other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            int nameComparison = string.Compare(_name, other._name, StringComparison.CurrentCultureIgnoreCase);
            if (nameComparison != 0) return nameComparison;
            int gpComparison = string.Compare(_gp, other._gp, StringComparison.CurrentCultureIgnoreCase);
            if (gpComparison != 0) return gpComparison;
            return string.Compare(_adress, other._adress, StringComparison.CurrentCultureIgnoreCase);
        }
        public new int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            if (!(obj is sor)) throw new ArgumentException($"Object must be of type {nameof(sor)}");
            return CompareTo((sor) obj);
        }
        public static bool operator <(sor left, sor right)
        {
            return Comparer<sor>.Default.Compare(left, right) < 0;
        }
        public static bool operator >(sor left, sor right)
        {
            return Comparer<sor>.Default.Compare(left, right) > 0;
        }
        public static bool operator <=(sor left, sor right)
        {
            return Comparer<sor>.Default.Compare(left, right) <= 0;
        }
        public static bool operator >=(sor left, sor right)
        {
            return Comparer<sor>.Default.Compare(left, right) >= 0;
        }
        public bool Equals(sor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_name, other._name, StringComparison.CurrentCultureIgnoreCase) && string.Equals(_gp, other._gp, StringComparison.CurrentCultureIgnoreCase) && string.Equals(_adress, other._adress, StringComparison.CurrentCultureIgnoreCase);
            //return base.Equals(other) && string.Equals(_name, other._name, StringComparison.CurrentCultureIgnoreCase) && string.Equals(_gp, other._gp, StringComparison.CurrentCultureIgnoreCase) && string.Equals(_adress, other._adress, StringComparison.CurrentCultureIgnoreCase);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as sor;
            return other != null && Equals(other);
        }
        public override int GetHashCode()
        {
            /*unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (_name   != null ? StringComparer.CurrentCultureIgnoreCase.GetHashCode(_name) : 0);
                hashCode = (hashCode * 397) ^ (_gp     != null ? StringComparer.CurrentCultureIgnoreCase.GetHashCode(_gp) : 0);
                hashCode = (hashCode * 397) ^ (_adress != null ? StringComparer.CurrentCultureIgnoreCase.GetHashCode(_adress) : 0);
                return hashCode;
            }*/
            return base.GetHashCode();
        }
        public static bool operator ==(sor left, sor right) { return Equals(left, right); }
        public static bool operator !=(sor left, sor right) { return !Equals(left, right); }

        #endregion

        #region Events



        #endregion

        #region Propeties

        public override EntityType       Type       => EntityType.sor;

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

        public string GP
        {
            get => _gp;
            set
            {
                value = conv.ClearString(value);
                if (_gp == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _gp = value;
                OnPropertyChanged(nameof(GP));
            }
        }

        public string Adress
        {
            get => _adress;
            set
            {
                value = conv.ClearString(value);
                if (_adress == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _adress = value;
                OnPropertyChanged(nameof(Adress));
            }
        }

        public bindviewcond<prj> CPrjs
        {
            get => _prjs;
            set
            {
                if (value != null) value.IsCascading    = true;
                _prjs = value;
            }
        }
        public pls Pls
        {
            get => _pls;
            set
            {
                if(value == null || _plsId == value.Id) _pls = value;
            }
        }

        public long PlsId
        {
            get => _plsId;
            set
            {
                if (_plsId == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _plsId = value;
                if (_pls?.Id != _plsId) _pls = null;
                OnPropertyChanged(nameof(PlsId));
            }
        }

        #endregion

        #region Methods
        public override void Clear()
        {
            base.Clear();
            _changed      = Status.not_changed;

            _pls = null;
            _plsId = -1;
            _adress = "";
            _gp = "";
            _name = "";
            _prjs = null;
        }

        public override void ClearLinks() { _pls = null;
            _prjs = null;
        }

        public override string ToString()
        {
            return $"ГП {GP}, {Name}";
        }
        public override void PrepareForDelete()
        {
            if (_prjs != null && _prjs.Count>0)
            {
                _prjs.IsCascading = true;
                for (int i = 0; i < _prjs.Count;) _prjs.Remove(_prjs[i]);
            }
            ClearLinks();
        }
        #endregion

        //public new pls Parent { get; set; }
        //public new prjs Children { get; set; }


    }
}