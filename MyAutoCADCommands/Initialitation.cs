using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;

namespace MyAutoCADCommands
{
    internal class Initialitation: IExtensionApplication
    {
        [CommandMethod("MyFirstCommand")]
        public void cmdMyFirstCommand()
        {

        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Terminate()
        {
            throw new NotImplementedException();
        }
    }
}
