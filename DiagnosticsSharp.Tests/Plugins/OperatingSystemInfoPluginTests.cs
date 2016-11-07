using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using DiagnosticsSharp.Core;
using DiagnosticsSharp.Core.Interfaces;
using DiagnosticsSharp.Core.Models;
using DiagnosticsSharp.Core.Services;
using DiagnosticsSharp.Plugins;
using Moq;
using Xunit;

namespace DiagnosticsSharp.Tests.Plugins
{
    public class OperatingSystemInfoDiagnosticsPluginTests
    {
        public MockConsole Console { get; set; }
        public Mock<IMachineInfoService> MachineInfo { get; set; }
        public Mock<IScreenInfo> TestScreen1 { get; set; }
        public Mock<IScreenInfo> TestScreen2 { get; set; }
        public Mock<IScreenInfo> TestScreen3 { get; set; }
        public AppSettingsSection Config { get; set; }
        public CommandLineOptions Options { get; set; }

        public OperatingSystemInfoDiagnosticsPluginTests()
        {
            Options = CommandLineOptions.ParseCommandLine(new string[] { });
            Config = new AppSettingsSection
            {

                Settings =
                {
                    new KeyValueConfigurationElement("ExecutablePath", @".\Gateway\rcp\Gateway_CDP_DEV.cmd"),
                    new KeyValueConfigurationElement("OCAServerApi", "http://jboss-vm-61a46.dynamic.dcts.cloud.td.com:8080/td-gtw-phone-channel-2.0.0/v1"),
                    new KeyValueConfigurationElement("ConfigFileName", "testvalue3"),
                    new KeyValueConfigurationElement("SharedAppSettingsPath", @"P:\UnitySettings\Unity.appconfig"),
                    new KeyValueConfigurationElement("MenuItemJsonPath", "./menuitems.json"),
                    new KeyValueConfigurationElement("MenuItemUrlConfigJsonPath", "./MenuItemUrlConfig.json"),
                    new KeyValueConfigurationElement("ClientSettingsProvider.ServiceUri", ""),
                    new KeyValueConfigurationElement("ChromiumDisableGPU", "1"),
                    new KeyValueConfigurationElement("ConfigFileName", "UnitySettings.config"),
                }
            };

            Console = new MockConsole();

            TestScreen1 = new Mock<IScreenInfo>();
            TestScreen2 = new Mock<IScreenInfo>();
            TestScreen3 = new Mock<IScreenInfo>();

            //todo: refactor screen implementation 
            TestScreen1.Setup(q => q.DeviceName).Returns(@"\\.\DISPLAY1");
            TestScreen1.Setup(q => q.Bounds).Returns(new Rectangle(new Point(0, 0), new Size(1280, 720)));
            TestScreen1.Setup(q => q.Primary).Returns(true);

            TestScreen2.Setup(q => q.DeviceName).Returns(@"\\.\DISPLAY2");
            TestScreen2.Setup(q => q.Bounds).Returns(new Rectangle(new Point(0, 0), new Size(1920, 1080)));
            TestScreen2.Setup(q => q.Primary).Returns(false);

            TestScreen3.Setup(q => q.DeviceName).Returns(@"\\.\DISPLAY3");
            TestScreen3.Setup(q => q.Bounds).Returns(new Rectangle(new Point(0, 0), new Size(4096, 2160)));
            TestScreen3.Setup(q => q.Primary).Returns(false);


            MachineInfo = new Mock<IMachineInfoService>();
            MachineInfo.Setup(q => q.MachineName).Returns("TestETAG");
            MachineInfo.Setup(q => q.OperatingSystemArchitecture).Returns("Test Architecture");
            MachineInfo.Setup(q => q.OperatingSystemName).Returns("Test OS");
            MachineInfo.Setup(q => q.OperatingSystemServicePack).Returns("Test Service Pack");
            MachineInfo.Setup(q => q.Processor).Returns("Test Processor");
            MachineInfo.Setup(q => q.TotalRam).Returns(16000);
            MachineInfo.Setup(q => q.UserDomain).Returns("TESTDOMAIN");
            MachineInfo.Setup(q => q.Username).Returns("TESTUSER");
            var videoCard1 = new Mock<IDisplayAdapter>();
            videoCard1.Setup(q=>q.Name).Returns("Test Video Card A");
            videoCard1.Setup(q => q.FullName).Returns("Test Video Card A Full Name");
            videoCard1.Setup(q => q.DriverVersion).Returns("1.0");
            videoCard1.Setup(q => q.Frequency).Returns("60hz");

            var videoCard2 = new Mock<IDisplayAdapter>();
            videoCard2.Setup(q => q.Name).Returns("Test Video Card B");
            videoCard2.Setup(q => q.FullName).Returns("Test Video Card B Full Name");
            videoCard2.Setup(q => q.DriverVersion).Returns("2.0");
            videoCard2.Setup(q => q.Frequency).Returns("59hz");

            MachineInfo.Setup(q => q.DisplayAdapters)
                .Returns(new List<IDisplayAdapter>() {videoCard1.Object, videoCard2.Object});

            MachineInfo.Setup(q => q.Screens).Returns(new List<IScreenInfo>
            {
                TestScreen1.Object,
                TestScreen2.Object,
                TestScreen3.Object
            });

            MachineInfo.Setup(q => q.LoadConfiguration(It.IsAny<FileInfo>())).Returns(Config);

        }

        [Fact]
        public void PluginHasNameAndSectionTitle()
        {
            IDiagnosticRenderer machine = new OperatingSystemInfoPlugin(Console, MachineInfo.Object);
            Assert.False(string.IsNullOrEmpty(machine.Name));
            Assert.False(string.IsNullOrEmpty(machine.SectionTitle));
        }

        [Fact]
        public void OperatingSystemInfoDisplaysAsExpected()
        {

            MachineInfo.Setup(q => q.GetVersionInfo(It.IsAny<FileInfo>()))
                .Returns(new FileVersionInfo() { Version = "2.1.1" });

            IDiagnosticRenderer machine = new OperatingSystemInfoPlugin(Console, MachineInfo.Object);

            machine.Render();

            var o = Console.ConsoleOutput;

            Assert.Contains("OS Version: Test OS", o);
            Assert.Contains("OS Architecture: Test Architecture", o);
            Assert.Contains("OS Service Pack: Test Service Pack", o);

        }
    }
}
