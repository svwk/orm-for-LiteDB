using System;
using System.Collections.Generic;
using entitymodel.Annotations;

namespace entitymodel
{
    public interface IBindview: IEntityBindinglist
    {

        #region Fields


        #endregion

        #region Constructors


        #endregion

        #region Indexators/Enumerators


        #endregion

        #region Comparer



        #endregion

        #region Events


        #endregion

        #region Propeties
        bool IsCascading { get; set; }
        IBindbaselist MainBindinglist { get; set; }

        SourceType Sourcetype { get;}

        #endregion

        #region Methods

        void AddLink(List<long> list, bool renew = true);
        int AddLink(long itemId);
        int AddLink([NotNull] entity item, int index = -1);
        bool CheckForLosen(bool removeLosen = true);
        bool CheckList(bool toRemove = true);
        void ClearView(bool generateEvent);
        void RefreshList();
        void RemoveLink(int index);
        void RemoveLink(long id);
 
        #endregion


    }

    public interface IBindviewList : IBindview
    {
        List<long> SourceList { get; set; }
        
        void GenerateSourceList();

    }
}