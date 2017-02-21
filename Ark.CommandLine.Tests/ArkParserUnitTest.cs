namespace Ark.CommandLine.UnitTests
{
    using Ark.CommandLine.Attribute;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ark.CommandLine.Exceptions;

    [TestClass]
    public class ArkParserUnitTest
    {

        [TestInitialize]
        public void TestInitialize()
        {
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ArkParser_CannotCreateInstance_PropertyNameDuplications()
        {
            var instance = new ArkParser<ClassWithPropertyDuplications>();
            var result = instance.Parse(new string[] { }, "-");


            Assert.AreEqual(expected: false, actual: result.IsSucceeded);
            Assert.IsTrue(result.Exception is PropertyNameDuplicationException);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ArkParser_ValidClassWithArguments()
        {
            var number = 10;
            var name = "myName";
            var instance = new ArkParser<ValidArgumentClass>();
            var result = instance.Parse(new[] { "-num", number.ToString(), "-name", name , "- fg" , "flag1" }, "-");


            Assert.AreEqual(expected: true, actual: result.IsSucceeded);
            Assert.AreEqual(expected: number, actual: result.TargetClass.Number);
            Assert.AreEqual(expected: name, actual: result.TargetClass.Name);

        }

    }

    internal class ValidArgumentClass
    {
        public enum FlagType
        {
            Flag1, Flag2
        }

        [ArkCmdDesc(fullName: "number", shortName: "num", isRequire: true)]
        public int Number { get; set; }

        [ArkCmdDesc(fullName: "name", shortName: "nm", isRequire: true)]
        public string Name { get; set; }


        [ArkCmdDesc(fullName: "flag", shortName: "fg", isRequire: true)]
        public FlagType Flag { get; set; }



    }

    internal class ClassWithPropertyDuplications
    {
        public ClassWithPropertyDuplications()
        {
            int x = 0;
        }

        [ArkCmdDesc(fullName: "number", shortName: "num", isRequire: true)]
        public int Number { get; set; }

        [ArkCmdDesc(fullName: "number", shortName: "num", isRequire: true)]
        public string Name { get; set; }

    }

    public enum ForOlegType
    {
        Flag1, Flag2
    }

    internal class ForOleg
    {

        [ArkCmdDesc(fullName: "number", shortName: "num", isRequire: true)]
        public int Number { get; set; }


        [ArkCmdDesc(fullName: "oleg", shortName: "ol", isRequire: true)]
        public ForOlegType Name { get; set; }


        [ArkCmdArgumentsDesc("--save")]
        [ArkCmdArgumentsDesc("--save", "topath")]
        [ArkCmdDesc(fullName: "copyTo", shortName: "cpt", isRequire: true)]
        public string CopyItem { get; set; }


        [ArkCmdDesc(fullName: "ItemPath", shortName: "ip", isRequire: true)]
        public string ItemPath { get; set; }


        //  -ip  "c:\suorce\txtme.txt"  -cpt "c:\stam\txtme.txt
    }
}
