// <copyright file="CompetitionViewModelTest.cs" company="C.Small">Copyright © C.Small 2013</copyright>
using System;
using System.Collections.Generic;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roadkill.Core.Database;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Mvc.ViewModels.Tests
{
    /// <summary>Cette classe contient des tests unitaires paramétrables pour CompetitionViewModel</summary>
    [PexClass(typeof(CompetitionViewModel))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class CompetitionViewModelTest
    {
        /// <summary>Stub de test pour .ctor()</summary>
        [PexMethod]
        public CompetitionViewModel ConstructorTest()
        {
            CompetitionViewModel target = new CompetitionViewModel();
            return target;
            // TODO: ajouter des assertions à méthode CompetitionViewModelTest.ConstructorTest()
        }

        /// <summary>Stub de test pour .ctor(Competition)</summary>
        [PexMethod]
        public CompetitionViewModel ConstructorTest01(Competition competition)
        {
            CompetitionViewModel target = new CompetitionViewModel(competition);
            return target;
            // TODO: ajouter des assertions à méthode CompetitionViewModelTest.ConstructorTest01(Competition)
        }

        /// <summary>Stub de test pour get_DateString()</summary>
        [PexMethod]
        public string DateStringGetTest([PexAssumeUnderTest]CompetitionViewModel target)
        {
            string result = target.DateString;
            return result;
            // TODO: ajouter des assertions à méthode CompetitionViewModelTest.DateStringGetTest(CompetitionViewModel)
        }

        /// <summary>Stub de test pour get_EncodedTitle()</summary>
        [PexMethod]
        public string EncodedTitleGetTest([PexAssumeUnderTest]CompetitionViewModel target)
        {
            string result = target.EncodedTitle;
            return result;
            // TODO: ajouter des assertions à méthode CompetitionViewModelTest.EncodedTitleGetTest(CompetitionViewModel)
        }

        /// <summary>Stub de test pour get_PublicationStartString()</summary>
        [PexMethod]
        public string PublicationStartStringGetTest([PexAssumeUnderTest]CompetitionViewModel target)
        {
            string result = target.PublicationStartString;
            return result;
            // TODO: ajouter des assertions à méthode CompetitionViewModelTest.PublicationStartStringGetTest(CompetitionViewModel)
        }

        /// <summary>Stub de test pour set_PublicationStartString(String)</summary>
        [PexMethod]
        public void PublicationStartStringSetTest([PexAssumeUnderTest]CompetitionViewModel target, string value)
        {
            target.PublicationStartString = value;
            // TODO: ajouter des assertions à méthode CompetitionViewModelTest.PublicationStartStringSetTest(CompetitionViewModel, String)
        }

        /// <summary>Stub de test pour get_PublicationStopString()</summary>
        [PexMethod]
        public string PublicationStopStringGetTest([PexAssumeUnderTest]CompetitionViewModel target)
        {
            string result = target.PublicationStopString;
            return result;
            // TODO: ajouter des assertions à méthode CompetitionViewModelTest.PublicationStopStringGetTest(CompetitionViewModel)
        }

        /// <summary>Stub de test pour set_PublicationStopString(String)</summary>
        [PexMethod]
        public void PublicationStopStringSetTest([PexAssumeUnderTest]CompetitionViewModel target, string value)
        {
            target.PublicationStopString = value;
            // TODO: ajouter des assertions à méthode CompetitionViewModelTest.PublicationStopStringSetTest(CompetitionViewModel, String)
        }

        /// <summary>Stub de test pour get_RatingStartString()</summary>
        [PexMethod]
        public string RatingStartStringGetTest([PexAssumeUnderTest]CompetitionViewModel target)
        {
            string result = target.RatingStartString;
            return result;
            // TODO: ajouter des assertions à méthode CompetitionViewModelTest.RatingStartStringGetTest(CompetitionViewModel)
        }

        /// <summary>Stub de test pour set_RatingStartString(String)</summary>
        [PexMethod]
        public void RatingStartStringSetTest([PexAssumeUnderTest]CompetitionViewModel target, string value)
        {
            target.RatingStartString = value;
            // TODO: ajouter des assertions à méthode CompetitionViewModelTest.RatingStartStringSetTest(CompetitionViewModel, String)
        }

        /// <summary>Stub de test pour get_RatingStopString()</summary>
        [PexMethod]
        public string RatingStopStringGetTest([PexAssumeUnderTest]CompetitionViewModel target)
        {
            string result = target.RatingStopString;
            return result;
            // TODO: ajouter des assertions à méthode CompetitionViewModelTest.RatingStopStringGetTest(CompetitionViewModel)
        }

        /// <summary>Stub de test pour set_RatingStopString(String)</summary>
        [PexMethod]
        public void RatingStopStringSetTest([PexAssumeUnderTest]CompetitionViewModel target, string value)
        {
            target.RatingStopString = value;
            // TODO: ajouter des assertions à méthode CompetitionViewModelTest.RatingStopStringSetTest(CompetitionViewModel, String)
        }

        /// <summary>Stub de test pour StatusStringToEnum(String)</summary>
        [PexMethod]
        public CompetitionViewModel.Statuses StatusStringToEnumTest(string statusString)
        {
            CompetitionViewModel.Statuses result
               = CompetitionViewModel.StatusStringToEnum(statusString);
            return result;
            // TODO: ajouter des assertions à méthode CompetitionViewModelTest.StatusStringToEnumTest(String)
        }

        /// <summary>Stub de test pour StatusToString(Statuses)</summary>
        [PexMethod]
        public string StatusToStringTest(CompetitionViewModel.Statuses status)
        {
            string result = CompetitionViewModel.StatusToString(status);
            return result;
            // TODO: ajouter des assertions à méthode CompetitionViewModelTest.StatusToStringTest(Statuses)
        }

        /// <summary>Stub de test pour get_StatusesList()</summary>
        [PexMethod]
        public IEnumerable<string> StatusesListGetTest([PexAssumeUnderTest]CompetitionViewModel target)
        {
            IEnumerable<string> result = target.StatusesList;
            return result;
            // TODO: ajouter des assertions à méthode CompetitionViewModelTest.StatusesListGetTest(CompetitionViewModel)
        }
    }
}
