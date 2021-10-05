using System;
using System.Collections.Generic;
using APISkillTest.Model;
using APISkillTest.Service;

namespace APISkillTest.Logic
{
    public class clsLogic
    {
        private readonly IServices _apiService;

        public clsLogic(IServices service)
        {
            _apiService = service;
        }

        public APIMessages<List<InquiryEmployeeRs>> InquiryEmployee(APIMessages<InquiryEmployeeRq> inModel)
        {
            APIMessages<List<InquiryEmployeeRs>> msgResponse = new APIMessages<List<InquiryEmployeeRs>>();

            try
            {
                msgResponse = this._apiService.InquiryEmp(inModel); ;
                msgResponse.IsSuccess = true;
            }
            catch (Exception ex)
            {
                msgResponse.ErrorDescription = ex.Message;
                msgResponse.IsSuccess = false;
            }

            return msgResponse;
        }

        public APIMessage InsertEmp(APIMessages<CRUDEmployee> inModel)
        {
            APIMessage msgResponse = new APIMessage();
            try
            {
                if (string.IsNullOrEmpty(inModel.Data.Department))
                    throw new Exception("Department cannot be null or empty");

                if (string.IsNullOrEmpty(inModel.Data.Jabatan))
                    throw new Exception("Jabatan cannot be null or empty");

                msgResponse = this._apiService.InsertEmpToDB(inModel);

            }
            catch (Exception ex)
            {
                msgResponse.ErrorDescription = ex.Message;
                msgResponse.IsSuccess = false;
            }

            return msgResponse;
        }

        public APIMessage UpdateEmp(APIMessages<CRUDEmployee> inModel)
        {
            APIMessage msgResponse = new APIMessage();

            try
            {
                if (inModel.Data.Employee <= 0)
                    throw new Exception("Employee cannot be 0 / empty");

                if (string.IsNullOrEmpty(inModel.Data.Department))
                    throw new Exception("Department cannot be null or empty");

                if (string.IsNullOrEmpty(inModel.Data.Jabatan))
                    throw new Exception("Jabatan cannot be null or empty");

                msgResponse = this._apiService.UpdateEmpToDB(inModel);

            }
            catch (Exception ex)
            {
                msgResponse.ErrorDescription = ex.Message;
                msgResponse.IsSuccess = false;
            }

            return msgResponse;
        }

        public APIMessage DeleteEmp(APIMessages<CRUDEmployee> inModel)
        {
            APIMessage msgResponse = new APIMessage();

            try
            {
                if (inModel.Data.Employee <= 0)
                    throw new Exception("Employee cannot be 0 / empty");

                msgResponse = this._apiService.DeleteEmployee(inModel);

            }
            catch (Exception ex)
            {
                msgResponse.ErrorDescription = ex.Message;
                msgResponse.IsSuccess = false;
            }

            return msgResponse;
        }
    }
}
