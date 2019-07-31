// <copyright file="PexAssemblyInfo.cs" company="C.Small">Copyright © C.Small 2013</copyright>
using Microsoft.Pex.Framework.Coverage;
using Microsoft.Pex.Framework.Creatable;
using Microsoft.Pex.Framework.Instrumentation;
using Microsoft.Pex.Framework.Settings;
using Microsoft.Pex.Framework.Validation;

// Microsoft.Pex.Framework.Settings
[assembly: PexAssemblySettings(TestFramework = "VisualStudioUnitTest")]

// Microsoft.Pex.Framework.Instrumentation
[assembly: PexAssemblyUnderTest("Roadkill.Core")]
[assembly: PexInstrumentAssembly("System.Web.Optimization")]
[assembly: PexInstrumentAssembly("Recaptcha")]
[assembly: PexInstrumentAssembly("System.ComponentModel.DataAnnotations")]
[assembly: PexInstrumentAssembly("System.DirectoryServices")]
[assembly: PexInstrumentAssembly("NLog")]
[assembly: PexInstrumentAssembly("LogentriesNLog")]
[assembly: PexInstrumentAssembly("System.DirectoryServices.AccountManagement")]
[assembly: PexInstrumentAssembly("System.Web.Http.WebHost")]
[assembly: PexInstrumentAssembly("Microsoft.CSharp")]
[assembly: PexInstrumentAssembly("Mindscape.LightSpeed.Linq")]
[assembly: PexInstrumentAssembly("Microsoft.Web.Administration")]
[assembly: PexInstrumentAssembly("Ionic.Zip")]
[assembly: PexInstrumentAssembly("Mindscape.LightSpeed")]
[assembly: PexInstrumentAssembly("System.Data.SqlServerCe")]
[assembly: PexInstrumentAssembly("HtmlAgilityPack")]
[assembly: PexInstrumentAssembly("System.Runtime.Caching")]
[assembly: PexInstrumentAssembly("StructureMap")]
[assembly: PexInstrumentAssembly("System.Web.Mvc")]
[assembly: PexInstrumentAssembly("System.Configuration")]
[assembly: PexInstrumentAssembly("System.Web")]
[assembly: PexInstrumentAssembly("System.Data")]
[assembly: PexInstrumentAssembly("System.Net.Http.Formatting")]
[assembly: PexInstrumentAssembly("System.Xml.Linq")]
[assembly: PexInstrumentAssembly("System.Core")]
[assembly: PexInstrumentAssembly("System.Net.Http")]
[assembly: PexInstrumentAssembly("Newtonsoft.Json")]
[assembly: PexInstrumentAssembly("Lucene.Net")]
[assembly: PexInstrumentAssembly("System.Web.Http")]

// Microsoft.Pex.Framework.Creatable
[assembly: PexCreatableFactoryForDelegates]

// Microsoft.Pex.Framework.Validation
[assembly: PexAllowedContractRequiresFailureAtTypeUnderTestSurface]
[assembly: PexAllowedXmlDocumentedException]

// Microsoft.Pex.Framework.Coverage
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Web.Optimization")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Recaptcha")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.ComponentModel.DataAnnotations")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.DirectoryServices")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "NLog")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "LogentriesNLog")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.DirectoryServices.AccountManagement")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Web.Http.WebHost")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.CSharp")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Mindscape.LightSpeed.Linq")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.Web.Administration")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Ionic.Zip")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Mindscape.LightSpeed")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Data.SqlServerCe")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "HtmlAgilityPack")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Runtime.Caching")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "StructureMap")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Web.Mvc")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Configuration")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Web")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Data")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Net.Http.Formatting")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Xml.Linq")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Core")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Net.Http")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Newtonsoft.Json")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Lucene.Net")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Web.Http")]

