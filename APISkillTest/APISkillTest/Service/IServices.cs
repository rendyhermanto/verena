using System;
using System.Collections.Generic;
using APISkillTest.Model;

namespace APISkillTest.Service
{
    public interface IServices
    {
        APIMessages<List<InquiryEmployeeRs>> InquiryEmp(APIMessages<InquiryEmployeeRq> inModel);
        APIMessage InsertEmpToDB(APIMessages<CRUDEmployee> inModel);
        APIMessage UpdateEmpToDB(APIMessages<CRUDEmployee> inModel);
        APIMessage DeleteEmployee(APIMessages<CRUDEmployee> inModel);
    }
}
