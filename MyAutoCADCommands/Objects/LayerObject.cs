using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    class LayerObject: BaseObject
    {
        private Boolean _isFrozen = false;
        public Boolean isFrozen
        {
            get
            {
                return _isFrozen;
            }
            set
            {
                _isFrozen = value;
            }
        }
    }

    class LayerObjectcollection : System.Collections.ObjectModel.ObservableCollection<LayerObject>
    {
        public static string GetCurrentLayerName()
        {
            string retLayerName = "";
            Database db = HostApplicationServices.WorkingDatabase;
            Transaction trans = db.TransactionManager.StartTransaction();

            LayerTableRecord lyrTblRec = trans.GetObject(db.Clayer, OpenMode.ForRead) as LayerTableRecord;
            retLayerName = lyrTblRec.Name;

            trans.Commit();
            return retLayerName;
        }
    }
}
