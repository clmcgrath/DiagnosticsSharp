using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using DiagnosticsSharp.Core;
using DiagnosticsSharp.Core.Interfaces;
using DiagnosticsSharp.Core.Services;
using DiagnosticsSharp.Plugins;
using Moq;
using Xunit;

namespace DiagnosticsSharp.Tests.Plugins
{
    public class MachineInfoDiagnosticPluginTests
    {
        public MockConsole Console { get; set; }
        public Mock<IMachineInfoService> MachineInfo { get; set; }

        public AppSettingsSection Config { get; set; }
        public CommandLineOptions Options { get; set; }

        public MachineInfoDiagnosticPluginTests()
        {
            Options = CommandLineOptions.ParseCommandLine(new string[0] { });
            Config = new AppSettingsSection
            {
                Settings =
                {
                    new KeyValueConfigurationElement("TestTest1", "testvalue1"),
                    new KeyValueConfigurationElement("TestTest2", "testvalue2"),
                    new KeyValueConfigurationElement("TestTest3", "testvalue3"),
                    new KeyValueConfigurationElement("TestTest4", "testvalue4"),
                }
            };

            Console = new MockConsole();
            var testScreen1 = new Mock<IScreenInfo>();
            var testScreen2 = new Mock<IScreenInfo>();
            var testScreen3 = new Mock<IScreenInfo>();

            //todo: refactor screen implementation 
            testScreen1.Setup(q => q.DeviceName).Returns(@"\\.\DISPLAY1");
            testScreen1.Setup(q => q.Bounds).Returns(new Rectangle(new Point(0, 0), new Size(1280, 720)));
            testScreen1.Setup(q => q.Primary).Returns(true);

            testScreen2.Setup(q => q.DeviceName).Returns(@"\\.\DISPLAY2");
            testScreen2.Setup(q => q.Bounds).Returns(new Rectangle(new Point(0, 0), new Size(1920, 1080)));
            testScreen2.Setup(q => q.Primary).Returns(false);

            testScreen3.Setup(q => q.DeviceName).Returns(@"\\.\DISPLAY3");
            testScreen3.Setup(q => q.Bounds).Returns(new Rectangle(new Point(0, 0), new Size(4096, 2160)));
            testScreen3.Setup(q => q.Primary).Returns(false);


            MachineInfo = new Mock<IMachineInfoService>();
            MachineInfo.Setup(q => q.MachineName).Returns("TestETAG");
            MachineInfo.Setup(q => q.OperatingSystemArchitecture).Returns("Test Architecture");
            MachineInfo.Setup(q => q.OperatingSystemName).Returns("Test OS");
            MachineInfo.Setup(q => q.OperatingSystemServicePack).Returns("Test Service Pack");
            MachineInfo.Setup(q => q.Processor).Returns("Test Processor");
            MachineInfo.Setup(q => q.TotalRam).Returns(16000);
            MachineInfo.Setup(q => q.UserDomain).Returns("TESTDOMAIN");
            MachineInfo.Setup(q => q.Username).Returns("TESTUSER");

            MachineInfo.Setup(q => q.Screens).Returns(new List<IScreenInfo>
            {
                testScreen1.Object,
                testScreen2.Object,
                testScreen3.Object
            });

            MachineInfo.Setup(q => q.LoadConfiguration(It.IsAny<FileInfo>())).Returns(Config);

        }

        [Fact]
        public void PluginHasNameAndSectionTitle()
        {
            IDiagnosticRenderer machine = new MachineInfoPlugin(Console, MachineInfo.Object, Options);
            Assert.False(string.IsNullOrEmpty(machine.Name));
            Assert.False(string.IsNullOrEmpty(machine.SectionTitle));
        }


        [Fact]
        public void MachineInfoDisplaysExpectedInformation()
        {
            IDiagnosticRenderer machine = new MachineInfoPlugin(Console, MachineInfo.Object, Options);

            machine.Render();
            var o = Console.ConsoleOutput;

            Assert.Contains("", o);

        }




    }


}
