using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountDAO accountDao;

        public AccountController(IAccountDAO accDao)
        {
            this.accountDao = accDao;
        }

        [HttpGet("{id}")]
        public ActionResult<Account> GetAccount(int id)
        {
            Account account = accountDao.GetAccountByUser(id);
            
            if (account != null)
            {
                return Ok(account);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
