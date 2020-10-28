using System;
using System.Collections.Generic;
using System.ComponentModel;
using entitymodel.Annotations;

namespace entitymodel
{
    
    public class bindview<T> : bindlist<T>, IBindview where T : entity, new()
    {
        #region Fields

        protected bindbaselist<T> _mainBindinglist;
        
        protected bool _isCascading;  //удалять ли полностью элементы
        protected SourceType _source = SourceType.Free;

        protected bool _mainBindinglistChange = true;
        #endregion

        #region Constructors

        public bindview()
        {
            _isVeiw   = true;
        }
        public bindview([NotNull] bindbaselist<T> baseList) :base()
        {
            _isVeiw = true;
            _mainBindinglist= baseList;
            _mainBindinglist.ListChanged += _mainBindinglist_ListChanged;
        }

        public bindview([NotNull] bindbaselist<T> baseList, [NotNull] List<T> list) : base()
        {
            _isVeiw          = true;
            _mainBindinglist = baseList;
            _mainBindinglist.ListChanged += _mainBindinglist_ListChanged;
            AddLink(list);
         }


        #endregion


        #region Events

        protected virtual void _mainBindinglist_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (!_mainBindinglistChange) return;
            if (_mainBindinglist == null || sender==null || !Object.ReferenceEquals(sender, _mainBindinglist)) return;

            T o= null;
            if (e is entListChangedEventArgs en) o = en.ChangedEntity as T;           
            
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset: RefreshList();break;
                case ListChangedType.ItemAdded:break;
                case ListChangedType.ItemDeleted:
                    if (o != null &&  Contains(o) && (o.Changed==Status.deleted ||o.Changed==Status.newdeleted)) RemoveLink(o.Id);
                    break;
                case ListChangedType.ItemMoved:                 break;
                case ListChangedType.ItemChanged:               break;
                case ListChangedType.PropertyDescriptorAdded:   break;
                case ListChangedType.PropertyDescriptorDeleted: break;
                case ListChangedType.PropertyDescriptorChanged: break;
                
            }
        }

        #endregion



        #region Propeties

        public bool IsCascading { get => _isCascading; set => _isCascading = value; }
       
        public bindbaselist<T> MainBindinglist
        {
            get => _mainBindinglist;
            set
            {
                if (_mainBindinglist != null && !object.ReferenceEquals(_mainBindinglist,value)) throw new Exception("Нельзя перемещать вид в другую таблицу");
                if (value==null) return;
                if (_mainBindinglist == null)
                {
                    _mainBindinglist = value;
                    _mainBindinglist.ListChanged += _mainBindinglist_ListChanged;
                    RefreshList();
                }

            }
        }

        IBindbaselist IBindview.MainBindinglist
        {
            get => _mainBindinglist;
            set
            {
                if ((value is bindbaselist<T> tmpList))
                {
                    this.MainBindinglist = tmpList;
                }
            }
        }
        public override IEntityBindinglist DefaultView => this;
        public SourceType Sourcetype => _source;

        #endregion

        #region Methods

        #region Checks
        public bool CheckForLosen(bool removeLosen = true)
        {
            if (_items == null || _items.Count == 0) return true;
            int c = Count;
            if (_mainBindinglist == null || _mainBindinglist.Count == 0)
            {
                if (removeLosen && c > 0)
                {
                        ClearItems();
                        return false;
                }
                return false;
            }
            
            for (int j = 0; j < _items.Count; j++)
            {
                if (!_mainBindinglist.CheckItem(_items[j]))
                {
                    if (removeLosen)
                    {
                        RemoveLink(j);
                        j -= 1;
                    }
                    else return false;
                }
            }
            return (c == Count);
        }
        public bool CheckList(bool toRemove = true)
        {
            if (!(CheckForLosen(toRemove)))
            {
                if (toRemove)
                {
                    _m = true;
                    FireListChanged(ListChangedType.Reset, -1);
                }
                return false;
            }
            return true;
        }

        protected override bool IsSortedCore => true;


        #endregion

        #region Resetting

 
        public override void Reset()
        {
            _modified  = false;
            _addNewPos = -1;
            _m         = false;
        }

        public virtual void ClearView(bool generateEvent)
        {
            base.Clear(generateEvent);
        }
        public virtual void RefreshList()
        {
            if (_items == null || _items.Count == 0) return;
            CheckList();
        }

        #endregion

        #region Add

        #region добавление ссылок
        public virtual void AddLink([NotNull] List<T> list,bool renew=false)
        {
            int c = Count;
            if (renew && _items != null && _items.Count > 0) ClearItems();
            if (_mainBindinglist == null || _mainBindinglist.Count == 0 || list==null || list.Count==0)
            {
                if (renew && c!=Count)
                {
                    _m = true;
                    FireListChanged(ListChangedType.Reset, -1);
                }
                return;
            }
            List<T> list1 = list;
            _mainBindinglist.CheckList(list1);
            if ( list1.Count>0)
            {
                bool a = _raiseListChangedEvents;
                _raiseListChangedEvents = false;
                foreach (var v in list1)
                {
                    AddLink(v);
                    //if (!Renew && FindIndexById(v.Id) >= 0)continue;
                    //base.InsertItem(-1, v);
                }
                _raiseListChangedEvents = a;
            }

            if (c != Count)
            {
                _m = true;
                FireListChanged(ListChangedType.Reset, -1);
            }
        }

        public virtual void AddLink([NotNull] List<T> list, [NotNull] bindbaselist<T> baseList)
        {
            if (baseList == null) return;
            if (_mainBindinglist==null)MainBindinglist = baseList;
            AddLink(list,true);
        }

        public virtual void AddLink([NotNull] List<long> list, bool renew = false)
        {
            if (list != null && list.Count > 0 && _mainBindinglist != null && _mainBindinglist.Count > 0)
            {
                List<T> list3 = _mainBindinglist.CreateList(list);
                AddLink(list3,renew);
            }
        }

        public virtual void AddLink([NotNull] List<long> list, [NotNull] bindbaselist<T> baseList)
        {
            if (baseList == null) return;
            if (_mainBindinglist == null) MainBindinglist = baseList;
            AddLink(list,true);
        }

        public virtual  int AddLink([NotNull] T item, int index=-1)
        {
            if (_items == null) _items = new List<T>();
            if (_mainBindinglist == null || _mainBindinglist.Count == 0 ||item==null|| !_mainBindinglist.CheckItem(item)) return-1;

            
            if (_items.Count == 0) index = 0;
            else if (index >= 0 && index < _items.Count && _items[index].Id == item.Id) //попытка вставить в заданную позицию
            {
                if (object.ReferenceEquals(_items[index], item) || _items[index] == item) return index; //зачем вставлять то же самое значение
            }
            else
            {
                index = _items.Count;
                for (int i = 0; i < _items.Count; i++)
                {
                    if (_items[i].Id == item.Id)
                    {
                        if (item == _items[i]) return i;
                        index = i;
                        break;
                    }
                }
            }


            
            item.Fix();
            if (index < _items.Count && _items[index].Id == item.Id)
            {
                if (_raiseItemChangedEvents) UnhookPropertyChanged(this[index]);
                _items[index] = item;
                if (_raiseItemChangedEvents) HookPropertyChanged(item);
                FireListChanged(ListChangedType.ItemChanged, index, item);
            }
            else
            {
                EndNew(_addNewPos);
                _items.Insert(index, item);
                _addNewPos = index;
                if (_raiseItemChangedEvents) HookPropertyChanged(item);
                _m = true;
                FireListChanged(ListChangedType.ItemAdded, index, item);
            }

            if (item.Changed != Status.not_changed) _modified = true;
            return index;
        }
        public virtual int AddLink([NotNull] entity item, int index = -1)
        {
            if (item is T it)
            {
                return AddLink(it, index);
            }
            return -1;
        }
        public virtual int AddLink(long itemId)
        {
            if (_mainBindinglist == null || _mainBindinglist.Count == 0 || itemId<0 ) return -1;
             T item=_mainBindinglist.FindEntityById(itemId);
            if (item == null) return -1;
            return AddLink(item);
        }
        

        #endregion

        #region Добавление новых элементов в основную коллекцию и ссылок на них в эту

        protected internal override int InsertItem(int index, [NotNull] T item)
        {
            if (_mainBindinglist == null || item == null) return -1;
            if (!_mainBindinglist.CheckItem(item))
            {
                _mainBindinglistChange = false;
                _mainBindinglist.InsertItem(-1, item);
                _mainBindinglistChange = true;
            }

            return AddLink(item, index);
        }

        #endregion

        #region AddNew
  
        public override void EndNew(int itemIndex)
        {
            if (_items == null || _items.Count == 0 || itemIndex < 0 || itemIndex >= _items.Count) return;
            if (_addNewPos >= 0 && _addNewPos == itemIndex)
            {
                if (_items[itemIndex].Changed != Status.deleted && _items[itemIndex].Changed != Status.newdeleted)
                {
                    _mainBindinglistChange = false;
                    _mainBindinglist?.EndNew(_mainBindinglist.FindIndexById(_items[itemIndex].Id));
                    _mainBindinglistChange = true;
                }
                base.EndNew(itemIndex);
            }
        }
        #endregion

        #endregion

        #region Удаление самих элементов из основной коллекции и ссылок на них
        protected override void RemoveItem(int index)
        {
            //if (!_isCascading && _isLinked && _sourceString!=null) return;
            if (_items == null || _items.Count == 0 || index < 0 || index >= _items.Count) return; //throw new ArgumentOutOfRangeException(); //return;
            if (_mainBindinglist != null && _mainBindinglist.Count != 0)
            {
                if ((_isCascading || (_addNewPos == index)) && (_items[index].Changed != Status.deleted && _items[index].Changed != Status.newdeleted))
                {
                        _mainBindinglistChange = false;
                        _mainBindinglist?.Remove(_items[index]);
                        _mainBindinglistChange = true;
                    
                }
            }
            RemoveLink(index);
        }
        #endregion

        #region Удаление ссылок на элементы

        public virtual void RemoveLink(int index)
        {
            if (_items == null || _items.Count == 0 || index < 0 || index >= _items.Count) return; //throw new ArgumentOutOfRangeException(); //return;
            T a = _items[index];
            if (!_allowRemove && (a.Changed != Status.newcreated && a.Changed!=Status.newdeleted)) return;// throw new NotSupportedException();
            EndNew(index);
            if (_raiseItemChangedEvents) UnhookPropertyChanged(a);
            _items.RemoveAt(index);
            _m        = true;
            FireListChanged(ListChangedType.ItemDeleted, index, a);
        }

        public void RemoveLink(long id)
        {
            if (_items == null || _items.Count == 0 || id < 0) return;// throw new ArgumentOutOfRangeException(); 
            int i = FindIndexById(id);
            if (i>=0) RemoveLink(i);
        }
        #endregion




        #endregion





    }
}