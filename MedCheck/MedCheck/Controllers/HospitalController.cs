using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Controllers
{
    [Authorize(Roles = "admin")]
    public class HospitalController
    {
    }
}
