using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;

namespace Objects
{
    class BaseObject
    {
        private string _bName = "";
        public string Name 
        {
            get
            {
                return _bName;
            }

            set
            {
                _bName = value;

            } 
        }

        private ObjectId _objId = ObjectId.Null;
        public ObjectId baseId 
        { 
            get
            {
                return _objId;
            } 
            set
            {
                _objId = value;
            }
        }

        private Boolean _isSelected = false;
        public Boolean isSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
            }
        }
    }
}
