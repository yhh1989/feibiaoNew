using System.Collections.Generic;

namespace HealthExaminationSystem.ThirdParty.DataTransmission.ErrorInfo
{
    public class ClassModel
    {
        public string Name { get; set; }

        public object Description { get; set; }

        public string Type { get; set; }

        public List<ClassModel> Models { get; set; }
    }
}