using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Versioning;
using entitymodel.Annotations;

namespace entitymodel
{
    

    /// <summary>Предоставляет коллекцию сущностей, которая поддерживает привязку данных.</summary>
    /// <typeparam name="T">Тип элементов в списке, наследников класса entity.</typeparam>
    
    [DebuggerDisplay("Count={this.Count} type={this.Type} View={this.IsVeiw}")]
    [Serializable]
    public partial class bindlist<T> : IList<T>, IList, ICollection<T>, ICollection, IEnumerable<T>, IEnumerable, IReadOnlyList<T>, IReadOnlyCollection<T>, IBindingList, ICancelAddNew, IRaiseItemChangedEvents, IEntityBindinglist where T : entity, new()
    {
        #region Fields

        //списки
        protected List<T> _items;
        [NonSerialized] protected PropertyDescriptorCollection _itemTypeProperties;
        
        //вспом переменные
        protected                 int _addNewPos       = -1;
        [NonSerialized] protected int _lastChangeIndex = -1;
        [NonSerialized] protected object _syncRoot;


        //свойства
        protected readonly EntityType _type;
        protected          string     _name;

        //флаги
        protected bool _modified;
        protected bool _m;

        protected bool _allowNew               = true;
        protected bool _allowEdit              = true;
        protected bool _allowRemove            = true;
        
        protected bool _userSetAllowNew;

        protected bool _isVeiw;
        
        #endregion


        #region Constructors

        /// <summary>Инициализирует новый экземпляр класса <see cref="T:System.ComponentModel.BindingList`1" />, используя значения по умолчанию.</summary>
        public bindlist()
        {
            _items = new List<T>();
            _type = conv.SetType(typeof(T));
            _name = _type.ToString();

            _allowNew = ItemTypeHasDefaultConstructor;
            if (!typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(T))) _raiseItemChangedEvents = false;
        }

        /// <summary>Инициализирует новый экземпляр <see cref="T:System.ComponentModel.BindingList`1" /> класса с указанным списком.</summary>
        /// <param name="list">
        /// <see cref="T:System.Collections.Generic.IList`1" /> Элементов, которые должны содержаться в <see cref="T:System.ComponentModel.BindingList`1" />.</param>
        public bindlist(IList<T> list):this()
        {
            Add(list,false);
        }

        private bool ItemTypeHasDefaultConstructor
        {
            get
            {
                Type type = typeof(T);
                return type.IsPrimitive ||
                       type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance,
                                           (Binder) null, new Type[0], (ParameterModifier[]) null) !=
                       (ConstructorInfo) null;
            }
        }


        ~bindlist()
        {

        }

        #endregion

        #region Events

        [NonSerialized] protected AddingNewEventHandler       _onAddingNew;
        [NonSerialized] protected ListChangedEventHandler     _onListChanged;
        [NonSerialized] protected PropertyChangedEventHandler _propertyChangedEventHandler;
        protected bool _raiseListChangedEvents = true;
        protected readonly bool _raiseItemChangedEvents=true;

        bool IRaiseItemChangedEvents.RaisesItemChangedEvents => _raiseItemChangedEvents;

        #region Создание событий перед созданием новых элементов

        /// <summary>Происходит перед добавлением элемента в список.</summary>
        public event AddingNewEventHandler AddingNew
        {
            add
            {
                bool allowNew = AllowNew;
                _onAddingNew += value;
                if (allowNew == AllowNew) return;
                FireListChanged(ListChangedType.Reset, -1);
            }
            remove
            {
                bool allowNew = AllowNew;
                if (_onAddingNew != null) _onAddingNew -= value;
                if (allowNew == AllowNew) return;
                FireListChanged(ListChangedType.Reset, -1);
            }
        }

        /// <summary>Вызывает событие <see cref="E:System.ComponentModel.BindingList`1.AddingNew" />.</summary>
        /// <param name="e">Объект класса <see cref="T:System.ComponentModel.AddingNewEventArgs" />, содержащий данные события.</param>
        protected virtual void OnAddingNew(AddingNewEventArgs e)
        {
            _onAddingNew?.Invoke((object) this, e);
        }

        private T FireAddingNew()
        {
            AddingNewEventArgs e = new AddingNewEventArgs((object) null);
            OnAddingNew(e);
            return (e.NewObject as T);
        }

        private bool AddingNewHandled
        {
            get
            {
                if (_onAddingNew != null) return (uint)_onAddingNew.GetInvocationList().Length > 0U;
                return false;
            }
        }

        #endregion

        #region Создание событий изменения списка

        /// <summary>Происходит при изменении списка или элемента в списке.</summary>
        public event ListChangedEventHandler ListChanged
        {
            add => _onListChanged += value;
            remove
            {
                if (_onListChanged?.GetInvocationList().Length>0) _onListChanged -= value;
            }
        }

        /// <summary>Вызывает событие <see cref="E:System.ComponentModel.BindingList`1.ListChanged" />.</summary>
        /// <param name="e">Объект <see cref="T:System.ComponentModel.ListChangedEventArgs" />, содержащий данные события.</param>
        protected virtual void OnListChanged(entListChangedEventArgs e)
        {
            if (_onListChanged == null) return;
            _onListChanged((object) this, e);
        }

        /// <summary>Возвращает или задает значение, указывающее, было ли добавление или удаление элементов в списке вызывает <see cref="E:System.ComponentModel.BindingList`1.ListChanged" /> события.</summary>
        /// <returns>true При добавлении или удалении элементов вызывает <see cref="E:System.ComponentModel.BindingList`1.ListChanged" /> события; в противном случае — false. Значение по умолчанию — true.</returns>
        public bool RaiseListChangedEvents
        {
            get
            {
                return _raiseListChangedEvents;
            }
            set
            {
                if (_raiseListChangedEvents == value) return;
                _raiseListChangedEvents = value;
            }
        }

        /// <summary>Вызывает <see cref="E:System.ComponentModel.BindingList`1.ListChanged" /> событие типа <see cref="F:System.ComponentModel.ListChangedType.Reset" />.</summary>
        public void ResetBindings()
        {
            FireListChanged(ListChangedType.Reset, -1);
        }

        /// <summary>Вызывает <see cref="E:System.ComponentModel.BindingList`1.ListChanged" /> событие типа <see cref="F:System.ComponentModel.ListChangedType.ItemChanged" /> для элемента в указанной позиции.</summary>
        /// <param name="position">Отсчитываемый от нуля индекс элемента, который требуется сбросить.</param>
        public void ResetItem(int position)
        {
            FireListChanged(ListChangedType.ItemChanged, position,_items[position]);
        }

        protected void FireListChanged(ListChangedType type, int index)
        {
            if (!_raiseListChangedEvents || _items == null) return;
            OnListChanged(new entListChangedEventArgs(type, index));
        }

        protected void FireListChanged(ListChangedType type, int index,entity objEntity)
        {
            if (!_raiseListChangedEvents || _items == null) return;
            OnListChanged(new entListChangedEventArgs(type, index,objEntity));
        }
        #endregion


        #region Обработка событий изменения элементов


        protected void HookPropertyChanged(T item)
        {
            INotifyPropertyChanged notifyPropertyChanged = (object)item as INotifyPropertyChanged;
            if (notifyPropertyChanged == null) return;
            if (_propertyChangedEventHandler == null) _propertyChangedEventHandler = new PropertyChangedEventHandler(Child_PropertyChanged);
            notifyPropertyChanged.PropertyChanged += _propertyChangedEventHandler;
        }

        protected void UnhookPropertyChanged(T item)
        {
            INotifyPropertyChanged notifyPropertyChanged = (object)item as INotifyPropertyChanged;
            if (notifyPropertyChanged == null || _propertyChangedEventHandler == null ) return;
            if (_propertyChangedEventHandler.GetInvocationList().Length>0)
                notifyPropertyChanged.PropertyChanged -= _propertyChangedEventHandler;
        }

        protected virtual void Child_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (!this.RaiseListChangedEvents) return;
            if (sender != null && e != null)
            {

                T obj;
                try
                {
                    obj = (T)sender;
                }
                catch (InvalidCastException ex)
                {
                    ResetBindings();
                    return;
                }

                int newIndex = _lastChangeIndex;
                if (newIndex < 0 || newIndex >= Count || _items[newIndex].Id !=obj.Id)
                {
                    newIndex = FindIndexById(obj.Id);
                    _lastChangeIndex = newIndex;
                }

                if (newIndex == -1)
                {
                    UnhookPropertyChanged(obj);
                    ResetBindings();
                    return;
                }

                if (_items[newIndex].Changed != Status.not_changed)
                {
                    _modified = true;
                }
                if (!string.IsNullOrEmpty(e.PropertyName))
                {
                    //switch (e.PropertyName)
                    //{
                    //    case "": return;
                    //}

                    if (_itemTypeProperties == null) _itemTypeProperties = TypeDescriptor.GetProperties(typeof(T));
                    PropertyDescriptor propDesc = _itemTypeProperties.Find(e.PropertyName, true);
                    OnListChanged(new entListChangedEventArgs(ListChangedType.ItemChanged, newIndex, propDesc){ChangedEntity = obj});
                    return;
                }
                else
                {
                    FireListChanged(ListChangedType.ItemChanged, newIndex,obj); return;
                }
            }
            ResetBindings();
        }
        #endregion

        #endregion

        #region Indexators/Enumerators
        public virtual T this[int index]
        {
            get => ItemByIndex(index);
            set
            {
              if (value!=null ) Insert(index, value);
            } 
        }

        entity  IEntityBindinglist.this[int index]
        {
            get => ItemByIndex(index);
            set
            {
                if (value == null) return;
                if (value is T vt) Insert(index, vt);
            }
        }

        public virtual T this[long Id]
        {
            get => FindEntityById(Id);
 
        }

        entity IEntityBindinglist.this[long Id]
        {
            get => FindEntityById(Id);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEntityBindinglist.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion


        #region Propeties
        
        protected IList<T> Items => _items;

        public bool IsVeiw => _isVeiw;
        public bool Modified => _modified;
        public bool SizeChanged => _m;
        public Type SystemType => typeof(T);

        public string Name { get => _name; set => _name = value; }

        public EntityType Type => _type;

        public List<entity> FullList => (_items.FindAll(x=>x.Changed!=Status.deleted) )?.ToList<entity>();
        public List<T> FullListT => (_items.FindAll(x => x.Changed != Status.deleted))?.ToList<T>();

        public List<long> FullListId
        {
            get
            {
                if (_items == null) return null;
                List<long> es = new List<long>();
                foreach (T item in _items)
                {
                    if (item.Changed!=Status.deleted) es.Add(item.Id);
                }
                return es;
            }
        }

        public int Count => _items?.Count ?? -1;

        public virtual IEntityBindinglist DefaultView => this;

        public virtual bool CheckModified
        {
            get
            {
                _modified = false;
                if (_items == null) return false;
                foreach (var v in _items)
                {
                    if (v != null)
                    {
                        if (v.Changed != Status.not_changed  && v.Changed != Status.newdeleted)
                        {
                            _modified = true;
                            return _modified;
                        }
                    }
                }

                return _modified;
            }
        }

        #endregion

        #region Methods

        #region Find

        public T ItemByIndex(int index)
        {
            if (_items == null || _items.Count == 0 || index < 0 || index > _items.Count) return null;
            return _items[index];
        }

        public T FindEntityById(long Id)
        {
            if (_items == null || _items.Count == 0 || Id < _items[0].Id || Id > _items[_items.Count - 1].Id) return null;
            foreach (var v in _items)
            {
                if (v.Id == Id) return v;
            }
            return null;
        }

        entity IEntityBindinglist.FindEntityById(long Id)
        {
            return this.FindEntityById(Id);
        }


        public int FindIndexById(long Id)
        {
            if (_items == null || _items.Count == 0 || Id < _items[0].Id || Id > _items[_items.Count - 1].Id) return -1;
            //for (int i = 0; i < items.Count; i++)if (items[i].Id == Id) return i;
            int h = _items.Count - 1;
            int j = -1;
            for (int i = 0; i <= h; i++, h--)
            {
                if (_items[i].Id == Id) { j = i; break; }
                if (_items[h].Id == Id) { j = h; break; }
            }
            return j;
        }

        bool IEntityBindinglist.Contains(entity item)
        {
            if (item == null) return false;
            if (item is T a) return (FindIndexById(a.Id) >= 0);
            return false;
        }
        int IEntityBindinglist.IndexOf(entity item)
        {
            if (item == null) return -1;
            if (item is T a) return FindIndexById(a.Id);
            else return -1;
        }

        public long FindDouble(entity newEntity)
        {
            if (newEntity == null) return -1;
            if (_items    == null) return -1;
            T a = newEntity as T;
            if (a      == null) return -1;
            if (a.Type != _type) return -1;
            foreach (var v in _items)
            {
                if (object.ReferenceEquals(v, newEntity) || (a == v || a.Id == v.Id)) return v.Id;
            }
            return -1;
        }

        #endregion

        #region Checks

        public virtual bool CheckItem(entity en)
        {
            if (_items == null || _items.Count == 0 || en == null) return false;
            if (en is T ent) { return CheckItem(ent);}

            return false;
        }

        public virtual bool CheckItem(T en)
        {
            if (_items == null || _items.Count == 0 || en == null) return false;
            int i = FindIndexById(en.Id);
            return i >= 0 && object.ReferenceEquals(_items[i], en);
        }

        public virtual bool CheckItem(long entityId)
        {
            int i = FindIndexById(entityId);
            return i >= 0 ;
        }

        public virtual bool CheckNewId([NotNull] long id)
        {
            if (id                   <= 0) return false;
            return FindIndexById(id) < 0;
        }

        public bool CheckForDouble(bool removeDoubles = true)
        {
            if (_items == null || _items.Count == 0) return true;
            int c = Count;
            for (int j = 0; j < _items.Count; j++)
            {
                for (int i = j + 1; i < _items.Count; i++)
                {
                    if (i != j && _items[i].Id == _items[j].Id)
                    {
                        if (removeDoubles)
                        {
                            //_raiseListChangedEvents
                            if (this is bindview<T> a) a.RemoveLink(i);
                            else RemoveAt(i);
                            i -= 1;
                        }
                        else return false;
                    }
                }
            }
            return c == Count;
        }

        public bool CheckForDouble([NotNull] List<T> list, bool removeDoubles = true)
        {
            if (list == null || list.Count == 0) return true;
            int c = list.Count;
            for (int j = 0; j < list.Count; j++)
            {
                for (int i = j + 1; i < list.Count; i++)
                {
                    if (i != j && list[i].Id == list[j].Id)
                    {
                        if (removeDoubles)
                        {
                            list.RemoveAt(i);
                            i -= 1;
                        }
                        else return false;
                    }
                }
            }
            return c == list.Count;
        }

        public bool CheckForDouble([NotNull] List<long> list, bool removeDoubles = true)
        {
            if (list == null || list.Count == 0) return true;
            int c = list.Count;
            list.Sort();
            for (int j = 0; j < list.Count; j++)
            {
                if (j < list.Count - 1 && list[j] == list[j + 1])
                {
                    if (removeDoubles)
                    {
                        list.RemoveAt(j + 1);
                        j -= 1;
                    }
                    else return false;
                }
            }
            return c == list.Count;
        }
        
        public bool CheckForLosen([NotNull] List<T> list, bool removeLosen = true)
        {
            if (list == null || list.Count == 0) return true;
            if (_items == null || _items.Count == 0)
            {
                if (removeLosen && list.Count>0) list.Clear();
                return false;
            }

            int c = list.Count;
            for (int j = 0; j < list.Count; j++)
            {
                if (!CheckItem(list[j]))
                {
                    if (removeLosen)
                    {
                        list.RemoveAt(j);
                        j -= 1;
                    }
                    else return false;
                }
            }
            return c == list.Count;
        }

        public bool CheckForLosen([NotNull] List<long> list, bool removeLosen = true)
        {
            if (list == null || list.Count == 0) return true;
            if (_items == null || _items.Count == 0)
            {
                if (removeLosen && list.Count > 0) list.Clear();
                return false;
            }
            list.Sort();
            int c = list.Count;
            for (int j = 0; j < list.Count; j++)
            {
                if (FindIndexById(list[j]) < 0)
                {
                    if (removeLosen)
                    {
                        list.RemoveAt(j);
                        j -= 1;
                    }
                    else return false;
                }
            }
            return c == list.Count;
        }

        public virtual bool CheckList([NotNull] List<T> list, bool removeDoubles = true, bool removeLosen = true)
        {
            return CheckForDouble(list, removeDoubles) & CheckForLosen(list, removeLosen);
        }

        public bool CheckList([NotNull] List<long> list, bool removeDoubles = true, bool removeLosen = true)
        {
            return CheckForDouble(list, removeDoubles) & CheckForLosen(list, removeLosen);
        }
 

        #endregion

        #region Resetting


        /// <summary>
        /// Сброс в состояние "изменений нет" для всего списка
        /// </summary>
        public virtual void Reset()
        {
            foreach (T e in _items) e.Reset();
            _modified = false;
            _addNewPos = -1;
            _m = false;
        }

        public void Fix()
        {
            foreach (T e in _items)  e.Fix();
            _addNewPos = -1;
        }
        

        /// <summary>Удаляет из коллекции все элементы.</summary>
        public virtual void Clear(bool generateEvent)
        {
                if (!_isVeiw) ClearItemsChildren();
                ClearItems();
                if (generateEvent) FireListChanged(ListChangedType.Reset, -1);
        }
        public virtual void Clear()
        {
            Clear(false);
        }
        protected virtual void ClearItems()
        {
            EndNew(_addNewPos);
            //if (_raiseItemChangedEvents && items != null && items.Count > 0)
            //{
            if (_items != null)
            {
                foreach (T obj in _items)
                {
                    UnhookPropertyChanged(obj);
                }

                _items.Clear();
            }
            _m=false;
            _modified        = false;
            _lastChangeIndex = -1;
        }

        protected virtual  void ClearItemsChildren()
        {
            if (_items != null && _items.Count > 0)
            {
                foreach (var v in _items)
                {
                    v?.Clear();
                }
            }
        }


        #endregion

        #region Add

        protected internal virtual int InsertItem(int index, T item)
        {
            
            if (_items == null) _items=new List<T>();
            if (item == null) return -1; 
            
            if (_items.Count == 0) index = 0;
            else if (index >= 0 && index < _items.Count && _items[index].Id == item.Id) //попытка вставить в заданную позицию
            {
                if (object.ReferenceEquals(_items[index],item) || _items[index] == item) return index; //зачем вставлять то же самое значение
            }
            else
            {
                index = _items.Count;
                for (int i = 0; i < _items.Count; i++)
                {
                    if (_items[i].Id >= item.Id)
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
                if (!_isVeiw) this[index].Clear();
                _items[index] = item;
                if (_raiseItemChangedEvents) HookPropertyChanged(item);
                FireListChanged(ListChangedType.ItemChanged, index,item);
            }
            else
            {
                this.EndNew(_addNewPos);
                _items.Insert(index, item);
                _addNewPos = index;
                if (_raiseItemChangedEvents) HookPropertyChanged(item);
                _m = true;
                FireListChanged(ListChangedType.ItemAdded, index,item);
            }

            if (item.Changed != Status.not_changed) _modified = true;
            return index;
        }

        public void Insert(int index, T item) => InsertItem(index, item);

        int IEntityBindinglist.Insert(int index, entity item)
        {
            if (_items == null || item == null) return -1;
            if (item is T itemT) return InsertItem(index, itemT);
            return -1;
            //throw new Exception("Тип добавляемого элемента не соответсвует типу коллекции");
        }


        public void Add(T item) => InsertItem(_items.Count, item);
        int IEntityBindinglist.Add(entity item)
        {
            if (_items == null || item == null) return -1;
            if (item is T itemT) return InsertItem(_items.Count, itemT);
            return -1;
           
        }

        public virtual void Add([NotNull] IEnumerable<T> newItems, bool withRenew = true)
        {
            if (newItems == null) return;//throw new ArgumentNullException(nameof(newItems),"Нельзя добавить неинициализированную сущность");

            if (withRenew && _items != null && _items.Count > 0)
            {
                if (!_isVeiw) Clear(); else ClearItems();
            }
            bool a = _raiseListChangedEvents;
            _raiseListChangedEvents = false;
            
            foreach (var v in newItems)
            {
                if (v!=null) InsertItem(-1, v);
            }
            _raiseListChangedEvents = a;
            _m = true;
            FireListChanged(ListChangedType.Reset, -1);
        }

        void IEntityBindinglist.Add([NotNull] IEnumerable<entity> newItems, bool withRenew )
        {
            if (newItems == null) return; //throw new ArgumentNullException(nameof(newItems), "Нельзя добавить неинициализированную сущность");
           
            if (withRenew && _items!=null && _items.Count>0) {if (!_isVeiw) Clear(); else ClearItems();}
            bool a = _raiseListChangedEvents;
            _raiseListChangedEvents = false;

            foreach (var v in newItems)
            {
                if (v == null) continue;
                if (v is T itemT) InsertItem(-1, itemT);
            }
            _raiseListChangedEvents = a;
            _m = true;
            FireListChanged(ListChangedType.Reset, -1);
        }

        #endregion

        #region AddNew

        /// <summary>Добавляет новый элемент в коллекцию.</summary>
        /// <returns>Элемент, который нужно добавить в список.</returns>
        /// <exception cref="T:System.InvalidOperationException">Свойству <see cref="P:System.Windows.Forms.BindingSource.AllowNew" /> задано значение false.-или-Не удалось найти открытый конструктор по умолчанию для текущего типа элемента.</exception>
        public T AddNew() =>  AddNewCore();

        entity IEntityBindinglist.AddNew() => AddNewCore();
        /// <summary>Добавляет новый элемент в конец коллекции.</summary>
        /// <returns>Элемент, который был добавлен в коллекцию.</returns>
        /// <exception cref="T:System.InvalidCastException">Новый элемент не совпадает с типом объектов, содержащихся в <see cref="T:System.ComponentModel.BindingList`1" />.</exception>
        protected virtual T AddNewCore()
        {
            T obj = FireAddingNew() ?? new T();
            while (FindIndexById(obj.Id)>=0)
            {
                obj.Id=conv.GenerateId(_type);
            }

            int i= InsertItem(-1, obj);
            if (i >= 0)
            {
                _modified = true;
                _m = true;
                return obj;
            }
            else return null;
        }


        /// <summary>Удаляет незафиксированный новый элемент.</summary>
        /// <param name="itemIndex">Индекс добавляемого нового элемента </param>
        public virtual void CancelNew(int itemIndex)
        {
            if (_items == null || _items.Count == 0 || itemIndex < 0 || itemIndex>=_items.Count) return;
            if ((_addNewPos >= 0 && _addNewPos == itemIndex) ||_items[itemIndex].Changed==Status.newcreated )
            RemoveItem(itemIndex);
            //if (_addNewPos >= 0 && _addNewPos == itemIndex)  _addNewPos = -1;
            
        }

        /// <summary>Фиксирует незафиксированный новый элемент в коллекции.</summary>
        /// <param name="itemIndex">Индекс добавляемого нового элемента.</param>
        public virtual void EndNew(int itemIndex)
        {
            if (_items == null || _items.Count == 0 || itemIndex < 0 || itemIndex >= _items.Count) return;
            if (_addNewPos >= 0 && _addNewPos == itemIndex)  _addNewPos = -1;
            //if (_items[itemIndex].Changed == Status.newcreated || _items[itemIndex].Changed == Status.newdeleted) _items[itemIndex].Fix();
        }

        #endregion

        #region Remove

        /// <summary>Удаляет элемент с указанным индексом.</summary>
        /// <param name="index">Отсчитываемый от нуля индекс удаляемого элемента.</param>
        // <exception cref="T:System.ArgumentOutOfRangeException">Значение параметра <paramref name="index" /> меньше нуля.-или-Значение параметра <paramref name="index" /> больше или равно значению свойства <see cref="P:System.Collections.ObjectModel.Collection`1.Count" />.</exception>
        // <exception cref="T:System.NotSupportedException">Вы удаляете вновь добавленный элемент и <see cref="P:System.ComponentModel.IBindingList.AllowRemove" /> имеет значение false.</exception>
        protected virtual void RemoveItem(int index)
        {
            if (_items == null || _items.Count == 0 || index < 0 || index >= _items.Count) return;//throw new ArgumentOutOfRangeException(); //return;
            T a = _items[index];
            if (!_allowRemove && (a.Changed!=Status.newcreated && a.Changed != Status.newdeleted))return;// throw new NotSupportedException();
            EndNew(index);
            
            if (_raiseItemChangedEvents) UnhookPropertyChanged(a);
            
            a.PrepareForDelete();
            _items.RemoveAt(index);
            if (a.Changed != Status.newcreated) // если элнмент не вновь добавленный
            {
                _modified = true;
                _m        = true;
                a.Changed = Status.deleted;
            }
            else a.Changed = Status.newdeleted;

            FireListChanged(ListChangedType.ItemDeleted, index, a);
        }

        /// <summary>Удаляет элемент списка <see cref="T:System.Collections.ObjectModel.Collection`1" /> с указанным индексом.</summary>
        /// <param name="index">Индекс (с нуля) элемента, который требуется удалить.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">Значение параметра <paramref name="index" /> меньше нуля.-или-Значение параметра <paramref name="index" /> больше или равно значению свойства <see cref="P:System.Collections.ObjectModel.Collection`1.Count" />.</exception>
        public void RemoveAt(int index)
        {
            //if (_items == null || index < 0 || index >= _items.Count) throw new ArgumentOutOfRangeException();
            RemoveItem(index);
        }


        /// <summary>Удаляет первое вхождение указанного объекта из коллекции <see cref="T:System.Collections.ObjectModel.Collection`1" />.</summary>
        /// <param name="item">Объект, который необходимо удалить из коллекции <see cref="T:System.Collections.ObjectModel.Collection`1" />. Для ссылочных типов допускается значение null.</param>
        /// <returns>Значение true, если элемент <paramref name="item" /> успешно удален, в противном случае — значение false.  Этот метод также возвращает false, если объект <paramref name="item" /> не был найден в исходной коллекции <see cref="T:System.Collections.ObjectModel.Collection`1" />.</returns>
        public bool Remove(T item)
        {
            if (item == null) return false;
            int index = FindIndexById(item.Id);
            if (index < 0) return false;
            RemoveItem(index);
            return true;
        }

        bool IEntityBindinglist.Remove(entity item)
        {
            if (item == null) return false;
            if (item is T itemT) return Remove(itemT);
            return false;
            //throw new Exception("Тип удаляемого элемента не соответсвует типу коллекции");
        }


        public bool RemoveById(long id)
        {
            if (_items == null || _items.Count == 0 || id < _items[0].Id || id > _items[_items.Count - 1].Id) return false;
            //throw new ArgumentOutOfRangeException();
            int index = FindIndexById(id);
            if (index < 0) return false; //throw new ArgumentOutOfRangeException(); //return false;
            RemoveItem(index);
            return true;
        }

        public void Remove(IEnumerable<T> itemsToDelete)
        {
            if (itemsToDelete == null) return;
                //throw new Exception("Нельзя удалить неинициализированную сущность");
            //_raiseListChangedEvents = false;
            //if (_isVeiw) this._raiseItemChangedEvents = false;

            foreach (T item in itemsToDelete) RemoveById(item.Id);
            //_raiseListChangedEvents = true;
            //if (_isVeiw && typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(T))) this._raiseItemChangedEvents = true;
            FireListChanged(ListChangedType.Reset, -1);
        }
        void IEntityBindinglist.Remove(IEnumerable<entity> itemsToDelete)
        {
            if (itemsToDelete == null) return; //throw new Exception("Нельзя удалить неинициализированную сущность");
            //_raiseListChangedEvents = false;
            //if (_isVeiw) this._raiseItemChangedEvents = false;

            foreach (entity item in itemsToDelete) RemoveById(item.Id);
            //_raiseListChangedEvents = true;
            //if (_isVeiw && typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(T))) this._raiseItemChangedEvents = true;
            FireListChanged(ListChangedType.Reset, -1);
        }

        public void Remove(IEnumerable<long> idItemsToDelete)
        {
            if (idItemsToDelete == null) return; //throw new Exception("Нельзя удалить неинициализированную сущность");
            //bool a = _raiseListChangedEvents;
            //_raiseListChangedEvents = false;
            //if (_isVeiw) this._raiseItemChangedEvents = false;

            foreach (long i in idItemsToDelete) RemoveById(i);
            //_raiseListChangedEvents = a;
            //if (_isVeiw && typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(T))) this._raiseItemChangedEvents = true;
            FireListChanged(ListChangedType.Reset, -1);
        }






        #endregion

        #region Views Methods

 
        public List<T> CreateList([NotNull] Predicate<T> condition)
        {
            if (condition == null || _items == null || _items.Count == 0) return null;
            List<T> t = new List<T>();
            foreach (var item in _items)
            {
                if (condition(item)) t.Add(item);
            }
            return t;
        }
        public List<T> CreateList([NotNull] List<long> idList)
        {
            if (idList == null|| idList.Count==0|| _items ==null ||_items.Count == 0) return null;
           List<long> ll=new List<long>(idList);
            List<T> t=new List<T>();
            foreach (var item in _items)
            {
                foreach (var id in ll)
                {
                    if (id == item.Id) { t.Add(item);
                        ll.Remove(id); break;}
                }
                if (ll.Count==0) break;
            }
            return t;
        }

        #endregion

 
        #endregion




    }
}
