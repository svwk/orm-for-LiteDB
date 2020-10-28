using System;
using System.Collections.Generic;
using System.ComponentModel;
using entitymodel.Annotations;

namespace entitymodel
{
    public class bindviewcond<T> : bindview<T> where T : entity, new()
    {
        #region Fields

        private Predicate<T> _predicate;

        #endregion

        #region Constructors

        public bindviewcond()
        {
            _isVeiw     = true;
            _source     = SourceType.Condition;
            _predicate = (x => true);
        }


        public bindviewcond([NotNull] bindbaselist<T> baseList) :base()
        {
            _isVeiw                      =  true;
            _source                      =  SourceType.Condition;
            _mainBindinglist             =  baseList;
            _mainBindinglist.ListChanged += _mainBindinglist_ListChanged;
            _predicate = (x => true);
        }

        public bindviewcond([NotNull] bindbaselist<T> baseList, [NotNull] Predicate<T> condition)
        {
            _isVeiw                      =  true;
            _source                      =  SourceType.Condition;
            _mainBindinglist             =  baseList;
            _mainBindinglist.ListChanged += _mainBindinglist_ListChanged;
            Condition = condition;
            //RefreshList();
        }


        #endregion

        #region Indexators/Enumerators

        public override T this[int index] => ItemByIndex(index);

        public override T this[long Id] => FindEntityById(Id);

        #endregion

        #region Comparer



        #endregion

        #region Events

        protected override void _mainBindinglist_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (!_mainBindinglistChange) return;
            if (e.ListChangedType==ListChangedType.ItemAdded || e.ListChangedType==ListChangedType.ItemDeleted||e.ListChangedType==ListChangedType.Reset || (e.ListChangedType == ListChangedType.ItemChanged && e.PropertyDescriptor.DisplayName.Contains("Id")))
            RefreshList();
        }
        #endregion

        #region Propeties

        public Predicate<T> Condition
        {
            get => _predicate;
            set
            {
                if (value == null) return;
                _predicate = value;
                RefreshList();
            }
        }

        #endregion

        #region Methods

        #region Checks проверки ссылок

        public override bool CheckItem([NotNull] T en)
        {
            return _mainBindinglist.CheckItem(en) && _predicate(en);
        }

        public override bool CheckItem([NotNull] entity en)
        {
            if (en is T ent) { return CheckItem(ent); }

            return false;
        }

        public override bool CheckNewId([NotNull] long id)
        {
            return !_mainBindinglist.CheckNewId(id) && _predicate(_mainBindinglist[id]);
        }

        public bool CheckListForCondition([NotNull] List<T> list, bool removeInvalid=true)
        {
            if (list == null || list.Count == 0) return true;
            int c = list.Count;
            for (int j = 0; j < list.Count; j++)
            {
                if (!_predicate(list[j]))
                {
                    if (removeInvalid)
                    {
                        list.RemoveAt(j);
                        j -= 1; continue;
                    }
                    return false;
                }
            }
            return c != list.Count;
        }

        public bool CheckListForCondition(bool removeInvalid = true)
        {
            if (_items == null || _items.Count == 0) return true;
            int c = Count;
            for (int j = 0; j < _items.Count; j++)
            {
                if (!_predicate(_items[j]))
                {
                    if (removeInvalid)
                    {
                        RemoveLink(j);
                        j -= 1; continue;
                    }
                    return false;
                }
            }

            if (removeInvalid && c != _items.Count)
            {
                _m = true;
                FireListChanged(ListChangedType.Reset, -1);
                return false;
            }
            return true;
        }

        #endregion

        #region Resetting

        public override void ClearView(bool generateEvent)
        {
            _predicate = (x => true);
            base.ClearView(generateEvent);
        }


        public override void RefreshList()
        {
            int c = Count;
            if (_items == null) _items = new List<T>();

            if (_mainBindinglist == null || _mainBindinglist.Count == 0 || _predicate == null)
            {
                if (_items.Count > 0) ClearItems();
                if (c != Count)
                {
                    _m = true;
                    FireListChanged(ListChangedType.Reset, -1);
                }
                return;
            }
            
                List<T> list = _mainBindinglist?.CreateList(_predicate);

            if (list == null || list.Count == 0)
            {
                if (_items.Count > 0) ClearItems();
                if (c != Count)
                {
                    _m = true;
                    FireListChanged(ListChangedType.Reset, -1);
                }
                return;
            }

            bool b;
            c = list.Count;
            for (int j = 0; j < _items.Count; j++)
            {
                if (_items[j].Changed == Status.deleted || _items[j].Changed == Status.newdeleted || !_predicate(_items[j]))
                {
                    RemoveLink(j);j -= 1;
                    continue;
                    
                }

                b = true;
                foreach (var v in list)
                {
                    if (v.Id == _items[j].Id)
                    {
                        b = false;
                        list.Remove(v);
                        break;
                    }
                }

                if (b)
                {
                    RemoveLink(j);
                    j -= 1;
                }

                if (list.Count == 0 && _items.Count==0) return;
            }

            if (list.Count > 0 || Count != c) AddLink(list);
        }

        #endregion

        #region Add

        #region добавление ссылок

        public override void AddLink([NotNull] List<T> list, bool renew = false)
        {
            CheckListForCondition(list);
            if (list?.Count > 0) base.AddLink(list, renew);
        }

        public override void AddLink([NotNull] List<long> list, bool renew = false)
        {
            int c = Count;
            if (renew && _items != null && _items.Count > 0) ClearItems();
            if (_mainBindinglist == null || _mainBindinglist.Count == 0 || list == null || list.Count == 0)
            {
                if (renew && c != Count)
                {
                    _m = true;
                    FireListChanged(ListChangedType.Reset, -1);
                }
                return;
            }
            List<T> list2 = _mainBindinglist?.CreateList(list);
            if (list2?.Count > 0) AddLink(list2);
        }


        public override int AddLink([NotNull] T item, int index = -1)
        {
            if (_predicate == null) _predicate = (x => true);
            if (_mainBindinglist == null || _mainBindinglist.Count == 0 || item == null) return -1;
            if ((item.Changed != Status.newcreated) && !_predicate(item)) return -1; //throw new Exception("Добавляемое значение не соотвествует условию отбора");

            int i = base.AddLink(item, index);
            return i;
        }

        public override int AddLink(long itemId)
        {
            if (_predicate == null) _predicate = (x => true);
            if (_mainBindinglist == null || _mainBindinglist.Count == 0 || itemId < 0) return -1;
            T item = _mainBindinglist[itemId];
            if (item == null) return -1;
            if ((item.Changed != Status.newcreated) && !_predicate(item)) return -1; //throw new Exception("Добавляемое значение не соотвествует условию отбора");
            int i = base.AddLink(itemId);
            return i;
        }

        #endregion

        #region Добавление новых элементов в основную коллекцию и ссылок на них в эту

        protected internal override int InsertItem(int index, [NotNull] T item)
        {
            if (_predicate == null) _predicate = (x => true);
            if (_mainBindinglist == null || item == null) return -1;
            if ((item.Changed != Status.newcreated) && !_predicate(item)) return -1; //throw new Exception("Добавляемое значение не соотвествует условию отбора");
            int i = base.InsertItem(index, item);
            return i;
        }

        #endregion

        #region AddNew


        #endregion

        #endregion


        #region Удаление самих элементов из основной коллекции и ссылок на них
 
        #endregion

        #region Удаление ссылок на элементы
 
        #endregion

        #endregion


    }
}