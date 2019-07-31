using Microsoft.Pex.Framework.Generated;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roadkill.Core.Database;
using Roadkill.Core.Mvc.ViewModels;
// <copyright file="CompetitionViewModelTest.RatingStartStringGetTest.g.cs" company="C.Small">Copyright © C.Small 2013</copyright>
// <auto-generated>
// Ce fichier contient des tests générés automatiquement.
// Ne modifiez pas ce fichier manuellement.
// 
// Si le contenu de ce fichier est obsolète, vous pouvez le supprimer.
// Par exemple, s'il n'est plus compilé.
//  </auto-generated>
using System;

namespace Roadkill.Core.Mvc.ViewModels.Tests
{
    public partial class CompetitionViewModelTest
    {

[TestMethod]
[PexGeneratedBy(typeof(CompetitionViewModelTest))]
public void RatingStartStringGetTest393()
{
    CompetitionViewModel competitionViewModel;
    string s;
    Competition s0 = new Competition();
    s0.Id = 0;
    s0.PublicationStart = default(DateTime);
    s0.PublicationStop = default(DateTime);
    s0.RatingStart = default(DateTime);
    s0.RatingStop = default(DateTime);
    s0.Status = 0;
    s0.PageTag = (string)null;
    s0.PageId = 0;
    s0.ObjectId = default(Guid);
    competitionViewModel = new CompetitionViewModel(s0);
    competitionViewModel.Results = (CompetitionResult[])null;
    s = this.RatingStartStringGetTest(competitionViewModel);
    Assert.IsNotNull((object)competitionViewModel);
    Assert.IsNull((object)(competitionViewModel.Results));
    Assert.AreEqual<int>(0, competitionViewModel.Id);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStart.Day);
    Assert.AreEqual<DayOfWeek>
        (DayOfWeek.Monday, competitionViewModel.PublicationStart.DayOfWeek);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStart.DayOfYear);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStart.Hour);
    Assert.AreEqual<DateTimeKind>
        (DateTimeKind.Unspecified, competitionViewModel.PublicationStart.Kind);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStart.Millisecond);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStart.Minute);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStart.Month);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStart.Second);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStart.Year);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStop.Day);
    Assert.AreEqual<DayOfWeek>
        (DayOfWeek.Monday, competitionViewModel.PublicationStop.DayOfWeek);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStop.DayOfYear);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStop.Hour);
    Assert.AreEqual<DateTimeKind>
        (DateTimeKind.Unspecified, competitionViewModel.PublicationStop.Kind);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStop.Millisecond);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStop.Minute);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStop.Month);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStop.Second);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStop.Year);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStart.Day);
    Assert.AreEqual<DayOfWeek>
        (DayOfWeek.Monday, competitionViewModel.RatingStart.DayOfWeek);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStart.DayOfYear);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStart.Hour);
    Assert.AreEqual<DateTimeKind>
        (DateTimeKind.Unspecified, competitionViewModel.RatingStart.Kind);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStart.Millisecond);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStart.Minute);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStart.Month);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStart.Second);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStart.Year);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStop.Day);
    Assert.AreEqual<DayOfWeek>
        (DayOfWeek.Monday, competitionViewModel.RatingStop.DayOfWeek);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStop.DayOfYear);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStop.Hour);
    Assert.AreEqual<DateTimeKind>
        (DateTimeKind.Unspecified, competitionViewModel.RatingStop.Kind);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStop.Millisecond);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStop.Minute);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStop.Month);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStop.Second);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStop.Year);
    Assert.AreEqual<CompetitionViewModel.Statuses>
        (CompetitionViewModel.Statuses.Init, competitionViewModel.Status);
    Assert.AreEqual<string>((string)null, competitionViewModel.PageTag);
    Assert.AreEqual<int>(0, competitionViewModel.PageId);
    Assert.AreEqual<string>("", competitionViewModel.PageTitle);
    Assert.AreEqual<string>("", competitionViewModel.PageText);
    Assert.AreEqual<string>("En préparation", competitionViewModel.StatusString);
}

[TestMethod]
[PexGeneratedBy(typeof(CompetitionViewModelTest))]
public void RatingStartStringGetTest993()
{
    CompetitionViewModel competitionViewModel;
    string s;
    Competition s0 = new Competition();
    s0.Id = 0;
    s0.PublicationStart = default(DateTime);
    s0.PublicationStop = default(DateTime);
    s0.RatingStart = default(DateTime);
    s0.RatingStop = default(DateTime);
    s0.Status = 1;
    s0.PageTag = (string)null;
    s0.PageId = 0;
    s0.ObjectId = default(Guid);
    competitionViewModel = new CompetitionViewModel(s0);
    competitionViewModel.Results = (CompetitionResult[])null;
    s = this.RatingStartStringGetTest(competitionViewModel);
    Assert.IsNotNull((object)competitionViewModel);
    Assert.IsNull((object)(competitionViewModel.Results));
    Assert.AreEqual<int>(0, competitionViewModel.Id);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStart.Day);
    Assert.AreEqual<DayOfWeek>
        (DayOfWeek.Monday, competitionViewModel.PublicationStart.DayOfWeek);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStart.DayOfYear);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStart.Hour);
    Assert.AreEqual<DateTimeKind>
        (DateTimeKind.Unspecified, competitionViewModel.PublicationStart.Kind);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStart.Millisecond);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStart.Minute);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStart.Month);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStart.Second);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStart.Year);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStop.Day);
    Assert.AreEqual<DayOfWeek>
        (DayOfWeek.Monday, competitionViewModel.PublicationStop.DayOfWeek);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStop.DayOfYear);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStop.Hour);
    Assert.AreEqual<DateTimeKind>
        (DateTimeKind.Unspecified, competitionViewModel.PublicationStop.Kind);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStop.Millisecond);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStop.Minute);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStop.Month);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStop.Second);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStop.Year);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStart.Day);
    Assert.AreEqual<DayOfWeek>
        (DayOfWeek.Monday, competitionViewModel.RatingStart.DayOfWeek);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStart.DayOfYear);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStart.Hour);
    Assert.AreEqual<DateTimeKind>
        (DateTimeKind.Unspecified, competitionViewModel.RatingStart.Kind);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStart.Millisecond);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStart.Minute);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStart.Month);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStart.Second);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStart.Year);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStop.Day);
    Assert.AreEqual<DayOfWeek>
        (DayOfWeek.Monday, competitionViewModel.RatingStop.DayOfWeek);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStop.DayOfYear);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStop.Hour);
    Assert.AreEqual<DateTimeKind>
        (DateTimeKind.Unspecified, competitionViewModel.RatingStop.Kind);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStop.Millisecond);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStop.Minute);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStop.Month);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStop.Second);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStop.Year);
    Assert.AreEqual<CompetitionViewModel.Statuses>
        (CompetitionViewModel.Statuses.PublicationOngoing, 
         competitionViewModel.Status);
    Assert.AreEqual<string>((string)null, competitionViewModel.PageTag);
    Assert.AreEqual<int>(0, competitionViewModel.PageId);
    Assert.AreEqual<string>("", competitionViewModel.PageTitle);
    Assert.AreEqual<string>("", competitionViewModel.PageText);
    Assert.AreEqual<string>
        ("Publication en cours", competitionViewModel.StatusString);
}

[TestMethod]
[PexGeneratedBy(typeof(CompetitionViewModelTest))]
public void RatingStartStringGetTest934()
{
    CompetitionViewModel competitionViewModel;
    string s;
    Competition s0 = new Competition();
    s0.Id = 0;
    s0.PublicationStart = default(DateTime);
    s0.PublicationStop = default(DateTime);
    s0.RatingStart = default(DateTime);
    s0.RatingStop = default(DateTime);
    s0.Status = 2;
    s0.PageTag = (string)null;
    s0.PageId = 0;
    s0.ObjectId = default(Guid);
    competitionViewModel = new CompetitionViewModel(s0);
    competitionViewModel.Results = (CompetitionResult[])null;
    s = this.RatingStartStringGetTest(competitionViewModel);
    Assert.IsNotNull((object)competitionViewModel);
    Assert.IsNull((object)(competitionViewModel.Results));
    Assert.AreEqual<int>(0, competitionViewModel.Id);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStart.Day);
    Assert.AreEqual<DayOfWeek>
        (DayOfWeek.Monday, competitionViewModel.PublicationStart.DayOfWeek);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStart.DayOfYear);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStart.Hour);
    Assert.AreEqual<DateTimeKind>
        (DateTimeKind.Unspecified, competitionViewModel.PublicationStart.Kind);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStart.Millisecond);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStart.Minute);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStart.Month);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStart.Second);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStart.Year);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStop.Day);
    Assert.AreEqual<DayOfWeek>
        (DayOfWeek.Monday, competitionViewModel.PublicationStop.DayOfWeek);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStop.DayOfYear);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStop.Hour);
    Assert.AreEqual<DateTimeKind>
        (DateTimeKind.Unspecified, competitionViewModel.PublicationStop.Kind);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStop.Millisecond);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStop.Minute);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStop.Month);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStop.Second);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStop.Year);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStart.Day);
    Assert.AreEqual<DayOfWeek>
        (DayOfWeek.Monday, competitionViewModel.RatingStart.DayOfWeek);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStart.DayOfYear);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStart.Hour);
    Assert.AreEqual<DateTimeKind>
        (DateTimeKind.Unspecified, competitionViewModel.RatingStart.Kind);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStart.Millisecond);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStart.Minute);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStart.Month);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStart.Second);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStart.Year);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStop.Day);
    Assert.AreEqual<DayOfWeek>
        (DayOfWeek.Monday, competitionViewModel.RatingStop.DayOfWeek);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStop.DayOfYear);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStop.Hour);
    Assert.AreEqual<DateTimeKind>
        (DateTimeKind.Unspecified, competitionViewModel.RatingStop.Kind);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStop.Millisecond);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStop.Minute);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStop.Month);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStop.Second);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStop.Year);
    Assert.AreEqual<CompetitionViewModel.Statuses>
        (CompetitionViewModel.Statuses.RatingOngoing, competitionViewModel.Status);
    Assert.AreEqual<string>((string)null, competitionViewModel.PageTag);
    Assert.AreEqual<int>(0, competitionViewModel.PageId);
    Assert.AreEqual<string>("", competitionViewModel.PageTitle);
    Assert.AreEqual<string>("", competitionViewModel.PageText);
    Assert.AreEqual<string>("Notation en cours", competitionViewModel.StatusString);
}

[TestMethod]
[PexGeneratedBy(typeof(CompetitionViewModelTest))]
public void RatingStartStringGetTest318()
{
    CompetitionViewModel competitionViewModel;
    string s;
    Competition s0 = new Competition();
    s0.Id = 0;
    s0.PublicationStart = default(DateTime);
    s0.PublicationStop = default(DateTime);
    s0.RatingStart = default(DateTime);
    s0.RatingStop = default(DateTime);
    s0.Status = 3;
    s0.PageTag = (string)null;
    s0.PageId = 0;
    s0.ObjectId = default(Guid);
    competitionViewModel = new CompetitionViewModel(s0);
    competitionViewModel.Results = (CompetitionResult[])null;
    s = this.RatingStartStringGetTest(competitionViewModel);
    Assert.IsNotNull((object)competitionViewModel);
    Assert.IsNull((object)(competitionViewModel.Results));
    Assert.AreEqual<int>(0, competitionViewModel.Id);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStart.Day);
    Assert.AreEqual<DayOfWeek>
        (DayOfWeek.Monday, competitionViewModel.PublicationStart.DayOfWeek);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStart.DayOfYear);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStart.Hour);
    Assert.AreEqual<DateTimeKind>
        (DateTimeKind.Unspecified, competitionViewModel.PublicationStart.Kind);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStart.Millisecond);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStart.Minute);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStart.Month);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStart.Second);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStart.Year);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStop.Day);
    Assert.AreEqual<DayOfWeek>
        (DayOfWeek.Monday, competitionViewModel.PublicationStop.DayOfWeek);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStop.DayOfYear);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStop.Hour);
    Assert.AreEqual<DateTimeKind>
        (DateTimeKind.Unspecified, competitionViewModel.PublicationStop.Kind);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStop.Millisecond);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStop.Minute);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStop.Month);
    Assert.AreEqual<int>(0, competitionViewModel.PublicationStop.Second);
    Assert.AreEqual<int>(1, competitionViewModel.PublicationStop.Year);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStart.Day);
    Assert.AreEqual<DayOfWeek>
        (DayOfWeek.Monday, competitionViewModel.RatingStart.DayOfWeek);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStart.DayOfYear);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStart.Hour);
    Assert.AreEqual<DateTimeKind>
        (DateTimeKind.Unspecified, competitionViewModel.RatingStart.Kind);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStart.Millisecond);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStart.Minute);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStart.Month);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStart.Second);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStart.Year);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStop.Day);
    Assert.AreEqual<DayOfWeek>
        (DayOfWeek.Monday, competitionViewModel.RatingStop.DayOfWeek);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStop.DayOfYear);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStop.Hour);
    Assert.AreEqual<DateTimeKind>
        (DateTimeKind.Unspecified, competitionViewModel.RatingStop.Kind);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStop.Millisecond);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStop.Minute);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStop.Month);
    Assert.AreEqual<int>(0, competitionViewModel.RatingStop.Second);
    Assert.AreEqual<int>(1, competitionViewModel.RatingStop.Year);
    Assert.AreEqual<CompetitionViewModel.Statuses>
        (CompetitionViewModel.Statuses.Achieved, competitionViewModel.Status);
    Assert.AreEqual<string>((string)null, competitionViewModel.PageTag);
    Assert.AreEqual<int>(0, competitionViewModel.PageId);
    Assert.AreEqual<string>("", competitionViewModel.PageTitle);
    Assert.AreEqual<string>("", competitionViewModel.PageText);
    Assert.AreEqual<string>("Terminé", competitionViewModel.StatusString);
}
    }
}
