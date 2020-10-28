using System;
using System.Security.Cryptography.X509Certificates;

namespace entitymodel
{
    public class managerpCommonData : managerp
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
        public bindbaselist<zak> Zaks
        {
            get
            {
                if (this[EntityType.zak] != null) return (this[EntityType.zak] as bindbaselist<zak>);
                return null;
            }
            set
            {
                if (value.Type != EntityType.zak) { throw new Exception("Неверный тип коллекции. Присваивание не выполнено"); }
                else AddCollection(value);
            }
        }

        public bindbaselist<dog> Dogs
        {
            get
            {
                if (this[EntityType.dog] != null) return (this[EntityType.dog] as bindbaselist<dog>);
                return null;
            }
            set
            {
                if (value.Type != EntityType.dog) { throw new Exception("Неверный тип коллекции. Присваивание не выполнено"); }
                else AddCollection(value);
            }
        }

        /*public entity_bindinglist<dogc> Dogcs
        {
            get { return _dogcs; }
            set
            {
                if (value.Type != EntityType.dogc) { throw new Exception("Неверный тип коллекции. Присваивание не выполнено"); }
                else AddReplaceCollection(value);
            }
        }*/

        public bindbaselist<pls> Plss
        {
            get
            {
                if (this[EntityType.pls] != null) return (this[EntityType.pls] as bindbaselist<pls>);
                return null;
            }
            set
            {
                if (value.Type != EntityType.pls) { throw new Exception("Неверный тип коллекции. Присваивание не выполнено"); }
                else AddCollection(value);
            }
        }

        public bindbaselist<sor> Sors
        {
            get
            {
                if (this[EntityType.sor] != null) return (this[EntityType.sor] as bindbaselist<sor>);
                return null;
            }
            set
            {
                if (value.Type != EntityType.sor) { throw new Exception("Неверный тип коллекции. Присваивание не выполнено"); }
                else AddCollection(value);
            }
        }

        public bindbaselist<prj> Prjs
        {
            get
            {
                if (this[EntityType.prj] != null) return (this[EntityType.prj] as bindbaselist<prj>);
                return null;
            }
            set
            {
                if (value.Type != EntityType.prj) { throw new Exception("Неверный тип коллекции. Присваивание не выполнено"); }
                else AddCollection(value);
            }
        }

        public bindbaselist<smt> Smts
        {
            get
            {
                if (this[EntityType.smt] != null) return (this[EntityType.smt] as bindbaselist<smt>);
                return null;
            }
            set
            {
                if (value.Type != EntityType.smt) { throw new Exception("Неверный тип коллекции. Присваивание не выполнено"); }
                else AddCollection(value);
            }
        }
        #endregion

        #region Methods



        public override void FillLinks(entity item)
        {
            if (item == null || d == null) return;
            EntityType eType = item.Type;
            switch (eType)
            {
                case EntityType.zak:
                    if (item is zak vz)
                    {
                        if (Dogs != null)
                        {
                            if (vz.CDogs!=null && vz.CDogs.MainBindinglist!=null && !object.ReferenceEquals(vz.CDogs.MainBindinglist, Dogs))
                            {vz.CDogs.MainBindinglist.RemoveView(vz.Id.ToString()); vz.CDogs = null;}

                            if (vz.CDogs == null)
                            {
                                if (Dogs.Count>0) vz.CDogs =(bindviewcond<dog>)Dogs.Views[Dogs.AddView(vz.Id.ToString(), x => x.ZakId == vz.Id)];
                            }
                            else vz.CDogs.RefreshList();
                            if (vz.CDogs != null && vz.CDogs.Count>0){foreach (var p in vz.CDogs) p.Zak = vz;}
                        }
                        else vz.CDogs = null;
                    }

                    break;
                case EntityType.dog:
                    if (item is dog vd)
                    {
                        if (Plss != null)
                        {
                            if (vd.CPlss != null && vd.CPlss.MainBindinglist != null &&
                                !object.ReferenceEquals(vd.CPlss.MainBindinglist, Plss))
                            {
                                vd.CPlss.MainBindinglist.RemoveView(vd.Id.ToString());
                                vd.CPlss = null;
                            }
                            if (vd.CPlss == null) {if (Plss.Count>0) vd.CPlss = (bindviewcond<pls>)Plss.Views[Plss.AddView(vd.Id.ToString(), x => x.DogId == vd.Id)];}
                            else vd.CPlss.RefreshList();
                            if (vd.CPlss!=null && vd.CPlss.Count>0) { foreach (var p in vd.CPlss) p.Dog = vd;}
                        }
                        else
                        {
                            vd.CPlss = null;
                        }

                        if (Zaks != null && vd.ZakId > 0 && (vd.Zak == null || vd.Zak.Id != vd.ZakId))
                        {
                            vd.Zak = Zaks[vd.ZakId];
                            if (vd.Zak!=null) FillLinks(vd.Zak);
                        }
                    }

                    break;
                case EntityType.pls:
                    if (item is pls vl)
                    {
                        if (Sors != null)
                        {
                            if (vl.CSors != null && vl.CSors.MainBindinglist != null &&
                                !object.ReferenceEquals(vl.CSors.MainBindinglist, Sors))
                            {
                                vl.CSors.MainBindinglist.RemoveView(vl.Id.ToString());
                                vl.CSors = null;
                            }
                            if (vl.CSors == null) { if (Sors.Count > 0) vl.CSors = (bindviewcond<sor>)Sors.Views[Sors.AddView(vl.Id.ToString(), x => x.PlsId == vl.Id)];}
                            else vl.CSors.RefreshList();
                            if (vl.CSors != null && vl.CSors.Count>0) { foreach (var p in vl.CSors) p.Pls = vl; }
                        }
                        else
                        {
                            vl.CSors = null;
                        }

                        if (Dogs != null && vl.DogId > 0 && (vl.Dog == null || vl.Dog.Id != vl.DogId))
                        { vl.Dog = Dogs[vl.DogId]; if (vl.Dog != null) FillLinks(vl.Dog); }
                    }

                    break;
                case EntityType.sor:
                    if (item is sor vs)
                    {
                        if (Prjs != null)
                        {
                            if (vs.CPrjs != null && vs.CPrjs.MainBindinglist != null &&
                                !object.ReferenceEquals(vs.CPrjs.MainBindinglist, Prjs))
                            {
                                vs.CPrjs.MainBindinglist.RemoveView(vs.Id.ToString());
                                vs.CPrjs = null;
                            }
                            if (vs.CPrjs == null) {if (Prjs.Count>0) vs.CPrjs = (bindviewcond<prj>)Prjs.Views[Prjs.AddView(vs.Id.ToString(), x => x.SorId == vs.Id)];}
                            else vs.CPrjs.RefreshList();
                            if (vs.CPrjs != null && vs.CPrjs.Count>0) { foreach (var p in vs.CPrjs) p.Sor = vs; }
                        }
                        else
                        {
                            vs.CPrjs = null;
                        }

                        if (Plss != null && vs.PlsId > 0 && (vs.Pls == null || vs.Pls.Id != vs.PlsId))
                        { vs.Pls = Plss[vs.PlsId]; if (vs.Pls != null) FillLinks(vs.Pls); }
                    }

                    break;
                case EntityType.prj:
                    if (item is prj vj)
                    {
                        if (Smts != null)
                        {
                            if (vj.CSmts != null && vj.CSmts.MainBindinglist != null &&
                                !object.ReferenceEquals(vj.CSmts.MainBindinglist, Smts))
                            {
                                vj.CSmts.MainBindinglist.RemoveView(vj.Id.ToString());
                                vj.CSmts = null;
                            }
                            if (vj.CSmts == null) { if (Smts.Count > 0) vj.CSmts = (bindviewcond<smt>)Smts.Views[Smts.AddView(vj.Id.ToString(), x => x.PrjId == vj.Id)]; }
                            else vj.CSmts.RefreshList();
                            if (vj.CSmts != null && vj.CSmts.Count > 0) { foreach (var p in vj.CSmts) p.Project = vj; }
                        }
                        else
                        {
                            vj.CSmts = null;
                        }

                        if (Sors != null && vj.SorId > 0 && (vj.Sor == null || vj.Sor.Id != vj.SorId))
                        { vj.Sor = Sors[vj.SorId]; if (vj.Sor != null) FillLinks(vj.Sor); }
                    }

                    break;

                case EntityType.smt:
                    if (item is smt vm)
                    {
                        //if (Plss != null)
                        //{
                        //    if (vd.CPlss != null && vd.CPlss.MainBindinglist != null &&
                        //        !object.ReferenceEquals(vd.CPlss.MainBindinglist, Plss))
                        //    {
                        //        vd.CPlss.MainBindinglist.RemoveView(vd.Id.ToString());
                        //        vd.CPlss = null;
                        //    }
                        //    if (vd.CPlss == null) { if (Plss.Count > 0) vd.CPlss = (bindviewcond<pls>)Plss.Views[Plss.AddView(vd.Id.ToString(), x => x.DogId == vd.Id)]; }
                        //    else vd.CPlss.RefreshList();
                        //    if (vd.CPlss != null && vd.CPlss.Count > 0) { foreach (var p in vd.CPlss) p.Dog = vd; }
                        //}
                        //else
                        //{
                        //    vd.CPlss = null;
                        //}

                        if (Prjs != null && vm.PrjId > 0 && (vm.Project == null || vm.Project.Id != vm.PrjId))
                        {
                            vm.Project = Prjs[vm.PrjId];
                            if (vm.Project != null) FillLinks(vm.Project);
                        }
                    }

                    break;


            }
        }

        #endregion



    }
}