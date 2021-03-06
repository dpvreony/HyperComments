﻿using System;
using System.Linq;

using HyperComments.Recorder;
using HyperComments.Tests.Stubs;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.Text.Tagging;

using Moq;
using Microsoft.VisualStudio.Text;

namespace HyperComments.Tests.Recorder
{
    [TestClass]
    public class AudioRecorderTaggerTest : BDD<AudioRecorderTaggerTest>
    {
        [TestMethod]
        public void Creates_tag_if_audio_file_comment_is_found()
        {
            Given.we_have_one_span_matching_audio_comment();
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Gets_the_name_of_the_active_document_and_pass_it_to_the_recorder()
        {
            Given.user_is_editing_some_file();
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Gets_the_recording_directory_from_solution_file_path()
        {
            Given.user_is_editing_some_file();
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Creates_recording_directory_if_not_existing()
        {
            Given.recording_directory_does_not_exist();
            Assert.Inconclusive();

            fileAccess.Verify();
        }

        [TestInitialize]
        public void Setup()
        {
            textBuffer = new Mock<ITextBuffer>();
            serviceProvider = new Mock<SVsServiceProvider>();
            fileAccess = new Mock<IAccessFiles>();
        }

        private Mock<IAccessFiles> fileAccess;
        private Mock<ITextBuffer> textBuffer;
        private Mock<SVsServiceProvider> serviceProvider;

        private void we_have_one_span_matching_audio_comment()
        {
            setup_dte("some_file.cs", @"c:\source\my_project\my_solution.sln", true);
        }

        private void user_is_editing_some_file()
        {
            setup_dte("some_file.cs", @"c:\source\my_project\my_solution.sln", true);
        }

        private void recording_directory_does_not_exist()
        {
            setup_dte("some_file.cs", @"c:\source\my_project\my_solution.sln", false);
        }

        private void setup_dte(string documentName, string solutionPath, bool directoryExists)
        {
            //create_spans(@"// {recorder}");

            serviceProvider.Setup(s => s.GetService(It.IsAny<Type>()))
                .Returns(new DTE2Stub(documentName, solutionPath));

            fileAccess.Setup(f => 
                f.DirectoryExists(It.Is<string>(s => s == @"c:\source\my_project\Audio Comments"))).Returns(directoryExists);

            fileAccess.Setup(f => 
                f.CreateDirectory(It.Is<string>(s => s == @"c:\source\my_project\Audio Comments"))).Verifiable();

            //tagger = new RcorderTagger(fileAccess.Object, serviceProvider.Object, textBuffer.Object;
        }
    }
}
