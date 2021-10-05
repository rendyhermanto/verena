using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APISkillTest.Logic;
using APISkillTest.Model;
using APISkillTest.Service;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APISkillTest.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly IServices _apiService;
        private readonly clsLogic _clsLogic;

        public EmployeeController(IServices service)
        {
            _apiService = service;
            _clsLogic = new clsLogic(service);
        }


        [HttpGet]
        [HttpHead]
        public string Get()
        {
            string Message = "Back end Employee berhasil di jalakan";

            return Message;
        }

        [HttpPost("[action]")]
        public ActionResult<APIMessages<List<InquiryEmployeeRs>>> InquiryEmployee ([FromBody] APIMessages<InquiryEmployeeRq> inModel)
        {
            APIMessages<List<InquiryEmployeeRs>> msgRsp = new APIMessages<List<InquiryEmployeeRs>>();
            msgRsp = _clsLogic.InquiryEmployee(inModel);

            return msgRsp;
        }

        [HttpPost("[action]")]
        public ActionResult<APIMessage> InsertEmployee([FromBody] APIMessages<CRUDEmployee> inModel)
        {
            APIMessage msgRsp = new APIMessage();
            msgRsp = _clsLogic.InsertEmp(inModel);

            return msgRsp;
        }

        [HttpPost("[action]")]
        public ActionResult<APIMessage> UpdateEmployee([FromBody] APIMessages<CRUDEmployee> inModel)
        {
            APIMessage msgRsp = new APIMessage();
            msgRsp = _clsLogic.UpdateEmp(inModel);

            return msgRsp;
        }

        [HttpPost("[action]")]
        public ActionResult<APIMessage> DeleteEmployee([FromBody] APIMessages<CRUDEmployee> inModel)
        { 
            APIMessage msgRsp = new APIMessage();
            msgRsp = _clsLogic.DeleteEmp(inModel);

            return msgRsp;
        }
    }
}
