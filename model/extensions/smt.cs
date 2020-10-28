using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace entitymodel
{
    [DebuggerDisplay("Id={this.Id} Name={this.SmetaName} type={this.Type}")]
    public class smt : entity, IComparable<smt>, IEquatable<smt>
    {
        #region Fields

        private int _vers;
        private List<string> _projectlists;
        private int _smetanumber;
        private string _smetaoboznach;
        private string _name;
        private string _link;

        private prj _prj;
        private long _prjId=-1;

        private bindviewcond<wgr> _wgrs;

        #endregion

        #region Constructors

        public smt() => Init();
        public smt(int newId) : base(newId) => Init();
        private void Init()
        {
            _projectlists=new List<string>();

        }
        public smt(prj prjEntity)
        {
            Init();
            _prj   = prjEntity;
            _prjId = prjEntity.Id;
        }

        #endregion

        #region Indexators/Enumerators


        #endregion

        #region Comparer

        public int CompareTo(smt other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var entityComparison = base.CompareTo(other);
            if (entityComparison != 0) return entityComparison;
            int versComparison = _vers.CompareTo(other._vers);
            if (versComparison != 0) return versComparison;
            int smetanumberComparison = _smetanumber.CompareTo(other._smetanumber);
            if (smetanumberComparison != 0) return smetanumberComparison;
            int smetaoboznachComparison = string.Compare(_smetaoboznach, other._smetaoboznach, StringComparison.CurrentCultureIgnoreCase);
            if (smetaoboznachComparison != 0) return smetaoboznachComparison;
            return string.Compare(_name, other._name, StringComparison.CurrentCultureIgnoreCase);
        }
        public new int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            if (!(obj is smt)) throw new ArgumentException($"Object must be of type {nameof(smt)}");
            return CompareTo((smt) obj);
        }
        public static bool operator <(smt left, smt right)
        {
            return Comparer<smt>.Default.Compare(left, right) < 0;
        }
        public static bool operator >(smt left, smt right)
        {
            return Comparer<smt>.Default.Compare(left, right) > 0;
        }
        public static bool operator <=(smt left, smt right)
        {
            return Comparer<smt>.Default.Compare(left, right) <= 0;
        }
        public static bool operator >=(smt left, smt right)
        {
            return Comparer<smt>.Default.Compare(left, right) >= 0;
        }
        public bool Equals(smt other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && _vers == other._vers && _smetanumber == other._smetanumber && string.Equals(_smetaoboznach, other._smetaoboznach, StringComparison.CurrentCultureIgnoreCase) && string.Equals(_name, other._name, StringComparison.CurrentCultureIgnoreCase);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as smt;
            return other != null && Equals(other);
        }
        public override int GetHashCode()
        {
            //unchecked
            //{
            //    int hashCode = base.GetHashCode();
            //    hashCode = (hashCode * 397) ^ _vers;
            //    hashCode = (hashCode * 397) ^ _smetanumber;
            //    hashCode = (hashCode * 397) ^ (_smetaoboznach != null ? StringComparer.CurrentCultureIgnoreCase.GetHashCode(_smetaoboznach) : 0);
            //    hashCode = (hashCode * 397) ^ (_name          != null ? StringComparer.CurrentCultureIgnoreCase.GetHashCode(_name) : 0);
            //    return hashCode;
            //}
            return base.GetHashCode();
        }
        public static bool operator ==(smt left, smt right) { return Equals(left, right); }
        public static bool operator !=(smt left, smt right) { return !Equals(left, right); }

        #endregion

        #region Events


        #endregion

        #region Propeties
        public override EntityType Type => EntityType.smt;
        public int ProjectVersiya
        {
            get => _vers;
            set
            {
                if (_vers == value) return;
                if ( Changed == Status.not_changed) Changed = Status.edited;
                _vers = value;
                OnPropertyChanged(nameof(ProjectVersiya));
            }
        }
        public List<string> Projectlists
        {
            get => _projectlists;
            set => _projectlists = value;
        }

        public int SmetaNumber
        {
            get => _smetanumber;
            set
            {
                if (_smetanumber == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _smetanumber = value;
                OnPropertyChanged(nameof(SmetaNumber));
            }
        }
        public string SmetaOboznach
        {
            get => _smetaoboznach;
            set
            {
                value = conv.ClearString(value);
                if (_smetaoboznach == value) return;
                if ( Changed == Status.not_changed) Changed = Status.edited;
                _smetaoboznach = value;
                OnPropertyChanged(nameof(SmetaOboznach));
            }
        }
        public string SmetaName
        {
            get => _name;
            set
            {
                value = conv.ClearString(value);
                if (_name == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _name = value;
                OnPropertyChanged(nameof(SmetaName));
               
            }
        }
        public string SmetaLink
        {
            get => _link;
            set
            {
                value = conv.ClearString(value);
                if (_link == value) return;
                if (_link != value && Changed == Status.not_changed) Changed = Status.edited;
                _link = value;
                OnPropertyChanged(nameof(SmetaLink));
            }
        }
        public prj Project
        {
            get
            {
                return _prj;
            }
            set
            {
                if (value == null || _prjId == value.Id) _prj = value;
            }
        }
        public long PrjId
        {
            get
            {
                return _prjId;
            }
            set
            {
                if (_prjId == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _prjId = value;
                if (_prj?.Id != _prjId) _prj = null;
                OnPropertyChanged(nameof(PrjId));
            }
        }
        public bindviewcond<wgr> CWgrs
        {
            get => _wgrs;
            set
            {
                if (value != null) value.IsCascading = true;
                _wgrs = value;
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"{_smetaoboznach}. - {_name}({_vers})";
        }

        public override void Clear()
        {
            base.Clear();
            _prj          = null;
            _prjId        = -1;
            _wgrs         = null;
            _projectlists.Clear();
            _vers = 0;
            _link = "";
            _name   = "";
            _smetanumber         = 0;
            _smetaoboznach        = "";
            _changed      = Status.not_changed;
        }
        public override void ClearLinks()
        {
            _prj = null;
            _wgrs = null;
        }

        public override void PrepareForDelete()
        {
            if (_wgrs != null && _wgrs.Count > 0)
            {
                _wgrs.IsCascading = true;
                for (int i = 0; i < _wgrs.Count;) _wgrs.Remove(_wgrs[i]);

                //foreach (var v in _plss) _plss.Remove(v);
            }
            ClearLinks();
        }


        public void AddProjectList(string listPath)
        {
            if (listPath=="" || _projectlists.Contains(listPath)) return;
            _projectlists.Add(listPath);
            if (Changed == Status.not_changed) Changed = Status.edited;
            OnPropertyChanged(nameof(Projectlists));
        }

        public void RemoveProjectList(string listPath)
        {
            if (listPath == "" || !_projectlists.Contains(listPath)) return;
            _projectlists.Remove(listPath);
            if (Changed == Status.not_changed) Changed = Status.edited;
            OnPropertyChanged(nameof(Projectlists));
        }
        #endregion


    }
}