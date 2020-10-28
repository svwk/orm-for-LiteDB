using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace entitymodel
{
    [DebuggerDisplay("Id={this.Id} Name={this.ObjectName} type={this.Type}")]
    public class dog : entity, IComparable<dog>, IEquatable<dog>
    {
        #region Fields

        private DateTime _dataZakl = DateTime.Now;
        private decimal  _summa;
        private DateTime _srok         = DateTime.Now;
        private string   _objectName   = "";
        private string   _objectAdress = "";
        private dogc     _content      = null;

        private bindviewcond<pls> _plss;
        private zak _zak;
        private long _zakId = -1;
        #endregion

        #region Constructors

        public dog() => Init();
        public dog(int newId) : base(newId) => Init();
        private void Init()
        {

        }
        public dog(zak zakEntity)
        {
            Init();
            _zak = zakEntity;
            _zakId = zakEntity.Id;
        }

        #endregion

        #region Comparer

        public int CompareTo(dog other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            int objectNameComparison = string.Compare(_objectName, other._objectName, StringComparison.CurrentCultureIgnoreCase);
            if (objectNameComparison != 0) return objectNameComparison;

            objectNameComparison =string.Compare(_objectAdress, other._objectAdress, StringComparison.CurrentCultureIgnoreCase);
            if (objectNameComparison != 0) return objectNameComparison;

            int dataZaklComparison = _dataZakl.CompareTo(other._dataZakl);
            if (dataZaklComparison != 0) return dataZaklComparison;

            int srokComparison = _srok.CompareTo(other._srok);
            if (srokComparison != 0) return srokComparison;

            return _summa.CompareTo(other._summa);
        }
        public new int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            if (!(obj is dog)) throw new ArgumentException($"Object must be of type {nameof(dog)}");
            return CompareTo((dog) obj);
        }
        public static bool operator <(dog left, dog right)
        {
            return Comparer<dog>.Default.Compare(left, right) < 0;
        }
        public static bool operator >(dog left, dog right)
        {
            return Comparer<dog>.Default.Compare(left, right) > 0;
        }
        public static bool operator <=(dog left, dog right)
        {
            return Comparer<dog>.Default.Compare(left, right) <= 0;
        }
        public static bool operator >=(dog left, dog right)
        {
            return Comparer<dog>.Default.Compare(left, right) >= 0;
        }
        public bool Equals(dog other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _dataZakl.Equals(other._dataZakl) && _summa.Equals(other._summa) && _srok.Equals(other._srok) && string.Equals(_objectName, other._objectName, StringComparison.CurrentCultureIgnoreCase) && string.Equals(_objectAdress, other._objectAdress, StringComparison.CurrentCultureIgnoreCase);
            //return base.Equals(other) && _dataZakl.Equals(other._dataZakl) && _summa.Equals(other._summa) && _srok.Equals(other._srok) && string.Equals(_objectName, other._objectName, StringComparison.CurrentCultureIgnoreCase) && string.Equals(_objectAdress, other._objectAdress, StringComparison.CurrentCultureIgnoreCase);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as dog;
            return other != null && Equals(other);
        }
        public override int GetHashCode()
        {
            /*unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ _dataZakl.GetHashCode();
                hashCode = (hashCode * 397) ^ _summa.GetHashCode();
                hashCode = (hashCode * 397) ^ _srok.GetHashCode();
                hashCode = (hashCode * 397) ^ (_objectName   != null ? StringComparer.CurrentCultureIgnoreCase.GetHashCode(_objectName) : 0);
                hashCode = (hashCode * 397) ^ (_objectAdress != null ? StringComparer.CurrentCultureIgnoreCase.GetHashCode(_objectAdress) : 0);
                return hashCode;
            }
            */
            return base.GetHashCode();
        }
        public static bool operator ==(dog left, dog right) { return Equals(left, right); }
        public static bool operator !=(dog left, dog right) { return !Equals(left, right); }

        #endregion

        #region Events

 
        #endregion

        #region Propeties

        public override EntityType       Type       => EntityType.dog;
        //public          entitiesp<zak>   PZaks      => ParentManager.Zaks;
        //public          entitiesp<pls>   CPlss      => ChildrenManager.Plss;
        //public new      BindingList<zak> BLParents  => ParentManager.Zaks.Items;
        //public new      BindingList<pls> BLChildren => ChildrenManager.Plss.Items;

        public DateTime DataZakl
        {
            get => _dataZakl;
            set
            {
                if (_dataZakl == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _dataZakl = value;
                OnPropertyChanged(nameof(DataZakl));
            }
        }

        public decimal Summa
        {
            get => _summa;
            set
            {
                if (_summa == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _summa = value;
                OnPropertyChanged(nameof(Summa));
            }
        }

        public DateTime Srok
        {
            get => _srok;
            set
            {
                if (_srok == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _srok = value;
                OnPropertyChanged(nameof(Srok));
            }
        }

        public string ObjectName
        {
            get => _objectName;
            set
            {
                value = conv.ClearString(value);
                if (_objectName == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _objectName = value;
                OnPropertyChanged(nameof(ObjectName));
            }
        }

        public string ObjectAdress
        {
            get => _objectAdress;
            set
            {
                value = conv.ClearString(value);
                if (_objectAdress == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _objectAdress = value;
                OnPropertyChanged(nameof(ObjectAdress));
            }
        }

        public dogc Content
        {
            get => _content;
            set
            {
                if (_content == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }


        public bindviewcond<pls> CPlss
        {
            get => _plss;
            set
            {
                if (value != null) value.IsCascading = true;
                _plss= value;
            }
        }
        public zak Zak
        {
            get => _zak;
            set
            {
                if (value == null || _zakId == value.Id) _zak = value;
            }
        }

        public long ZakId
        {
            get => _zakId;
            set
            {
                if (_zakId == value) return;
                if (Changed == Status.not_changed) Changed = Status.edited;
                _zakId = value;
                if (_zak?.Id != _zakId) _zak = null;
                OnPropertyChanged(nameof(ZakId));
            }
        }

        #endregion
        #region Methods

        public override string ToString()
        {
            return $"{ObjectName}. - {ObjectAdress}";
        }

        public override void Clear()
        {
            base.Clear();
            _zak = null;
            _zakId = -1;
            _plss = null;
            _content = null;
            _dataZakl=DateTime.MinValue;
            _objectAdress = "";
            _objectName = "";
            _srok=DateTime.MinValue;
            _summa = 0;
            _changed  = Status.not_changed;
        }
        public override void ClearLinks() { _zak = null;
            _plss = null;
        }

        public override void PrepareForDelete()
        {
            if (_plss != null && _plss.Count>0)
            {
                _plss.IsCascading = true;
                for (int i = 0; i < _plss.Count;) _plss.Remove(_plss[i]);

                //foreach (var v in _plss) _plss.Remove(v);
            }
            ClearLinks();
        }
        #endregion

        //public new zak Parent { get; set; }
        //public new plss Children { get; set; }   


    }
}