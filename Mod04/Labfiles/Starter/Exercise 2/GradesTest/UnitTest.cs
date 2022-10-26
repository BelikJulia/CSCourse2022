
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GradesTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Init()
        {
            GradesPrototype.Data.DataSourse.CreateData();
        }
        [TestMethod]
        public void TestValidGrade()
        {
            GradesPrototype.Data.Grade grade = new GradesPrototype.Data.Grade(1, "01/01/2022", "Math", "A-", "Very good");
            Assert.AreEqual(grade.AssessmentDate, "01/01/2012");
            Assert.AreEqual(grade.SubjectName, "Math");
            Assert.AreEqual(grade.Assessment, "A-");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestBadDate()
        {
            GradesPrototype.Data.Grade grade = new GradesPrototype.Data.Grade(1, "1/1/2023", "Math", "A-", "Very good");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDateNotRecognized()
        {
            GradesPrototype.Data.Grade grade = new GradesPrototype.Data.Grade(1, "13/13/2022", "Math", "A-", "Very good");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestBadAssessment()
        {
            GradesPrototype.Data.Grade grade = new GradesPrototype.Data.Grade(1, "1/1/2022", "Math", "F-", "Terrible");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBadSubject()
        {
            GradesPrototype.Data.Grade grade = new GradesPrototype.Data.Grade(1, "1/1/2022", "French", "B-", "OK");
        }
    }
}
