using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.Data.Services.Common;
using System.Data.Services;

namespace Grades.Web.Services
{
    // Excersice 1: Task 2a: Replace the object keyword with your data source class name.
    public class GradesWebDataService : DataService<SchoolGradesDBEntities>
    {
        public static void InitializeService(DataServiceConfiguration config)
        {
            // Excersice 1: Task 2b: set rules to indicate which entity sets and service operations are visible, updatable, etc
            config.SetServiceOperationAccessRule("StudentsInClass", ServiceOperationRights.AllRead);
            config.SetEntitySetAccessRule("Grades", EntitySetRights.All);
            config.SetEntitySetAccessRule("Teachers", EntitySetRights.All);
            config.SetEntitySetAccessRule("Students", EntitySetRights.All);
            config.SetEntitySetAccessRule("Subjects", EntitySetRights.All);
            config.SetEntitySetAccessRule("Users", EntitySetRights.All);
        }
        [WebGet]
        public IEnumerable<Student> StudentsInClass(string className)
        {
            var students = from Student s in this.CurrentDataSource.Students where String.Equals(s.Teacher.Class, className) select s;
            return students;
        }
    }


}
