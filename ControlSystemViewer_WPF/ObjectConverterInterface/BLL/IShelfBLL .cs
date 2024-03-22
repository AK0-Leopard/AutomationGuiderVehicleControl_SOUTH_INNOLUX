using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface.BLL
{
    public interface IShelfBLL
    {
        //List<ViewerObject.Shelf> LoadAllShelves(List<ViewerObject.Address> addresses);
        bool GetAllShelves(ref List<ViewerObject.Shelf> shelves, out string result);
        bool UpdateShelves(ref List<ViewerObject.Shelf> shelves, out string result);
    }
}
