using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Helper
{
    public static class DoorData
    {
        public static int DoorId { get; set; }
        public static List<int> StatusDoors { get; set; } = new List<int>() { 0, -1, -1, -1, -1, -1, -1, -1, -1 };
    }
}
