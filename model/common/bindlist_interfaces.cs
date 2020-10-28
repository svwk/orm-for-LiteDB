using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace entitymodel
{
    public partial class bindlist<T>
    {
        #region BindingList Methods


        #region флаги allow

        /// <summary>Возвращает или задает значение, указывающее, является ли элементы добавляются в список с помощью <see cref="M:System.ComponentModel.BindingList`1.AddNew" /> метод.</summary>
        /// <returns>true При добавлении элементов в список с <see cref="M:System.ComponentModel.BindingList`1.AddNew" /> метод; в противном случае — false. Значение по умолчанию зависит от базового типа, содержащегося в списке.</returns>
        public bool AllowNew
        {
            get
            {
                if (_userSetAllowNew || _allowNew) return _allowNew;
                return AddingNewHandled;
            }
            set
            {
                _userSetAllowNew = true;
                if (_allowNew == value) return;
                _allowNew = value;
                FireListChanged(ListChangedType.Reset, -1);
            }
        }

        /// <summary>Возвращает или задает значение, указывающее, можно ли редактировать элементы в списке.</summary>
        /// <returns>true Если элементы списка можно редактировать; в противном случае — false. Значение по умолчанию — true.</returns>
        public bool AllowEdit
        {
            get => _allowEdit;
            set
            {
                if (_allowEdit == value) return;
                _allowEdit = value;
                FireListChanged(ListChangedType.Reset, -1);
            }
        }


        /// <summary>Возвращает или задает значение, указывающее, можно ли удалять элементы из коллекции.</summary>
        /// <returns>true При удалении элементов из списка с <see cref="M:System.ComponentModel.BindingList`1.RemoveItem(System.Int32)" /> в противном случае — метод false. Значение по умолчанию — true.</returns>
        public bool AllowRemove
        {
            get => _allowRemove;
            set
            {
                if (_allowRemove == value) return;
                _allowRemove = value;
                FireListChanged(ListChangedType.Reset, -1);
            }
        }

        #endregion


        /// <summary>Возвращает значение, указывающее, является ли <see cref="E:System.ComponentModel.BindingList`1.ListChanged" /> включены события.</summary>
        /// <returns>true Если <see cref="E:System.ComponentModel.BindingList`1.ListChanged" /> событий поддерживается; в противном случае — false. Значение по умолчанию — true.</returns>
        protected virtual bool SupportsChangeNotificationCore => true;

        /// <summary>Возвращает значение, указывающее, поддерживает ли список поиска.</summary>
        /// <returns>true Если список поддерживает поиск; в противном случае — false. Значение по умолчанию — false.</returns>
        protected virtual bool SupportsSearchingCore => false;

        /// <summary>Возвращает значение, указывающее, поддерживает ли список сортировку.</summary>
        /// <returns>true Если список поддерживает сортировку; в противном случае — false. Значение по умолчанию — false.</returns>
        protected virtual bool SupportsSortingCore => false;

        /// <summary>Возвращает значение, указывающее, сортируется ли список.</summary>
        /// <returns>true Если список сортируется; в противном случае — false. Значение по умолчанию — false.</returns>
        protected virtual bool IsSortedCore => false;

        /// <summary>Возвращает дескриптор свойства, используемый для сортировки списка, если сортировка реализуется в производном классе. в противном случае — возвращает null.</summary>
        /// <returns>
        /// <see cref="T:System.ComponentModel.PropertyDescriptor" /> Используется для сортировки списка.</returns>
        protected virtual PropertyDescriptor SortPropertyCore => (PropertyDescriptor)null;

        /// <summary>Возвращает направление сортировки списка.</summary>
        /// <returns>Одно из значений <see cref="T:System.ComponentModel.ListSortDirection" />. Значение по умолчанию — <see cref="F:System.ComponentModel.ListSortDirection.Ascending" />.</returns>
        protected virtual ListSortDirection SortDirectionCore => ListSortDirection.Ascending;

        /// <summary>Сортирует элементы при переопределении в производном классе. в противном случае — вызывает <see cref="T:System.NotSupportedException" />.</summary>
        /// <param name="prop">Объект <see cref="T:System.ComponentModel.PropertyDescriptor" /> задающий свойства для сортировки.</param>
        /// <param name="direction">Один из <see cref="T:System.ComponentModel.ListSortDirection" />  значения.</param>
        /// <exception cref="T:System.NotSupportedException">Метод не переопределен в производном классе.</exception>
        protected virtual void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction) => throw new NotSupportedException();

        /// <summary>Удаляет любую сортировку, выполненную с <see cref="M:System.ComponentModel.BindingList`1.ApplySortCore(System.ComponentModel.PropertyDescriptor,System.ComponentModel.ListSortDirection)" /> Если сортировка реализуется в производном классе; в противном случае — вызывает <see cref="T:System.NotSupportedException" />.</summary>
        /// <exception cref="T:System.NotSupportedException">Метод не переопределен в производном классе.</exception>
        protected virtual void RemoveSortCore() => throw new NotSupportedException();

        /// <summary>Выполняет поиск индекса элемента, который имеет заданного дескриптора свойства с указанным значением, если поиск реализуется в производном классе. в противном случае — <see cref="T:System.NotSupportedException" />.</summary>
        /// <param name="prop">Объект <see cref="T:System.ComponentModel.PropertyDescriptor" />, который требуется найти.</param>
        /// <param name="key">Значение <paramref name="property" /> для сопоставления.</param>
        /// <returns>Отсчитываемый от нуля индекс элемента, соответствующего дескриптору свойств и содержит указанное значение.</returns>
        /// <exception cref="T:System.NotSupportedException">
        /// <see cref="M:System.ComponentModel.BindingList`1.FindCore(System.ComponentModel.PropertyDescriptor,System.Object)" /> не переопределен в производном классе.</exception>
        protected virtual int FindCore(PropertyDescriptor prop, object key) => throw new NotSupportedException();
        #endregion

        #region Collections Methods

        /// <summary>Копирует целый массив <see cref="T:System.Collections.ObjectModel.Collection`1" /> в совместимый одномерный массив <see cref="T:System.Array" />, начиная с заданного индекса целевого массива.</summary>
        /// <param name="array">Одномерный массив <see cref="T:System.Array" />, в который копируются элементы из интерфейса <see cref="T:System.Collections.ObjectModel.Collection`1" />. Массив <see cref="T:System.Array" /> должен иметь индексацию, начинающуюся с нуля.</param>
        /// <param name="index">Отсчитываемый от нуля индекс в массиве <paramref name="array" />, указывающий начало копирования.</param>
        /// <exception cref="T:System.ArgumentNullException">Свойство <paramref name="array" /> имеет значение null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">Значение параметра <paramref name="index" /> меньше нуля.</exception>
        /// <exception cref="T:System.ArgumentException">Количество элементов в исходной коллекции <see cref="T:System.Collections.ObjectModel.Collection`1" /> больше, чем свободное пространство от <paramref name="index" /> до конца массива назначения <paramref name="array" />.</exception>
        public void CopyTo(T[] array, int index) => _items.CopyTo(array, index);

        /// <summary>Определяет, входит ли элемент в коллекцию <see cref="T:System.Collections.ObjectModel.Collection`1" />.</summary>
        /// <param name="item">Объект для поиска в <see cref="T:System.Collections.ObjectModel.Collection`1" />. Для ссылочных типов допускается значение null.</param>
        /// <returns>Значение true, если параметр <paramref name="item" /> найден в коллекции <see cref="T:System.Collections.ObjectModel.Collection`1" />; в противном случае — значение false.</returns>
        public bool Contains(T item) => (IndexOf(item) >= 0);

        /// <summary>Осуществляет поиск указанного объекта и возвращает отсчитываемый от нуля индекс первого вхождения, найденного в пределах всего списка <see cref="T:System.Collections.ObjectModel.Collection`1" />.</summary>
        /// <param name="item">Объект для поиска в <see cref="T:System.Collections.Generic.List`1" />. Для ссылочных типов допускается значение null.</param>
        /// <returns>Индекс (с нуля) первого вхождения параметра <paramref name="item" />, если оно найдено в коллекции <see cref="T:System.Collections.ObjectModel.Collection`1" />; в противном случае -1.</returns>
        public int IndexOf(T item)
        {
            if (item == null) return -1;
            return FindIndexById(item.Id);
        }

        private bool IsReadOnly => ((IList<entity>)_items).IsReadOnly;

        private static bool IsCompatibleObject(object value)
        {
            if (value is T) return true;
            if (value == null) return (object)default(T) == null;
            return false;
        }

        #endregion

        #region IBindingList

        object IBindingList.AddNew() => AddNewCore();
        bool IBindingList.AllowNew => AllowNew;
        bool IBindingList.AllowEdit => AllowEdit;
        bool IBindingList.SupportsSearching => SupportsSearchingCore;
        bool IBindingList.SupportsSorting => SupportsSortingCore;
        bool IBindingList.IsSorted => IsSortedCore;
        PropertyDescriptor IBindingList.SortProperty => SortPropertyCore;
        ListSortDirection IBindingList.SortDirection => SortDirectionCore;
        void IBindingList.ApplySort(PropertyDescriptor prop, ListSortDirection direction) => ApplySortCore(prop, direction);
        void IBindingList.RemoveSort() => RemoveSortCore();
        bool IBindingList.AllowRemove => AllowRemove;
        bool IBindingList.SupportsChangeNotification => SupportsChangeNotificationCore;
        int IBindingList.Find(PropertyDescriptor prop, object key) => FindCore(prop, key);

        void IBindingList.AddIndex(PropertyDescriptor prop) { }

        void IBindingList.RemoveIndex(PropertyDescriptor prop) { }


        #endregion

        #region ICollection

        bool ICollection<T>.IsReadOnly => ((IList<entity>)_items).IsReadOnly;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    ICollection items = this._items as ICollection;
                    if (items != null) _syncRoot = items.SyncRoot;
                    else
                        Interlocked.CompareExchange<object>(ref _syncRoot, new object(), (object)null);
                }
                return _syncRoot;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null) throw new ArgumentNullException();
            if (array.Rank != 1) throw new ArgumentException();
            if (array.GetLowerBound(0) != 0) throw new ArgumentException();
            if (index < 0) throw new ArgumentOutOfRangeException();
            if (array.Length - index < Count) throw new ArgumentException();
            T[] array1 = array as T[];
            if (array1 != null)
            {
                _items.CopyTo(array1, index);
            }
            else
            {
                Type elementType = array.GetType().GetElementType();
                Type c = typeof(T);
                if (elementType == null || (!elementType.IsAssignableFrom(c) && !c.IsAssignableFrom(elementType))) throw new ArgumentException();
                object[] objArray = array as object[];
                if (objArray == null) throw new ArgumentException();
                int count = _items.Count;
                try
                {
                    for (int index1 = 0; index1 < count; ++index1)
                        objArray[index++] = (object)_items[index1];
                }
                catch (ArrayTypeMismatchException ex)
                {
                    throw new ArgumentException();
                }
            }
        }

        #endregion

        #region IList

        object IList.this[int index]
        {
            get => (object)ItemByIndex(index);
            set => Insert(index, (T)value);
        }
        bool IList.IsReadOnly => IsReadOnly;

        bool IList.IsFixedSize
        {
            get
            {
                IList items = this._items as IList;
                if (items != null) return items.IsFixedSize;
                return IsReadOnly;
            }
        }

        int IList.Add(object value)
        {
            try
            {
                Add((T)value);
            }
            catch (InvalidCastException ex)
            {
                throw new Exception();
            }
            return _items.Count - 1;
        }

        bool IList.Contains(object value)
        {
            if (IsCompatibleObject(value)) return Contains((T)value);
            return false;
        }

        int IList.IndexOf(object value)
        {
            if (IsCompatibleObject(value)) return IndexOf((T)value);
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            try
            {
                Insert(index, (T)value);
            }
            catch (InvalidCastException ex)
            {
                throw new Exception();
            }
        }

        void IList.Remove(object value)
        {
            if (!IsCompatibleObject(value)) return;
            Remove((T)value);
        }

        #endregion

    }
}