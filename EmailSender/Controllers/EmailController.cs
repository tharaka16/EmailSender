using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailSender.Models;
using EmailSender.Data;
using Microsoft.AspNetCore.Mvc;
using EmailSender.Services;

namespace EmailSender.Controllers
{
    [ApiController]
    [Route("email")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailSenderService _emailSender;

        public EmailController(IEmailSenderService emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpPost]
        [Route("send")]
        public IActionResult QueueEmail([FromBody]QueuedEmail email)
        {
            if (email == null)
                return BadRequest();

            var result = _emailSender.QueueEmail(email);
            if (result.Result == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }
    }
}

