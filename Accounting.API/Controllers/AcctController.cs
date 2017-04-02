using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accounting.API.Data;
using Accounting.API.Models;
using Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using static Extensions.Extension;
using Dapper;

namespace Accounting.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Acct")]
    public class AcctController : Controller
    {
        public readonly IAcctRepo _repo;
        private readonly ADContext _adContext;
        public AcctController(IAcctRepo repo, ADContext adContext)
        {
            _repo = repo;
            _adContext = adContext;
        }

      
    }
}