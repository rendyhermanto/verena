using System;
namespace APISkillTest.Model
{
        public class APIMessage
        {
            public bool IsSuccess { get; set; }
            public int ErrorCode { get; set; }
            public string ErrorDescription { get; set; }
        }

        public class APIMessages<DataType>
        {
            public bool IsSuccess { get; set; }
            public int ErrorCode { get; set; }
            public string ErrorDescription { get; set; }
            public DataType Data { get; set; }
        }
}
