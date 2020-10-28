using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace entitymodel
{
    public static class conv
    {
        
        public static EntityType StringToEntity(string value)
        {
            return (EntityType) (Convert.ToInt32(value));
        }

        public static EntityType IdToEntity(long value)
        {
            return (EntityType)(Convert.ToInt32(value.ToString().Substring(0, 2)));
        }
        public static EntityType SetType(Type newType)
        {
            if (newType      == typeof(zak)) return EntityType.zak;
            else if (newType == typeof(dog)) return EntityType.dog;
            else if (newType == typeof(dogc)) return EntityType.dogc;
            else if (newType == typeof(sor)) return EntityType.sor;
            else if (newType == typeof(prj)) return EntityType.prj;
            else if (newType == typeof(pls)) return EntityType.pls;
            else if (newType == typeof(smt)) return EntityType.smt;
            else if (newType == typeof(wgr)) return EntityType.wgr;
            else if (newType == typeof(rab)) return EntityType.rab;
            else return EntityType.ent;
        }

        public static entity NewEntity(EntityType eType)
        {
            switch (eType)
            {
                case EntityType.dog: return new dog(); 
                case EntityType.dogc:return new dogc(); 
                case EntityType.zak: return new zak();
                case EntityType.pls:return new pls();
                case EntityType.prj:return new prj(); 
                case EntityType.sor:return new sor();
                case EntityType.smt: return new smt();
                case EntityType.wgr: return new wgr();
                case EntityType.rab: return new rab();
                default: return new entity();
            }
        }


        public static IEntityBindinglist CreateNew(EntityType eType)
        {
            switch (eType)
            {
                case EntityType.zak:  return new bindview<zak>();
                case EntityType.dog:  return new bindview<dog>();
                case EntityType.pls:  return new bindview<pls>();
                case EntityType.sor:  return new bindview<sor>();
                case EntityType.prj:  return new bindview<prj>();
                case EntityType.dogc: return new bindview<dogc>();
                case EntityType.smt: return new bindview<smt>();
                case EntityType.wgr: return new bindview<wgr>();
                case EntityType.rab: return new bindview<rab>();
                case EntityType.ent:  return new bindview<entity>();
            }

            return null;
        }


        public static IEntityBindinglist CreateNew(EntityType eType, BindListTypes listType)
        {
            switch (eType)
            {
                case EntityType.zak:
                    switch (listType)
                    {
                        case BindListTypes.bindlist: return new bindlist<zak>(); 
                        case BindListTypes.BindBaseList: return new bindbaselist<zak>(); 
                        case BindListTypes.bindview: return new bindview<zak>();
                        case BindListTypes.bindviewlist: return new bindviewlist<zak>();
                        case BindListTypes.bindviewcond: return new bindviewcond<zak>();
                    }
                    break;
                case EntityType.dog:
                    switch (listType)
                    {
                        case BindListTypes.bindlist: return new bindlist<dog>(); 
                        case BindListTypes.BindBaseList: return new bindbaselist<dog>(); 
                        case BindListTypes.bindview: return new bindview<dog>();
                        case BindListTypes.bindviewlist: return new bindviewlist<dog>();
                        case BindListTypes.bindviewcond: return new bindviewcond<dog>();
                    }
                    break;

                case EntityType.pls:
                    switch (listType)
                    {
                        case BindListTypes.bindlist: return new bindlist<pls>(); 
                        case BindListTypes.BindBaseList: return new bindbaselist<pls>(); 
                        case BindListTypes.bindview: return new bindview<pls>();
                        case BindListTypes.bindviewlist: return new bindviewlist<pls>();
                        case BindListTypes.bindviewcond: return new bindviewcond<pls>();
                    }
                    break;

                case EntityType.sor:
                    switch (listType)
                    {
                        case BindListTypes.bindlist: return new bindlist<sor>(); 
                        case BindListTypes.BindBaseList: return new bindbaselist<sor>(); 
                        case BindListTypes.bindview: return new bindview<sor>();
                        case BindListTypes.bindviewlist: return new bindviewlist<sor>();
                        case BindListTypes.bindviewcond: return new bindviewcond<sor>();
                    }
                    break;

                case EntityType.prj:
                    switch (listType)
                    {
                        case BindListTypes.bindlist: return new bindlist<prj>(); 
                        case BindListTypes.BindBaseList: return new bindbaselist<prj>(); 
                        case BindListTypes.bindview: return new bindview<prj>();
                        case BindListTypes.bindviewlist: return new bindviewlist<prj>();
                        case BindListTypes.bindviewcond: return new bindviewcond<prj>();
                    }
                    break;

                case EntityType.dogc:
                    switch (listType)
                    {
                        case BindListTypes.bindlist: return new bindlist<dogc>(); 
                        case BindListTypes.BindBaseList: return new bindbaselist<dogc>(); 
                        case BindListTypes.bindview: return new bindview<dogc>();
                        case BindListTypes.bindviewlist: return new bindviewlist<dogc>();
                        case BindListTypes.bindviewcond: return new bindviewcond<dogc>();
                    }
                    break;

                case EntityType.smt:
                    switch (listType)
                    {
                        case BindListTypes.bindlist:     return new bindlist<smt>();
                        case BindListTypes.BindBaseList: return new bindbaselist<smt>();
                        case BindListTypes.bindview:     return new bindview<smt>();
                        case BindListTypes.bindviewlist: return new bindviewlist<smt>();
                        case BindListTypes.bindviewcond: return new bindviewcond<smt>();
                    }
                    break;

                case EntityType.wgr:
                    switch (listType)
                    {
                        case BindListTypes.bindlist:     return new bindlist<wgr>();
                        case BindListTypes.BindBaseList: return new bindbaselist<wgr>();
                        case BindListTypes.bindview:     return new bindview<wgr>();
                        case BindListTypes.bindviewlist: return new bindviewlist<wgr>();
                        case BindListTypes.bindviewcond: return new bindviewcond<wgr>();
                    }
                    break;

                case EntityType.rab:
                    switch (listType)
                    {
                        case BindListTypes.bindlist:     return new bindlist<rab>();
                        case BindListTypes.BindBaseList: return new bindbaselist<rab>();
                        case BindListTypes.bindview:     return new bindview<rab>();
                        case BindListTypes.bindviewlist: return new bindviewlist<rab>();
                        case BindListTypes.bindviewcond: return new bindviewcond<rab>();
                    }
                    break;

                case EntityType.ent:
                    switch (listType)
                    {
                        case BindListTypes.bindlist: return new bindlist<entity>(); 
                        case BindListTypes.BindBaseList: return new bindbaselist<entity>(); 
                        case BindListTypes.bindview: return new bindview<entity>();
                        case BindListTypes.bindviewlist: return new bindviewlist<entity>();
                    }
                    break;
            }

            return null;
        }

        public static string ClearString(string value)
        {
            if (value == String.Empty) return String.Empty;
            value = value.Trim();
            if (value.Contains(" "))
            {
                List<string> d = new List<string>(value.Split(' '));
                value = String.Join(" ", d);
            }

            return value;
        }
        private static readonly DateTime ZeroTime = new DateTime(2019, 01, 01);
        private static readonly Random Rnd = new Random(DateTime.Now.Millisecond + DateTime.Now.DayOfYear * 1000);
        public static long GenerateId(EntityType eType)
        {
            long   t   = Math.Abs(DateTime.Now.Ticks - ZeroTime.Ticks) / 10000;
            string str = t.ToString();
            if (str.Length      > 14) str = str.Substring(0, 14);
            else if (str.Length < 14) str = str.PadLeft(14, '0');
            string str1 = ((int)eType).ToString().PadLeft(2, '0');
            int    j;
            #if HAVE_PROCESS
            j = Process.GetCurrentProcess().Id;
            #else
            j = Rnd.Next(1000); // Magic number
            #endif
            string str3 = j.ToString();
            if (str3.Length      > 3) str3 = str3.Substring(0, 3);
            else if (str3.Length < 3) str3 = str3.PadLeft(3, '0');
            if (Int64.TryParse(str1 + str3 + str, out long res)) return res;
            //return 0;
            throw new Exception("Невозможно сформировать Id");
            
        }

        public static EntityType TypeById(long newId)
        {
            if (newId <= 0) return EntityType.ent;
            string str = newId.ToString().PadLeft(19, '0').Substring(0, 2); ;
            if (Int32.TryParse(str, out int i)) return (EntityType)i;
            return EntityType.ent;
        }

        public static EntityType ToEntityType(string str)
        {
            switch (str)
            {
                case "ent":return EntityType.ent;
                case "zak": return EntityType.zak;
                case "dog": return EntityType.dog;
                case "pls": return EntityType.pls;
                case "sor": return EntityType.sor;
                case "prj": return EntityType.prj;
                case "dogc": return EntityType.dogc;
                case "smt": return EntityType.smt;
                case "wgr": return EntityType.wgr;
                case "rab": return EntityType.rab;

            }
            return EntityType.ent;
        }
    }

    public enum BindListTypes { bindlist, BindBaseList, bindview, bindviewlist, bindviewcond };
    public enum EntityType
    {
        ent,
        zak,
        dog,
        pls,
        sor,
        prj,
        dogc,
        smt,
        wgr,
        rab
        
    }

    

    public enum Status
    {
        edited,
        newcreated,
        deleted,
        newdeleted,
        not_changed
    };

    public enum SourceType { Free, List, Condition,Default}

}
