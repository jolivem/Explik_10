using Microsoft.Pex.Framework.Generated;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roadkill.Core.Database;
using Roadkill.Core.Mvc.ViewModels;
// <copyright file="CompetitionViewModelTest.PublicationStartStringSetTest.g.cs" company="C.Small">Copyright © C.Small 2013</copyright>
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
[PexRaisedException(typeof(ArgumentNullException))]
public void PublicationStartStringSetTestThrowsArgumentNullException858()
{
    CompetitionViewModel competitionViewModel;
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
    this.PublicationStartStringSetTest(competitionViewModel, (string)null);
}

[TestMethod]
[PexGeneratedBy(typeof(CompetitionViewModelTest))]
[PexRaisedException(typeof(ArgumentNullException))]
public void PublicationStartStringSetTestThrowsArgumentNullException619()
{
    CompetitionViewModel competitionViewModel;
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
    this.PublicationStartStringSetTest(competitionViewModel, (string)null);
}

[TestMethod]
[PexGeneratedBy(typeof(CompetitionViewModelTest))]
[PexRaisedException(typeof(ArgumentNullException))]
public void PublicationStartStringSetTestThrowsArgumentNullException691()
{
    CompetitionViewModel competitionViewModel;
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
    this.PublicationStartStringSetTest(competitionViewModel, (string)null);
}

[TestMethod]
[PexGeneratedBy(typeof(CompetitionViewModelTest))]
[PexRaisedException(typeof(ArgumentNullException))]
public void PublicationStartStringSetTestThrowsArgumentNullException841()
{
    CompetitionViewModel competitionViewModel;
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
    this.PublicationStartStringSetTest(competitionViewModel, (string)null);
}
    }
}
